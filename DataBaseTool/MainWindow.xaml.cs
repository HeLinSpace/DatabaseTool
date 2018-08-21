using System.Windows;
using DataBaseTool.Model;
using DataBaseTool.Common;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DataBaseTool.Controls;
using static DataBaseTool.Controls.TreeViewEx;

namespace DataBaseTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        public ObservableCollection<TreeViewEx.TreeViewExNode> ModelList { get; set; }



        public void Init()
        {
            this.ModelList = new ObservableCollection<TreeViewEx.TreeViewExNode>();

            var connection = LoadingConnection.GetConnectionList();

            foreach (var item in connection)
            {
                var id = GuidExtend.NewId();
                TreeViewExNode node = new TreeViewExNode()
                {
                    ID = id,
                    Name = "N" + id,
                    ConnectionId = item.Id,
                    IsExpanded = false,
                    Level = 1,
                    Header = item.ConnectionName,
                    DataSource = item.DataSource,
                    DataBase = item.DataBase,
                    FontSize = 18
                };

                node.MouseDoubleClick += ServerClick;

                ModelList.Add(node);

            }
            this.DataContext = this;
        }

        /// <summary>
        /// 获取服务器信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerClick(object sender, RoutedEventArgs e)
        {
            TreeViewExNode item = sender as TreeViewExNode;

            if (!item.IsSelected)
            {
                return;
            }

            var dataBases = new List<string>();

            string connStr = string.Empty;

            var connection = LoadingConnection.GetConnectionById(item.ConnectionId, out connStr);

            item.DataType = connection.DataType;
            item.ConnectionStr = connStr;

            if (BaseUnity.GetConnection(connection))
            {
                if (connection.DataType == DataTypes.ORACLE)
                {
                    TreeItemClick(item, ItemTypes.DataBase);
                }
                else
                {
                    TreeItemClick(item, ItemTypes.Server);
                }
            }
            else
            {
                MessageBox.Show("无法与目标服务建立链接！");
            }
        }

        /// <summary>
        /// 获取数据库信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataBaseClick(object sender, RoutedEventArgs e)
        {
            TreeItemClick(sender as TreeViewExNode, ItemTypes.DataBase);
        }

        private void TableClick(object sender, RoutedEventArgs e)
        {
            var item = sender as TreeViewExNode;

            foreach (TabItemEx tbItem in TabController.Items)
            {
                if (tbItem.ID == item.ID)
                {
                    TabController.SelectedItem = tbItem;

                    return;
                }
            }

            TableView tv = new TableView();

            tv.TableKey = item.TableKey;
            tv.DataBase = item.DataBase;
            tv.DataType = item.DataType;
            if (item.DataType == DataTypes.SQLSERVER)
            {
                tv.ConnectionStr = string.Format(Consts.Connection.SqlConnEx, item.ConnectionStr, item.DataBase);
            }
            else
            {
                tv.ConnectionStr = item.ConnectionStr;
            }

            TabItemEx tb = new TabItemEx();

            tb.ID = item.ID;

            tb.DataBase = item.DataBase;

            tb.DataSource = item.DataSource;

            tb.ConnectionId = item.ConnectionId;

            tb.Header = item.TableKey;

            tb.Content = tv;

            TabController.Items.Add(tb);
            TabController.SelectedItem = tb;
        }

        private void TreeItemClick(TreeViewExNode item, ItemTypes itemTypes)
        {
            if (!item.IsSelected || item.HasItems)
            {
                return;
            }

            item.Items.Clear();

            var data = new List<string>();

            switch (itemTypes)
            {
                case ItemTypes.Server:
                    data = Service.GetDataBases(item.ConnectionStr, item.DataType);
                    break;
                case ItemTypes.DataBase:
                    data = Service.GetTables(item.ConnectionStr, item.DataType, item.DataBase);
                    break;
            }

            DockPanel1.RegisterName(item.Name, item);

            ContextMenu menu = GetOpenItemContextMenu(item.ID, itemTypes);

            item.ContextMenu = menu;

            foreach (var dataItem in data)
            {
                var id = GuidExtend.NewId();
                TreeViewExNode node = new TreeViewExNode();
                node.ID = id;
                node.DataType = item.DataType;
                node.Name = "N" + id;
                node.IsExpanded = false;
                node.Level = item.Level + 1;
                node.Header = dataItem;
                node.FontSize = item.FontSize - 2;
                node.DataSource = item.DataSource;
                node.ConnectionId = item.ConnectionId;
                node.ConnectionStr = item.ConnectionStr;

                switch (itemTypes)
                {
                    case ItemTypes.Server:
                        node.MouseDoubleClick += DataBaseClick;
                        node.DataBase = dataItem;
                        break;

                    case ItemTypes.DataBase:
                        node.MouseDoubleClick += TableClick;
                        node.DataBase = item.DataBase;
                        node.TableKey = dataItem;
                        break;
                }

                item.Items.Add(node);
            }

            item.IsExpanded = true;

        }


        private void Select_Click(object sender, RoutedEventArgs e)
        {
            //Select tv = new Select(DataBase,DataType);

            //TabItemEx tb = new TabItemEx();

            //tb.Header = "查询";

            //tb.Content = tv;

            //TabController.Items.Add(tb);
            //TabController.SelectedItem = tb;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabItem_Close(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            string header = btn.Tag.ToString();
            foreach (TabItemEx item in TabController.Items)
            {
                if (item.Header.ToString() == header)
                {
                    TabController.Items.Remove(item);
                    break;
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;

            Setting setting;

            switch (item.Header.ToString())
            {
                case DataTypeStr._SQLSERVER:
                    setting = new Setting(DataTypes.SQLSERVER, this);
                    setting.Show();
                    break;
                case DataTypeStr._ORACLE:
                    setting = new Setting(DataTypes.ORACLE, this);
                    setting.Show();
                    break;
                case DataTypeStr._MYSQL:
                    setting = new Setting(DataTypes.MYSQL, this);
                    setting.Show();
                    break;
            }
        }

        private void doInit(object sender, RoutedEventArgs e)
        {
            //Header header = new Header();

            //header.Show();

        }

        private void ContextMenuClose(object sender, RoutedEventArgs e)
        {
            MenuItemEx menuItem = sender as MenuItemEx;

            var name = "N" + menuItem.Name.Split('_')[1];

            TreeViewExNode item = DataBaseList.FindName(name) as TreeViewExNode;

            var removeItems = new List<TabItemEx>();

            // 关闭关联视图
            if (menuItem.ItemType == ItemTypes.Server)
            {
                foreach (TabItemEx temp in TabController.Items)
                {
                    if (temp.DataSource == item.DataSource
                        && temp.ConnectionId == item.ConnectionId)
                    {
                        removeItems.Add(temp);
                    }
                }
            }
            else if (menuItem.ItemType == ItemTypes.DataBase)
            {
                foreach (TabItemEx temp in TabController.Items)
                {
                    if (temp.DataBase == item.DataBase
                        && temp.DataSource == item.DataSource
                        && temp.ConnectionId == item.ConnectionId)
                    {
                        removeItems.Add(temp);
                    }
                }
            }

            foreach (TabItemEx temp in removeItems)
            {
                TabController.Items.Remove(temp);
            }

            if (item != null)
            {
                DockPanel1.UnregisterName(name);
                item.Items.Clear();
            }
        }

        #region 工具
        private ContextMenu GetOpenItemContextMenu(string rootName, ItemTypes itemTypes)
        {
            ContextMenu menu = new ContextMenu();
            MenuItemEx menuItem = new MenuItemEx();

            switch (itemTypes)
            {
                case ItemTypes.Server:
                    menuItem.Header = "关闭链接";

                    menuItem.Click += ContextMenuClose;

                    menuItem.Name = "MC_" + rootName;

                    menuItem.ItemType = ItemTypes.Server;

                    menu.Items.Add(menuItem);
                    break;
                case ItemTypes.DataBase:

                    menuItem.Header = "关闭数据库";

                    menuItem.Name = "MC_" + rootName;
                    menuItem.Click += ContextMenuClose;

                    menu.Items.Add(menuItem);
                    menuItem.ItemType = ItemTypes.DataBase;
                    break;

            }

            return menu;
        }

        #endregion
    }
}

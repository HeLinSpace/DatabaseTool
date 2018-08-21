using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DataBaseTool.Common;
using DataBaseTool.Controls;
using DataBaseTool.Model;

namespace DataBaseTool
{
    /// <summary>
    /// TableView.xaml 的交互逻辑
    /// </summary>
    public partial class TableView : UserControl
    {
        public TableView()
        {
            InitializeComponent();
        }

        public string DataBase { get; set; }

        public DataTypes? DataType { get; set; }

        public List<FieldInfo> TableFields { get; set; }

        public string TableKey { get; set; }

        public string SqlWhere { get; set; }

        public string ConnectionStr { get; set; }


        private void Init(object sender, RoutedEventArgs e)
        {
            try
            {
                TableData.Columns.Clear();
                TableData.ItemsSource = null;

                TableData.Visibility = Visibility.Visible;
                ErrorText.Visibility = Visibility.Hidden;

                GridPage.Init(this);
            }
            catch (Exception ex)
            {
                TableData.Visibility = Visibility.Hidden;
                ErrorText.Text = ex.Message;
                ErrorText.Visibility = Visibility.Visible;
            }
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            var tabController = (((this.Parent as TabItem).Parent) as TabControl);

            Select st = new Select();
            st.DataBase = DataBase;
            st.DataType = DataType;
            st.ConnectionStr = ConnectionStr;

            TabItemEx tb = new TabItemEx();

            tb.Header = string.Format("查询 {0}", DataBase);

            tb.Content = st;

            tabController.Items.Add(tb);
            tabController.SelectedItem = tb;
        }

        private void TableData_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = e.Row.GetIndex() + 1;
        }

        /// <summary>
        /// 筛选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Chose_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;

            if (ToolsGrid.Visibility == Visibility.Visible)
            {
                ToolsGrid.Visibility = Visibility.Collapsed;

                SolidColorBrush brush = new SolidColorBrush();
                item.BorderBrush = brush;
                brush = new SolidColorBrush(Colors.Black);
                item.Foreground = brush;
                TableData.Height = TableData.ActualHeight + ToolsGrid.Height;
            }
            else
            {
                ToolsGrid.Visibility = Visibility.Visible;
                SolidColorBrush brush = new SolidColorBrush(Colors.Blue);
                item.BorderBrush = brush;

                item.Foreground = brush;
                if (!(ToolsGrid.Height > 0))
                {
                    ToolsGrid.Height = ToolsGrid.MinHeight;
                }
                TableData.Height = TableData.ActualHeight - ToolsGrid.Height;
            }
        }


        private void Chose_All(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            
            foreach (ChoseEx item in Tools.Items)
            {
                item.CheckeRules.IsChecked = checkbox.IsChecked;
                item._ChoseModel.CheckeRules = checkbox.IsChecked.Value;
            }
        }

        /// <summary>
        /// 添加条件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Chose_Add(object sender, RoutedEventArgs e)
        {
            var count = Tools.Items.Count;
            if (count > 0)
            {
                var contentGrid = (Tools.Items.GetItemAt(count - 1) as ChoseEx).FindName("ConnectGrid") as Grid;
                contentGrid.Visibility = Visibility.Visible;
            }

            ChoseEx chose = new ChoseEx();
            chose.Init(this);
            chose.VerticalAlignment = VerticalAlignment.Top;
            Tools.Items.Add(chose);
            TableData.Height = TableData.Height - 35;
            ToolsGrid.Height = ToolsGrid.Height + 35;
        }


        private void Chose_Remove(object sender, RoutedEventArgs e)
        {
            List<ChoseEx> removeItem = new List<ChoseEx>();

            foreach (ChoseEx item in Tools.Items)
            {
                var model = item._ChoseModel;

                if (!model.CheckeRules)
                {
                    removeItem.Add(item);
                }
            }

            foreach (ChoseEx item in removeItem)
            {
                Tools.Items.Remove(item);
                TableData.Height = TableData.Height + 35;
                ToolsGrid.Height = ToolsGrid.Height - 35;
            }
        }


        /// <summary>
        /// 应用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            var connect = string.Empty;
            SqlWhere = string.Empty;

            foreach (ChoseEx item in Tools.Items)
            {
                var model = item._ChoseModel;

                if (model.CheckeRules && !string.IsNullOrEmpty(model.CompareValue))
                {
                    if (!string.IsNullOrEmpty(connect))
                    {
                        SqlWhere += " " + connect;
                        connect = string.Empty;
                    }

                    SqlWhere += " " + model.FieldKey + " " + model.CompareRule + " '" + model.CompareValue + "'";

                    if (!string.IsNullOrEmpty(model.Connect))
                    {
                        connect = model.Connect;
                    }
                }
            }

            if (!string.IsNullOrEmpty(SqlWhere))
            {
                SqlWhere = " where " + SqlWhere;
            }

            GridPage.Init(this);
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            Import import = new Import(this);
            import.Width = 605;
            import.Height = 450;
            import.Show();
        }
    }
}

using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using DataBaseTool.Common;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using DataBaseTool.Model;

namespace DataBaseTool
{
    /// <summary>
    /// DataGridPage.xaml 的交互逻辑
    /// </summary>
    public partial class DataGridPage : UserControl
    {

        public DataGridPage()
        {
            InitializeComponent();

            List<KeyValueView<int, int>> list = new List<KeyValueView<int, int>>();

            list.Add(new KeyValueView<int, int> { Key = 100, Value = 100 });
            list.Add(new KeyValueView<int, int> { Key = 500, Value = 500 });
            list.Add(new KeyValueView<int, int> { Key = 1000, Value = 1000 });

            PageNo.ItemsSource = list;

            PageNo.SelectedIndex = 0;
        }

        private bool PageNoLoaded { get; set; }

        //每页显示多少条
        private int pageNum = 100;

        //当前是第几页
        private int pIndex = 1;

        //最大页数
        private int MaxIndex = 1;

        //一共多少条
        private int allNum = 0;

        private TableView _TableView { get; set; }

        private string PageOldValue { get; set; } = "1";



        #region 初始化数据
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="grd"></param>
        /// <param name="dtt"></param>
        /// <param name="Num"></param>
        public void Init(TableView view)
        {
            this._TableView = view;

            var sqlCount = string.Empty;

            switch (_TableView.DataType)
            {
                case DataTypes.SQLSERVER:
                    sqlCount = string.Format(Consts.Data.TableDataCount, _TableView.TableKey);
                    break;
                case DataTypes.ORACLE:
                    sqlCount = string.Format(Consts.Data.TableDataCount, _TableView.TableKey);
                    break;
            }

            if (!string.IsNullOrEmpty(_TableView.SqlWhere))
            {
                sqlCount += " " + _TableView.SqlWhere;
            }

            this.allNum = BaseUnity.GetTValueData<int>(_TableView.ConnectionStr, _TableView.DataType, sqlCount, null);

            ReadDataTable();
        }
        #endregion

        #region 画数据
        /// <summary>
        /// 画数据
        /// </summary>
        private void ReadDataTable()
        {
            try
            {
                var sqlData = string.Empty;

                _TableView.TableFields = Service.GetTableColumns(_TableView.ConnectionStr, _TableView.DataType, _TableView.DataBase, _TableView.TableKey);

                switch (_TableView.DataType)
                {
                    case DataTypes.SQLSERVER:

                        var orderKey = string.Empty;

                        if (_TableView.TableFields.Where(s => "ID".Equals(s.ColumnName.ToUpper())).Any())
                        {
                            orderKey = "ID";
                        }
                        else
                        {
                            orderKey = _TableView.TableFields.FirstOrDefault().ColumnName;
                        }

                        sqlData = string.Format(Consts.Data.SqlTableData, string.Join(",", _TableView.TableFields.Select(s => s.ColumnName)), orderKey, _TableView.TableKey, (pIndex - 1) * pageNum + 1, pageNum * pIndex, _TableView.SqlWhere);
                        break;
                    case DataTypes.ORACLE:

                        sqlData = string.Format(Consts.Data.OracleTableData, string.Join(",", _TableView.TableFields.Select(s => s.ColumnName)), _TableView.TableKey, (pIndex - 1) * pageNum + 1, pageNum * pIndex, _TableView.SqlWhere);
                        break;

                }

                var dt = BaseUnity.QueryForDataTable(_TableView.ConnectionStr, _TableView.DataType, sqlData, null);

                _TableView.TableData.ItemsSource = dt.DefaultView;
            }
            catch (Exception e)
            {
                throw (e);
            }
            finally
            {
                SetMaxIndex();
                DisplayPagingInfo();
            }

        }
        #endregion

        #region 画每页显示等数据
        /// <summary>
        /// 画每页显示等数据
        /// </summary>
        private void DisplayPagingInfo()
        {
            SolidColorBrush brush = new SolidColorBrush(Colors.Gray);
            SolidColorBrush brush2 = new SolidColorBrush(Color.FromArgb(255, 1, 84, 74));

            if (this.allNum == 0)
            {
                this.page.IsEnabled = false;

                this.btnGO.IsEnabled = false;
                this.btnGO.Foreground = brush;

                PageNo.IsEnabled = false;
            }
            else
            {
                page.IsEnabled = true;
            }

            if (this.pIndex == 1)
            {
                page.Text = PageOldValue;
                this.btnPrev.IsEnabled = false;
                this.btnFirst.IsEnabled = false;
                this.btnPrev.Foreground = brush;
                this.btnFirst.Foreground = brush;
            }
            else
            {
                this.btnPrev.IsEnabled = true;
                this.btnFirst.IsEnabled = true;
                this.btnPrev.Foreground = brush2;
                this.btnFirst.Foreground = brush2;
            }
            if (this.pIndex == this.MaxIndex)
            {
                this.btnNext.IsEnabled = false;
                this.btnLast.IsEnabled = false;

                this.btnNext.Foreground = brush;
                this.btnLast.Foreground = brush;
            }
            else
            {
                this.btnNext.IsEnabled = true;
                this.btnLast.IsEnabled = true;
                this.btnNext.Foreground = brush2;
                this.btnLast.Foreground = brush2;
            }

            this.TotalNo.Content = string.Format("条/共{1}条 ", this.pageNum, this.allNum);

            this.countPage.Content = string.Format("页/共{0}页", MaxIndex);

            int first = (this.pIndex - 4) > 0 ? (this.pIndex - 4) : 1;

            int last = (first + 9) > this.MaxIndex ? this.MaxIndex : (first + 9);
        }
        #endregion


        #region 设置最多大页面
        /// <summary>
        /// 设置最多大页面
        /// </summary>
        private void SetMaxIndex()
        {
            //多少页
            int Pages = allNum / pageNum;

            if (allNum != (Pages * pageNum))
            {
                if (allNum < (Pages * pageNum))
                    Pages--;
                else
                    Pages++;
            }

            this.MaxIndex = Pages == 0 ? 1 : Pages;
        }
        #endregion

        private void tbl_MouseLeftButtonUp(object sender, RoutedEventArgs e)
        {
            TextBlock tbl = sender as TextBlock;
            if (tbl == null)
                return;
            int index = int.Parse(tbl.Text.ToString());
            this.pIndex = index;
            if (index > this.MaxIndex)
                this.pIndex = this.MaxIndex;
            if (index < 1)
                this.pIndex = 1;
            ReadDataTable();
        }

        private void btnFirst_MouseDown(object sender, RoutedEventArgs e)
        {
            this.pIndex = 1;

            page.Text = pIndex.ToString();

            ReadDataTable();
        }

        private void btnPrev_MouseDown(object sender, RoutedEventArgs e)
        {
            if (this.pIndex <= 1)
                return;

            this.pIndex--;

            page.Text = pIndex.ToString();

            ReadDataTable();
        }

        private void btnNext_MouseDown(object sender, RoutedEventArgs e)
        {
            if (this.pIndex >= this.MaxIndex)
                return;

            this.pIndex++;

            page.Text = pIndex.ToString();

            ReadDataTable();
        }

        private void btnLast_MouseDown(object sender, RoutedEventArgs e)
        {
            this.pIndex = this.MaxIndex;

            page.Text = pIndex.ToString();

            ReadDataTable();
        }

        private void btnGO_Click(object sender, RoutedEventArgs e)
        {
            if (page.Text == "")
                return;

            if (Convert.ToInt32(page.Text) < 1)
                return;

            this.pIndex = Convert.ToInt32(page.Text);

            ReadDataTable();
        }

        private void Page_TextInput(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            PageOldValue = page.Text;
            e.Handled = re.IsMatch(e.Text);
        }

        private void page_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Convert.ToInt32(page.Text) > MaxIndex)
            {
                page.Text = PageOldValue;
            }
        }

        private void PageNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.pageNum = Convert.ToInt32(PageNo.SelectedValue);

            this.pIndex = 1;

            page.Text = "1";

            if (PageNoLoaded)
            {
                ReadDataTable();
            }
            else
            {
                PageNoLoaded = true;
            }
        }
    }
}

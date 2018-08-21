using DataBaseTool.Common;
using DataBaseTool.Model;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DataBaseTool
{
    /// <summary>
    /// Import.xaml 的交互逻辑
    /// </summary>
    public partial class Import : Window
    {
        public Import(TableView tableView)
        {
            _TableView = tableView;
            InitializeComponent();
            Init();
        }

        private TableView _TableView { get; set; }
        private void Init()
        {
            var list = new List<KeyValueView<DataSourceFormats, string>>();

            list.Add(new KeyValueView<DataSourceFormats, string> { Key = DataSourceFormats.EXCEL, Value = "EXCEL" });
            list.Add(new KeyValueView<DataSourceFormats, string> { Key = DataSourceFormats.CSV, Value = "CSV" });
            list.Add(new KeyValueView<DataSourceFormats, string> { Key = DataSourceFormats.JSON, Value = "JSON" });
            list.Add(new KeyValueView<DataSourceFormats, string> { Key = DataSourceFormats.XML, Value = "XML" });

            DataFormat.ItemsSource = list;
            DataFormat.SelectedIndex = 0;

            TableName.ItemsSource = Service.GetTables(_TableView.ConnectionStr, _TableView.DataType, _TableView.DataBase).Select(s => new KeyValueStr { Key = s, Value = s });
        }

        private DataSourceFormats _DataSourceFormats { get; set; }

        private void FolderPreview_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Title = "请选择文件";

            switch (_DataSourceFormats)
            {
                case DataSourceFormats.CSV:
                    dialog.Filter = "CSV|*.CSV;*.csv";
                    break;
                case DataSourceFormats.EXCEL:
                    dialog.Filter = "EXCEL|*.xlsx;*.xls";
                    break;
                case DataSourceFormats.JSON:
                    dialog.Filter = "JSON|*.Json;*.json";
                    break;
                case DataSourceFormats.XML:
                    dialog.Filter = "XML|*.Xml;*.xml";
                    break;
            }

            if (dialog.ShowDialog() == true)
            {
                DataPath.Text = dialog.FileName;
            }
        }

        private void DataFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _DataSourceFormats = (DataSourceFormats)DataFormat.SelectedValue;
        }
    }
}

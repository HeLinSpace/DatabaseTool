using System;
using System.Linq;
using System.Windows;
using DataBaseTool.Model;
using System.Windows.Controls;
using System.Collections.Generic;

namespace DataBaseTool.Controls
{
    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// </summary>
    public partial class ChoseEx : UserControl
    {
        public ChoseEx()
        {
            InitializeComponent();
        }

        public ChoseModel _ChoseModel { get; set; }

        private TableView _TableView { get; set; }

        public void Init(TableView tableView)
        {
            _TableView = tableView;
            _ChoseModel = new ChoseModel();

            CheckeRules.IsChecked = true;
            _ChoseModel.CheckeRules = true;

            List<KeyValueStr> list = new List<KeyValueStr>();
            list.Add(new KeyValueStr { Key = "<", Value = "小于" });
            list.Add(new KeyValueStr { Key = "=", Value = "等于" });
            list.Add(new KeyValueStr { Key = ">", Value = "大于" });
            CompareRuleList.ItemsSource = list;

            FieldKeys.SelectedValuePath = "ColumnName";
            FieldKeys.DisplayMemberPath = "ColumnName";
            FieldKeys.ItemsSource = _TableView.TableFields.OrderBy(s => s.OrderNo);
            FieldKeys.SelectedIndex = 0;

            List<KeyValueStr> listContent = new List<KeyValueStr>();
            listContent.Add(new KeyValueStr { Key = "and", Value = "and" });
            listContent.Add(new KeyValueStr { Key = "or", Value = "or" });
            ConnectList.ItemsSource = listContent;

            CompareKey.Content = "<?>";

        }

        private void CompareRule_Click(object sender, RoutedEventArgs e)
        {
            if (CompareRuleList.IsDropDownOpen)
            {
                CompareRuleList.IsDropDownOpen = false;
            }
            else
            {
                CompareRuleList.IsDropDownOpen = true;
            }
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (ConnectList.IsDropDownOpen)
            {
                ConnectList.IsDropDownOpen = false;
            }
            else
            {
                ConnectList.IsDropDownOpen = true;
            }
        }

        private void CompareRule_Initialized(object sender, EventArgs e)
        {
            CompareRuleList.SelectedIndex = 0;
        }

        private void CompareRuleList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = ((KeyValueStr)CompareRuleList.SelectedItem);

            this.CompareRule.Content = item.Value;
            _ChoseModel.CompareRule = item.Key;
        }

        private void ConnectList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = ((KeyValueStr)ConnectList.SelectedItem);

            this.Connect.Content = item.Value;
            _ChoseModel.Connect = item.Key;
        }

        private void CompareKey_Click(object sender, RoutedEventArgs e)
        {
            if (pop1.IsOpen)
            {
                pop1.IsOpen = false;
            }
            else
            {
                pop1.IsOpen = true;
                CompareKeyValue.Focus();
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            var value = this.CompareKeyValue.Text;
            this.CompareKey.Content = string.IsNullOrEmpty(value) ? "<?>" : value;
            pop1.IsOpen = false;
            _ChoseModel.CompareValue = string.IsNullOrEmpty(value) ? "" : value; ;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.CompareKeyValue.Text = "";
            pop1.IsOpen = false;
        }

        private void CheckeRules_Click(object sender, RoutedEventArgs e)
        {
            _ChoseModel.CheckeRules = CheckeRules.IsChecked.Value;
        }

        private void ConnectGrid_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (ConnectGrid.Visibility == Visibility.Visible)
            {
                ConnectList.SelectedIndex = 0;
            }
        }

        private void FieldKeys_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _ChoseModel.FieldKey = FieldKeys.SelectedValue.ToString();
        }
    }
}

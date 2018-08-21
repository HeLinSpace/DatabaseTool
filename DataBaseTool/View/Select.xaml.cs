using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DataBaseTool.Common;
using DataBaseTool.Model;
using Newtonsoft.Json;

namespace DataBaseTool
{
    /// <summary>
    /// Footer.xaml 的交互逻辑
    /// </summary>
    public partial class Select : UserControl
    {
        public Select()
        {
            InitializeComponent();

            //Init();
        }

        public string DataBase { get; set; }

        public string ConnectionStr { get; set; }

        public DataTypes? DataType { get; set; }


        private List<KeyColor> Keys = null;


        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            Keys = new List<KeyColor>();

            string config = "./Resources/Resource.txt";

            StreamReader reader = new StreamReader(config, Encoding.UTF8);

            string line = string.Empty;

            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
            {
                KeyColor item = JsonConvert.DeserializeObject<KeyColor>(line);

                string[] keys = item.Key.Split(',');

                System.Windows.Media.Color color = new System.Windows.Media.Color();

                switch (item.Color)
                {
                    case "red":
                        color = Colors.Red;
                        break;
                    case "blue":
                        color = Colors.Blue;
                        break;
                    default:
                        color = Colors.Black;
                        break;
                }

                foreach (var key in keys)
                {
                    var temp = new KeyColor();
                    temp.Key = key;
                    temp.SolidColorBrush = new SolidColorBrush(color);
                    Keys.Add(temp);
                }
            }
        }

        private void Execute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TableData.Columns.Clear();
                TableData.ItemsSource = null;

                var sql = Context.Text;

                var data = BaseUnity.QueryForDataTable(ConnectionStr,DataType, sql, null);
                
                TableData.Visibility = Visibility.Visible;
                ErrorText.Visibility = Visibility.Hidden;

                if (data.Rows.Count > 0)
                {
                    TableData.ItemsSource = data.DefaultView;
                }
                else
                {
                    foreach (var item in data.Columns)
                    {
                        var column = new DataGridTextColumn();
                        column.IsReadOnly = false;
                        column.Header = item.ToString();
                        column.Width = 100;
                        TableData.Columns.Add(column);
                    }
                }
            }
            catch (Exception ex)
            {
                TableData.Visibility = Visibility.Hidden;
                ErrorText.Text = ex.Message;
                ErrorText.Visibility = Visibility.Visible;
            }
        }


        private void tbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            //ContextBlock.Text = "";
            //ContextBlock.TextEffects.Clear();
            //for (int i = 0; i < Context.LineCount; i++)
            //{
            //    string a = Context.GetLineText(i);
            //    if (a.Contains("\r\n"))
            //    {
            //        a = a.Substring(0, a.IndexOf("\r\n"));
            //    }
            //    if (i == 0)
            //    {
            //        ContextBlock.Text += a;
            //    }
            //    else
            //    {
            //        ContextBlock.Text += "\n" + a;
            //    }
            //}
            //string s = this.ContextBlock.Text;

            //int start = 0;

            //foreach (var key in Keys)
            //{
            //    start = 0;
            //    while (start < s.Length)
            //    {
            //        var i = s.IndexOf(key.Key, start);
            //        if (i != -1)
            //        {
            //            TextEffect tex = new TextEffect()
            //            {
            //                Foreground = key.SolidColorBrush,
            //                PositionCount = key.Key.Length,
            //                PositionStart = i,
            //            };
            //            this.ContextBlock.TextEffects.Add(tex);
            //            start = i + key.Key.Length;
            //        }
            //        else
            //        {
            //            break;
            //        }
            //    }
            //}

        }

        private void tbx_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            tbx_TextChanged(sender, null);
        }
    }
}

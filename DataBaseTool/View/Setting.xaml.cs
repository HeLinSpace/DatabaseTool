using DataBaseTool.Common;
using DataBaseTool.Model;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace DataBaseTool
{
    /// <summary>
    /// Header.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Window
    {
        private DataTypes _dataType { get; set; }

        private MainWindow _mainWindow { get; set; }


        public Setting(DataTypes type, MainWindow window)
        {
            InitializeComponent();

            this.Title = "链接 " + type.GetName();

            switch (type)
            {
                case DataTypes.MYSQL:

                    Port.Visibility = Visibility.Visible;
                    PortLab.Visibility = Visibility.Visible;
                    Port.Text = "1521";
                    break;

                case DataTypes.ORACLE:
                    Port.Visibility = Visibility.Visible;
                    PortLab.Visibility = Visibility.Visible;
                    DataBase.Visibility = Visibility.Visible;
                    DataBaseLab.Visibility = Visibility.Visible;
                    Port.Text = "1521";
                    break;
            }

            _dataType = type;

            _mainWindow = window;
        }

        private string LoginCheck()
        {
            if (string.IsNullOrEmpty(ConnectionName.Text))
            {
                return "请输入链接名！";
            }

            if (string.IsNullOrEmpty(DatabaseAddress.Text))
            {
                return "请输入主机名或IP地址！";
            }

            if (_dataType == DataTypes.ORACLE && string.IsNullOrEmpty(DataBase.Text))
            {
                return "请输入数据库名！";
            }

            if (string.IsNullOrEmpty(LoginName.Text))
            {
                return "请输入用户名！";
            }

            if (string.IsNullOrEmpty(PassWord.Password))
            {
                return "请输入密码！";
            }

            return "";
        }



        private void Button_Test(object sender, RoutedEventArgs e)
        {
            var errMSG= LoginCheck();

            if (!string.IsNullOrEmpty(errMSG))
            {
                ErrorMessage.Text = errMSG;
                return;
            }

            var config = new ConnectConfig();
            config.ConnectionName = ConnectionName.Text;
            config.DataBase = DataBase.Text;
            config.DataType = _dataType;
            config.DataSource = DatabaseAddress.Text;
            config.UserID = LoginName.Text;
            config.PassWord = PassWord.Password;
            config.Port = Port.Text;

            if (BaseUnity.GetConnection(config))
            {
                ErrorMessage.Text = "链接成功！";
            }
            else
            {
                ErrorMessage.Text = "链接失败！";
            }
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_OK(object sender, RoutedEventArgs e)
        {
            var errMSG = LoginCheck();

            if (!string.IsNullOrEmpty(errMSG))
            {
                ErrorMessage.Text = errMSG;
                return;
            }

            _mainWindow.Init();

            this.Close();
        }
    }
}

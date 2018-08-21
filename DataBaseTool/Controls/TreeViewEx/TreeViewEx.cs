using DataBaseTool.Common;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using DataBaseTool.Model;

namespace DataBaseTool.Controls
{
    public class TreeViewEx : TreeView
    {
        #region 公共方法

        //构造
        public TreeViewEx()
        {
            
        }
 
        #endregion

        public class TreeViewExNode : TreeViewItem,INotifyPropertyChanged
        {
            #region private

            private bool _IsExpanded = false;
            private int _Level = 1;
            private string _ID;
            private string _DataSource;
            private string _DataBase;
            private string _TableKey;
            private string _ConnectionId;
            private DataTypes? _DataType;
            private string _ConnectionStr;


            #endregion

            public event PropertyChangedEventHandler PropertyChanged;

            #region 公共属性

            public string ID
            {
                get { return _ID; }
                set { this._ID = value; RaisePropertyChanged("ID"); }
            }

            public string DataSource
            {
                get { return _DataSource; }
                set { this._DataSource = value; RaisePropertyChanged("DataSource"); }
            }

            public string TableKey
            {
                get { return _TableKey; }
                set { this._TableKey = value; RaisePropertyChanged("TableKey"); }
            }

            public string DataBase
            {
                get { return _DataBase; }
                set { this._DataBase = value; RaisePropertyChanged("DataBase"); }
            }

            public DataTypes? DataType
            {
                get { return _DataType; }
                set { this._DataType = value; RaisePropertyChanged("DataType"); }
            }

            public string ConnectionId
            {
                get { return _ConnectionId; }
                set { this._ConnectionId = value; RaisePropertyChanged("ConnectionId"); }
            }

            public string ConnectionStr
            {
                get { return _ConnectionStr; }
                set { this._ConnectionStr = value; RaisePropertyChanged("ConnectionStr"); }
            }
            /// <summary>
            /// 是否展开
            /// </summary>
            public new bool IsExpanded
            {
                get { return this._IsExpanded; }
                set
                {
                    if (this._IsExpanded != value)
                    {
                        this._IsExpanded = value;
                        RaisePropertyChanged("IsExpanded");
                    } 
                }
            }

            /// <summary>
            /// 层级 >= 1
            /// </summary>
            public int Level
            {
                get { return this._Level; }
                set
                {
                    this._Level = value;
                }
            }

            /// <summary>
            /// Level颜色
            /// </summary>
            public LinearGradientBrush LevelColor
            {
                get
                {
                    LinearGradientBrush levelColor = null;
                    switch (Level)
                    {
                        case 1: { levelColor = ResourceDictionaries.ResourceCollection_Colors["TreeViewExLevel01Color"] as LinearGradientBrush; } break;
                        case 2: { levelColor = ResourceDictionaries.ResourceCollection_Colors["TreeViewExLevel02Color"] as LinearGradientBrush; } break;
                        case 3: { levelColor = ResourceDictionaries.ResourceCollection_Colors["TreeViewExLevel03Color"] as LinearGradientBrush; } break;
                    }
                    return levelColor;
                }
            }

            /// <summary>
            /// 子列表
            /// </summary>
            public ObservableCollection<TreeViewExNode> Children { get; set; }

            #endregion

            #region 公共方法
            
            //构造
            public TreeViewExNode()
            {
                Children = new ObservableCollection<TreeViewExNode>();
            }
            //通知
            private void RaisePropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            #endregion
        };

    }
}

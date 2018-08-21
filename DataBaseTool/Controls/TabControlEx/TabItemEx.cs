using System.ComponentModel;
using System.Windows.Controls;

namespace DataBaseTool.Controls
{
    public class TabItemEx : TabItem
    {
        #region private

        #endregion

        #region 公共属性

        public string ID { get; set; }
        public string DataSource { get; set; }
        public string DataBase { get; set; }

        public string ConnectionId { get; set; }

        #endregion

        #region 公共方法

        //构造
        public TabItemEx()
        {
        }

        #endregion
    }

}

using System;
using System.Windows;

namespace DataBaseTool.Common
{
    public class ResourceDictionaries
    {
        #region ==私有==

        private static ResourceDictionary _ResourceCollection_Colors;
        private static ResourceDictionary _ResourceCollection_TreeViewEx;
        private static ResourceDictionary _ResourceCollection_ComboBoxEx;
        
        #endregion

        #region ==公有==

        /// <summary>
        /// 颜色字典
        /// </summary>
        public static ResourceDictionary ResourceCollection_Colors
        {
            get
            {
                if (_ResourceCollection_Colors == null)
                {
                    _ResourceCollection_Colors = new ResourceDictionary();
                    _ResourceCollection_Colors.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("DataBaseTool;Component/Resource/Colors/Colors.xaml", UriKind.RelativeOrAbsolute) });
                }
                return _ResourceCollection_Colors;
            }
        }

        /// <summary>
        /// TreeViewEx资源
        /// </summary>
        public static ResourceDictionary ResourceCollection_TreeViewEx
        {
            get
            {
                if (_ResourceCollection_TreeViewEx == null)
                {
                    _ResourceCollection_TreeViewEx = new ResourceDictionary();
                    _ResourceCollection_TreeViewEx.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("DataBaseTool;Component/Controls/TreeViewEx/TreeViewEx.xaml", UriKind.RelativeOrAbsolute) });
                }
                return _ResourceCollection_TreeViewEx;
            }
        }
        
        #endregion
    }
}

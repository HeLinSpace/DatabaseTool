using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataBaseTool.Model
{
    public class KeyValueView<TKey,TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }
    }

    public class KeyValueStr: KeyValueView<string,string>
    {
    }


    public class KeyColor
    {
        public string Key { get; set; }

        public string Color { get; set; }

        public SolidColorBrush SolidColorBrush { get; set; } = new SolidColorBrush();
    }
}

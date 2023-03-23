using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaMVC.EN.Utils
{
    public class Graficas
    {
        public titleChart title { get; set; }
        public Tooltip tooltip { get; set; }
        public Legend legend { get; set; }
        public XAxis xAxis { get; set; }
        public YAxis yAxis { get; set; }
        public List<Series> series { get; set; }
        public Toolbox toolbox { get; set; }

    }

    public class titleChart
    {
        public string text { get; set; }
        public string subtext { get; set; }
        public string x { get; set; }
    }

    public class Tooltip
    {
        public string trigger { get; set; }
        public string formatter { get; set; }
    }

    public class Legend
    {
        public string orient { get; set; }
        public string left { get; set; }
        public List<string> data { get; set; }
    }

    public class Toolbox
    {
        public string show { get; set; }
    }

    public class SplitArea
    {
        public string show { get; set; }
    }

    public class XAxis
    {
        public string type { get; set; }
        public AxisLabel axisLabel { get; set; }
        public List<string> data { get; set; }
        public bool boundaryGap { get; set; }
    }

    public class AxisLabel
    {
        public string show { get; set; }
        public int interval { get; set; }
        public int rotate { get; set; }
        public string margin { get; set; }
        public string formatter { get; set; }
        public TextStyle textStyle { get; set; }
    }

    public class TextStyle
    {
        public string color { get; set; }
        public string fontFamily { get; set; }
        public string fontSize { get; set; }
        public string fontStyle { get; set; }
        public string fontWeight { get; set; }
    }

    public class YAxis
    {
        public string type { get; set; }
        public Boolean boundaryGap { get; set; }
        public List<string> data { get; set; }
    }

    public class Series
    {
        public string name { get; set; }
        public string type { get; set; }
        public string[] radius { get; set; }
        public string[] color { get; set; }
        public string stack { get; set; }
        public string[] center { get; set; }
        public ItemStyle itemStyle { get; set; }
        public labelLine labelLine { get; set; }
        public List<dataGrafica> data { get; set; }
        public List<areaStyle> areaStyle { get; set; }
    }
    public class areaStyle
    {

    }
    public class dataGrafica
    {
        public int value { get; set; }
        public string name { get; set; }
    }

    public class ItemStyle
    {
        public emphasis emphasis { get; set; }
        public normal normal { get; set; }
        public string color { get; set; }
    }
    public class labelLine
    {
        public normal normal { get; set; }
    }
    public class emphasis
    {
        public int shadowBlur { get; set; }
        public int shadowOffsetX { get; set; }
        public string shadowColor { get; set; }
    }
    public class normal
    {
        public Boolean? show { get; set; }
    }
    public class Data
    {
        public string name { get; set; }
        public int value { get; set; }
    }

    public class SeriesGauge
    {
        public string name { get; set; }
        public string type { get; set; }
        public List<Data> data { get; set; }
    }
}

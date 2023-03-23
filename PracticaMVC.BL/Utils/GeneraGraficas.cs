using Newtonsoft.Json;
using PracticaMVC.EN.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PracticaMVC.BL.Utils
{
    public class GeneraGraficas
    {
        public Graficas GeneraJsonGrafica(string textTitle, List<dataGrafica> datosGrafica, string tipoGrafica, string nombreTooltip, Boolean graficaDona)
        {
            Graficas gb = new Graficas();

            if (tipoGrafica == "line")
            {
                gb.xAxis = new XAxis()
                {
                    type = "category",
                    boundaryGap = false
                };
                gb.xAxis.axisLabel = new AxisLabel()
                {
                    interval = 0,
                    rotate = 90
                };
                gb.xAxis.data = new List<string>();
                foreach (var item in datosGrafica)
                {
                    gb.xAxis.data.Add(item.name);
                }
                gb.yAxis = new YAxis()
                {
                    type = "value",
                    boundaryGap = false
                };
                gb.yAxis.data = new List<string>();
                foreach (var item in datosGrafica)
                {
                    gb.yAxis.data.Add(item.value.ToString());
                }
                gb.series = new List<Series>()
                {
                    new Series()
                    {

                        name = nombreTooltip,
                        type = tipoGrafica,
                        color= new string[]{ "#89A8E2"},
                        labelLine = new labelLine()
                        {
                            normal = new normal()
                            {
                                show = null
                            }
                        },
                        areaStyle = new List<areaStyle>() { },
                        data = datosGrafica
                    }
                };
            }
            else
            {
                gb.title = new titleChart()
                {
                    text = textTitle,
                    subtext = "",
                    x = "center"
                };
                gb.legend = new Legend()
                {
                    orient = "vertical",
                    left = "left"
                };
                gb.legend.data = new List<string>();
                foreach (var item in datosGrafica)
                {
                    gb.legend.data.Add(item.name);
                }
                gb.series = new List<Series>()
                {
                    new Series()
                    {
                        name = nombreTooltip,
                        type = tipoGrafica,
                        radius = new string[] { graficaDona ? "30%" : "0%", graficaDona ? "60%" : "75%"},
                        center = new string[] {"50%","50%"},
                        labelLine = new labelLine()
                        {
                            normal = new normal()
                            {
                                show = null
                            }
                        },
                        itemStyle = new ItemStyle()
                        {
                            emphasis = new emphasis()
                            {
                                shadowBlur  = 10,
                                shadowOffsetX= 0,
                                shadowColor = "rgba(0, 0, 0, 0.5)"
                            }
                        },
                        data = datosGrafica
                    }
                };
            } 

            return gb;
        }
    }
}

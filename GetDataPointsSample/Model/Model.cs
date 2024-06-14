using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDataPointsSample
{
    public class ChartDataModel
    {
        public double XValue { get; set; }

        public double YValue { get; set; }

        public ChartDataModel() { }

        public ChartDataModel(double xValue, double yValue)
        {
            XValue = xValue;
            YValue = yValue;
        }
    }
}

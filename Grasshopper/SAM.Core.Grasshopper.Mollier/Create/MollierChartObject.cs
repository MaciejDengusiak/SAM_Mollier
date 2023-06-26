﻿using SAM.Core.Mollier;
using System.Drawing;

namespace SAM.Core.Grasshopper.Mollier
{
    public static partial class Create
    {
        public static MollierChartObject MollierChartObject(this IMollierProcess mollierProcess, Color color, ChartType chartType, double z = 0)
        {
            if (mollierProcess == null || chartType == ChartType.Undefined)
            {
                return null;
            }

            if(color == Color.Empty)
            {
                return MollierChartObject(mollierProcess as UIMollierProcess, chartType, z);
            }

            return MollierChartObject(mollierProcess, color, chartType, z);
        }

        public static MollierChartObject MollierChartObject(this MollierProcess mollierProcess, Color color, ChartType chartType, double z = 0)
        {
            if (mollierProcess == null || chartType == ChartType.Undefined || color == Color.Empty)
            {
                return null;
            }

            return MollierChartObject(mollierProcess, color, chartType, z);
        }

        public static MollierChartObject MollierChartObject(this UIMollierProcess uIMollierProcess, ChartType chartType, double z = 0)
        {
            if (uIMollierProcess == null || chartType == ChartType.Undefined)
            {
                return null;
            }

            Color color = Color.Empty;
            UIMollierAppearance uIMollierAppearance = uIMollierProcess.UIMollierAppearance;
            if(uIMollierAppearance != null)
            {
                color = uIMollierAppearance.Color;
            }

            return MollierChartObject(uIMollierProcess, color, chartType, z);
        }
    }
}

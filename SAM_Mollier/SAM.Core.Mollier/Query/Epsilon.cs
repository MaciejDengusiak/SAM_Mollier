﻿namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        public static double Epsilon(this MollierPoint mollierPoint_1, MollierPoint mollierPoint_2)
        {
            if (mollierPoint_1 == null || mollierPoint_2 == null)
            {
                return double.NaN;
            }

            double humidityRatioDifference = mollierPoint_2.HumidityRatio - mollierPoint_1.HumidityRatio;
            if (humidityRatioDifference == 0)
            {
                return double.NegativeInfinity;
            }

            double enthalpyDifference = (mollierPoint_2.Enthalpy - mollierPoint_1.Enthalpy) / 1000;

            return enthalpyDifference / humidityRatioDifference;
        }


        public static double Epsilon(this MollierProcess mollierProcess)
        {
            if(mollierProcess == null)
            {
                return double.NaN;
            }

            return Epsilon(mollierProcess.Start, mollierProcess.End);

        }
    }
}
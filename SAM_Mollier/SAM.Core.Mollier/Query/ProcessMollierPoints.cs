﻿using System.Collections.Generic;

namespace SAM.Core.Mollier
{
    public static partial class Query
    {
        //public static List<MollierPoint> ProcessMollierPoints(this CoolingProcess coolingProcess, double tolerance = Tolerance.MacroDistance)
        //{
        //    if (coolingProcess == null)
        //    {
        //        return null;
        //    }

        //    MollierPoint mollierPoint_Start = coolingProcess.Start;
        //    if(mollierPoint_Start == null || !mollierPoint_Start.IsValid())
        //    {
        //        return null;
        //    }

        //    MollierPoint mollierPoint_End = coolingProcess.End;
        //    if (mollierPoint_End == null || !mollierPoint_End.IsValid())
        //    {
        //        return null;
        //    }

        //    if(Core.Query.AlmostEqual(mollierPoint_Start.HumidityRatio, mollierPoint_End.HumidityRatio, tolerance))
        //    {
        //        return new List<MollierPoint>() { mollierPoint_Start, mollierPoint_End };
        //    }

        //    MollierPoint mollierPoint_75 = null;
        //    MollierPoint mollierPoint_85 = null;

        //    if(mollierPoint_Start.RelativeHumidity > 80)
        //    {
        //        return new List<MollierPoint>() { mollierPoint_Start, mollierPoint_End };
        //    }

        //    double dryBulbTemperature = mollierPoint_Start.RelativeHumidity < 75 ? DryBulbTemperature_ByHumidityRatio(mollierPoint_Start.HumidityRatio, 75, mollierPoint_Start.Pressure) : mollierPoint_Start.DryBulbTemperature;
        //    if (mollierPoint_Start.RelativeHumidity < 85)
        //    {
        //        mollierPoint_85 = Create.MollierPoint_ByRelativeHumidity(dryBulbTemperature - 2.5, 85, mollierPoint_Start.Pressure);
        //        if (mollierPoint_Start.RelativeHumidity < 75)
        //        {
        //            mollierPoint_75 = Create.MollierPoint_ByRelativeHumidity(dryBulbTemperature, 75, mollierPoint_Start.Pressure);
        //        }
        //    }

        //    List<MollierPoint> result = null;
        //    if (mollierPoint_85 != null)
        //    {
        //        result = new List<MollierPoint>();
        //        result.Add(new MollierPoint(mollierPoint_Start));
                
        //        if (mollierPoint_75 != null)
        //        {
        //            result.Add(mollierPoint_75);
        //        }

        //        result.Add(mollierPoint_85);
        //        result.Add(mollierPoint_End);
        //    }

        //    return result;
        //}

        public static List<MollierPoint> ProcessMollierPoints(this CoolingProcess coolingProcess, double tolerance = Tolerance.MacroDistance)
        {
            if (coolingProcess == null)
            {
                return null;
            }

            MollierPoint mollierPoint_Start = coolingProcess.Start;
            if (mollierPoint_Start == null || !mollierPoint_Start.IsValid())
            {
                return null;
            }

            MollierPoint mollierPoint_End = coolingProcess.End;
            if (mollierPoint_End == null || !mollierPoint_End.IsValid())
            {
                return null;
            }

            double pressure = mollierPoint_Start.Pressure;

            List<MollierPoint> result = new List<MollierPoint>();
            result.Add(mollierPoint_Start);

            if (!Core.Query.AlmostEqual(mollierPoint_Start.HumidityRatio, mollierPoint_End.HumidityRatio, tolerance))
            {
                double dryBulbTemperature_Start_Saturation = DryBulbTemperature_ByHumidityRatio(mollierPoint_Start.HumidityRatio, 100, pressure);

                double dryBulbTemperature_1 = mollierPoint_Start.DryBulbTemperature - ((mollierPoint_Start.DryBulbTemperature - dryBulbTemperature_Start_Saturation) / 3);
                MollierPoint mollierPoint_1 = new MollierPoint(dryBulbTemperature_1, mollierPoint_Start.HumidityRatio, pressure);
                result.Add(mollierPoint_1);

                double relativeHumidity = 95;

                double humidityRatio_2 = mollierPoint_Start.HumidityRatio - ((mollierPoint_Start.HumidityRatio - mollierPoint_End.HumidityRatio) / 3);
                double dryBulbTemperature_2 = DryBulbTemperature_ByHumidityRatio(humidityRatio_2, relativeHumidity, pressure);
                MollierPoint mollierPoint_2 = new MollierPoint(dryBulbTemperature_2, humidityRatio_2, pressure);
                result.Add(mollierPoint_2);

                double step = 0.5;

                double dryBulbTemperature = dryBulbTemperature_2 + step;
                while(dryBulbTemperature > mollierPoint_End.DryBulbTemperature)
                {
                    double humidityRatio = HumidityRatio(dryBulbTemperature, relativeHumidity, pressure);
                    if(!double.IsNaN(humidityRatio))
                    {
                        MollierPoint mollierPoint = new MollierPoint(dryBulbTemperature, humidityRatio, pressure);
                        if(mollierPoint != null)
                        {
                            result.Add(mollierPoint);
                        }
                    }

                    dryBulbTemperature += step;
                }
            }

            result.Add(mollierPoint_End);

            return result;
        }
    }
}

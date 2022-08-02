﻿namespace SAM.Core.Mollier
{
    public static partial class Create
    {
        public static SteamHumidificationProcess SteamHumidificationProcess_ByHumidityRatioDifference(this MollierPoint start, double humidityRatioDifference)
        {
            if (start == null || double.IsNaN(humidityRatioDifference))
            {
                return null;
            }

            IsotermicHumidificationProcess isotermicHumidificationProcess = IsotermicHumidificationProcess_ByHumidityRatioDifference(start, humidityRatioDifference);
            if(isotermicHumidificationProcess == null)
            {
                return null;
            }

            return SteamHumidificationProcess(isotermicHumidificationProcess);
        }

        public static SteamHumidificationProcess SteamHumidificationProcess_ByRelativeHumidity(this MollierPoint start, double relativeHumidity)
        {
            if (start == null || double.IsNaN(relativeHumidity))
            {
                return null;
            }

            IsotermicHumidificationProcess isotermicHumidificationProcess = IsotermicHumidificationProcess_ByRelativeHumidity(start, relativeHumidity);
            if (isotermicHumidificationProcess == null)
            {
                return null;
            }

            return SteamHumidificationProcess(isotermicHumidificationProcess);
        }

        public static SteamHumidificationProcess SteamHumidificationProcess(IsotermicHumidificationProcess isotermicHumidificationProcess)
        {
            if (isotermicHumidificationProcess == null)
            {
                return null;
            }

            MollierPoint start_Temp = isotermicHumidificationProcess.Start;
            if(start_Temp == null)
            {
                return null;
            }

            MollierPoint end_Temp = isotermicHumidificationProcess.End;
            if(end_Temp == null)
            {
                return null;
            }

            MollierPoint start_Saturation = start_Temp.SaturationMollierPoint();
            if (start_Saturation == null)
            {
                return null;
            }

            double heatCapacity = start_Temp.HeatCapacity();
            double heatCapacity_Saturation = start_Saturation.HeatCapacity();

            //The small rise in dry-bulb temperature from a steam humidifier is due to the sensible heating effect of the steam.
            //https://www.sciencedirect.com/topics/engineering/steam-humidifier
            double temperatureDifference = (heatCapacity / heatCapacity_Saturation) * (end_Temp.HumidityRatio - start_Temp.HumidityRatio) * (100 - start_Temp.DryBulbTemperature);

            end_Temp = new MollierPoint(end_Temp.DryBulbTemperature + temperatureDifference, end_Temp.HumidityRatio, end_Temp.Pressure);
            if (end_Temp == null)
            {
                return null;
            }

            return new SteamHumidificationProcess(start_Temp, end_Temp);
        }
    }
}
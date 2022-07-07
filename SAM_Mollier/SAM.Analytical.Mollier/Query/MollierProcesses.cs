﻿using SAM.Core.Mollier;
using System.Collections.Generic;

namespace SAM.Analytical.Mollier
{
    public static partial class Query
    {
        public static List<IMollierProcess> MollierProcesses(this AirHandlingUnitResult airHandlingUnitResult)
        {
            if (airHandlingUnitResult == null)
            {
                return null;
            }

            double pressure = 101325;

            List<IMollierProcess> result = new List<IMollierProcess>();

            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterDesignTemperature, out double winterDesignTemperature);
            airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterDesignRelativeHumidity, out double winterDesignRelativeHumidity);
            if(!double.IsNaN(winterDesignTemperature) && !double.IsNaN(winterDesignRelativeHumidity))
            {
                MollierPoint start = Core.Mollier.Create.MollierPoint_ByRelativeHumidity(winterDesignTemperature, winterDesignRelativeHumidity, pressure);
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.FrostCoilOffTemperature, out double winterFrostOffCoilTemperature);

                if (!double.IsNaN(winterFrostOffCoilTemperature) && winterFrostOffCoilTemperature > winterDesignTemperature)
                {
                    Heating heating = Core.Mollier.Create.Heating(start, winterFrostOffCoilTemperature);
                    result.Add(heating);

                    start = heating.End;
                }

                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterHeatRecoveryDryBulbTemperature, out double winterHeatRecoveryDryBulbTemperature);
                airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterHeatRecoveryRelativeHumidity, out double winterHeatRecoveryRelativeHumidity);

                if(!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterHeatRecoverySensibleEfficiency, out double winterHeatRecoverySensibleEfficiency) || double.IsNaN(winterHeatRecoverySensibleEfficiency))
                {
                    winterHeatRecoverySensibleEfficiency = 0;
                }

                if (!airHandlingUnitResult.TryGetValue(AirHandlingUnitResultParameter.WinterHeatRecoveryLatentEfficiency, out double winterHeatRecoveryLatentEfficiency) || double.IsNaN(winterHeatRecoveryLatentEfficiency))
                {
                    winterHeatRecoveryLatentEfficiency = 0;
                }

                if (!double.IsNaN(winterHeatRecoveryRelativeHumidity) && !double.IsNaN(winterHeatRecoveryDryBulbTemperature))
                {
                    MollierPoint @return = Core.Mollier.Create.MollierPoint_ByRelativeHumidity(winterHeatRecoveryDryBulbTemperature, winterHeatRecoveryRelativeHumidity, pressure);
                    if(@return != null)
                    {
                        HeatRecovery heatRecovery = Core.Mollier.Create.HeatRecovery(start, @return, winterHeatRecoverySensibleEfficiency, winterHeatRecoveryLatentEfficiency);
                        result.Add(heatRecovery);

                        start = heatRecovery.End;
                    }
                }
            }



            return result;
        }
    }
}


﻿using Newtonsoft.Json.Linq;
using System.Drawing;

namespace SAM.Core.Mollier
{
    public class UIMollierPoint : IMollierPoint, IUIMollierObject
    {
        private MollierPoint mollierPoint;

        private UIMollierAppearance uIMollierAppearance;

        public MollierPoint MollierPoint
        {
            get
            {
                return mollierPoint?.Clone();
            }
        }

        /// <summary>
        /// Pressure [Pa]
        /// </summary>
        public double Pressure
        {
            get
            {

                return mollierPoint == null ? double.NaN : mollierPoint.Pressure;
            }
        }

        /// <summary>
        /// Enthalpy [J/kg]
        /// </summary>
        public double Enthalpy
        {
            get
            {
                if (mollierPoint == null || !mollierPoint.IsValid())
                {
                    return double.NaN;
                }

                return mollierPoint.Enthalpy;
            }
        }

        /// <summary>
        /// Dry Bulb Temperature [C]
        /// </summary>
        public double DryBulbTemperature
        {
            get
            {
                if (mollierPoint == null || !mollierPoint.IsValid())
                {
                    return double.NaN;
                }

                return mollierPoint.DryBulbTemperature;
            }
        }

        /// <summary>
        /// Humidity Ratio [kg/kg]
        /// </summary>
        public double HumidityRatio
        {
            get
            {
                if (mollierPoint == null || !mollierPoint.IsValid())
                {
                    return double.NaN;
                }

                return mollierPoint.HumidityRatio;
            }
        }

        /// <summary>
        /// Relative Humidity [%]
        /// </summary>
        public double RelativeHumidity
        {
            get
            {
                if (mollierPoint == null || !mollierPoint.IsValid())
                {
                    return double.NaN;
                }

                return mollierPoint.RelativeHumidity;
            }
        }

        public UIMollierAppearance UIMollierAppearance
        {
            get
            {
                return uIMollierAppearance;
            }

            set
            {
                uIMollierAppearance = value;
            }
        }

        public UIMollierPoint(MollierPoint mollierPoint, Color color, string label)
        {
            this.mollierPoint = mollierPoint?.Clone();
            uIMollierAppearance = new UIMollierAppearance(color, label);
        }

        public UIMollierPoint(MollierPoint mollierPoint, Color color)
        {
            this.mollierPoint = mollierPoint?.Clone();
            uIMollierAppearance = new UIMollierAppearance(color);
        }

        public UIMollierPoint(MollierPoint mollierPoint, UIMollierAppearance uIMollierAppearance)
        {
            this.mollierPoint = mollierPoint?.Clone();
            this.uIMollierAppearance = uIMollierAppearance?.Clone();
        }

        public UIMollierPoint(MollierPoint mollierPoint)
        {
            this.mollierPoint = mollierPoint?.Clone();
            uIMollierAppearance = new UIMollierAppearance();
        }   

        public UIMollierPoint(UIMollierPoint uIMollierPoint)
        {
            if(uIMollierPoint != null)
            {
                mollierPoint = uIMollierPoint.mollierPoint?.Clone();
                uIMollierAppearance = uIMollierPoint.uIMollierAppearance?.Clone();
            }
        }

        public UIMollierPoint(JObject jObject)
        {
            FromJObject(jObject);
        }

        public bool FromJObject(JObject jObject)
        {
            if (jObject == null)
            {
                return false;
            }

            if (jObject.ContainsKey("MollierPoint"))
            {
                mollierPoint = new MollierPoint(jObject.Value<JObject>("MollierPoint"));
            }

            if (jObject.ContainsKey("UIMollierAppearance"))
            {
                uIMollierAppearance = new UIMollierAppearance(jObject.Value<JObject>("UIMollierAppearance"));
            }

            return true;
        }
        
        public JObject ToJObject()
        {
            JObject jObject = new JObject();
            jObject.Add("_type", Core.Query.FullTypeName(this));

            if(mollierPoint != null)
            {
                jObject.Add("MollierPoint", mollierPoint.ToJObject());
            }

            if (uIMollierAppearance != null)
            {
                jObject.Add("UIMollierAppearance", uIMollierAppearance.ToJObject());
            }


            return jObject;
        }
    }
}

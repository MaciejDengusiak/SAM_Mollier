﻿using Grasshopper.Kernel;
using SAM.Core.Grasshopper.Mollier.Properties;
using System;
using System.Collections.Generic;
using SAM.Core.Mollier;

namespace SAM.Core.Grasshopper.Mollier
{
    public class SAMMollierLoadByProcess : GH_SAMVariableOutputParameterComponent
    {
        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid => new Guid("4dfc2d51-cc75-4e77-a95e-06092b622ab0");

        /// <summary>
        /// The latest version of this component
        /// </summary>
        public override string LatestComponentVersion => "1.0.0";

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Resources.SAM_Mollier;

        public override GH_Exposure Exposure => GH_Exposure.primary;
 
        protected override GH_SAMParam[] Inputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new GooMollierProcessParam() { Name = "_mollierProcess", NickName = "_mollierProcess", Description = "Mollier Process", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "_massFlow", NickName = "_massFlow", Description = "Mass flow [kg/s]", Access = GH_ParamAccess.item, }, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        protected override GH_SAMParam[] Outputs
        {
            get
            {
                List<GH_SAMParam> result = new List<GH_SAMParam>();
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "sensibleLoad", NickName = "sensibleLoad", Description = "Sensible load [kg/s]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "totalLoad", NickName = "totalLoad", Description = "Total load [W]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));
                result.Add(new GH_SAMParam(new global::Grasshopper.Kernel.Parameters.Param_Number() { Name = "latentLoad", NickName = "latentLoad", Description = "Latent load [W]", Access = GH_ParamAccess.item }, ParamVisibility.Binding));

                return result.ToArray();
            }
        }

        /// <summary>
        /// Updates PanelTypes for AdjacencyCluster
        /// </summary>
        public SAMMollierLoadByProcess()
          : base("SAMMollier.LoadByProcess", "SAMMollier.LoadByProcess",
              "Calculates sensible, total and latent load by Mollier Process",
              "SAM", "Mollier")
        {
        }

        protected override void SolveInstance(IGH_DataAccess dataAccess)
        {
            int index;

            index = Params.IndexOfInputParam("_massFlow");
            if (index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }
            double massFlow = double.NaN;
            if (!dataAccess.GetData(index, ref massFlow) || double.IsNaN(massFlow))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }


            index = Params.IndexOfInputParam("_mollierProcess");
            if(index == -1)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
            }
            MollierProcess mollierProcess = null;
            if (!dataAccess.GetData(index, ref mollierProcess) || mollierProcess == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Invalid data");
                return;
            }

            MollierPoint mollierPoint = mollierProcess.End;
            double temperatureDifference = mollierProcess.End.DryBulbTemperature - mollierProcess.Start.DryBulbTemperature;
            double sensibleLoad = Core.Mollier.Query.SensibleLoad_ByMassFlow(mollierPoint, temperatureDifference, massFlow);

            index = Params.IndexOfOutputParam("sensibleLoad");
            if (index != -1)
            {
                dataAccess.SetData(index, sensibleLoad);
            }

            double enthalpyDifference = mollierProcess.End.Enthalpy - mollierProcess.Start.Enthalpy;
            double totalLoad = Core.Mollier.Query.TotalLoad_ByMassFlow(enthalpyDifference, massFlow);
            index = Params.IndexOfOutputParam("totalLoad");
            if (index != -1)
            {
                dataAccess.SetData(index, totalLoad);
            }

            double humidityRatioDifference = mollierProcess.End.HumidityRatio - mollierProcess.Start.HumidityRatio;
            double latentLoad = Core.Mollier.Query.LatentLoad_ByMassFlow(humidityRatioDifference, massFlow);
            index = Params.IndexOfOutputParam("latentLoad");
            if (index != -1)
            {
                dataAccess.SetData(index, latentLoad);
            }
        }
    }
}
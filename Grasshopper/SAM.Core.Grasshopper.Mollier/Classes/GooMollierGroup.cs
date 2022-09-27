﻿using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using SAM.Core.Grasshopper.Mollier.Properties;
using SAM.Core.Mollier;
using System;
using System.Collections.Generic;

namespace SAM.Core.Grasshopper.Mollier
{
    public class GooMollierGroup : GooJSAMObject<MollierPoint>
    {
        public GooMollierGroup()
            : base()
        {
        }

        public GooMollierGroup(MollierPoint mollierPoint)
            : base(mollierPoint)
        {
        }

        public override IGH_Goo Duplicate()
        {
            return new GooMollierPoint(Value);
        }
    }

    public class GooMollierGroupParam : GH_PersistentParam<GooMollierPoint>
    {
        public override Guid ComponentGuid => new Guid("2bc4d547-aa2b-473e-b42e-575722ce8367");

        protected override System.Drawing.Bitmap Icon => Resources.SAM_Small;

        public GooMollierGroupParam()
            : base("MollierGroup", "MollierGroup", "SAM Core Mollier MollierGroup", "Params", "SAM")
        {
        }

        protected override GH_GetterResult Prompt_Plural(ref List<GooMollierPoint> values)
        {
            throw new NotImplementedException();
        }

        protected override GH_GetterResult Prompt_Singular(ref GooMollierPoint value)
        {
            throw new NotImplementedException();
        }
    }
}
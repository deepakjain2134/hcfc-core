using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HCF.Web.ViewModels
{
    public class DrawingFloorViewModel
    {
        public List<FloorTypes> FloorTypes { get; set; }

        public List<Floor> Floors { get; set; }

        public List<FloorPlan> FloorPlans { get; set; }

        public Floor Floor { get; set; }
    }
}
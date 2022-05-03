using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hcf.Api.Tms.Interface
{
    public abstract class AssetsFactory
    {
        public abstract List<TFloorAssets> GetAssetsByWorkOrder(int workOrderId);        
    }
}
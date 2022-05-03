using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HCF.BDO;

namespace Hcf.Api.Tms.Interface
{
    public abstract class ResourceFactory
    {
        public abstract List<UserProfile> GetResource();

        public abstract List<UserProfile> GetResourceByWorkOrder(int workOrderId);

        public abstract UserProfile SaveWoAssignment(UserProfile woAssignments, int workorderPk);
    }
       
}
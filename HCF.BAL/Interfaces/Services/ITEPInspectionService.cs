using HCF.BDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BAL.Interfaces.Services
{
    public interface ITEPInspectionService
    {
        bool AddInspectionDueDate();
        int SaveAssetInspectionDate(int floorAssetId, DateTime inspectionDate);
        int SaveEpInspectionDate(int epId, DateTime inspectionDate);
        int SaveFixedInspectionDate(AssetsFixInsDate objAssetsFixInsDate);
    }
}

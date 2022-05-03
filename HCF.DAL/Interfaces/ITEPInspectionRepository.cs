using HCF.BDO;
using System;

namespace HCF.DAL
{
    public interface ITEPInspectionRepository
    {
        bool AddInspectionDueDate();
        int SaveAssetInspectionDate(int floorAssetId, DateTime inspectionDate);
        int SaveEpInspectionDate(int epId, DateTime inspectionDate);
        int SaveFixedInspectionDate(AssetsFixInsDate objAssetsFixInsDate);
    }
}
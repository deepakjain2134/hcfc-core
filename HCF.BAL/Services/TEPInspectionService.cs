using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.BAL
{
    public  class TEPInspectionService : ITEPInspectionService
    {
        private readonly ITEPInspectionRepository _iTEPInspectionRepository;
        public TEPInspectionService(ITEPInspectionRepository iTEPInspectionRepository)
        {
            _iTEPInspectionRepository = iTEPInspectionRepository;
        }
        

        ///// <summary>
        ///// 
        ///// </summary>
        //public  void AddTEPInspectionDate()
        //{
        //    DAL.TEPInspectionRepository.AddTEPInspectionDate();
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objTEPInspectionRepository"></param>
        /// <returns></returns>
        public  int SaveEpInspectionDate(int epId,DateTime inspectionDate)
        {
            return _iTEPInspectionRepository.SaveEpInspectionDate(epId, inspectionDate);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objTEPInspectionRepository"></param>
        /// <returns></returns>
        public  int SaveAssetInspectionDate(int floorAssetId, DateTime inspectionDate)
        {
            return _iTEPInspectionRepository.SaveAssetInspectionDate(floorAssetId, inspectionDate);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objTEPInspectionRepository"></param>
        /// <returns></returns>
        public  int SaveFixedInspectionDate(AssetsFixInsDate objAssetsFixInsDate)
        {
            return _iTEPInspectionRepository.SaveFixedInspectionDate(objAssetsFixInsDate);
        }

        public  bool AddInspectionDueDate()
        {
            return _iTEPInspectionRepository.AddInspectionDueDate();
        }
    }
}

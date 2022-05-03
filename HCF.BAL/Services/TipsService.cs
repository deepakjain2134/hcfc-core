using HCF.BDO;
using HCF.DAL;
using System.Collections.Generic;

namespace HCF.BAL
{
    public  class TipsService :ITipsService
    {
        private readonly ITipsRepository _tipsRepository;
        public TipsService(ITipsRepository tipsRepository)
        {
            _tipsRepository = tipsRepository;
        }
        public  List<Tips> GetTipsByClientNo(int clientNo,string routeUrl)
        {
            return _tipsRepository.GetTipsByClientNo(clientNo, routeUrl);
        }
        public  List<Tips> GetAllTips(int clientNo)
        {
            return _tipsRepository.GetAllTipsByClientNo(clientNo);
        }
        public  bool InsertOrUpdateTip(Tips tip)
        {
          return _tipsRepository.InsertOrUpdateTips(tip);
        }

        public  bool ApproveTip(int tipId,int ApproveStatus)
        {
            return _tipsRepository.ApproveTip(tipId, ApproveStatus);
        }
    }
}

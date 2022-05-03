using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.DAL;
using System;
using System.Collections.Generic;
using System.Data;

namespace HCF.BAL
{
    public class InspectionService : IInspectionService
    {
        private readonly IEmailService _emailService;
        private readonly IInspectionRepository _inspectionRepository;

        public InspectionService(IEmailService emailService, IInspectionRepository inspectionRepository)
        {
            _emailService = emailService;
            _inspectionRepository = inspectionRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inspectionId"></param>
        /// <param name="isComplete"></param>
        /// <returns></returns>
        //public bool MarkInsdocStatus(int inspectionId, int isComplete)
        //{
        //    return _inspectionRepository.MarkInsdocStatus(inspectionId, isComplete);
        //}

        //public EPDetails GetInspectionHistory(int userId, int epdetailId, int inspectionId)
        //{
        //    return _inspectionRepository.GetInspectionHistory(userId, epdetailId, inspectionId);
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public  DataTable GetIlsmInspections(Guid? activityId)
        //{
        //    return _inspectionRepository.GetIlsmInspections(activityId);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public  List<Inspection> GetAllInspections()
        //{
        //    return _inspectionRepository.GetCurrentInspections();
        //}

        //public  bool MarkInspectionRa()
        //{
        //    return _inspectionRepository.MarkInspectionRa();
        //}


        //public List<EPsDocument> GetEpsDocument(int epId)
        //{
        //    return _inspectionRepository.GetEPsDocument(epId);
        //}


        #region Inspection Group

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        //public  int Save(InspectionGroup objInspectionGroup)
        //{
        //    return _inspectionRepository.Save(objInspectionGroup);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        //public  bool UpdateInspectionGroup(InspectionGroup objInspectionGroup)
        //{
        //    return _inspectionRepository.UpdateInspectionGroup(objInspectionGroup);
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public List<InspectionGroup> GetInspectionGroup()
        //{
        //    return _inspectionRepository.GetInspectionGroup();
        //}

        #endregion

        #region Due Date Email

        //public void SendDueDateEmail()
        //{
        //    _emailService.SendDueDateMail();
        //}

        #endregion

        #region Inspection calendar 

        //public List<Inspection> GetInspectionsForCalendar(int userId, DateTime? startDate, DateTime? endDate)
        //{
        //    return _inspectionRepository.GetInspectionsForCalendar(userId, startDate, endDate);
        //}


        //public List<TInspectionActivity> GetInspections(int floorAssetId, int epId)
        //{
        //    return _inspectionRepository.GetInspections(floorAssetId, epId);
        //}


        //public  calenderViewDashBoard GetDashboardCalendar(int userId)
        //{
        //    return _inspectionRepository.GetDashboardCalendar(userId);
        //}
        

      

        //public  List<EPDetails> GetArchivedEPsInspection(int userId)
        //{
        //    return _inspectionRepository.GetArchivedEPsInspection(userId);

        //}

        #endregion
    }
}

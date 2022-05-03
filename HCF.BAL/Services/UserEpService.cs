using HCF.BAL.Interfaces.Services;
using HCF.BDO;
using HCF.BDO.Enums;
using HCF.DAL;
using HCF.Utility.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HCF.BAL.Services
{
    public class UserEpService : IUserEpService
    {
        //public static IRepository<EPAssignee> _repository;

        public static IGoalMaster _goalRepositiry;

        public UserEpService(IGoalMaster goalRepositiry)
        {
            _goalRepositiry = goalRepositiry;
        }

        public IEnumerable<UserProfile> GetEpUsers(int epdetailId)
        {
            var results = _goalRepositiry.GetEPUsers().Where(x => x.EPDetailId == epdetailId).Select(x => x.UserProfile);
            return results;
        }        

        public bool SaveEPUser(List<EPAssignee> model)
        {
            foreach (var item in model)
            {
                item.CreatedDate = DateTime.UtcNow;
                _goalRepositiry.Add(item);
            }
            return true;
        }

        public bool UpdateEpUser(int epId, int modifiedBy)
        {
            var model = Get().Where(x => x.IsCurrent && x.EPDetailId == epId).ToList();
            if (model.Count() > 0)
            {
                return MarkEpUserInactive(modifiedBy, model);
            }
            else
                return false;
        }

        public bool UpdateUserEps(int userId, int modifiedBy)
        {
            var model = Get().Where(x => x.IsCurrent && x.UserId == userId).ToList();
            if (model.Count() > 0)
            {
                return MarkEpUserInactive(modifiedBy, model);
            }
            else
                return false;
        }

        public IDictionary<UserActivityType, int[]> GetUpdatedRecordsByEp(int epDetailId, int[] userIds)
        {
            IDictionary<UserActivityType, int[]> records = new Dictionary<UserActivityType, int[]>();
            var existingRecords = Get().Where(x => x.IsCurrent && x.EPDetailId == epDetailId).Select(x => x.UserId);
            //var existingRecords = _repository.GetAll().Where(x => x.IsCurrent && x.EPDetailId == epDetailId).Select(x => x.UserId);
            if (existingRecords.Count() > 0)
            {
                var addedRecords = userIds.Except(existingRecords).ToArray();
                var removedRecords = existingRecords.Except(userIds).ToArray();
                records.Add(UserActivityType.EPAssigned, addedRecords);
                records.Add(UserActivityType.EPUnAssigned, removedRecords);
            }
            else
                records.Add(UserActivityType.EPAssigned, userIds);

            return records;
        }

        public IDictionary<UserActivityType, int[]> GetUpdatedRecordsByUser(int userId, int[] epDetailIds)
        {
            IDictionary<UserActivityType, int[]> records = new Dictionary<UserActivityType, int[]>();
            var existingRecords = Get().Where(x => x.IsCurrent && x.UserId == userId).Select(x => x.EPDetailId);
            //var existingRecords = _repository.GetAll().Where(x => x.IsCurrent && x.UserId == userId).Select(x => x.EPDetailId);
            if (existingRecords.Count() > 0)
            {
                var addedRecords = epDetailIds.Except(existingRecords).ToArray();
                var removedRecords = existingRecords.Except(epDetailIds).ToArray();
                records.Add(UserActivityType.EPAssigned, addedRecords);
                records.Add(UserActivityType.EPUnAssigned, removedRecords);
            }
            else
            {
                records.Add(UserActivityType.EPAssigned, epDetailIds);
            }
            return records;
        }

        #region private methods

        private bool MarkEpUserInactive(int modifiedBy, List<EPAssignee> model)
        {
            model.ForEach(a =>
            {
                a.IsCurrent = false;
                a.CreatedBy = modifiedBy;
                a.CreatedDate = DateTime.UtcNow;

            });

            foreach (var item in model)
            {
                _goalRepositiry.Update(item);               
            }
            return true;
        }

        public List<EPAssignee> Get()
        {
            return _goalRepositiry.GetEPUsers().ToList();
        }

        #endregion
    }
}

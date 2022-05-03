using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using HCF.BDO;

using System;

namespace HCF.DAL.Repository
{
    public static class IssueRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newIssue"></param>
        /// <returns></returns>
        //public static int CreateIssue(IssueMaster newIssue)
        //{
        //    int newId;
        //    const string sql = Utility.StoredProcedures.Insert_IssueMaster;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@CreatedBy", newIssue.CreatedBy);
        //        command.Parameters.AddWithValue("@EPdetailId", newIssue.EPdetailId);
        //        command.Parameters.AddWithValue("@ActivityId", newIssue.ActivityId);
        //        command.Parameters.Add("@newId", SqlDbType.Int);
        //        command.Parameters["@newId"].Direction = ParameterDirection.Output;
        //        newId = DBOperations.ExecuteNonQuery(command, "@newId");
        //    }
        //    return newId;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public static List<IssueMaster> GetIssueMaster()
        //{
        //    List<IssueMaster> lists =new List<IssueMaster>();
        //    string sql = Utility.StoredProcedures.Get_IssueMaster;
        //    using (var command = new SqlCommand(sql))
        //    {              
        //        command.CommandType = CommandType.StoredProcedure;
        //        var ds = DBOperations.GetDataSet(command);
        //        //lists = DBOperations.ConvertDataTable<IssueMaster>(ds.Tables[0]);

        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            IssueMaster list = new IssueMaster();
        //            list.IssueId = Convert.ToInt32(row["Id"].ToString());
        //            if (!string.IsNullOrEmpty(row["EpDetailId"].ToString()))
        //                list.EPdetailId = Convert.ToInt32(row["EpDetailId"].ToString());
        //            list.StatusCode = row["StatusCode"].ToString();
        //            list.SubStatusCode = row["SubStatusCode"].ToString();
        //            list.SkillCode = row["SkillCode"].ToString();
        //            list.Description = row["Description"].ToString();
        //            list.WorkOrderNumber = Convert.ToInt32(row["WorkOrderNumber"].ToString());
        //            list.WorkOrderId = Convert.ToInt32(row["WorkOrderId"].ToString());

        //            if (!string.IsNullOrEmpty(row["ActivityId"].ToString()))
        //                list.ActivityId = Guid.Parse(row["ActivityId"].ToString());

        //            if (!string.IsNullOrEmpty(row["CreatedDate"].ToString()))
        //                list.CreatedDate = Convert.ToDateTime(row["CreatedDate"].ToString());

        //            list.IsActive = true;
        //            list.CloseDate = DateTime.Now;
        //            list.CreatedBy = Convert.ToInt32(row["CreatedBy"].ToString());
        //            lists.Add(list);
        //        }

        //        List<UserProfile> users= Users.GetUsers();
        //        List<IssueFiles> files = DBOperations.ConvertDataTable<IssueFiles>(ds.Tables[1]);
        //        foreach (var issue in lists)
        //        {
        //            issue.CreatedDateTimeSpan = Utility.Conversion.ConvertToTimeSpan(issue.CreatedDate);
        //            issue.CloseDateTimeSpan = Utility.Conversion.ConvertToTimeSpan(issue.CloseDate);
        //            //issue.TInspectionActivity = Transaction.GetTInspectionActivity(issue.ActivityId);
        //            if (issue.EPdetailId != null)
        //            {
        //                issue.EPDetails = GoalMaster.GetEpDescription(issue.EPdetailId.Value);
        //               // issue.TInspectionActivity = Transaction.GetTInspectionActivity(issue.ActivityId);
        //            }
        //            issue.UserProfile = users.FirstOrDefault(x => x.UserId == issue.CreatedBy);
        //            issue.IssueFiles = files.Where(x => x.IssueId == issue.IssueId).ToList();
        //            if (issue.ClosedBy.HasValue)
        //                issue.CloseByUserProfile = users.FirstOrDefault(x => x.UserId == issue.ClosedBy.Value);
        //        }

        //    }
        //    return lists;
        //}
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        //public static int InsertIssueFile(IssueFiles files)
        //{
        //    int newId;
        //    const string sql = Utility.StoredProcedures.Insert_IssueFiles;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@IssueId", files.IssueId);
        //        command.Parameters.AddWithValue("@FileName", files.FileName);
        //        command.Parameters.AddWithValue("@FilePath", files.FilePath);
        //        command.Parameters.AddWithValue("@CreatedBy", files.CreatedBy);
        //        command.Parameters.Add("@newId", SqlDbType.Int);
        //        command.Parameters["@newId"].Direction = ParameterDirection.Output;
        //        newId = DBOperations.ExecuteNonQuery(command, "@newId");
        //    }
        //    return newId;
        //}
        
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="updateIssueMaster"></param>
        ///// <returns></returns>
        //public static IssueMaster UpdateIssue(IssueMaster updateIssueMaster)
        //{
        //    const string sql = Utility.StoredProcedures.Update_IssueMaster;
        //    using (var command = new SqlCommand(sql))
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.Parameters.AddWithValue("@ClosedBy", updateIssueMaster.ClosedBy);
        //        command.Parameters.AddWithValue("@Status", updateIssueMaster.Status);
        //        command.Parameters.AddWithValue("@Comments", updateIssueMaster.Comments);
        //        command.Parameters.AddWithValue("@WorkOrderId", updateIssueMaster.WorkOrderId);
        //        command.Parameters.AddWithValue("@Id", updateIssueMaster.IssueId); 
        //        DBOperations.ExecuteNonQuery(command);
        //    }
        //    return updateIssueMaster;
        
        //}
    }
}

using HCF.BDO;
using System.Data;
using System.Data.SqlClient;

namespace HCF.DAL
{
    public  class RoundsQuesRepository :IRoundsQuesRepository
    {
        private readonly ISqlHelper _sqlHelper;
        public RoundsQuesRepository(ISqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        #region RoundsQuestionnaires

        /// <summary>
        /// 
        /// </summary>
        /// <param name="questionnaires"></param>
        /// <returns></returns>
        public  int Save(RoundsQuestionnaires questionnaires)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_RoundsQuestionnaires;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RouQuesId", questionnaires.RouQuesId);
                command.Parameters.AddWithValue("@RoundCatId", questionnaires.RoundCatId);
                command.Parameters.AddWithValue("@RoundStep", questionnaires.RoundStep);
                command.Parameters.AddWithValue("@RiskStepCode", questionnaires.RiskStepCode);
                command.Parameters.AddWithValue("@RiskType", (int)questionnaires.RiskType);
                command.Parameters.AddWithValue("@IsActive", questionnaires.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", questionnaires.CreatedBy);
                command.Parameters.AddWithValue("@IsShared", questionnaires.IsShared);
                command.Parameters.AddWithValue("@shortdescription",questionnaires.ShortDescription);
                command.Parameters.AddWithValue("@AdditionalRoundType", questionnaires.AdditionalRoundType);
                command.Parameters.AddWithValue("@IsOneTimeStep", questionnaires.IsOneTimeStep);
                if (questionnaires.IsCommonRoundQues == true)
                    command.Parameters.AddWithValue("@Applicable", questionnaires.Applicable);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                if (questionnaires.IsCommonRoundQues == true)
                    newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
                else
                    newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  int Save(RoundCategory newRoundCategory)
        {
            int newId;
            const string sql = Utility.StoredProcedures.Insert_RoundCategory;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RoundCatId", newRoundCategory.RoundCatId);
                command.Parameters.AddWithValue("@CategoryName", newRoundCategory.CategoryName);
                command.Parameters.AddWithValue("@IsActive", newRoundCategory.IsActive);
                command.Parameters.AddWithValue("@CreatedBy", newRoundCategory.CreatedBy);
                if (newRoundCategory.IsCommonCat == true)
                    command.Parameters.AddWithValue("@Applicable", newRoundCategory.Applicable);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                if (newRoundCategory.IsCommonCat == true)
                    newId = _sqlHelper.CommonExecuteNonQuery(command, "@newId");
                else
                    newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }


        public  bool Update_RoundSchduleDatesOnRoundCat(RoundCategory newRoundCategory)
        {
            bool status;
            const string sql = Utility.StoredProcedures.Update_RoundSchdulesDates;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@RoundCatId", newRoundCategory.RoundCatId);
                command.Parameters.AddWithValue("@IsActive", newRoundCategory.IsActive);          

                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }
        #endregion

    }
}
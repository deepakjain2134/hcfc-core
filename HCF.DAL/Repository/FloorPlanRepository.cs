using HCF.BDO;

using HCF.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.DAL
{
    public  class FloorPlanRepository: IFloorPlanRepository
    {
        private readonly ISqlHelper _sqlHelper;
        private readonly ICommonRepository _commonRepository;
        private readonly IUsersRepository _usersRepository;

        public FloorPlanRepository(ISqlHelper sqlHelper, ICommonRepository commonRepository, IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
            _commonRepository = commonRepository;
            _sqlHelper = sqlHelper;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        private  List<FloorPlan> FloorPlans(int? floorId, Guid? floorPlanId)
        {
            List<FloorPlan> lists = new List<FloorPlan>();
            const string sql = StoredProcedures.Get_FloorPlans;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@floorId", floorId);
                command.Parameters.AddWithValue("@floorPlanId", floorPlanId);
                DataSet ds = _sqlHelper.GetDataSet(command);
                if (ds != null)
                {
                    lists = _sqlHelper.ConvertDataTable<FloorPlan>(ds.Tables[0]);
                    foreach (FloorPlan list in lists)
                    {
                        list.EffectiveDateTimeSpan =Conversion.ConvertToTimeSpan(list.EffectiveDate);
                        list.CreatedByUser = _usersRepository.GetUsersList(list.CreatedBy);

                    }
                }
            }
            return lists;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorId"></param>
        /// <returns></returns>
        public  List<FloorPlan> GetFloorPlans(int floorId)
        {
            return FloorPlans(floorId, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public  List<FloorPlan> FloorPlans()
        {
            return FloorPlans(null, null);
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorPlanId"></param>
        /// <returns></returns>
        //public  FloorPlan GetFloorPlan(Guid floorPlanId)
        //{
        //    FloorPlan floorPlan = FloorPlans(null, floorPlanId).FirstOrDefault();
        //    if (floorPlan != null)
        //    {
        //        floorPlan.TfloorAssets = FloorAssetRepository.GetFloorAsset(floorPlan.FloorPlanId);
        //        floorPlan.Floor = FloorRepository.GetFloorDescription(floorPlan.FloorId);
        //    }
        //    return floorPlan;
        //}        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="floorPlanId"></param>
        /// <returns></returns>
        public  bool UpdateActiveFloorPlan(Guid floorPlanId)
        {
           bool status;
            const string sql = StoredProcedures.Update_FloorPlan;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;             
                command.Parameters.AddWithValue("@floorPlanId", floorPlanId);
                status = _sqlHelper.ExecuteNonQuery(command);               
            }
            return status;
        }

        public  bool DeleteFloorPlan(Guid floorPlanId)
        {
            bool status;
            const string sql = StoredProcedures.Delete_FloorPlan;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@floorPlanId", floorPlanId);
                status = _sqlHelper.ExecuteNonQuery(command);
            }
            return status;
        }


        #region Drawing Viewer

        public  int SaveDrawingViewer(DrawingViewer newDrawingViewer)
        {
            int newId;
            const string sql = StoredProcedures.Insert_DrawingViewer;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@FloorPlanId", newDrawingViewer.FloorPlanId);
                command.Parameters.AddWithValue("@ViewerMode", newDrawingViewer.ViewerMode);
                command.Parameters.AddWithValue("@PermitNo", newDrawingViewer.PermitNo);
                command.Parameters.AddWithValue("@ModifiedBy", newDrawingViewer.ModifiedBy);
                command.Parameters.AddWithValue("@SpeceObject", newDrawingViewer.SpeceObject);
                command.Parameters.AddWithValue("@RedlineObject", newDrawingViewer.RedlineObject);
                command.Parameters.AddWithValue("@ImageObject", newDrawingViewer.ImageObject);
                command.Parameters.AddWithValue("@DrawingObjectType", newDrawingViewer.DrawingObjectType);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }

        public  int UpdatePermitsDrawingViewer(string PermitNo,string TempPermitNo,int? ModifiedBy=0)
        {
            int newId;
            const string sql = StoredProcedures.Update_PermitDrawingViewer;
            using (var command = new SqlCommand(sql))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@TempPermitNo", TempPermitNo);
                command.Parameters.AddWithValue("@PermitNo", PermitNo);
                command.Parameters.AddWithValue("@ModifiedBy", ModifiedBy);
                command.Parameters.Add("@newId", SqlDbType.Int);
                command.Parameters["@newId"].Direction = ParameterDirection.Output;
                newId = _sqlHelper.ExecuteNonQuery(command, "@newId");
            }
            return newId;
        }
        public  List<DrawingViewer> GetDrawingViewer()
        {
            return _commonRepository.GetTableData<DrawingViewer>(StoredProcedures.Get_DrawingViewer);
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCF.BDO;
using HCF.DAL;

namespace HCF.BAL
{
    public class TIcraProjectService : ITIcraProjectService
    {
        private readonly ITIcraProjectRepository _icraProjectRepository;
        public TIcraProjectService(ITIcraProjectRepository icraProjectRepository)
        {
            _icraProjectRepository = icraProjectRepository;
        }
        #region TIcra Crud

        public async Task<List<TIcraProject>> GetAllTIcraProject()
        {
            return await _icraProjectRepository.GetAllTIcraProject();
        }

       

        public List<TIcraProject> GetAllActiveTIcraProject(bool? HideInActive = true)
        {
            return _icraProjectRepository.GetAllActiveTIcraProject(HideInActive.Value);
        }
        public List<TIcraProject> GetCountAllActiveTIcraProject(int UserID, int? vendorID, bool? HideInActive = true)
        {
            return _icraProjectRepository.GetCountAllActiveTIcraProject(UserID, vendorID, HideInActive.Value);
        }


        public async Task<TIcraProject> GetTIcraProject(int ProjectId)
        {
            var data = await _icraProjectRepository.GetAllTIcraProject();

            //var objproject = data.FirstOrDefault(x => x.ProjectId == ProjectId);
            //if (!string.IsNullOrEmpty(objproject.TFileIds))
            //    objproject.Attachments = CommonRepository.GetTFiles(objproject.TFileIds);

            //if (!string.IsNullOrEmpty(objproject.TDrawingFields))
            //    objproject.DrawingAttachments = CommonRepository.GetUploadedDrawings(objproject.TDrawingFields);
            return await Task.FromResult(data.FirstOrDefault(x => x.ProjectId == ProjectId));

        }
        public bool DeleteProjectFiles(int ProjectId, string fileIds, int type)
        {
            return _icraProjectRepository.DeleteProjectFiles(ProjectId, fileIds, type);
        }
        public int CrudTIcraProject(TIcraProject item, int mode)
        {
            return _icraProjectRepository.CrudTIcraProject(item, mode);
        }

        #endregion
    }
}

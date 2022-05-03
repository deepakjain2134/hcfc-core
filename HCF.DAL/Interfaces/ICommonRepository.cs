using HCF.BDO;
using System;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface ICommonRepository
    {
        List<Audit> GetAuditConfiguration();       
        Organization GetMasterOrg(Guid orgKey);       
        List<TFiles> GetRecentFiles(int userId);
        List<T> GetTableData<T>(string sql);
        List<T> GetTableDataCommon<T>(string sql);      
        List<DrawingpathwayFiles> GetUploadedDrawings(string files);

        List<TFiles> FindUploadeFiles(string key, string fileName, int userId);
    }
}
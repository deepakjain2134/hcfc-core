using HCF.Module.LuceneSearch.Model;
using HCF.Module.LuceneSearch.Models;
using System.Collections.Generic;

namespace HCF.Module.LuceneSearch.Services
{
    public interface IGoLucene
    {      

        bool GetDirectory(string path);
        void AddUpdateLuceneIndex<T>(IEnumerable<T> sampleDatas) where T : new();
        void AddUpdateLuceneIndex<T>(T sampleData) where T : new();
        bool ClearLuceneIndex();
        void ClearLuceneIndexRecord(int record_id);
        IEnumerable<T> GetAllIndexRecords<T>() where T : new();
        void Optimize();
        IEnumerable<T> Search<T>(string input, LuceneFileDirectory fileDirectory, string fieldName = "") where T : new();
        IEnumerable<T> SearchDefault<T>(string input, LuceneFileDirectory fileDirectory,string fieldName = "") where T : new();       
    }
}
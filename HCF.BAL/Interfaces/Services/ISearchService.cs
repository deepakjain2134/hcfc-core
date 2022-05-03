using System;
using System.Collections.Generic;
using HCF.BDO;

namespace HCF.BAL
{
    public interface ISearchService
    {
        SampleData Get(int id);
        List<SampleData> GetAll();
        List<SampleData> GetILSM();
        SearchFilter GetSearchFilter(Guid filterId);
        List<SearchFilter> GetSearchFilter(int userId);
        SearchFilter GetSearchResult(SearchFilter objSearchFilter);
        int Save(SearchFilter objSearchFilter);
    }
}
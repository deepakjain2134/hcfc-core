using HCF.BDO;
using System;
using System.Collections.Generic;

namespace HCF.DAL
{
    public interface ISearchRepository
    {
        int AddSearchFilter(SearchFilter objSearchFilter);
        SearchFilter GetSearchFilter(Guid filterId);
        List<SearchFilter> GetSearchFilter(int userId);
        SearchFilter GetSearchResult(SearchFilter objSearchFilter);
    }
}
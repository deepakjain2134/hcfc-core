using HCF.Module.LuceneSearch.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Module.LuceneSearch.Areas.LuceneSearch.ViewModel
{
	public class SearchIndexViewModel
	{
		public int Limit { get; set; }
		public bool SearchDefault { get; set; }
		public SampleData SampleData { get; set; }
		public IEnumerable<SampleData> AllSampleData { get; set; }
		public IEnumerable<SampleData> SearchIndexData { get; set; }
		public IList<SelectedList> SearchFieldList { get; set; }
		public string SearchTerm { get; set; }
		public string SearchField { get; set; }
	}
}

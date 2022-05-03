//using System.Collections.Generic;
//using System.Web.Mvc;
//using Unity;
//using IActionFilter = System.Web.Mvc.IActionFilter;

//namespace HCF.Web.Filters
//{
//    public class FilterProvider : IFilterProvider
//    {
//        private readonly IUnityContainer container;

//        public FilterProvider(IUnityContainer container)
//        {
//            this.container = container;
//        }

//        public IEnumerable<Filter> GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
//        {
//            foreach (IActionFilter actionFilter in this.container.ResolveAll<IActionFilter>())
//            {
//                yield return new Filter(actionFilter, System.Web.Mvc.FilterScope.First, null);
//            }
//        }
//    }

//    public interface IFilterProvider
//    {

//    }
//}
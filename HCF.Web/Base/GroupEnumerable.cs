//using HCF.BDO;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//namespace HCF.Web.Base
//{
//    public static class GroupEnumerable
//    {
//        //public static IList<BinderFolders> BuildTree(this IEnumerable<BinderFolders> source)
//        //{
//        //    var groups = source.GroupBy(i => i.ParentFolderId);

//        //    var roots = groups.FirstOrDefault(g => g.Key.HasValue == false).ToList();

//        //    if (roots.Count > 0)
//        //    {
//        //        var dict = groups.Where(g => g.Key.HasValue).ToDictionary(g => g.Key.Value, g => g.ToList());
//        //        for (int i = 0; i < roots.Count; i++)
//        //            AddChildren(roots[i], dict);
//        //    }

//        //    return roots;
//        //}

//        //private static void AddChildren(BinderFolders node, IDictionary<int, List<BinderFolders>> source)
//        //{
//        //    if (source.ContainsKey(node.FolderId))
//        //    {
//        //        node.ChildBinderFolders = source[node.FolderId];
//        //        for (int i = 0; i < node.ChildBinderFolders.Count; i++)
//        //            AddChildren(node.ChildBinderFolders[i], source);
//        //    }
//        //    else
//        //    {
//        //        node.ChildBinderFolders = new List<BinderFolders>();
//        //    }
//        //}
//    }
//}
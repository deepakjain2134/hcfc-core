//using System;
//using System.Web.Mvc;


//namespace HCF.Web.Filters
//{
//    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
//    public sealed class InitializeSimpleMembershipAttribute : ActionFilterAttribute
//    {     

//        public override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            // Ensure ASP.NET Simple Membership is initialized only once per app start
//           // LazyInitializer.EnsureInitialized(ref _initializer, ref _isInitialized, ref _initializerLock);
//        }

//        //private class SimpleMembershipInitializer
//        //{
//        //    public SimpleMembershipInitializer()
//        //    {
//        //        Database.SetInitializer<UsersContext>(null);

//        //        try
//        //        {
//        //            using (var context = new UsersContext())
//        //            {
//        //                if (!context.Database.Exists())
//        //                {
//        //                    // Create the SimpleMembership database without Entity Framework migration schema
//        //                    ((IObjectContextAdapter)context).ObjectContext.CreateDatabase();
//        //                }
//        //            }

//        //            WebSecurity.InitializeDatabaseConnection("HCFConnection", "UserProfile", "UserId", "UserName", autoCreateTables: true);
//        //        }
//        //        catch (Exception ex)
//        //        {
//        //            throw new InvalidOperationException("The ASP.NET Simple Membership database could not be initialized. For more information, please see http://go.microsoft.com/fwlink/?LinkId=256588", ex);
//        //        }
//        //    }
//        //}
//    }
//}

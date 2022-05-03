//using Newtonsoft.Json;
//using System.Web.Mvc;

//namespace HCF.Web.Helpers
//{
//    public class AsyncLoader
//    {
//        private static JsonSerializerSettings CreateSerializerSettings()

//        {

//            var serializerSettings = new Newtonsoft.Json.JsonSerializerSettings();

//            serializerSettings.MaxDepth = 5;

//            serializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

//            serializerSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;

//            serializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

//            serializerSettings.ObjectCreationHandling = Newtonsoft.Json.ObjectCreationHandling.Reuse;

//            serializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore;

//            return serializerSettings;

//        }

//        private static string ObjectToJason(object obj)

//        {

//            var json = "null";

//            if (obj == null)

//                return json;

//            try

//            {

//                var serializerSettings = CreateSerializerSettings();

//                json = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None, serializerSettings);

//            }

//            catch

//            {

//                //log this

//            }

//            return json;

//        }

//        public static MvcHtmlString Render(string controller, string action, string placeHolder)
//        {
//            return Render(controller, action, placeHolder, null);

//        }

//        public static MvcHtmlString Render(string controller, string action, string placeHolder, dynamic model)
//        {
//            var modelData = ObjectToJason(model);
//            var html = $@"<script>$(document).ready(function(){{callGetAjax('{ placeHolder}','/{ controller}/{ action}', { modelData}); }});</script>";
//            return MvcHtmlString.Create(html);
//        }

//        public static MvcHtmlString Action(string controller, string Action, string placeHolder, string link)
//        {
//            return AsyncLoader.Action(controller, Action, placeHolder, link, null);
//        }

//        public static MvcHtmlString Action(string controller, string action, string placeHolder, string Link, dynamic Model)
//        {
//            var model = ObjectToJason(Model);
//            var html = $@"<script>$('#{Link}').click(function(){{ window.async.getFromController('/{controller}/{action}', '{placeHolder}', {model}); }});</script>";
//            return MvcHtmlString.Create(html);

//        }

//        public static MvcHtmlString Post(string controller, string action, string placeHolder)
//        {
//            var html = $@"<script>window.async.postToController('/{ controller}/{ action}', '{ placeHolder}'); </script>";
//            return MvcHtmlString.Create(html);

//        }     

//    }
//}
//using System.Web.Mvc;
//using System;
//using Microsoft.Ajax.Utilities;
//using HCF.Utility;

//namespace HCF.Helpers
//{
//    public static class Helpers
//    {
//        public static MvcHtmlString Image(this HtmlHelper htmlHelper, string source, string alternativeText, string cssClass)
//        {
//            var bucketName = Convert.ToString(Web.Base.UserSession.CurrentOrg.ClientNo);
//            var imageBasePath = AppSettings.ImageBasePath + bucketName + "/";
//            var newSource = $"{imageBasePath}{source?.Replace("~/", "")}";
//            var builder = new TagBuilder("image");
//            builder.MergeAttribute("src", newSource.ToLower());

//            if (!string.IsNullOrEmpty(alternativeText))
//                builder.MergeAttribute("alt", alternativeText);


//            if (!string.IsNullOrEmpty(cssClass))
//                builder.MergeAttribute("Class", cssClass);

//            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
//        }

//        public static MvcHtmlString ClinetLogo(this HtmlHelper htmlHelper, string source, string alternativeText, string baseUrl, string cssClass, int clientNo)
//        {
//            var bucketName = Convert.ToString(clientNo);
//            var imageBasePath = AppSettings.ImageBasePath + bucketName + "/";
//            var newSource = $"{imageBasePath}{source.Replace("~/", string.Empty)}";
//            var builder = new TagBuilder("image");
//            builder.MergeAttribute("src", newSource.ToLower());

//            if (!string.IsNullOrEmpty(alternativeText))
//                builder.MergeAttribute("alt", alternativeText);


//            if (!string.IsNullOrEmpty(cssClass))
//                builder.MergeAttribute("Class", cssClass);

//            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
//        }

//        public static MvcHtmlString IsInRole(this MvcHtmlString value, string role)
//        {
//            var status = Web.Base.UserSession.IsInRole(role);
//            return status ? value : MvcHtmlString.Empty;
//        }

//        public static MvcHtmlString CommonImage(this HtmlHelper htmlHelper, string source, string alternativeText, string cssClass)
//        {
//            var imageBasePath = AppSettings.CommonFileBasePath;
//            var builder = new TagBuilder("image");
//            if (!String.IsNullOrEmpty(source))
//            {
//                var newSource = $"{imageBasePath}{source.Replace("~/", string.Empty)}";
//                builder.MergeAttribute("src", newSource.ToLower());
//            }

//            if (!string.IsNullOrEmpty(alternativeText))
//                builder.MergeAttribute("alt", alternativeText);

//            if (!string.IsNullOrEmpty(cssClass))
//                builder.MergeAttribute("Class", cssClass);

//            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
//        }

//        public static MvcHtmlString JsMinify(this HtmlHelper helper, Func<object, object> markup)
//        {
//            var notMinifiedJs =
//             markup.Invoke(helper.ViewContext)?.ToString() ?? "";

//            var minifier = new Minifier();
//            var minifiedJs = minifier.MinifyJavaScript(notMinifiedJs, new CodeSettings
//            {
//                EvalTreatment = EvalTreatment.MakeImmediateSafe,
//                PreserveImportantComments = false
//            });
//            return new MvcHtmlString(minifiedJs);
//        }

//        public static MvcHtmlString AddImage(this HtmlHelper helper, string id, string title, string src, string className)
//        {
//            var builder = new TagBuilder("img");
//            builder.MergeAttribute("alt", title);
//            builder.MergeAttribute("title", title);

//            if (!string.IsNullOrEmpty(className))
//                builder.MergeAttribute("class", className);

//            builder.MergeAttribute("src", !string.IsNullOrEmpty(src) ? src : "/dist/Images/Icons/3d_add_button.png");

//            if (!string.IsNullOrEmpty(id))
//                builder.MergeAttribute("id", id);

//            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
//        }

//        public static MvcHtmlString ImageCtr(this HtmlHelper helper, string src, string altText, string height)
//        {
//            var builder = new TagBuilder("img");
//            builder.MergeAttribute("src", src);
//            builder.MergeAttribute("alt", altText);
//            builder.MergeAttribute("height", height);
//            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
//        }
//    }
//}
namespace HCF.Web.Base
{
    public interface IHttpPostFactory
    {
        string CallGetMethod(string url, ref int statusCode);
        string CallPostMethod(string postData, string apiUrl, ref int statusCode, bool isCompleteUrl = false);       
        string CallGetMethodAnonymous(string url, ref int statusCode);


    }
}
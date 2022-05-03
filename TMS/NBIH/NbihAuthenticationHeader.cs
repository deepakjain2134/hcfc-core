using System.Reflection;

namespace TMS.NBIH
{
   public static class NbihAuthenticationHeader
    {       

        public static T GetHeader<T>(object obj)
        {
           
            string property1 = "UserName";
            string property2 = "Password";
            string value1 = "Smathur";
            string value2 = "Apr2022";  

            PropertyInfo prop1 = obj.GetType().GetProperty(property1, BindingFlags.Public | BindingFlags.Instance);
            if (prop1 != null && prop1.CanWrite)
                prop1.SetValue(obj, value1, null);


            PropertyInfo prop2 = obj.GetType().GetProperty(property2, BindingFlags.Public | BindingFlags.Instance);
            if (prop2 != null && prop2.CanWrite)
                prop2.SetValue(obj, value2, null);

            return (T)obj;
        }     
    }
}

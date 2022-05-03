namespace HCF.Web.Base
{
    public static class DirectoryService
    {
        private static bool CheckDirectoryExist(string directoryname)
        {
            bool exists = false;//System.IO.Directory.Exists(System.Web.Hosting.HostingEnvironment.MapPath(directoryname));
            return exists;
        }
        public static void CreateDirectory(string fileName)
        {
            bool exists = CheckDirectoryExist("//temp//");
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(fileName);
            }
        }
    }
}
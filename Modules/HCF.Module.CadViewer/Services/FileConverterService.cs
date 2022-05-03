using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace HCF.Module.CadViewer.Services
{
    public class FileConverterService : IFileConverterService
    {
        private readonly IConfiguration _config; private readonly IWebHostEnvironment _env;
        public FileConverterService(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config; 
            _env = env;

        }
        public Dictionary<string, string> CallApiConversion(string fileName, string base64Image)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            try
            {
                string converterLocation = _config.GetValue<string>("CADViewer:converterLocation");
                string newfileName = fileName.Split(".")[0] + ".svg";
                var newFileBase64Image = string.Empty;
                string sourceFilePath = _env.ContentRootPath + "\\wwwroot\\temp" + "\\" + fileName;
                string outputPathPath = _env.ContentRootPath + "\\wwwroot\\temp" + "\\files\\" + newfileName;
                // save base64 to local dic
                File.WriteAllBytes(sourceFilePath, Convert.FromBase64String(base64Image));
                int exitCode = 0;
                string arguments = string.Format("D:/Projects/cadviewer-testapp-dotnet-core-01-master/cadviewer/wwwroot/converters/ax2020/windows/AX2022_W64_22_07_54.exe \"-i=\"" + sourceFilePath + "\"\" \"-o=\"" + outputPathPath + "\"\" \"-lpath =D:/Projects/cadviewer-testapp-dotnet-core-01-master/cadviewer/wwwroot/converters/ax2020/windows/\" \"-f=\"svg\"\" -last \"-fpath=\"D:/Projects/cadviewer-testapp-dotnet-core-01-master/cadviewer/wwwroot/converters/ax2020/windows/fonts/\"\"");
                ProcessStartInfo ProcessInfo;
                ProcessInfo = new ProcessStartInfo(converterLocation + "\\run_ax2020.bat", arguments);


                ProcessInfo.CreateNoWindow = true;
                ProcessInfo.UseShellExecute = false;    // false -> true
                ProcessInfo.WorkingDirectory = converterLocation;
                // *** Redirect the output ***
                ProcessInfo.RedirectStandardError = true;
                ProcessInfo.RedirectStandardOutput = true;
                Process myProcess;

                myProcess = Process.Start(ProcessInfo);

                // *** Read the streams ***
                string output = myProcess.StandardOutput.ReadToEnd();
                string error = myProcess.StandardError.ReadToEnd();

                // convert  outputPath file to base 64  and save to newFileBase64Image

                myProcess.WaitForExit();

                exitCode = myProcess.ExitCode;
                // delete both local file 

                if (File.Exists(outputPathPath))
                {
                    Byte[] bytes = System.IO.File.ReadAllBytes(outputPathPath);
                    newFileBase64Image = Convert.ToBase64String(bytes);
                    data.Add("FileName", newFileBase64Image);
                    data.Add("FileContent", newFileBase64Image);
                    File.Delete(outputPathPath);
                    File.Delete(sourceFilePath);
                }
            }
            catch (Exception ex)
            {

            }
            return data;
        }

        private static string DecodeUrlString(string url)
        {
            string newUrl;
            while ((newUrl = Uri.UnescapeDataString(url)) != url)
                url = newUrl;
            return newUrl;
        }
    }
}

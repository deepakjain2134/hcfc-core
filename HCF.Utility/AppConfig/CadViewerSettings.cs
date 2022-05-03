using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCF.Utility.AppConfig
{
    public class CadViewerSettings
    {
        public string ServerUrl { get; set; }
        public string FileLocationUrl { get; set; }
        public string CallbackMethod { get; set; }
        public string ServerLocation { get; set; }
        public string FileLocation { get; set; }
        public string ConverterLocation { get; set; }
        public string LicenseLocation { get; set; }

        public string XpathLocation { get; set; }
        public string Ax2020_Executable { get; set; }
        public string Cvjs_Debug { get; set; }
    }
}

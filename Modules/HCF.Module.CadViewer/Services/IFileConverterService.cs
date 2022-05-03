using System.Collections.Generic;

namespace HCF.Module.CadViewer.Services
{
    public interface IFileConverterService
    {
        Dictionary<string, string> CallApiConversion(string fileName, string base64String);
    }
}

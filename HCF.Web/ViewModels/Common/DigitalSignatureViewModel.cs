using System;
using System.Collections.Generic;
using HCF.BDO;

namespace HCF.Web.ViewModels
{
    public class DigitalSignatureViewModel
    {
     public  IList<string> stringarray { get; set; }
     public   DigitalSignature signs { get; set; }
     public string MainSignatureClass { get; set; }
     public string ImgSignatureClass { get; set; }       
     public int ApprovalStatus { get; set; }
        public string SignatureControlId { get; set; }
        
    public string HiddenFileControl { get; set; }
    public string HiddenFileName { get; set; }
    public string HiddenFileContent { get; set; }
        public int? SignSequence { get; set; }
        public int? SignIndex { get; set; }
    }
}
using System;


namespace HCF.Web.Filters
{
    public class ActionValidateAttribute : Attribute
    {
        private readonly bool _isRequired;

        public ActionValidateAttribute(bool isRequired)
        {
            _isRequired = isRequired;
        }

        public bool IsRequired { get { return _isRequired; } }
    }
}
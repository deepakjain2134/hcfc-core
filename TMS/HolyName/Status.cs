using System.Collections.Generic;

namespace TMS
{
    public static class Status
    {
        public static List<HCF.BDO.WoStatus> GetTmsStatus(string modulecode)
        {
            List<HCF.BDO.WoStatus> statuslist = new List<HCF.BDO.WoStatus>();
            HolyNameTmsService.HolyNameStatusClient client = new HolyNameTmsService.HolyNameStatusClient();
            HolyNameTmsService.HolyNameSubStatusClient sclient = new HolyNameTmsService.HolyNameSubStatusClient();
            HolyNameTmsService.Status[] lists = client.GetCodesStatus(modulecode, 11, true);
            foreach (HolyNameTmsService.Status list in lists)
            {
                HolyNameTmsService.SubStatus[] subStatus = sclient.GetCodesSubStatusbyStatus(modulecode, 11, true, list.codeField);
                HCF.BDO.WoStatus status = new HCF.BDO.WoStatus();
                status.Code = list.codeField;
                status.Name = list.descriptionField;
                status.ModuleCode = list.moduleCodeField;
                status.Description = list.descriptionField;
                status.IsActive = true;
                List<HCF.BDO.SubStatus> SubStatus = new List<HCF.BDO.SubStatus>();
                foreach (var item in subStatus)
                {
                    HCF.BDO.SubStatus substatus = new HCF.BDO.SubStatus();
                    substatus.Code = item.codeField;
                    substatus.Name = list.descriptionField;                   
                    substatus.Description = item.descriptionField;
                    SubStatus.Add(substatus);
                }
                status.SubStatus = SubStatus;
                statuslist.Add(status);
            }
            return statuslist;
        }


        public static List<HCF.BDO.WoStatus> GetBrukeTmsStatus(string modulecode)
        {
            List<HCF.BDO.WoStatus> statuslist = new List<HCF.BDO.WoStatus>();
            HolyNameTmsService.BurkeStatusClient client = new HolyNameTmsService.BurkeStatusClient();
            HolyNameTmsService.BurkeSubStatusClient sclient = new HolyNameTmsService.BurkeSubStatusClient();
            HolyNameTmsService.Status1[] lists = client.GetBurkeCodesStatus(modulecode, 1, true);
            foreach (HolyNameTmsService.Status1 list in lists)
            {
                HolyNameTmsService.SubStatus1[] subStatus = sclient.GetBurkeCodesSubStatusbyStatus(modulecode, 1, true, list.codeField);
                HCF.BDO.WoStatus status = new HCF.BDO.WoStatus();
                status.Code = list.codeField;
                status.Name = list.descriptionField;
                status.ModuleCode = list.moduleCodeField;
                status.Description = list.descriptionField;
                status.IsActive = true;
                List<HCF.BDO.SubStatus> SubStatus = new List<HCF.BDO.SubStatus>();
                foreach (var item in subStatus)
                {
                    HCF.BDO.SubStatus substatus = new HCF.BDO.SubStatus();
                    substatus.Code = item.codeField;
                    substatus.Name = list.descriptionField;
                    substatus.Description = item.descriptionField;
                    SubStatus.Add(substatus);
                }
                status.SubStatus = SubStatus;
                statuslist.Add(status);
            }
            return statuslist;
        }

    }
}

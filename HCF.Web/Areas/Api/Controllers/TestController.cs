using HCF.BAL;
using HCF.BDO;
using HCF.BDO.NBIH;
using HCF.Web.Areas.Api.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace HCF.Web.Areas.Api.Controllers
{   
    [Route("api/test")]  
    [ApiController]
    public class TestApiController : ApiController
    {
        private readonly ITmsConnectorService _tmsConnectorService;
        public TestApiController(ITmsConnectorService tmsConnectorService)
        {
            _tmsConnectorService = tmsConnectorService;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getTmsAssets")]
        public void GetTmsAssets(string days)
        {
            int day = 0;

            if (!string.IsNullOrEmpty(days))
                day = Convert.ToInt32(days);

            List<AssetType> assetsTypes = new List<AssetType>();
            var _objHttpResponseMessage = new HttpResponseMessage();
            List<TMSNBIAssetsResults> TMSAssetsResults = TMS.NBIH.Assets.GetTmsAssets(day);
            foreach (var item in TMSAssetsResults)
            {
                _tmsConnectorService.SaveAssetsTempTable(item);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getTmsWorkOrder")]
        public void GetTmsWorkOrder(string days)
        {
            int day = 0;

            if (!string.IsNullOrEmpty(days))
                day = Convert.ToInt32(days);

            var _objHttpResponseMessage = new HttpResponseMessage();
            List<TMSNBIWorkOrderResults> TMSNBIWorkOrderResults = TMS.NBIH.WorkOrders.GetTmsWorkOrder(day);

            foreach (var item in TMSNBIWorkOrderResults)
            {
                _tmsConnectorService.SaveWorkOrderTempTable(item);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getTmsBuildings")]
        public void GetTmsBuildings()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            List<TMSNBIBuildingsResults> TMSBuildingsResults = TMS.NBIH.Buildings.GetTmsBuildings();
            //var json = JsonConvert.SerializeObject(result);
            foreach (var item in TMSBuildingsResults)
            {
                _tmsConnectorService.SaveBuildingsTempTable(item);
            }

        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getTmsCategory")]
        public void GetTmsCategory()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var result = TMS.NBIH.Category.GetTmsCategory("WO");
            //foreach (var item in result)
            //{
            //    _tmsConnectorService.SaveAssetsTempTable(item);
            //}
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getTmsPriority")]      
        public ActionResult GetTmsPriority()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            List<TMSNBIPriorityResults> TMSPriorotyResults = TMS.NBIH.Priority.GetTmsPriorities("WO");
            //var json = JsonConvert.SerializeObject(result);
            //foreach (var item in TMSPriorotyResults)
            //{
            //    _tmsConnectorService.SavePriorityTempTable(item);
            //}
            return Ok(_objHttpResponseMessage);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getTmsResource")]
        public void GetTmsResource(string days)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            int day = 0;
            if (!string.IsNullOrEmpty(days))
                day = Convert.ToInt32(days);
            List<TMSNBIResourceResults> TMSResourceResults = TMS.NBIH.Resource.GetmsResources(day);
            //var result = TMS.NBIH.Resource.GetmsResources(day);
            //var json = JsonConvert.SerializeObject(result);
            foreach (var item in TMSResourceResults)
            {
                _tmsConnectorService.SaveResourcesTempTable(item);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getTmsSkills")]
        public void GetTmsSkills()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var result = TMS.NBIH.Skill.GetTmsSkill();
            var json = JsonConvert.SerializeObject(result);
            //foreach (var item in result)
            //{
            //    _tmsConnectorService.SaveAssetsTempTable(item);
            //}
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getTmsStatus")]
        public void GetTmsStatus()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            List<TMSNBIStatusResults> TMSStatusResults = TMS.NBIH.Status.GetTmsStatus("WO");
            foreach (var item in TMSStatusResults)
            {
                _tmsConnectorService.SaveStatusTempTable(item);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getTmsSubCategory")]
        public void GetTmsSubCategory()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var result = TMS.NBIH.SubCategory.GetTmsSubCategory("WO", 51, true);
            //foreach (var item in result)
            //{
            //    _tmsConnectorService.SaveAssetsTempTable(item);
            //}
        }


        //public void GetTmsLocations()
        //{
        //    var _objHttpResponseMessage = new HttpResponseMessage();            
        //    var result = TMS.NBIH.Locations.GetTmsLocations();
        //    var json = JsonConvert.SerializeObject(result);
        //}

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getTmsAccounts")]
        public void GetTmsAccounts()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            List<TMSNBIAccountResults> TMSNBIAccountResults = TMS.NBIH.Accounts.GetTmsAccounts();
            //var json = JsonConvert.SerializeObject(result);
            foreach (var item in TMSNBIAccountResults)
            {
                _tmsConnectorService.SaveAccountTempTable(item);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getTmsSites")]
        public void GetTmsSites()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            List<TMSNBISitesResults> TMSNBISitesResults = TMS.NBIH.Sites.GetTmsSites();
            //var json = JsonConvert.SerializeObject(result);
            foreach (var item in TMSNBISitesResults)
            {
                _tmsConnectorService.SaveSitesTempTable(item);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getTmsTypes")]
        public void GetTmsTypes(string modulecode)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            List<TMSNBITypeResults> TMSNBITypeResults = TMS.NBIH.Type.GetTmsTypes(modulecode);
            //var json = JsonConvert.SerializeObject(result);
            foreach (var item in TMSNBITypeResults)
            {
                _tmsConnectorService.SaveTypeTempTable(item);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getTmsSubStatus")]
        public void GetTmsSubStatus(string modulecode)
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            List<TMSNBISubStatusResults> TMSNBISubStatusResults = TMS.NBIH.SubStatus.GetTmsSubStatus(modulecode);
            //var json = JsonConvert.SerializeObject(result);
            foreach (var item in TMSNBISubStatusResults)
            {
                _tmsConnectorService.SaveSubStatusTempTable(item);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getTmsLocations")]
        public void GetTmsLocations()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            List<TMSNBISLocationResults> TMSNBISLocationResults = TMS.NBIH.Locations.GetTmsLocations();
            //var json = JsonConvert.SerializeObject(result);
            foreach (var item in TMSNBISLocationResults)
            {
                _tmsConnectorService.SaveLocationTempTable(item);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getTmsRequester")]
        public void GetTmsRequester()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            List<TMSNBISRequesterResults> TMSNBISRequesterResults = TMS.NBIH.Requester.GetTmsRequester();
            //var json = JsonConvert.SerializeObject(result);
            foreach (var item in TMSNBISRequesterResults)
            {
                _tmsConnectorService.SaveRequesterTempTable(item);
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("getTmsDepartment")]
        public void GetTmsDepartment()
        {
            var _objHttpResponseMessage = new HttpResponseMessage();
            var result = TMS.NBIH.Departments.GetDepartments();
            var json = JsonConvert.SerializeObject(result);
            //foreach (var item in TMSNBISRequesterResults)
            //{
            //    _tmsConnectorService.SaveRequesterTempTable(item);
            //}
        }
    }
}
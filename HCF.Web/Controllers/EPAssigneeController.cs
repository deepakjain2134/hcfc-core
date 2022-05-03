using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HCF.Web.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using HCF.BAL;
   
namespace HCF.Web.Controllers
{
    [Authorize]
    public class EPAssigneeController : BaseController
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IUserService _userService;
        public EPAssigneeController(IHostingEnvironment hostingEnvironment, IUserService userService)
        {
            _hostingEnvironment = hostingEnvironment;
            _userService = userService;
        }
        public IActionResult Index()
        {
            UISession.AddCurrentPage("EPAssignee_Index", 0, "EP Assignments");
            return View();
        }

        public ActionResult EPUploadPopUp()
        {
            return PartialView("EPUploadPopUp");
        }


        [HttpPost]
        public IActionResult EPUploadPopUp(IFormFile postedFile)
        {
            var users = _userService.GetUsers().ToList();
            if (postedFile != null)
            {
                string path = Path.Combine(this._hostingEnvironment.WebRootPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = Path.GetFileName(postedFile.FileName);
                string filePath = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                string csvData = System.IO.File.ReadAllText(filePath);
                DataTable dt = new DataTable();
                bool firstRow = true;
                foreach (string row in csvData.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            if (firstRow)
                            {
                                Regex regx = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                                foreach (string cell in regx.Split(row))
                                {
                                    dt.Columns.Add(cell.Replace("\"", "").Trim());
                                }
                                firstRow = false;
                            }
                            else
                            {
                                dt.Rows.Add();
                                int i = 0;
                                Regex regx = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
                                foreach (string cell in regx.Split(row))
                                {
                                    dt.Rows[dt.Rows.Count - 1][i] = cell.Replace("\"", "").Trim(); //cell.Trim();
                                    i++;
                                }
                            }
                        }
                    }
                }
                DataTable tblFiltered = dt.AsEnumerable().Where(row => !string.IsNullOrEmpty(row.Field<String>("Email(s)"))).CopyToDataTable();
                List<String> allEmails = new List<string>();
                foreach (DataRow row in tblFiltered.Rows)
                {
                    List<string> email = ((string)row["Email(s)"]).Split(',').Select(p => p.Trim()).ToList();
                    allEmails.AddRange(email);
                }
                var distinctlist = allEmails.Distinct().ToList();
                var nothavingemails = distinctlist.Where(x => !users.Select(x => x.Email).Contains(x)).ToList();
                tblFiltered.Columns.Add("UserIds", typeof(string));
                foreach (DataRow row in tblFiltered.Rows)
                {
                    List<string> emails = ((string)row["Email(s)"]).Split(',').Select(p => p.Trim()).ToList();
                    var userids = users.Where(t => emails.Contains(t.Email)).Select(x => x.UserId);                    
                    row["UserIds"] = String.Join(",", userids);
                }
                ViewData["nothavingemails"] = String.Join(",", nothavingemails);
                return PartialView("_epUploadView", tblFiltered);
            }
            return View();
        }
    }
}

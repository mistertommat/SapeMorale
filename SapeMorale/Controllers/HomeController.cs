using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Data.Entity;
using SapeMoraleModel.Models;
//using SapeMoraleModel;
using System.Net;

namespace SapeMorale.Controllers
{
    public class HomeController : Controller
    {
        private bool _moraleRecDeleted;

        public ActionResult Index(string fingerprint = "")
        {
            if (fingerprint != string.Empty)
            {
                ViewBag.UserMoraleForToday = GetUserMorale();
            }
            else
            {
                ViewBag.PageMode = "GetFingerprint";
            }
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetMorale(string fingerprint)
        {
            string userMorale = GetUserMorale(fingerprint);
            return Json(new { success = true, morale = userMorale });
            
        }

        [HttpPost]
        public async Task<JsonResult> SubmitHigh(string fingerprint)
        {            
            if (await SubmitMorale("H", fingerprint))
            {
                return Json(new { success = true, msg = _moraleRecDeleted ? "Deleted" : "Submitted" });
            }
            else
            {
                return Json(new { success = false, msg = "Failed" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> SubmitHighish()
        {
            if (await SubmitMorale("MH"))
            {
                return Json(new { success = true, msg = _moraleRecDeleted ? "Deleted" : "Submitted" });
            }
            else
            {
                return Json(new { success = false, msg = "Failed" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> SubmitMedium()
        {
            if (await SubmitMorale("M"))
            {
                return Json(new { success = true, msg = _moraleRecDeleted ? "Deleted" : "Submitted" });
            }
            else
            {
                return Json(new { success = false, msg = "Failed" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> SubmitLow()
        {
            if (await SubmitMorale("L"))
            {
                return Json(new { success = true, msg = _moraleRecDeleted ? "Deleted" : "Submitted" });
            }
            else
            {
                return Json(new { success = false, msg = "Failed" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> SubmitVeryLow()
        {
            if (await SubmitMorale("VL"))
            {
                return Json(new { success = true, msg = _moraleRecDeleted ? "Deleted" : "Submitted" });
            }
            else
            {
                return Json(new { success = false, msg = "Failed" });
            }
        }

        private string GetUserMorale(string fingerprint = "")
        {
            string strUserIP = string.Empty;
            SapeMoraleDB DBCon = new SapeMoraleDB();
            string UserMorale = string.Empty;

            string IPAddress = Request.UserHostAddress;
            string compName = DetermineCompName(IPAddress);

            strUserIP = string.Concat(Request.UserHostAddress, " | ", compName);
            
            try
            {
                sape_morale moraleRec = DBCon.sape_morale.Where(m => DbFunctions.TruncateTime(m.LASTUPDATEDDATE) == DateTime.Today.Date && m.IPADDRESS == strUserIP).FirstOrDefault();
                if (moraleRec != null)
                {
                    UserMorale = moraleRec.MORALE;
                }
                return UserMorale;
            }
            catch
            {
                return UserMorale;
            }
        }

        private async Task<bool> SubmitMorale(string Morale, string fingerprint = "")
        {
            string strUserIP = string.Empty;
            SapeMoraleDB DBCon = new SapeMoraleDB();
            
            _moraleRecDeleted = false;
            string IPAddress = Request.UserHostAddress;
            //string compName = DetermineCompName(IPAddress);

            strUserIP = string.Concat(Request.UserHostAddress, " | ", fingerprint);
            
            try
            {
                sape_morale moraleRec = DBCon.sape_morale.Where(m => DbFunctions.TruncateTime(m.LASTUPDATEDDATE) == DateTime.Today.Date && m.IPADDRESS == strUserIP).FirstOrDefault();

                if (moraleRec == null)
                {
                    // new record
                    moraleRec = new sape_morale();
                    moraleRec.IPADDRESS = strUserIP;
                    moraleRec.MORALE = Morale;
                    moraleRec.LASTUPDATEDDATE = DateTime.Now;
                    DBCon.sape_morale.Add(moraleRec);
                }
                else if (moraleRec.MORALE == Morale)
                {
                    // delete
                    DBCon.sape_morale.Remove(moraleRec);
                    _moraleRecDeleted = true;
                }
                else
                { 
                    // update
                    moraleRec.MORALE = Morale;
                    moraleRec.LASTUPDATEDDATE = DateTime.Now;                
                }
                
                int result = await DBCon.SaveChangesAsync();

                if (result == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private string DetermineCompName(string IP)
        {
            IPAddress myIP = IPAddress.Parse(IP);
            IPHostEntry GetIPHost = Dns.GetHostEntry(myIP);
            List<string> compName = GetIPHost.HostName.ToString().Split('.').ToList();
            return compName.First();
        }

    }
}

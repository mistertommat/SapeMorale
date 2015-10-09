using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using System.Data.Entity;
using SapeMorale.Models;
using System.Web.Mvc;


namespace SapeMoraleStatsApi.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public JToken Get()
        {
            JToken json = JObject.Parse("{ 'High_Count' : 1, 'Medium_Count' : 2, 'Low_Count' : 3 }");
            return json;
        }

        private string GetUserMorale()
        {
            string strUserIP = string.Empty;
            SapeMoraleDB DBCon = new SapeMoraleDB();
            string UserMorale = string.Empty;

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
      
    }
}

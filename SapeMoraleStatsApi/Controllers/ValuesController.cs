using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using SapeMoraleModel.Models;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace SapeMoraleStatsApi.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public JToken Get()
        {
            int intMoraleStatsForToday = GetOverallMoraleForToday();
            string result = string.Concat("{ 'OverallMoraleToday' : ", intMoraleStatsForToday, " }");
            JToken json = JObject.Parse(result);
            return json;
        }

        private int GetOverallMoraleForToday()
        {
            testEntities DBCon = new testEntities();
            int UserMorale = 0;

            try
            {                
                morale_stats moraleStats = DBCon.morale_stats.FirstOrDefault();

                if (moraleStats != null)
                {
                    switch (moraleStats.MORALE){
                        case "H":
                            UserMorale = 5;
                            break;
                        case "MH":
                            UserMorale = 4;
                            break;
                        case "M":
                            UserMorale = 3;
                            break;
                        case "L":
                            UserMorale = 2;
                            break;
                        case "VL":
                            UserMorale = 1;
                            break;
                    }
                        
                }
                return UserMorale;
            }
            catch
            {
                return UserMorale;
            }
        }

        private int SaveMorale(string morale, string clientIPAddress)
        {
            int result = 0;
            bool _moraleRecDeleted = false;
            string strUserIP = string.Empty;
            SapeMoraleDB DBCon = new SapeMoraleDB();

            _moraleRecDeleted = false;
            string IPAddress = clientIPAddress;

            strUserIP = string.Concat(IPAddress);

            try
            {
                sape_morale moraleRec = DBCon.sape_morale.Where(m => DbFunctions.TruncateTime(m.LASTUPDATEDDATE) == DateTime.Today.Date && m.IPADDRESS == strUserIP).FirstOrDefault();

                if (moraleRec == null)
                {
                    // new record
                    moraleRec = new sape_morale();
                    moraleRec.IPADDRESS = strUserIP;
                    moraleRec.MORALE = morale;
                    moraleRec.LASTUPDATEDDATE = DateTime.Now;
                    DBCon.sape_morale.Add(moraleRec);
                }
                else if (moraleRec.MORALE == morale)
                {
                    // delete
                    DBCon.sape_morale.Remove(moraleRec);
                    _moraleRecDeleted = true;
                }
                else
                {
                    // update
                    moraleRec.MORALE = morale;
                    moraleRec.LASTUPDATEDDATE = DateTime.Now;
                }

                result = DBCon.SaveChanges();
                
            }
            catch
            {
                return 0;
            }

            return result;
        }

      
    }
}

using LunchCommon.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TodayLunchCore.Controllers
{
    public class Common
    {
        /// <summary>
        /// 사용자 정보 얻어오기
        /// </summary>
        /// <param name="_ownerName"></param>
        /// <returns></returns>
        public async Task<Owner> _GetUserInfoAsync(string _ownerName)
        {
            try
            {
                Owner _owner = new Owner();
                using (HttpClient client = new HttpClient())
                {
                    var response
                        = await client.PostAsync
                        (
                            LunchCommon.PreDefined.ServiceApiUrl + "SelectAPI/GetUserInfo",
                            new StringContent(JsonConvert.SerializeObject(_ownerName), Encoding.UTF8, "application/json")
                        );

                    string content = await response.Content.ReadAsStringAsync();
                    if (response.IsSuccessStatusCode)
                    {
                        _owner = JsonConvert.DeserializeObject<Owner>(content);
                        //return View(model.results);
                        return _owner;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}

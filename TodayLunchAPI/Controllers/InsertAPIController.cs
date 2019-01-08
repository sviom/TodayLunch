using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using LunchLibrary.Models;
using Newtonsoft.Json;

namespace TodayLunchAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/InsertAPI")]
    public class InsertAPIController : Controller
    {
        private AppSettings appSettings { get; set; }

        public InsertAPIController(IOptions<AppSettings> settings)
        {
            appSettings = settings.Value;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// 사용자 생성
        /// </summary>
        /// <param name="ownerName">사용자가 입력한 이름</param>
        /// <returns>생성된 사용자 ID</returns>
        [HttpPost("CreateUser")]
        public int CreateUser([FromBody]string ownerName)
        {
            int userId = -1;
            try
            {
                SqlConnection conn = new SqlConnection(appSettings.DbConnectionString);
                if (conn.State.ToString() != "Open")
                {
                    conn.Open();
                }

                SqlCommand sqlQuery = new SqlCommand("p_create_owner", conn)
                {
                    // 데이터 입력 형식은 저장 프로시저
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter[] parameterList = new SqlParameter[]
                {
                        new SqlParameter() {ParameterName="@ownerName",SqlDbType=SqlDbType.NVarChar,Size=30,Value=ownerName }
                };

                sqlQuery.Parameters.AddRange(parameterList);

                // 실행
                SqlDataReader resultData = sqlQuery.ExecuteReader();
                while (resultData.Read())
                {
                    userId = Convert.ToInt32(resultData["ownerId"]);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return userId;
        }

        /// <summary>
        /// 장소 생성
        /// </summary>
        /// <param name="placeList">장소 목록</param>
        /// <returns>생성된 장소 갯수</returns>
        [HttpPost("CreatePlace")]
        public int CreatePlace([FromBody]List<Place> placeList)
        {
            int createdPlaceCount = -1;
            try
            {
                //List<Place> placeList = JsonConvert.DeserializeObject<List<Place>>(jsonPlaceList);

                SqlConnection conn = new SqlConnection(appSettings.DbConnectionString);
                if (conn.State.ToString() != "Open")
                {
                    conn.Open();
                }

                for (int i = 0; i < placeList.Count; i++)
                {
                    SqlParameter[] parameterList = new SqlParameter[]
                    {
                        new SqlParameter() {ParameterName="@placeOwnerId",SqlDbType=SqlDbType.Int,Value=0 },
                        new SqlParameter() {ParameterName="@placeName",SqlDbType=SqlDbType.NVarChar,Size=50,Value=placeList[i].Name },
                        new SqlParameter() {ParameterName="@placeLocation",SqlDbType=SqlDbType.NVarChar,Size=1000,Value=placeList[i].Location }
                    };

                    SqlCommand sqlQuery = new SqlCommand("p_create_place", conn)
                    {
                        // 데이터 입력 형식은 저장 프로시저
                        CommandType = CommandType.StoredProcedure
                    };

                    sqlQuery.Parameters.AddRange(parameterList);

                    if (Convert.ToInt32(sqlQuery.ExecuteScalar()) >= 0)
                    {
                        // 정상적으로 생성되었으면 
                        createdPlaceCount++;
                    }

                    sqlQuery.Dispose();
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return createdPlaceCount;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LunchLibrary.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Options;

namespace TodayLunchAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/DeleteAPI")]
    public class DeleteAPIController : Controller
    {
        private AppSettings appSettings { get; set; }

        public DeleteAPIController(IOptions<AppSettings> settings)
        {
            appSettings = settings.Value;
        }

        /// <summary>
        /// 장소 아이디를 가지고 장소 삭제
        /// </summary>
        /// <param name="inPlaceId"></param>
        [HttpPost]
        public bool DeletePlace([FromBody]int inPlaceId)
        {
            bool deleteResult = false;
            try
            {
                SqlConnection conn = new SqlConnection(appSettings.DbConnectionString);
                if (conn.State.ToString() != "Open")
                {
                    conn.Open();
                }

                // 고유아이디/이름/나이/성별/중성화상태/사진/상태메모
                SqlCommand sqlQuery = new SqlCommand("p_delete_place", conn)
                {
                    // 데이터 입력 형식은 저장 프로시저
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter[] parameterList = new SqlParameter[]
                {
                        new SqlParameter() {ParameterName="@placeId",SqlDbType=SqlDbType.Int,Value=Convert.ToInt32(inPlaceId) }
                };

                sqlQuery.Parameters.AddRange(parameterList);

                // 실행
                SqlDataReader resultData = sqlQuery.ExecuteReader();
                while (resultData.Read())
                {
                    
                }

                deleteResult = true;

                conn.Close();
            }
            catch
            {
                throw;
            }
            return deleteResult;
        }
    }
}
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
        /// ��� ���̵� ������ ��� ����
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

                // �������̵�/�̸�/����/����/�߼�ȭ����/����/���¸޸�
                SqlCommand sqlQuery = new SqlCommand("p_delete_place", conn)
                {
                    // ������ �Է� ������ ���� ���ν���
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter[] parameterList = new SqlParameter[]
                {
                        new SqlParameter() {ParameterName="@placeId",SqlDbType=SqlDbType.Int,Value=Convert.ToInt32(inPlaceId) }
                };

                sqlQuery.Parameters.AddRange(parameterList);

                // ����
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
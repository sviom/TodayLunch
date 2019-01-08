using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using LunchCommon.Models;
using Newtonsoft.Json;

namespace TodayLunchAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/SelectAPI")]
    public class SelectAPIController : Controller
    {
        private AppSettings appSettings { get; set; }

        public SelectAPIController(IOptions<AppSettings> settings)
        {
            appSettings = settings.Value;
        }

        /// <summary>
        /// ����ڰ� ������ �ִ� ��Ҹ�� ���� ��������
        /// </summary>
        /// <returns></returns>
        [HttpPost("GetPlaceList")]
        public List<Place> GetPlaceList([FromBody]Owner inOwner)
        {
            List<Place> placeList = new List<Place>();
            try
            {
                SqlConnection conn = new SqlConnection(appSettings.DbConnectionString);
                if (conn.State.ToString() != "Open")
                {
                    conn.Open();
                }

                // �������̵�/�̸�/����/����/�߼�ȭ����/����/���¸޸�
                SqlCommand sqlQuery = new SqlCommand("p_get_place_owner", conn)
                {
                    // ������ �Է� ������ ���� ���ν���
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter[] parameterList = new SqlParameter[]
                {
                        new SqlParameter() {ParameterName="@placeOwnerId",SqlDbType=SqlDbType.Int,Value=Convert.ToInt32( inOwner.Id) }
                };

                sqlQuery.Parameters.AddRange(parameterList);

                // ����
                SqlDataReader resultData = sqlQuery.ExecuteReader();
                while (resultData.Read())
                {
                    Place launchPlace = new Place()
                    {
                        Id = Convert.ToInt32(resultData["placeId"]),
                        Owner = new Owner() { Id = Convert.ToInt32(resultData["placeOwnerId"]) },
                        Name = resultData["placeName"].ToString()
                    };
                    if (resultData["placeLocation"] != DBNull.Value)
                    {
                        launchPlace.Location = resultData["placeLocation"].ToString();
                    }

                    if (resultData["placeTakeNum"] != DBNull.Value)
                    {
                        launchPlace.UsingCount = Convert.ToInt32(resultData["placeTakeNum"]);
                    }

                    placeList.Add(launchPlace);
                }

                conn.Close();
            }
            catch
            {
                throw;
            }
            return placeList;
        }

        /// <summary>
        /// ����� ���̵� �ߺ��˻�
        /// </summary>
        /// <param name="inputUserId"></param>
        [HttpPost("GetCheckUserDuplicate")]
        public int GetCheckUserDuplicate([FromBody]string inputUserId)
        {
            int userCounting = -1;
            try
            {
                SqlConnection conn = new SqlConnection(appSettings.DbConnectionString);
                if (conn.State.ToString() != "Open")
                {
                    conn.Open();
                }
                
                SqlCommand sqlQuery = new SqlCommand("p_get_user_counting", conn)
                {
                    // ������ �Է� ������ ���� ���ν���
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter[] parameterList = new SqlParameter[]
                {
                        new SqlParameter() {ParameterName="@inputUserId",SqlDbType=SqlDbType.NVarChar,Size=30,Value=inputUserId }
                };

                sqlQuery.Parameters.AddRange(parameterList);

                // ����
                SqlDataReader resultData = sqlQuery.ExecuteReader();
                while (resultData.Read())
                {
                    userCounting = Convert.ToInt32(resultData["UserCountingNum"]);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return userCounting;
        }

        /// <summary>
        /// ����� ���� ������
        /// </summary>
        /// <param name="ownerInfo"></param>
        /// <returns></returns>
        [HttpPost("GetUserInfo")]
        public Owner GetUserInfo([FromBody]string ownerName)
        {
            Owner _owner = new Owner();
            try
            {
                SqlConnection conn = new SqlConnection(appSettings.DbConnectionString);
                if (conn.State.ToString() != "Open")
                {
                    conn.Open();
                }

                SqlCommand sqlQuery = new SqlCommand("p_get_user_information", conn)
                {
                    // ������ �Է� ������ ���� ���ν���
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter[] parameterList = new SqlParameter[]
                {
                        new SqlParameter() {ParameterName="@ownerName",SqlDbType=SqlDbType.NVarChar,Size=30,Value=ownerName }
                };

                sqlQuery.Parameters.AddRange(parameterList);

                // ����
                SqlDataReader resultData = sqlQuery.ExecuteReader();
                while (resultData.Read())
                {
                    _owner.Id = Convert.ToInt32( resultData["ownerId"]);
                    _owner.Name = resultData["ownerName"].ToString();
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            return _owner;
        }
    }
}
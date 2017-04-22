using System;
using System.Collections.Generic;
using System.Text;

namespace LunchCommon.Models
{
    /// <summary>
    /// 서비스 사용자 관련
    /// </summary>
    public class Owner
    {
        /// <summary>
        /// 사용자 고유번호
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// 사용자 이름
        /// </summary>
        public string OwnerName { get; set; }
    }
}

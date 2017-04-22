using System;
using System.Collections.Generic;
using System.Text;

namespace LunchCommon.Models
{
    /// <summary>
    /// 먹는 장소 관련
    /// </summary>
    public class Place
    {
        /// <summary>
        /// 장소 이름
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// 장소 고유번호
        /// </summary>
        public int PlaceId { get; set; }

        /// <summary>
        /// 장소 위치
        /// </summary>
        public string PlaceLocation { get; set; }

        /// <summary>
        /// 장소 관련 입력/소유자
        /// </summary>
        public Owner PlaceOwner { get; set; }

        /// <summary>
        /// 장소 이용 횟수
        /// </summary>
        public int PlaceTakeNum { get; set; }
    }
}

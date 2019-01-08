using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LunchLibrary.Models
{
    /// <summary>
    /// 먹는 장소 관련
    /// </summary>
    [Table("Place")]
    public class Place
    {
        /// <summary>
        /// 장소 이름
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 장소 고유번호
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 장소 위치
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 장소 관련 입력/소유자
        /// </summary>
        [Required]
        public Owner Owner { get; set; }

        /// <summary>
        /// 장소 이용 횟수
        /// </summary>
        public int UsingCount { get; set; } = 0;

        /// <summary>
        /// 장소 작성 시간
        /// </summary>
        [Required]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 장소 업데이트 시간
        /// </summary>
        public DateTime UpdatedTime { get; set; }
    }
}

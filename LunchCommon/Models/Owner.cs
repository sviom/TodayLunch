using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// 사용자 이름
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 사용자 생성 시간
        /// </summary>
        [Required]
        public DateTime CreatedTime { get; set; } = DateTime.Now;
    }
}

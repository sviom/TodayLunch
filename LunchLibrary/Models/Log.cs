using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LunchLibrary.Models
{
    /// <summary>
    /// 로깅 정보
    /// </summary>
    public class Log : ICommon
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string StackTrace { get; set; }

        [Required]
        public DateTimeOffset CreatedTime { get; set; } = DateTimeOffset.Now;


        public string Name { get; set; }
    }
}

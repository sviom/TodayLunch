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
    public class Log : ModelActionGuide, ICommon
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

        public override bool Delete<T>(T input)
        {
            return input.Delete();
        }

        public override bool Insert<T>(T input)
        {
            if (input is Log)
            {
                if (input.Insert() != null)
                    return true;
            }
            return false;
        }

        public override bool Update<T>(T input)
        {
            if (input is Log owner)
            {
                var result = owner.Update();
                if (result.Id != Guid.Empty)
                    return true;
            }
            return false;
        }

        public static void Report(Exception ex)
        {
            var log = new Log
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace
            };
            log.Insert();
        }
    }
}

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
    public class Log : ModelAction, ICommon
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

        public override T Insert<T>(T input)
        {
            if (input is Log log)
            {
                var insertedLog = SqlLauncher.Insert(log);
                if (insertedLog != null)
                    return insertedLog.ConvertType<T>();
            }
            return null;
        }

        public override T Update<T>(T input)
        {
            if (input is Log owner)
            {
                var result = SqlLauncher.Update(owner);
                if (result.Id != Guid.Empty)
                    return result.ConvertType<T>();
            }
            return null;
        }

        public static void Report(Exception ex)
        {
            var log = new Log
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace
            };
            SqlLauncher.Insert(log);
        }
    }
}

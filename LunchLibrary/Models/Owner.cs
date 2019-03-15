using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LunchLibrary.Models
{
    /// <summary>
    /// 서비스 사용자 관련
    /// </summary>
    [Table("Owner")]
    public class Owner : ModelActionGuide, ICommon
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
        /// 비밀번호
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// 사용자 생성 시간
        /// </summary>
        [Required]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 사용자 정보 수정 시간
        /// </summary>
        [Required]
        public DateTime UpdatedTime { get; set; }

        public override bool Delete<T>(T input)
        {
            return input.Delete();
        }

        public override bool Insert<T>(T input)
        {
            if (input is Owner owner)
            {
                owner.CreatedTime = DateTime.Now;

                if (string.IsNullOrEmpty(owner.Name) || string.IsNullOrEmpty(owner.Password))
                    return false;

                var result = owner.Insert();
                if (result.Id != Guid.Empty)
                    return true;
            }
            return false;
        }

        public override bool Update<T>(T input)
        {
            if (input is Owner owner)
            {
                owner.UpdatedTime = DateTime.Now;
                var result = owner.Update();

                if (result.Id != Guid.Empty)
                    return true;
            }
            return false;
        }
    }
}

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
    public class Place : ModelAction, ICommon
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

        [Required]
        public Guid OwnerId { get; set; }

        [Required]
        public Guid AddressId { get; set; }


        public Address Address { get; set; }

        /// <summary>
        /// 장소 이용 횟수
        /// </summary>
        public int UsingCount { get; set; }

        /// <summary>
        /// 장소 작성 시간
        /// </summary>
        [Required]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 장소 업데이트 시간
        /// </summary>
        public DateTime UpdatedTime { get; set; } = DateTime.Now;

        public override T Insert<T>(T input)
        {
            if (input is Place place)
            {
                place.CreatedTime = DateTime.Now;
                place.UsingCount = 0;

                var result = SqlLauncher.Insert(place);
                if (result.Id != Guid.Empty)
                    return result.ConvertType<T>();
            }
            return null;
        }

        /// <summary>
        /// Place가 update될 때 필요한 내용들 수정
        /// </summary>
        /// <param name="place"></param>
        public override T Update<T>(T input)
        {
            if (input is Place place)
            {
                place.UsingCount++;
                place.UpdatedTime = DateTime.Now;
                var result = SqlLauncher.Update(place);

                if (result.Id != Guid.Empty)
                    return result.ConvertType<T>();
            }
            return null;
        }
    }
}

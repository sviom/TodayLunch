using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Text;

namespace LunchLibrary.Models
{
    public class Owner : ICommon
    {
        //public static Owner OwnerInstance;
        //private static readonly Lazy<Owner> Lazy = new Lazy<Owner>(() => new Owner());
        //public new static Owner Instance => Lazy.Value;
        //public Owner()
        //{
        //}

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "아이디가 너무 깁니다", MinimumLength = 1)]
        public string Name { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "비밀번호의 길이가 적절하지 않습니다.")]
        public string Password { get; set; }

        [Required]
        public DateTime CreatedTime { get; set; }

        [Required]
        public DateTime UpdatedTime { get; set; }

        public List<Address> Addresses { get; set; }


        //public override T Insert<T>(T input)
        //{
        //    if (input is Owner owner)
        //    {
        //        owner.CreatedTime = DateTime.Now;

        //        if (string.IsNullOrEmpty(owner.Name) || string.IsNullOrEmpty(owner.Password))
        //            return null;

        //        var insertResult = SqlLauncher.Insert(owner);
        //        if (insertResult.Id != Guid.Empty)
        //            return insertResult.ConvertType<T>();
        //    }
        //    return null;
        //}

        //public override T Update<T>(T input)
        //{
        //    if (input is Owner owner)
        //    {
        //        owner.UpdatedTime = DateTime.Now;
        //        var result = SqlLauncher.Update(owner);
        //        if (result.Id != Guid.Empty)
        //            return result.ConvertType<T>();
        //    }
        //    return null;
        //}
    }
}

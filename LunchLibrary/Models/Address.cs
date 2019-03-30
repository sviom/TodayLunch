using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LunchLibrary.Models
{
    /// <summary>
    /// 주소 별 장소 관련
    /// </summary>
    [Table("Address")]
    public class Address : ModelActionGuide, ICommon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public Guid OwnerId { get; set; }

        public Owner Owner { get; set; }

        public List<Place> Places { get; set; }

        [Required]
        public bool IsDefault { get; set; } = false;

        /// <summary>
        /// 주소 작성 시간
        /// </summary>
        [Required]
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 주소 업데이트 시간
        /// </summary>
        public DateTime UpdatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 가로
        /// </summary>
        public double? Lat { get; set; }

        /// <summary>
        /// 세로
        /// </summary>
        public double? Lng { get; set; }

        /// <summary>
        /// SelectBox에서 선택된 항목 여부
        /// </summary>
        [NotMapped]
        public bool IsForeground { get; set; } = false;

        public override List<T> GetAll<T>(Expression<Func<T, bool>> expression = null)
        {
            var addressList = LunchLibrary.SqlLauncher.GetAll<T>(expression);
            foreach (var addressItem in addressList)
            {
                if (addressItem is Address address)
                {
                    address.Name = UtilityLauncher.DecryptAES256(address.Name, LunchLibrary.PreDefined.SaltPassword);
                }
            }
            return addressList;
        }

        public override bool Get<T>(ref T input, Expression<Func<T, bool>> expression = null)
        {
            var returnValue = LunchLibrary.SqlLauncher.Get(ref input, expression);
            if (input is Address address)
            {
                address.Name = UtilityLauncher.DecryptAES256(address.Name, LunchLibrary.PreDefined.SaltPassword);
            }
            return returnValue;
        }

        public override bool Delete<T>(T input)
        {
            return input.Delete();
        }

        public override bool Insert<T>(T input)
        {
            if (input is Address address)
            {
                address.CreatedTime = DateTime.Now;
                address.Name = UtilityLauncher.EncryptAES256(address.Name, LunchLibrary.PreDefined.SaltPassword);

                var insertResult = address.Insert();
                if (insertResult != null)
                    return true;
            }

            return false;
        }

        public async Task<bool> InsertWithGooglePlaces(Address address)
        {
            address.CreatedTime = DateTime.Now;
            address.Name = UtilityLauncher.EncryptAES256(address.Name, LunchLibrary.PreDefined.SaltPassword);

            var insertResult = address.Insert();
            if (insertResult != null)
            {
                // Place 자동 입력
                if (insertResult.Lat != null && insertResult.Lng != null)
                {
                    var placeInsertTask = Task.Run(async () =>
                    {
                        var googleNearbyPlaces = await GooglePlatform.PlaceAPI.GetNearbyPlacesAsync(insertResult.Lat, insertResult.Lng);
                        foreach (var item in googleNearbyPlaces.results)
                        {
                            var _newPlace = new Place
                            {
                                Name = item.name,
                                Address = insertResult,
                                AddressId = insertResult.Id,
                                Location = item.formatted_address,
                                CreatedTime = DateTime.Now,
                                OwnerId = insertResult.OwnerId
                            };

                            new Place().Insert(_newPlace);
                        }
                    });

                    await Task.WhenAll(placeInsertTask);
                }

                return true;
            }

            return false;
        }

        public async Task<bool> UpsertWithGeometry(Address input, bool isUpdate = false)
        {
            var geometry = await LunchLibrary.GooglePlatform.PlaceAPI.GetAddressGeometry(input.Name);

            if (geometry != null)
            {
                if (geometry.results.Count > 0)
                {
                    input.Lat = geometry.results[0].geometry.location.lat;
                    input.Lng = geometry.results[0].geometry.location.lng;
                }
            }

            if (!isUpdate)
            {
                return await InsertWithGooglePlaces(input);
            }
            else
            {
                return Update(input);
            }
        }

        public override bool Update<T>(T input)
        {
            if (input is Address address)
            {
                address.UpdatedTime = DateTime.Now;
                address.Name = UtilityLauncher.EncryptAES256(address.Name, LunchLibrary.PreDefined.SaltPassword);

                var result = address.Update();
                if (result.Id != Guid.Empty)
                    return true;
            }
            return false;
        }
    }
}

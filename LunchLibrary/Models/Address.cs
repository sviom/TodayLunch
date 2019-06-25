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
    public class Address : ModelAction, ICommon
    {
        private static readonly Lazy<Address> Lazy = new Lazy<Address>(() => new Address());
        public new static Address Instance => Lazy.Value;
        public Address()
        {
        }


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
            var addressList = SqlLauncher.GetAll(expression);
            foreach (var addressItem in addressList)
            {
                if (addressItem is Address address)
                {
                    address.Name = UtilityLauncher.DecryptAES256(address.Name, LunchLibrary.PreDefined.SaltPassword);
                }
            }
            return addressList;
        }

        public override T Get<T>(Expression<Func<T, bool>> expression = null)
        {
            var returnValue = LunchLibrary.SqlLauncher.Get(expression);
            if (returnValue is Address address)
            {
                address.Name = UtilityLauncher.DecryptAES256(address.Name, LunchLibrary.PreDefined.SaltPassword);
            }
            return returnValue;
        }

        public override T Insert<T>(T input)
        {
            if (input is Address address)
            {
                address.CreatedTime = DateTime.Now;
                address.Name = UtilityLauncher.EncryptAES256(address.Name, LunchLibrary.PreDefined.SaltPassword);
                Task.Run(async () => await GetGeometry(address));

                var insertResult = SqlLauncher.Insert(address);

                Task.Run(async () => await UpsertWithGooglePlaces(insertResult));
                if (insertResult != null)
                    return insertResult.ConvertType<T>();
            }

            return null;
        }

        public async Task<Address> GetGeometry(Address input)
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
            return input;
        }

        public async Task<Address> UpsertWithGooglePlaces(Address address)
        {
            if (address != null)
            {
                // Place 자동 입력
                if (address.Lat != null && address.Lng != null)
                {
                    var placeInsertTask = Task.Run(async () =>
                    {
                        var googleNearbyPlaces = await GooglePlatform.PlaceAPI.GetNearbyPlacesAsync(address.Lat, address.Lng);
                        foreach (var item in googleNearbyPlaces.results)
                        {
                            var existPlaces = ModelAction.Instance.GetAll<Place>(x => x.AddressId.Equals(address.Id));
                            foreach (var existPlace in existPlaces)
                            {
                                if (item.formatted_address.Equals(existPlace.Location))
                                {
                                    existPlace.Name = item.name;
                                    existPlace.Address = address;
                                    existPlace.AddressId = address.Id;
                                    UpdatedTime = DateTime.Now;
                                    SqlLauncher.Update(existPlace);
                                }
                                else
                                {
                                    var _newPlace = new Place
                                    {
                                        Name = item.name,
                                        Address = address,
                                        AddressId = address.Id,
                                        Location = item.formatted_address,
                                        CreatedTime = DateTime.Now,
                                        OwnerId = address.OwnerId
                                    };

                                    SqlLauncher.Insert(_newPlace);
                                }
                            }
                        }
                    });

                    await Task.WhenAll(placeInsertTask);
                }
                return address;
            }
            return null;
        }

        public override T Update<T>(T input)
        {
            if (input is Address address)
            {
                address.UpdatedTime = DateTime.Now;
                address.Name = UtilityLauncher.EncryptAES256(address.Name, LunchLibrary.PreDefined.SaltPassword);
                Task.Run(async () => await GetGeometry(address));
                var result = SqlLauncher.Update(address);
                Task.Run(async () => await UpsertWithGooglePlaces(result));
                if (result.Id != Guid.Empty)
                    return result.ConvertType<T>();
            }
            return null;
        }
    }
}

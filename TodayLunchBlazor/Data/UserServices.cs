using LunchLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodayLunchBlazor.Context;

namespace TodayLunchBlazor.Data
{
    public class UserServices
    {
        public (Owner ownerVal, bool isOnwer) LogIn(string name, string password)
        {
            var hashedPw = LunchLibrary.UtilityLauncher.EncryptSHA256(password);
            //var owner = ModelAction.Instance.Get<Owner>(x => x.Name.Equals(name) && x.Password.Equals(hashedPw));
            var owner = SqlLauncher.Get<Owner>(x => x.Name.Equals(name) && x.Password.Equals(hashedPw));
            if (owner != null)
            {
                //HttpContext.Session.SetString("ownerName", owner.Name.ToString());
                //HttpContext.Session.SetString("ownerGuid", owner.Id.ToString());
                //Owner.OwnerInstance = owner;
                return (ownerVal: owner, isOnwer: true);
            }
            else
                return (ownerVal: owner, isOnwer: false);
        }
    }
}
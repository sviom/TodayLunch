using LunchLibrary.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodayLunchUWP.ViewModels
{
    public class LunchViewModel
    {
        public ObservableCollection<Place> Places { get; set; }
        public ObservableCollection<Address> Addresses { get; set; }
    }

    public class LunchPageNavigate
    {
        public Owner Owner { get; set; }
        public string AddressId { get; set; }
    }
}

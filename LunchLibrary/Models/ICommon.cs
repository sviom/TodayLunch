using System;
using System.Collections.Generic;
using System.Text;

namespace LunchLibrary.Models
{
    public interface ICommon
    {
        Guid Id { get; set; }

        string Name { get; set; }
    }
}

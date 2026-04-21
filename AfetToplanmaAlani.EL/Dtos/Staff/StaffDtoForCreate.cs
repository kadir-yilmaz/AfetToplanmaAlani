using AfetToplanmaAlani.EL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfetToplanmaAlani.EL.Dtos.Staff
{
    public record StaffDtoForCreate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public int? PlaceId { get; set; }
    }
}

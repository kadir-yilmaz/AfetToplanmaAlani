using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfetToplanmaAlani.EL.Dtos.WorkGroupStaff
{
    public class WorkGroupStaffDtoForUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? PhoneNumber { get; set; }
        public string WorkGroup { get; set; }  // dropdown
        public string? Crew { get; set; }
        public string? Duty { get; set; }
    }
}

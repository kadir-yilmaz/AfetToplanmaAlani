using AfetToplanmaAlani.EL.Models;
using System.ComponentModel.DataAnnotations;

namespace AfetToplanmaAlani.EL.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string? Brand { get; set; }

        public string? Model { get; set; }

        public string? Type { get; set; }

        public string? Color { get; set; }

        [Required]
        public string Plate { get; set; }

        public List<VehicleStaff> VehicleStaff { get; set; } = new();
    }
}


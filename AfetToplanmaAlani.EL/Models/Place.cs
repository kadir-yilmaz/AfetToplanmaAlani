using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AfetToplanmaAlani.EL.Models;


namespace AfetToplanmaAlani.EL.Models
{
    public class Place
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string? Neighborhood { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? ContactNumber { get; set; } // İletişim numarası
        public int? PlaceCategoryId { get; set; } // foreign key
        public PlaceCategory? PlaceCategory { get; set; } // Place category
        public ICollection<Staff> Staff { get; set; } = new List<Staff>();

    }
}

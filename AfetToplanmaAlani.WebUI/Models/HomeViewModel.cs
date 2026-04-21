using System.Collections.Generic;

namespace AfetToplanmaAlani.WebUI.Models
{
    public class HomeViewModel
    {
        public int TotalPlaces { get; set; }
        public int TotalStaff { get; set; }
        public List<PlaceViewModel> Places { get; set; } = new();
        public List<string> Categories { get; set; } = new();
        public List<string> Cities { get; set; } = new();
    }

    public class PlaceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string City { get; set; } = "";
        public string District { get; set; } = "";
        public string Neighborhood { get; set; } = "";
        public string Category { get; set; } = "";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<StaffViewModel> Staff { get; set; } = new();
    }


    public class StaffViewModel
    {
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
    }

}

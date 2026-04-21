using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfetToplanmaAlani.EL.Models
{
    public class PlaceCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Place> Places { get; set; } = new List<Place>();
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace AfetToplanmaAlani.EL.Dtos.Place
{
    public class PlaceDtoForCreate
    {
        [Required(ErrorMessage = "Yer adı zorunludur.")]
        [Display(Name = "Yer Adı")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Şehir zorunludur.")]
        [Display(Name = "Şehir")]
        public string City { get; set; }

        [Required(ErrorMessage = "İlçe zorunludur.")]
        [Display(Name = "İlçe")]
        public string District { get; set; }

        [Display(Name = "Mahalle")]
        public string? Neighborhood { get; set; }

        [Required(ErrorMessage = "Enlem zorunludur.")]
        [Display(Name = "Enlem")]
        public double Latitude { get; set; }

        [Required(ErrorMessage = "Boylam zorunludur.")]
        [Display(Name = "Boylam")]
        public double Longitude { get; set; }

        [Display(Name = "Telefon Numarası")]
        public string? ContactNumber { get; set; }

        [Required(ErrorMessage = "Kategori seçmelisiniz.")]
        [Display(Name = "Kategori")]
        public int? PlaceCategoryId { get; set; }
    }
}

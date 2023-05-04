using System.ComponentModel.DataAnnotations;

namespace ProLi.Models
{
    public class Office
    {
        public Office()
        {

        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "A mező kitöltése kötelező")]
        [Display(Name = "Pozíció")]
        public string OfficePost { get; set; }

        [Required(ErrorMessage = "A mező kitöltése kötelező")]
        [Display(Name = "Jogviszony kezdete")]
        public DateTime OfficeStart { get; set; }


        [Display(Name = "Jogviszony vége")]
        public DateTime? OfficeEnd { get; set; }


        [Display(Name = "Hivatal neve 1.szint")]
        [Required(ErrorMessage = "Legalább egy hivatalt meg kell adni!")]
        public string OfficeName1 { get; set; }


        [Display(Name = "Hivatal neve 2.szint")]
        public string? OfficeName2 { get; set; }


        [Display(Name = "Hivatal neve 3.szint")]
        public string? OfficeName3 { get; set; }


        [Display(Name = "Cím")]
        public string? OfficeAddress { get; set; }


        [Display(Name = "E-mail")]
        public string? OfficeEmail { get; set; }


        [Display(Name = "Telefonszám")]
        public string? OfficePhone { get; set; }


        [Display(Name = "Azonosító kód")]
        [Required(ErrorMessage = "A mező kitöltése kötelező")]
        public int People_Id { get; set; }


        public People People { get; set; }

    }
}

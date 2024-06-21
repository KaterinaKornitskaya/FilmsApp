using System.ComponentModel.DataAnnotations;

namespace FilmsApp.Models
{
    public class Film
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Назва")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Рік випуску")]
        [Range(1800,2024)]
        public int Year { get; set; }

        [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Режисер")]
        public string? Director { get; set; }

        [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Акторський склад")]
        public string? Actors { get; set; }

        [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Жанр")]
        public string? Genre { get; set; }

        [Required(ErrorMessage = "Поле не може бути пустим")]
        [Display(Name = "Опис")]
        public string? Description { get; set; }

        [Display(Name = "Постер")]
        public string? Image { get; set; }     
    }
}

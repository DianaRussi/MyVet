using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyVet.Web.Data.Entities
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(30, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Display(Name = "Documento")]
        public string Document { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Display(Name = "Nombres")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }
        [MaxLength(100, ErrorMessage = "El campo {0} debe tener máximo {1} caracteres")]
        [Display(Name = "Dirección")]
        public string Address { get; set; }
        [Display(Name ="Latitud")]
        [DisplayFormat(DataFormatString = "{0:N6}")]
        public double Latitude { get; set; }
        [DisplayFormat(DataFormatString = "{0:N6}")]
        [Display(Name = "Longitud")]
        public double Longitude { get; set; }
        [Display(Name = "Nombre completo")]
        public string FullNameWithDocument => $"{FirstName} {LastName} - {Document}";
        [Display(Name = "Nombre completo")]
        public string FullName => $"{FirstName} {LastName}";
    }
}
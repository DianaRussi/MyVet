using System.ComponentModel.DataAnnotations;

namespace MyVet.Web.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="El campo {0} es obligatorio")]
        [EmailAddress(ErrorMessage ="Ingrese un correo valido en el campo{0}")]
        [Display(Name ="Usuario")]
        public string Username { get; set; }
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [MinLength(6, ErrorMessage = "El campo {0} debe tener minimo {1} caracteres")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
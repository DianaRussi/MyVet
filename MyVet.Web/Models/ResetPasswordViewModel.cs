using System.ComponentModel.DataAnnotations;

namespace MyVet.Web.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Nueva contraseña")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirmar contraseña")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "El campo {0} debe contener entre {2} y {1} caracteres.")]
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="La nueva contraseña y la confirmación deben ser iguales")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
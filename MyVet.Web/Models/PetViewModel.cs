using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyVet.Web.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyVet.Web.Models
{
    public class PetViewModel : Pet
    {
        public int OwnerId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Tipo de mascota")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe selleccionar un tipo de mascota.")]
        public int PetTypeId { get; set; }

        [Display(Name = "Imagen")]
        public IFormFile ImageFile { get; set; }

        public IEnumerable<SelectListItem> PetTypes { get; set; }
    }
}
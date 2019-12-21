using Microsoft.AspNetCore.Mvc.Rendering;
using MyVet.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyVet.Web.Models
{
    public class AgendaViewModel : Agenda
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Propietario")]
        [Range(1, int.MaxValue, ErrorMessage = "Usted debe seleccionar un propietario.")]
        public int OwnerId { get; set; }


        public IEnumerable<SelectListItem> Owners { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [Display(Name = "Mascota")]
        [Range(1, int.MaxValue, ErrorMessage = "Usted debe seleccionar una mascota.")]
        public int PetId { get; set; }
        public IEnumerable<SelectListItem> Pets { get; set; }
        public bool IsMine { get; set; }
        public string Reserved => "Reserved";
    }
}
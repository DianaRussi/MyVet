using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyVet.Web.Data.Entities
{
    public class Agenda
    {
        public int Id { get; set; }

        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Comentarios")]
        public string Remarks { get; set; }

        [Display(Name = "Disponible?")]
        public bool IsAvailable { get; set; }

        [Display(Name = "Fecha:")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime DateLocal => Date.ToLocalTime();
        public Pet Pet { get; set; }
        public Owner Owner { get; set; }
    }
}
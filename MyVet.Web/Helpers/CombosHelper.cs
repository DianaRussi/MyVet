using Microsoft.AspNetCore.Mvc.Rendering;
using MyVet.Web.Data;
using System.Collections.Generic;
using System.Linq;

namespace MyVet.Web.Helpers
{
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _dataContext;

        public CombosHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<SelectListItem> GetComboPetTypes()
        {
            var list = _dataContext.PetTypes.Select(pt => new SelectListItem
            {
                Text = pt.Name,
                Value = $"{pt.Id}"
            })
                .OrderBy(pt => pt.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un tipo de mascota...]",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboServiceTypes()
        {
            var list = _dataContext.ServiceTypes.Select(st => new SelectListItem
            {
                Text = st.Name,
                Value = $"{st.Id}"
            })
                .OrderBy(st => st.Text)
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "[Seleccione un tipo de servicio...]",
                Value = "0"
            });
            return list;
        }

        public IEnumerable<SelectListItem> GetComboOwners()
        {
            var list = _dataContext.Owners.Select(p => new SelectListItem
            {
                Text = p.User.FullNameWithDocument,
                Value = p.Id.ToString()
            }).OrderBy(p => p.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un propietario...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboPets(int ownerId)
        {
            var list = _dataContext.Pets.Where(p => p.Owner.Id == ownerId).Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            }).OrderBy(p => p.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona una mascota...)",
                Value = "0"
            });

            return list;
        }
    }
}
using MyVet.Common.Models;
using MyVet.Web.Data.Entities;
using MyVet.Web.Models;
using System.Threading.Tasks;

namespace MyVet.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<Pet> ToPetAsync(PetViewModel model, string path, bool isNew);
        PetViewModel ToPetViewModel(Pet pet);
        Task<History> ToHistoryAsync(HistoryViewModel model, bool isNew);
        HistoryViewModel ToHistoryViewModel(History history);
        PetResponse ToPetResponse(Pet pet);
        OwnerResponse ToOwnerResposne(Owner owner);
    }
}
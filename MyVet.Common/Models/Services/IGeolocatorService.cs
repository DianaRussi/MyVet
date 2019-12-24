using System.Threading.Tasks;

namespace MyVet.Common.Models.Services
{
    public interface IGeolocatorService
    {
        double Latitude { get; set; } // lo que se aleja o acerca linea del ecuador
        double Longitude { get; set; }// lo que nos movemos al oriente u occidente del meridiano de grinwich
        Task GetLocationAsync();
    }
}
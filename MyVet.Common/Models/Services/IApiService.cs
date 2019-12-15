using System.Threading.Tasks;

namespace MyVet.Common.Models.Services
{
    public interface IApiService
    {
        Task<Response<OwnerResponse>> GetOwnerByEmailAsync( 
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType, // para el caso es bearer
            string accessToken, // num largo que genera
            string email); 

        Task<Response<TokenResponse>> GetTokenAsync( //se obtiene token con este metodo
            string urlBase, // url de publicacion 
            string servicePrefix, //ruteo del controlador
            string controller, // nombre controlador y la accion 
            TokenRequest request); // este ultimo combinacion d usuario y pasa

        Task<bool> CheckConnection(string url);
    }
}
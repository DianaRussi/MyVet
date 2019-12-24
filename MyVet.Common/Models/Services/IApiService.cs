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

        Task<Response<object>> RegisterUserAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            UserRequest userRequest);

        Task<Response<object>> RecoverPasswordAsync(
            string urlBase,
            string servicePrefix,
            string controller,
           EmailRequest emailRequest);

        Task<Response<object>> PutAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            T model,
            string tokenType,
            string accessToken);
        Task<Response<object>> ChangePasswordAsync(
            string urlBase,
            string servicePrefix,
            string controller,
            ChangePasswordRequest changePasswordRequest,
            string tokenType,
            string accessToken);

        Task<Response<object>> GetListAsync<T>(
            string urlBase,
            string servicePrefix,
            string controller,
            string tokenType,
            string accessToken);
    }
}
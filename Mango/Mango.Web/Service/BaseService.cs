using Mango.Web.Models;
using Mango.Web.Service.IService;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service
{
    public sealed class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public BaseService(IHttpClientFactory httpClientFactory) 
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResponseDto?> SendAsync(RequestDto requestDto)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("MangoAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");

                message.RequestUri = new Uri(requestDto.Url);
                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage? apiResponse = null;

                message.Method = requestDto.ApiType switch
                {
                    ApiType.GET => HttpMethod.Get,
                    ApiType.POST => HttpMethod.Post,
                    ApiType.PUT => HttpMethod.Put,
                    ApiType.DELETE => HttpMethod.Delete,
                    _ => throw new NotImplementedException(),
                };

                apiResponse = await client.SendAsync(message);

                return apiResponse.StatusCode switch
                {
                    HttpStatusCode.NotFound => new() { IsSuccess = false, Message = "Not Found" },
                    HttpStatusCode.Forbidden => new() { IsSuccess = false, Message = "Access Denied" },
                    HttpStatusCode.Unauthorized => new() { IsSuccess = false, Message = "Unauthorized" },
                    HttpStatusCode.InternalServerError => new() { IsSuccess = false, Message = "Internal Server Error" },
                    _ => JsonConvert.DeserializeObject<ResponseDto>(await apiResponse.Content.ReadAsStringAsync()),
                };

            }
            catch (Exception ex)
            {
                return new ResponseDto
                {
                    Message = ex.Message.ToString(),
                    IsSuccess = false,
                };
            }
        }
    }
}

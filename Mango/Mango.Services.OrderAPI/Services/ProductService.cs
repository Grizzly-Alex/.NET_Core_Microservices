using Mango.Services.OrderAPI.Models.Dto;
using Mango.Services.OrderAPI.Services.IServices;
using Newtonsoft.Json;

namespace Mango.Services.OrderAPI.Services
{
    public sealed class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;               
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            HttpClient client = _httpClientFactory.CreateClient("Product");
            HttpResponseMessage response =  await client.GetAsync($"/api/product");
            string apiContent = await response.Content.ReadAsStringAsync();
            ResponseDto responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (responseDto.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(responseDto.Result));
            }

            return responseDto.IsSuccess
                ? JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(responseDto.Result))
                : Array.Empty<ProductDto>();
        }
    }
}

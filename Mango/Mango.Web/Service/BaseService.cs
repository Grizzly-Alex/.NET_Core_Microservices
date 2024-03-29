﻿using Mango.Web.Models;
using Mango.Web.Service.IService;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Text;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service
{
    public sealed class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;

        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider) 
        {
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }

        public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("MangoAPI");
                HttpRequestMessage message = new();

                switch (requestDto.ContentType)
                {
                    case ContentType.Json: 
                        message.Headers.Add("Accept", "application/json"); 
                        break;
					case ContentType.MultipartFormData: 
                        message.Headers.Add("Accept", "*/*"); 
                        break;
				}

                message.Headers.Add("Accept", "application/json");

                if(withBearer)
                {
                    message.Headers.Add("Authorization", $"Bearer {_tokenProvider.GetToken()}");
                }

                message.RequestUri = new Uri(requestDto.Url);

                if(requestDto.ContentType is ContentType.MultipartFormData)
                {
                    var content = new MultipartFormDataContent();

                    foreach(var prop in requestDto.Data.GetType().GetProperties())
                    {
                        var value = prop.GetValue(requestDto.Data);
                        if(value is FormFile file)
                        {                           
                            if (file is not null)
                            {
                                content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                            }                           
                        }
                        else
                        {
                            content.Add(new StringContent(value is null ? string.Empty : value.ToString()), prop.Name);
                        }
                    }
                    message.Content = content;
                }
                else
                {
					if (requestDto.Data != null)
					{
						message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
					}
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

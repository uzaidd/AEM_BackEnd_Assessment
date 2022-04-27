using Back_End_Assesment.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Back_End_Assesment.Controllers
{
    [Route(Endpoint)]
    [ApiController]
    public class AEMRestApiController : ControllerBase
    {
        public const string Endpoint = "api/AEMRestApi/";
        private IConfiguration _config;

        public AEMRestApiController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        [Route("GetPlatformWellActual")]
        public async Task<JsonResult> GetPlatformWellActual()
        {
            try
            {
                var httpClient = new HttpClient();
                string username = _config["LoginCredential:username"];
                string password = _config["LoginCredential:password"];
                string loginUri = _config["LoginCredential:loginUri"];
                string platformWellUri = _config["LoginCredential:platformWellUri"];

                var loginResponse = await httpClient.PostAsJsonAsync(loginUri, new { username, password });
                loginResponse.EnsureSuccessStatusCode();
                string rawToken = await loginResponse.Content.ReadAsStringAsync();
                string? token = JsonConvert.DeserializeObject<string>(rawToken);

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var platformWellResponse = await httpClient.GetStringAsync(platformWellUri);
                List<Platform>? platformResponse = JsonConvert.DeserializeObject<List<Platform>>(platformWellResponse);

                if (platformResponse.Count != 0)
                {
                    var dbContext = new AEM_Assesment_DBContext();
                    foreach (var platform in platformResponse)
                    {
                        var platformModel = dbContext.Platforms.FirstOrDefault(instance => instance.Id == platform.Id);
                        if (platformModel == null)
                        {
                            dbContext.Add(platform);  
                        }
                        else
                        {
                            platformModel.Id = platform.Id;
                            platformModel.UniqueName = platform.UniqueName;
                            platformModel.UpdatedAt = platform.UpdatedAt;
                            platformModel.CreatedAt = platform.CreatedAt;
                            platformModel.Latitude = platform.Latitude;
                            platformModel.Longitude = platform.Longitude;
                            dbContext.Update(platformModel);
                        }

                        if (platform.Well.Count != 0)
                        {
                            foreach(var well in platform.Well)
                            {
                                var wellModel = dbContext.Wells.FirstOrDefault(instance => instance.Id == well.Id);
                                if (wellModel == null)
                                {
                                    dbContext.Add(well);
                                }
                                else
                                {
                                    wellModel.Id = well.Id;
                                    wellModel.PlatformId = well.PlatformId;
                                    wellModel.UniqueName = well.UniqueName;
                                    wellModel.UpdatedAt = well.UpdatedAt;
                                    wellModel.CreatedAt = well.CreatedAt;
                                    wellModel.Latitude = well.Latitude;
                                    wellModel.Longitude = well.Longitude;
                                    dbContext.Update(wellModel);
                                }
                            }
                        }
                        dbContext.SaveChanges();
                    }
                }

                List<Well> wells = new List<Well>();
                var wellResponse = platformResponse.Where(instance => instance.Well.Count != 0).Select(instance => instance.Well).ToList();
                foreach (var wellArray in wellResponse)
                {
                    wells.Add(wellArray.OrderByDescending(x => x.UpdatedAt).FirstOrDefault());
                }
                return new JsonResult(wells);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

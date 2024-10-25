using Refit;
using WebApi.Models;

namespace WebApi;

public interface IUserApi
{
    [Get("/api/map_objects")]
    Task<ApiResponse<List<MapObjectsModel>>> GetMapObjects();

    [Get("/api/map_objects/{id}")]
    Task<ApiResponse<MapObjectsModel>> GetObject(string id);
}
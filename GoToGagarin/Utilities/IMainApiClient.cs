using GoToGagarin.Model;
using MapControlLib.Models;
using Refit;

namespace GoToGagarin.Utilities;

public interface IMainApiClient
{
    [Get("/api/map_objects")]
    Task<List<MapObjectsModel>> GetMapObjects();

    [Get("/api/floors")]
    Task<List<Floor>> GetFloors();

    [Get("/api/areas")]
    Task<List<Area>> GetAreas();

    [Get("/api/terminals")]
    Task<List<Terminal>> GetTerminals();

    //[Get("/api/navigate?from={fromNode}&to={toNode}")]
    //Task<ApiResponse<List<NaviPoint>>> Navigate(int fromNode,int toNode);
}
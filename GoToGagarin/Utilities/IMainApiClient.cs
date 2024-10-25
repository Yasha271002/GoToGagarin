using GoToGagarin.Model;
using Refit;

namespace GoToGagarin.Utilities;

public interface IMainApiClient
{
    [Get("/api/map_objects")]
    Task<List<MapObjectsModel>> GetMapObjects();

    //[Get("/api/floors")]
    //Task<ApiResponse<List<Floor>>> GetFloors();

    //[Get("/api/areas")]
    //Task<ApiResponse<List<Area>>> GetAreas();

    //[Get("/api/terminals/{id}")]
    //Task<ApiResponse<Terminal>> GetTerminal(int id);

    //[Get("/api/navigate?from={fromNode}&to={toNode}")]
    //Task<ApiResponse<List<NaviPoint>>> Navigate(int fromNode,int toNode);
}
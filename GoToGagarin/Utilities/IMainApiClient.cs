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
    
    [Get("/api/standbies")]
    Task<List<InactivityModel>> GetVideo();

    [Get("/api/navigate?from={fromNode}&to={toNode}&routeType={routeType}")]
    Task<List<NaviPoint>> Navigate(int fromNode, int toNode, int routeType);
}
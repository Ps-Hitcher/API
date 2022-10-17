namespace WebApplication2.Models.Travel;

public interface ITravelRepository
{
    TravelModel GetTravel(Guid TravelId);
    List<TravelModel> GetTravelList();
}

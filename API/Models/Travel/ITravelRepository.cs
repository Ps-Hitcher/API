namespace WebApplication2.Models.Travel;

public interface ITravelRepository
{
    TravelModel GetTravel(Guid TravelId);
    List<TravelModel> GetTravelList();
    
    public void SerializeTravelList(List<TravelModel> TravelList);

}

namespace WebApplication2.Models.Travel;
using Microsoft.EntityFrameworkCore;


public interface ITravelRepository
{
    TravelModel GetTravel(Guid TravelId);
    // List<TravelModel> GetTravelList();
    DbSet<TravelModel> GetTravelList();

    public void SerializeTravelList(List<TravelModel> TravelList);

}

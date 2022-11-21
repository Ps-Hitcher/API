namespace WebApplication2.Models.Travel;
using Microsoft.EntityFrameworkCore;


public interface ITravelRepository
{
    TravelModel GetTravel(Guid travelId);
    public void Save();
    DbSet<TravelModel> GetTravelList();
}

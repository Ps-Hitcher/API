namespace WebApplication2.Models.Travel;
using Microsoft.EntityFrameworkCore;


public interface ITravelRepository
{
    TravelModel GetTravel(Guid TravelId);
    DbSet<TravelModel> GetTravelList();
}

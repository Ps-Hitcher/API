using Microsoft.EntityFrameworkCore;
using WebApplication2.Models.Travel;
using WebApplication2.Models.User;

namespace WebApplication2.Models;

public interface IChosenTripsRepository
{
    DbSet<ChosenTripsModel> GetChosenTrips();
    IQueryable<ChosenTripsModel> GetUsers(Guid travelId);
    IQueryable<ChosenTripsModel> GetTravels(Guid userId);
    public void Save();
}
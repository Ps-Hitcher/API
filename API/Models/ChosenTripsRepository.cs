using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models.Travel;
using WebApplication2.Models.User;

namespace WebApplication2.Models;

public class ChosenTripsRepository : IChosenTripsRepository
{
    private readonly DbSet<ChosenTripsModel> _chosenTripsList;
    private readonly DataContext _context;
    
    public ChosenTripsRepository(DataContext context)
    {
        _context = context;
        _chosenTripsList = context.ChosenTrips;
    }

    public DbSet<ChosenTripsModel> GetChosenTrips()
    {
        return _chosenTripsList;
    }

    public IQueryable<ChosenTripsModel> GetUsers(Guid travelId)
    {
        return _chosenTripsList.Where(e => e.TravelId == travelId);
    }

    public IQueryable<ChosenTripsModel> GetTravels(Guid userId)
    {
        return _chosenTripsList.Where(e => e.UserId == userId);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
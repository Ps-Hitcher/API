using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using FileIO = System.IO.File;

namespace WebApplication2.Models.Travel;

public class TravelRepository : ITravelRepository
{
    private readonly DbSet<TravelModel> _travelList;
    private readonly DataContext _context;

    public TravelRepository(DataContext context)
    {
        _context = context;
        _travelList = context.Trips;
    }

    public TravelModel GetTravel(Guid id)
    {
        return _travelList.FirstOrDefault(e => e.Id == id) ?? throw new InvalidOperationException();
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public DbSet<TravelModel> GetTravelList()
    {
        return _travelList;
    }

    // public DbSet<TravelModel> GetTravelList(SearchInfo searchInfo)
    // {
    //     TravelModel t = new TravelModel();
    //     return t.TravelResults;
    // }
}
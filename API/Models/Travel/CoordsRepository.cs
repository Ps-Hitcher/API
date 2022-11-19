using Newtonsoft.Json;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using WebApplication2.Utilities;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using FileIO = System.IO.File;
namespace WebApplication2.Models.Travel;

public class CoordsRepository : ICoordsRepository
{
    private DbSet<CoordsModel> CoordsList;
    public DataContext _context;
    public const string _jsonPath = "TravelDB.json";

    public CoordsRepository(DataContext context)
    {
        _context = context;
        CoordsList = context.Coords;
    }

    public CoordsModel GetCoords(Guid Id, int position)
    {
        return CoordsList.FirstOrDefault(e => ((e.MetaId == Id) && (e.position == position)));
    }

    public DbSet<CoordsModel> GetCoordsList()
    {
        return CoordsList;
    }

}


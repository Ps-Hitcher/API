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
    public CoordsRepository(DataContext context)
    {
        _context = context;
        CoordsList = context.Coords;
    }

    public CoordsModel GetCoords(Guid id, int position)
    {
        return CoordsList.FirstOrDefault(e => ((e.MetaId == id) && (e.position == position))) ?? throw new InvalidOperationException();
    }
    public void Save()
    {
        _context.SaveChanges();
    }
    public DbSet<CoordsModel> GetCoordsList()
    {
        return CoordsList;
    }

}


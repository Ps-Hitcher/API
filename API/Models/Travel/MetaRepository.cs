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
public class MetaRepository : IMetaRepository
{
    private DbSet<MetaModel> MetaList;
    public DataContext _context;
    public MetaRepository(DataContext context)
    {
        _context = context;
        MetaList = context.Meta;
    }
    public MetaModel GetMeta(Guid id, string destination)
    {
        return MetaList.FirstOrDefault(e => e.TravelId == id && e.Destination == destination) ?? throw new InvalidOperationException();
    }
    public void Save()
    {
        _context.SaveChanges();
    }
    public DbSet<MetaModel> GetMetaList()
    {
        return MetaList;
    }
}
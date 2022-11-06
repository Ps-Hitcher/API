using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json;
using WebApplication2.Models.Travel;
using WebApplication2.Models.User;

namespace WebApplication2.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<UserModel> Users { get; set; }
    public DbSet<TravelModel> Trips { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TravelModel>().Property(x => x.Stopovers).HasConversion(new ValueConverter<List<string>, string>(
            v => JsonConvert.SerializeObject(v), // Convert to string for persistence
            v => JsonConvert.DeserializeObject<List<string>>(v))
            );
    }
}
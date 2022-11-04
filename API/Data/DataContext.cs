using Microsoft.EntityFrameworkCore;
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
}
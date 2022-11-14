namespace WebApplication2.Models.Travel;
using Microsoft.EntityFrameworkCore;


public interface IMetaRepository
{
    MetaModel GetMeta(Guid TravelId);
    DbSet<MetaModel> GetMetaList();
}

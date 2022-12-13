namespace WebApplication2.Models.Travel;
using Microsoft.EntityFrameworkCore;


public interface IMetaRepository
{
    MetaModel GetMeta(Guid travelId, string destination);
    public void Save();
    DbSet<MetaModel> GetMetaList();
}

namespace WebApplication2.Models.Travel;
using Microsoft.EntityFrameworkCore;

public interface ICoordsRepository
{
    CoordsModel GetCoords(Guid metaId, int position);
    public void Save();
    DbSet<CoordsModel> GetCoordsList();
}
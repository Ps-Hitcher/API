namespace WebApplication2.Models.Travel;
using Microsoft.EntityFrameworkCore;

public interface ICoordsRepository
{
    CoordsModel GetCoords(Guid metaId, int position);
    DbSet<CoordsModel> GetCoordsList();
}
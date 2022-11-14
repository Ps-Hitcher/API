using Microsoft.EntityFrameworkCore;
using WebApplication2.Models.User;

namespace WebApplication2.Models.Errors
{
    public interface IErrorRepository
    {
        ErrorModel Get();
        void Add(ErrorModel error);
        DbSet<ErrorModel> GetErrorList();
        void Delete();
        void Save();
    }
}

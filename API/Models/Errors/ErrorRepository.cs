using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;

namespace WebApplication2.Models.Errors
{
    public class ErrorRepository : IErrorRepository
    {
        private DbSet<ErrorModel> _errorModels;
        private DataContext _dataContext;
        public ErrorRepository(DataContext context)
        {
            _dataContext = context;
            _errorModels = _dataContext.Errors;
        }
        public void Delete()
        {
            _errorModels.Remove(Get());
        }

        public ErrorModel Get()
        {
            return _errorModels.OrderByDescending(error => error.DateAndTime).First();
        }

        public DbSet<ErrorModel> GetErrorList()
        {
            return _errorModels;
        }

        public void Add(ErrorModel error)
        {
            _errorModels.Add(error);
        }

        public void Save()
        {
            _dataContext.SaveChanges();
        }

    }
}

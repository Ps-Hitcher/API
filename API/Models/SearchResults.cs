using WebApplication2.Models.Travel;
using WebApplication2.Models.User;

namespace WebApplication2.Models;

public class SearchResults
{
    public SearchTravel? SearchTravel { get; set; }
    public List<UserModel> UserModel { get; set; }


    public SearchResults()
    {
        this.SearchTravel = new SearchTravel();
        this.UserModel = new List<UserModel>();
    }
}
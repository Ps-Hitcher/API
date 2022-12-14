using WebApplication2.Models.Travel;
using WebApplication2.Models.User;

namespace WebApplication2.Models;

public class TravelUser
{
    public string LatLng { get; set; }
    public List<TravelModel> TravelModel { get; set; }
    public List<UserModel> UserModel { get; set; }

    public TravelUser()
    {
        this.TravelModel = new List<TravelModel>();
        this.UserModel = new List<UserModel>();
    }

    public TravelUser(string coords, List<TravelModel> t, List<UserModel> u)
    {
        this.LatLng = coords;
        this.TravelModel = t;
        this.UserModel = u;
    }
}
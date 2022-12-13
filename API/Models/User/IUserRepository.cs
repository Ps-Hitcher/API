namespace WebApplication2.Models.User;
using Microsoft.EntityFrameworkCore;
public interface IUserRepository
{
    public UserModel GetUser(Guid Id);
    DbSet<UserModel> GetUserList();
    public bool IsValidPhone(string PhoneNumber);
    public void DeleteUser(Guid Id);
    public void Save();

    public string GetHoroName_(string YearOfBirth);

    public void OnUserLogged(object source, EventArgs e);
    public static int GetUserAge(string YearOfBirth){
        DateTime today = DateTime.Today;
        int age = today.Year - DateTime.Parse(YearOfBirth).Year;
        if (DateTime.Parse(YearOfBirth) > today.AddYears(-age)) age--;

        return age;
    }

    public static string GetHoroName(string YearOfBirth)
    {
        int month = DateTime.Parse(YearOfBirth).Month;
        int day = DateTime.Parse(YearOfBirth).Day;

        switch (month)
        {
            case 1:
                if (day <= 19)
                    return "Capricorn";
                else
                    return "Aquarius";

            case 2:
                if (day <= 18)
                    return "Aquarius";
                else
                    return "Pisces";
            case 3:
                if (day <= 20)
                    return "Pisces";
                else
                    return "Aries";
            case 4:
                if (day <= 19)
                    return "Aries";
                else
                    return "Taurus";
            case 5:
                if (day <= 20)
                    return "Taurus";
                else
                    return "Gemini";
            case 6:
                if (day <= 20)
                    return "Gemini";
                else
                    return "Cancer";
            case 7:
                if (day <= 22)
                    return "Cancer";
                else
                    return "Leo";
            case 8:
                if (day <= 22)
                    return "Leo";
                else
                    return "Virgo";
            case 9:
                if (day <= 22)
                    return "Virgo";
                else
                    return "Libra";
            case 10:
                if (day <= 22)
                    return "Libra";
                else
                    return "Scorpio";
            case 11:
                if (day <= 21)
                    return "Scorpio";
                else
                    return "Sagittarius";
            case 12:
                if (day <= 21)
                    return "Sagittarius";
                else
                    return "Capricorn";
        }
        return "";

    }
}
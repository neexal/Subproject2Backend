using IMDB.DataServiceLayer.Models;
namespace IMDB.DataServiceLayer;

public class DataService : IDataService
{
    private readonly ImdbContext _imdbContext;

    public DataService()
    {
        _imdbContext = new ImdbContext();
    }
    
    //RegisterUser
    public int RegisterUser(string username, string password, string email)
    {
        var user = new AppUser
        {
            Username = username,
            Password = password,
            Email = email
        };
        _imdbContext.AppUsers.Add(user);
        _imdbContext.SaveChanges();
        return user.Id;
    }
    
    //GetUsers
    public IList<AppUser> GetUsers()
    {
        var query = _imdbContext.AppUsers.ToList();
        return query;
    }
    
    //LoginUserValidation
    public AppUser? UserLogin(string email, string password)
    {
        var query = _imdbContext.AppUsers
            .FirstOrDefault(u => u.Email == email && u.Password == password);
        return query;
    }
    
    //GetUserById
    public AppUser? GetUserById(int id)
    {
        var query = _imdbContext.AppUsers.FirstOrDefault(u => u.Id == id);
        return query;
    }
    
    //DeleteUserById
    public bool DeleteUserById(int id)
    {
        var query = _imdbContext.AppUsers.FirstOrDefault(u => u.Id == id);
        if (query != null)
        {
             _imdbContext.AppUsers.Remove(query);
             _imdbContext.SaveChanges();
             return true;
        }
        return false;
    }
}
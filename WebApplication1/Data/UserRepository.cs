using WebApplication1.Models;

namespace WebApplication1.Data;

public class UserRepository : IUserRepository
{
    private DataContextEF _ef;

    public UserRepository(IConfiguration config)
    {
        _ef = new DataContextEF(config);
    }

    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users = _ef.Users.ToList(); // return user as list
        return users;
    }

    public User GetSingleUser(int userId)
    {
        User? user = _ef.Users.FirstOrDefault(x => x.UserId == userId);
        if (user != null)
        {
            return user;
        }

        throw new Exception("Fail to get user");
    }

    public bool SaveChanges()
    {
        return _ef.SaveChanges() > 0;
    }

    public void AddEntity<T>(T entity)
    {
        if (entity != null)
        {
            _ef.Add(entity);
        }
    }

    public void RemoveEntity<T>(T entity)
    {
        if (entity != null)
        {
            _ef.Remove(entity);
        }
    }
}
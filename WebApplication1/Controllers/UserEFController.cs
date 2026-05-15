using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
    DataContextEF _ef;
    IMapper _mapper;
    IUserRepository _userRepository;

    public UserEFController(IConfiguration config, IUserRepository userRepository)
    {
        _ef = new DataContextEF(config);
        _userRepository = userRepository;

        _mapper = new Mapper(new MapperConfiguration(cfg => { cfg.CreateMap<UserDto, User>(); }));

        Console.WriteLine(config.GetConnectionString("DefaultConnection"));
    }


    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        // IEnumerable<User> users = _ef.Users.ToList(); // return user list
        return _userRepository.GetUsers();
    }

    [HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
        return _userRepository.GetSingleUser(userId);
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        User? userDb = _userRepository.GetSingleUser(user.UserId);

        if (userDb != null)
        {
            userDb.FirstName = user.FirstName;
            userDb.LastName = user.LastName;
            userDb.Email = user.Email;
            userDb.Gender = user.Gender;
            userDb.Active = user.Active;
            if (_userRepository.SaveChanges())
            {
                return Ok();
            }

            throw new Exception("Fail to update user");
        }

        throw new Exception("Fail to get user");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserDto user)
    {
        User userDb = _mapper.Map<User>(user);

        // var userDb = new User
        // {
        //     FirstName = user.FirstName,
        //     LastName = user.LastName,
        //     Email = user.Email,
        //     Gender = user.Gender,
        //     Active = user.Active
        // };

        // _ef.Users.Add(userDb);

        _userRepository.AddEntity<User>(userDb);

        if (_userRepository.SaveChanges())
        {
            return Ok();
        }

        throw new Exception("Fail to add user");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        User? userDb = _ef.Users.FirstOrDefault(x => x.UserId == userId);

        // _ef.Users.Remove(userDb);
        if (_userRepository.SaveChanges())
            _userRepository.RemoveEntity<User>(userDb);
        {
            return Ok();
        }

        throw new Exception("Fail to delete user");
    }
}
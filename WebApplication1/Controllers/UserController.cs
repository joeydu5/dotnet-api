using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    DataContextDapper _dapper;

    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
        Console.WriteLine(config.GetConnectionString("DefaultConnection"));
    }


    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers()
    {
        var sql = @"
        SELECT [Users].[UserId],
               [Users].[FirstName],
               [Users].[LastName],
               [Users].[Email],
               [Users].[Gender],
               [Users].[Active]
        FROM TutorialAppSchema.Users AS Users
";

        IEnumerable<User> users = _dapper.LoadData<User>(sql);
        return users;
    }

    [HttpGet("GetSingleUser/{UserId}")]
    public User GetSingleUser(int UserId)
    {
        var sql = @"
        SELECT [Users].[UserId],
               [Users].[FirstName],
               [Users].[LastName],
               [Users].[Email],
               [Users].[Gender],
               [Users].[Active]
          FROM TutorialAppSchema.Users AS Users
         WHERE Users.UserId = " + UserId.ToString();

        User user = _dapper.LoadDataSingle<User>(sql);
        return user;
    }

    [HttpPut("EditUser")]
    public IActionResult EditUser(User user)
    {
        var sql = @$"
        UPDATE TutorialAppSchema.Users
        SET FirstName = '{user.FirstName}',
            LastName = '{user.LastName}',
            Email = '{user.Email}',
            Gender = '{user.Gender}',
            Active = '{user.Active}'
        WHERE UserId = '{user.UserId}'";

        Console.WriteLine(sql);

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Error occured when updating user");
    }

    [HttpPost("AddUser")]
    public IActionResult AddUser(UserDto user)
    {
        var sql = @$"
        INSERT INTO TutorialAppSchema.Users (FirstName, LastName, Email, Gender, Active)
        VALUES ('{user.FirstName}', '{user.LastName}', '{user.Email}', '{user.Gender}', '{user.Active}')";

        Console.WriteLine($"SQL ADD: ${sql}");

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Error occured when adding user");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        var sql = @$"
        DELETE FROM TutorialAppSchema.Users
        WHERE UserId = {userId}";

        Console.WriteLine($"SQL DELETE: ${sql}");

        if (_dapper.ExecuteSql(sql))
        {
            return Ok();
        }

        throw new Exception("Error occured when Deleting user");
    }
}
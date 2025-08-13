using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(AppDbContext dbContext) : DatingAppController
{
    [HttpPost]
    [Route("register")]
    public async Task<ActionResult<AppUser>> Register([FromBody] RegisterDto request)
    {
        if (await EmailExists(request.Email))
        {
            return BadRequest("Email already exists");
        }

        using var hmac = new HMACSHA512();

        var user = new AppUser
        {
            Email = request.Email,
            DisplayName = request.DisplayName,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
            PasswordSalt = hmac.Key
        };

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        return Created(string.Empty, user);
    }

    private async Task<bool> EmailExists(string email)
    {
        return await dbContext.Users.AnyAsync(user => user.Email.ToLower() == email.ToLower());
    }
}

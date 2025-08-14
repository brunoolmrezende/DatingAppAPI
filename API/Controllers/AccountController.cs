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

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AppUser>> Login([FromBody] LoginDto request)
    {
        var user = await dbContext.Users.SingleOrDefaultAsync(user => user.Email.ToLower() == request.Email.ToLower());

        if (user is null)
        {
            return Unauthorized("Invalid email or password");
        }

        using var hmac = new HMACSHA512(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return Unauthorized("Invalid email or password");
            }
        }

        return Ok(user);
    }

    private async Task<bool> EmailExists(string email)
    {
        return await dbContext.Users.AnyAsync(user => user.Email.ToLower() == email.ToLower());
    }
}

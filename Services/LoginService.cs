using capybara_api.Infra;
using capybara_api.Models.DTO;
using capybara_api.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace capybara_api.Services;

public class LoginService : BaseService {
    public LoginService(
    IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration,
    AppDbContext context,
    UserManager<IdentityUser> userManager,
    IDistributedCache cache)
    : base(httpContextAccessor, configuration, context, userManager, cache) { }

    public LoginResponse Login(Login loginData) {
        IdentityUser user = userManager.FindByEmailAsync(loginData.email).Result;

        if(user == null)
            throw new HttpResponseException(400, "INVALID_USERNAME");

        bool isValidPassword = userManager.CheckPasswordAsync(user, loginData.password).Result;

        if(!isValidPassword)
            throw new HttpResponseException(400, "INVALID_PASSWORD");

        IList<Claim> claims = userManager.GetClaimsAsync(user).Result;

        ClaimsIdentity subject = new(new Claim[] {
            new Claim(ClaimTypes.Email, loginData.email),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
        });
        subject.AddClaims(claims);

        byte[] key = Encoding.ASCII.GetBytes(configuration["JwtBearerTokenSettings:SecretKey"]);

        SecurityTokenDescriptor tokenDescriptor = new() {
            Subject = subject,
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = configuration["JwtBearerTokenSettings:Audience"],
            Issuer = configuration["JwtBearerTokenSettings:Issuer"],
            Expires = DateTime.UtcNow.AddDays(31),
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        string tokenSerialized = tokenHandler.WriteToken(token);

        LoginResponse loginResponse = new() {
            token = tokenSerialized,
            email = loginData.email,
            name = claims.FirstOrDefault(c => c.Type == "name").Value,
        };

        return loginResponse;
    }
}

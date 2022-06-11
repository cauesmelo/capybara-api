using capybara_api.Infra;
using capybara_api.Models.DTO;
using capybara_api.Utils;
using Microsoft.AspNetCore.Identity;
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
    UserManager<IdentityUser> userManager)
    : base(httpContextAccessor, configuration, context, userManager) { }

    public string Login(Login loginData) {
        IdentityUser user = userManager.FindByEmailAsync(loginData.email).Result;

        if(user == null)
            throw new HttpResponseException(400, "INVALID_USERNAME");

        bool isValidPassword = userManager.CheckPasswordAsync(user, loginData.password).Result;

        if(!isValidPassword)
            throw new HttpResponseException(400, "INVALID_PASSWORD");

        byte[] key = Encoding.ASCII.GetBytes(configuration["JwtBearerTokenSettings:SecretKey"]);

        SecurityTokenDescriptor tokenDescriptor = new() {
            Subject = new ClaimsIdentity(new Claim[] {
            new Claim(ClaimTypes.Email, loginData.email),
        }),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Audience = configuration["JwtBearerTokenSettings:Audience"],
            Issuer = configuration["JwtBearerTokenSettings:Issuer"]
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}

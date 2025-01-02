using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PoetAPI.Data
{
    public class JWTUtil
    {
        public static string GenerateJwtToken(string username)
        {
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Name, username)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("notsosecretyetihbuasdf78231e8905132b4f701234f8719304pnf8p"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "todoFIll",
                audience: "todoFIll-audience",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

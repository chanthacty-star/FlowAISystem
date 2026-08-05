//using FlowAISystem.Application.Interfaces.Services;

//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using FlowAISystem.Application.Interfaces;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;

//namespace FlowAISystem.Application.Security;

//public class JwtService : IJwtService
//{
//    private readonly IConfiguration _configuration;

//    public JwtService(IConfiguration configuration)
//    {
//        _configuration = configuration;
//    }

//    public string GenerateToken(
//        int userId,
//        string username,
//        string role)
//    {
//        var key = _configuration["Jwt:Key"]!;
//        var issuer = _configuration["Jwt:Issuer"]!;
//        var audience = _configuration["Jwt:Audience"]!;

//        int expireHours =
//            int.TryParse(
//                _configuration["Jwt:ExpireHours"],
//                out var hours)
//                    ? hours
//                    : 2;

//        //var claims = new[]
//        //{
//        //    new Claim(
//        //        JwtRegisteredClaimNames.Sub,
//        //        userId.ToString()),

//        //    new Claim(
//        //        JwtRegisteredClaimNames.UniqueName,
//        //        username),

//        //    new Claim(
//        //        ClaimTypes.Name,
//        //        username),

//        //    new Claim(
//        //        ClaimTypes.Role,
//        //        role)
//        //};
//        var claims = new[]
//            {
//        // User Id for Blazor AuthenticationStateProvider
//        new Claim(
//            ClaimTypes.NameIdentifier,
//            userId.ToString()),


//        // Custom UserId (keep if you use it somewhere else)
//        new Claim(
//            "UserId",
//            userId.ToString()),


//        // JWT Subject
//        new Claim(
//            JwtRegisteredClaimNames.Sub,
//            userId.ToString()),


//        // Username
//        new Claim(
//            JwtRegisteredClaimNames.UniqueName,
//            username),


//        new Claim(
//            ClaimTypes.Name,
//            username),


//        // Role
//        new Claim(
//            ClaimTypes.Role,
//            role)
//    };

//        var securityKey =
//            new SymmetricSecurityKey(
//                Encoding.UTF8.GetBytes(key));

//        var credentials =
//            new SigningCredentials(
//                securityKey,
//                SecurityAlgorithms.HmacSha256);

//        var token =
//            new JwtSecurityToken(
//                issuer,
//                audience,
//                claims,
//                expires: DateTime.UtcNow.AddHours(expireHours),
//                signingCredentials: credentials);

//        return new JwtSecurityTokenHandler()
//            .WriteToken(token);
//    }
//}
using FlowAISystem.Application.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FlowAISystem.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FlowAISystem.Application.Security;

public class JwtService : IJwtService
{
    private readonly IConfiguration _configuration;


    public JwtService(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }



    public string GenerateToken(
        int userId,
        string username,
        string role)
    {

        var key =
            _configuration["Jwt:Key"]!;


        var issuer =
            _configuration["Jwt:Issuer"]!;


        var audience =
            _configuration["Jwt:Audience"]!;



        var expireHours =
            int.TryParse(
                _configuration["Jwt:ExpireHours"],
                out var hours)
                ? hours
                : 2;




        var claims = new[]
        {

            // ======================
            // USER ID
            // ======================

            new Claim(
                ClaimTypes.NameIdentifier,
                userId.ToString()),


            new Claim(
                "UserId",
                userId.ToString()),


            new Claim(
                JwtRegisteredClaimNames.Sub,
                userId.ToString()),



            // ======================
            // USERNAME
            // ======================

            new Claim(
                JwtRegisteredClaimNames.UniqueName,
                username),


            new Claim(
                ClaimTypes.Name,
                username),




            // ======================
            // ROLE
            // ======================

            // For Blazor
            new Claim(
                ClaimTypes.Role,
                role),


            // For JWT standard
            new Claim(
                "role",
                role)

        };




        var securityKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key));



        var credentials =
            new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);




        var token =
            new JwtSecurityToken(

                issuer,

                audience,

                claims,


                expires:
                    DateTime.UtcNow
                    .AddHours(expireHours),


                signingCredentials:
                    credentials

            );



        return new JwtSecurityTokenHandler()
            .WriteToken(token);

    }

}
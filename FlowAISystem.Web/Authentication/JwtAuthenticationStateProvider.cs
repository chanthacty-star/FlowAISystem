//using Microsoft.AspNetCore.Components.Authorization;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;

//namespace FlowAISystem.Web.Authentication;


//public class JwtAuthenticationStateProvider
//    : AuthenticationStateProvider
//{

//    private readonly TokenStorage _storage;



//    private readonly ClaimsPrincipal _anonymous =
//        new(new ClaimsIdentity());



//    public JwtAuthenticationStateProvider(
//        TokenStorage storage)
//    {
//        _storage = storage;
//    }





//    public override async Task<AuthenticationState>
//        GetAuthenticationStateAsync()
//    {

//        try
//        {

//            var token =
//                await _storage.GetTokenAsync();



//            if (string.IsNullOrWhiteSpace(token))
//            {
//                return new AuthenticationState(
//                    _anonymous);
//            }



//            var handler =
//                new JwtSecurityTokenHandler();



//            var jwt =
//                handler.ReadJwtToken(token);



//            var claims =
//                jwt.Claims.ToList();


//            // Find role claim
//            var roleClaim =
//                claims.FirstOrDefault(
//                    c =>
//                        c.Type == "role" ||
//                        c.Type == "Role" ||
//                        c.Type == ClaimTypes.Role ||
//                        c.Type ==
//                        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
//                );


//            // Add Blazor role claim
//            if (roleClaim != null &&
//               !claims.Any(c =>
//                   c.Type == ClaimTypes.Role))
//            {
//                claims.Add(
//                    new Claim(
//                        ClaimTypes.Role,
//                        roleClaim.Value));
//            }



//            var identity =
//                new ClaimsIdentity(
//                    claims,
//                    "jwt",
//                    ClaimTypes.Name,
//                    ClaimTypes.Role);



//            var user =
//                new ClaimsPrincipal(identity);


//            return new AuthenticationState(user);

//        }

//        catch
//        {

//            return new AuthenticationState(
//                _anonymous);

//        }

//    }






//    public async Task NotifyUserAuthentication(
//        string token)
//    {

//        await _storage.SaveTokenAsync(token);



//        NotifyAuthenticationStateChanged(
//            GetAuthenticationStateAsync());

//    }






//    public async Task NotifyUserLogout()
//    {

//        await _storage.RemoveTokenAsync();



//        NotifyAuthenticationStateChanged(
//            GetAuthenticationStateAsync());

//    }

//}

using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FlowAISystem.Web.Authentication;


public class JwtAuthenticationStateProvider
    : AuthenticationStateProvider
{

    private readonly TokenStorage _storage;



    private readonly ClaimsPrincipal _anonymous =
        new(new ClaimsIdentity());



    public JwtAuthenticationStateProvider(
        TokenStorage storage)
    {
        _storage = storage;
    }





    public override async Task<AuthenticationState>
        GetAuthenticationStateAsync()
    {

        try
        {

            var token =
                await _storage.GetTokenAsync();



            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(
                    _anonymous);
            }




            var handler =
                new JwtSecurityTokenHandler();



            var jwt =
                handler.ReadJwtToken(token);



            var claims =
                jwt.Claims.ToList();




            // ==========================
            // USER ID
            // ==========================

            var userId =
                jwt.Claims
                .FirstOrDefault(c =>
                    c.Type == "UserId" ||
                    c.Type == "sub")
                ?.Value;



            if (!string.IsNullOrWhiteSpace(userId)
               &&
               !claims.Any(c =>
                   c.Type == "UserId"))
            {
                claims.Add(
                    new Claim(
                        "UserId",
                        userId));
            }




            // ==========================
            // ROLE
            // ==========================

            var role =
                jwt.Claims
                .FirstOrDefault(c =>
                    c.Type == "role" ||
                    c.Type == ClaimTypes.Role ||
                    c.Type ==
                    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
                ?.Value;




            if (!string.IsNullOrWhiteSpace(role))
            {

                // remove duplicate role
                claims.RemoveAll(
                    c => c.Type == ClaimTypes.Role);



                claims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        role));

            }





            // ==========================
            // IDENTITY
            // ==========================

            var identity =
                new ClaimsIdentity(
                    claims,
                    "jwt",
                    ClaimTypes.Name,
                    ClaimTypes.Role);




            var user =
                new ClaimsPrincipal(identity);



            return new AuthenticationState(user);


        }


        catch (InvalidOperationException)
        {
            // ProtectedLocalStorage is not ready yet
            return new AuthenticationState(
                _anonymous);
        }


        catch
        {
            return new AuthenticationState(
                _anonymous);
        }

    }







    public async Task NotifyUserAuthentication(
        string token)
    {

        await _storage.SaveTokenAsync(token);



        NotifyAuthenticationStateChanged(
            GetAuthenticationStateAsync());

    }







    public async Task NotifyUserLogout()
    {

        await _storage.RemoveTokenAsync();



        NotifyAuthenticationStateChanged(
            GetAuthenticationStateAsync());

    }


}
//using Microsoft.AspNetCore.Components.Authorization;
//using System.Security.Claims;

//namespace FlowAISystem.Web.Authentication;

//public class JwtAuthenticationStateProvider : AuthenticationStateProvider
//{
//    private readonly ClaimsPrincipal _anonymous =
//        new(new ClaimsIdentity());

//    public override Task<AuthenticationState> GetAuthenticationStateAsync()
//    {
//        return Task.FromResult(
//            new AuthenticationState(_anonymous));
//    }

//    public void NotifyUserAuthentication(
//        string username,
//        string role)
//    {
//        var identity = new ClaimsIdentity(
//        [
//            new Claim(ClaimTypes.Name, username),
//            new Claim(ClaimTypes.Role, role)
//        ], "jwt");

//        var user = new ClaimsPrincipal(identity);

//        NotifyAuthenticationStateChanged(
//            Task.FromResult(
//                new AuthenticationState(user)));
//    }

//    public void NotifyUserLogout()
//    {
//        NotifyAuthenticationStateChanged(
//            Task.FromResult(
//                new AuthenticationState(_anonymous)));
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



            // Convert role claim for Blazor Authorization
            var roleClaim =
                claims.FirstOrDefault(
                    c =>
                    c.Type == "role" ||
                    c.Type == "Role" ||
                    c.Type == ClaimTypes.Role);



            if (roleClaim != null)
            {

                claims.Add(
                    new Claim(
                        ClaimTypes.Role,
                        roleClaim.Value));

            }





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
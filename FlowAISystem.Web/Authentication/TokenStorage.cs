//using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

//namespace FlowAISystem.Web.Authentication;

//public class TokenStorage
//{
//    private const string TokenKey = "authToken";

//    private readonly ProtectedLocalStorage _storage;

//    public TokenStorage(
//        ProtectedLocalStorage storage)
//    {
//        _storage = storage;
//    }

//    public async Task SaveTokenAsync(string token)
//    {
//        await _storage.SetAsync(TokenKey, token);
//    }

//    public async Task<string?> GetTokenAsync()
//    {
//        var result = await _storage.GetAsync<string>(TokenKey);

//        return result.Success
//            ? result.Value
//            : null;
//    }

//    public async Task RemoveTokenAsync()
//    {
//        await _storage.DeleteAsync(TokenKey);
//    }
//}


using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace FlowAISystem.Web.Authentication;

public class TokenStorage
{
    private const string TokenKey = "authToken";

    private readonly ProtectedLocalStorage _storage;


    public TokenStorage(
        ProtectedLocalStorage storage)
    {
        _storage = storage;
    }



    public async Task SaveTokenAsync(string token)
    {
        await _storage.SetAsync(
            TokenKey,
            token);
    }



    public async Task<string?> GetTokenAsync()
    {
        var result =
            await _storage.GetAsync<string>(
                TokenKey);


        return result.Success
            ? result.Value
            : null;
    }



    public async Task RemoveTokenAsync()
    {
        await _storage.DeleteAsync(
            TokenKey);
    }
}
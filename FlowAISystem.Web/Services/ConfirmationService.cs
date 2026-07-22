namespace FlowAISystem.Web.Services;

public class ConfirmationService
{
    public event Func<string, Task<bool>>? OnConfirm;


    public async Task<bool> ConfirmAsync(string message)
    {
        if (OnConfirm == null)
            return false;


        var handler =
            (Func<string, Task<bool>>)
            OnConfirm
            .GetInvocationList()
            .First();


        return await handler(message);
    }
}
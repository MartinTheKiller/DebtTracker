namespace DebtTracker.App.Services;

public class IdentityService
{
    public string AccessToken { get; private set; } = string.Empty;

    public void SetAccessToken(string accessToken)
    {
        AccessToken = accessToken;
    }
}
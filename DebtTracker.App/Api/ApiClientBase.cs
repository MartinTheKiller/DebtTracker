using DebtTracker.App.Services;

namespace DebtTracker.App.Api;

public class ApiClientBase : IApiClient
{
    private readonly IdentityService _identityService;

    protected ApiClientBase(ApiClientConfiguration configuration)
    {
        _identityService = configuration.IdentityService;
    }

    protected Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
    {
        var request = new HttpRequestMessage();
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _identityService.AccessToken);
        return Task.FromResult(request);
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DebtTracker.App.Api;
using DebtTracker.App.Services;
using DebtTracker.Common.ApiMessages;
using DebtTracker.Common.Models;

namespace DebtTracker.App.ViewModels;

public partial class MainPageViewModel : ViewModelBase
{
    private readonly IdentityService _identityService;
    private readonly IAuthenticationClient _authClient;
    private readonly IDebtsClient _debtsClient;


    [ObservableProperty]
    public string email = string.Empty;
    
    [ObservableProperty]
    public string password = string.Empty;

    [ObservableProperty]
    public DebtDetailModel newDebt = DebtDetailModel.Empty;

    [ObservableProperty] 
    public string response = "Response";

    public MainPageViewModel(
        IdentityService identityService,
        IAuthenticationClient authClient,
        IDebtsClient debtsClient)
    {
        _identityService = identityService;
        _authClient = authClient;
        _debtsClient = debtsClient;
    }

    [RelayCommand]
    public async Task LoginAsync()
    {
        AuthenticateResponse authResponse;

        try
        {
            authResponse = await _authClient.AuthenticateAsync(new AuthenticateRequest
            {
                Email = Email,
                Password = Password
            });
        }
        catch (ApiException e)
        {
            await Shell.Current.DisplayAlert("Error", e.Message, "OK");
            return;
        }

        _identityService.SetAccessToken(authResponse.AccessToken);

        Response = authResponse.AccessToken;
    }

    [RelayCommand]
    public async Task CreateDebtAsync()
    {
        DebtDetailModel savedModel;

        try
        {
            savedModel = await _debtsClient.SaveDebtAsync(
                NewDebt with
                {
                    CreditorId = Guid.Parse("7fa0a0b3-4f8e-4650-8d50-f1a3634bf153"),
                    DebtorId = Guid.Parse("1a2b20f1-a388-4f7e-899d-1bb812cf96e0"),
                    GroupId = Guid.Parse("480906a5-b7f5-4000-95bf-a87b4675c5e2")
                });
        }
        catch (ApiException e)
        {
            await Shell.Current.DisplayAlert("Error", e.Message, "OK");
            return;
        }

        Response = $"Saved:\n{savedModel}";
    }

    [RelayCommand]
    public async Task GetDebtsAsync()
    {
        IEnumerable<DebtListModel> debts;

        try
        {
            debts = await _debtsClient.GetDebtsAllAsync();
        }
        catch (ApiException e)
        {
            await Shell.Current.DisplayAlert("Error", e.Message, "OK");
            return;
        }

        Response = string.Join("\n", debts.Select(d => d.Name + " - " + d.Amount));
    }
}
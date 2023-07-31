using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace DebtTracker.App.ViewModels;

public abstract partial class ViewModelBase : ObservableRecipient, IViewModel
{
    public virtual Task OnAppearingAsync() => Task.CompletedTask;

    [RelayCommand]
    private Task GoBackAsync()
    {
        if (Shell.Current.Navigation.NavigationStack.Count > 1) 
            Shell.Current.SendBackButtonPressed();

        return Task.CompletedTask;
    }
}
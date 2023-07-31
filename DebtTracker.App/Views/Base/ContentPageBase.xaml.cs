using DebtTracker.App.ViewModels;

namespace DebtTracker.App.Views;

public abstract partial class ContentPageBase : ContentPage
{
    protected IViewModel viewModel;

    protected ContentPageBase(
        IViewModel viewModel)
    {
        BindingContext = this.viewModel = viewModel;
    }
}
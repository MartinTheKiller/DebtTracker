using DebtTracker.App.ViewModels;

namespace DebtTracker.App.Views
{
    public partial class MainPage : ContentPageBase
    {
        public MainPage(
            IViewModel viewModel) 
            : base(viewModel)
        {
            InitializeComponent();
        }
    }
}

using MauiBankingExercise.ViewModels;

//namespace MauiBankingExercise.Views;

//public partial class CustomerDashboardView : BasePage
//{
//    public CustomerDashboardView(CustomerDashboardViewModel vm)
//    {
//        InitializeComponent();
//        BindingContext = vm;
//    }
//}
using MauiBankingExercise.ViewModels;

namespace MauiBankingExercise.Views
{
    public partial class CustomerDashboardView : BasePage
    {
        private readonly CustomerDashboardViewModel _viewModel;

        public CustomerDashboardView(CustomerDashboardViewModel vm)
        {
            InitializeComponent();
            _viewModel = vm;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (_viewModel != null)
            {
                await _viewModel.OnAppearingAsync();
            }
        }
    }
}

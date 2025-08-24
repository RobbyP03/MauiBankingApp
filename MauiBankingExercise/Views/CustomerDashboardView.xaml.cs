
using MauiBankingExercise.ViewModels;

namespace MauiBankingExercise.Views;

public partial class CustomerDashboardView : BasePage
{
    public CustomerDashboardView(CustomerDashboardViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
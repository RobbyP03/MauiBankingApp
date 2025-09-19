using MauiBankingExercise.ViewModels;

namespace MauiBankingExercise.Views;

public partial class TransactionView : BasePage
{
    private TransactionViewModel _viewModel;

    public TransactionView(TransactionViewModel vm)
    {
        InitializeComponent();
        _viewModel = vm;
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Call async data loading safely
        _ = _viewModel.OnAppearingAsync();
    }
}

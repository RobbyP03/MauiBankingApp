using MauiBankingExercise.ViewModels;

namespace MauiBankingExercise.Views;

public partial class TransactionView : BasePage
{
    public TransactionView(TransactionViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}

using MauiBankingExercise.ViewModels;

namespace MauiBankingExercise.Views;

public partial class ListOfCustomersView : BasePage
{
	private ListOfCustomersViewModel _viewModel;
	public ListOfCustomersView(ListOfCustomersViewModel vm)
	{
		InitializeComponent();
		_viewModel = vm;
		BindingContext = vm;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		_viewModel.OnAppearing();
    }
}
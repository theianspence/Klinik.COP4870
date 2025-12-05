using Maui.Klinik.ViewModels;

namespace Maui.Klinik.Views;

public partial class PhysicianListPage : ContentPage
{
    private readonly PhysicianListViewModel _viewModel;

    public PhysicianListPage(PhysicianListViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.RefreshCommand.Execute(null);
    }
}

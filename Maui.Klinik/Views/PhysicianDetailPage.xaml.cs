using Maui.Klinik.ViewModels;

namespace Maui.Klinik.Views;

public partial class PhysicianDetailPage : ContentPage
{
    public PhysicianDetailPage(PhysicianDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

using Maui.Klinik.ViewModels;

namespace Maui.Klinik.Views;

public partial class AppointmentDetailPage : ContentPage
{
    public AppointmentDetailPage(AppointmentDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

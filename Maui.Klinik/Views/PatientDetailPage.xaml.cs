using Maui.Klinik.ViewModels;

namespace Maui.Klinik.Views;

public partial class PatientDetailPage : ContentPage
{
    public PatientDetailPage(PatientDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}

using Maui.Klinik.Views;

namespace Maui.Klinik;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// Register navigation routes
		Routing.RegisterRoute("patientdetail", typeof(PatientDetailPage));
		Routing.RegisterRoute("physiciandetail", typeof(PhysicianDetailPage));
		Routing.RegisterRoute("appointmentdetail", typeof(AppointmentDetailPage));

		// Handle tab navigation to ensure we're always at the root
		this.Navigated += OnNavigated;
	}

	private async void OnNavigated(object? sender, ShellNavigatedEventArgs e)
	{
		// When navigating to a tab (not a modal or detail page), pop to root
		if (e.Source == ShellNavigationSource.ShellSectionChanged ||
			e.Source == ShellNavigationSource.ShellItemChanged)
		{
			// Use Dispatcher to avoid conflicting with ongoing navigation
			await Dispatcher.DispatchAsync(async () =>
			{
				try
				{
					// Pop all pages in the current navigation stack to get back to the list view
					if (Current?.Navigation?.NavigationStack?.Count > 1)
					{
						await Current.Navigation.PopToRootAsync(false);
					}
				}
				catch (InvalidOperationException)
				{
					// Ignore if navigation is still pending
				}
			});
		}
	}
}

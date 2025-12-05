using Microsoft.Extensions.Logging;
using Library.Klinik.Services;
using Maui.Klinik.Services;
using Maui.Klinik.ViewModels;
using Maui.Klinik.Views;

namespace Maui.Klinik;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		// Register Services
		builder.Services.AddSingleton<PatientService>();
		builder.Services.AddSingleton<PhysicianService>();
		builder.Services.AddSingleton<AppointmentService>();
		
		// Register API Services (optional - comment out to use local services only)
		builder.Services.AddSingleton<PatientApiService>(sp => new PatientApiService("localhost", 5149));

		// Register ViewModels
		builder.Services.AddTransient<PatientListViewModel>();
		builder.Services.AddTransient<PatientDetailViewModel>();
		builder.Services.AddTransient<PhysicianListViewModel>();
		builder.Services.AddTransient<PhysicianDetailViewModel>();
		builder.Services.AddTransient<AppointmentListViewModel>();
		builder.Services.AddTransient<AppointmentDetailViewModel>();

		// Register Pages
		builder.Services.AddTransient<PatientListPage>();
		builder.Services.AddTransient<PatientDetailPage>();
		builder.Services.AddTransient<PhysicianListPage>();
		builder.Services.AddTransient<PhysicianDetailPage>();
		builder.Services.AddTransient<AppointmentListPage>();
		builder.Services.AddTransient<AppointmentDetailPage>();

		return builder.Build();
	}
}

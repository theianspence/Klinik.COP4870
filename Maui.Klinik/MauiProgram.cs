using Microsoft.Extensions.Logging;
using Library.Klinik.Services;
using Maui.Klinik.Services;
using Maui.Klinik.ViewModels;
using Maui.Klinik.Views;
using System.Threading.Tasks;

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

		// Register API Services first (optional - comment out to use local services only)
		builder.Services.AddSingleton<PatientApiService>(sp => new PatientApiService("localhost", 5149));

		// Register Services. PatientService will be pre-populated from the API if available.
		// Preload is done in a background task so app startup doesn't block if the API is unavailable.
		builder.Services.AddSingleton<PatientService>(sp =>
		{
			var svc = new PatientService();
			var api = sp.GetService<PatientApiService>();
			if (api != null)
			{
				// Run preload on a background thread to avoid blocking the UI/startup thread.
				Task.Run(async () =>
				{
					try
					{
						var dtos = await api.GetAllPatientsAsync().ConfigureAwait(false);
						foreach (var dto in dtos)
						{
							var model = PatientApiService.ToModel(dto);
							svc.AddPatient(model);
						}
					}
					catch
					{
						// ignore preload errors and leave PatientService empty (local mode)
					}
				});
			}
			return svc;
		});
		builder.Services.AddSingleton<PhysicianService>();
		builder.Services.AddSingleton<AppointmentService>();

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

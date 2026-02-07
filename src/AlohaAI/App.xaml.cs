namespace AlohaAI;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		AppDomain.CurrentDomain.UnhandledException += (s, e) =>
			System.Diagnostics.Debug.WriteLine($"Unhandled: {e.ExceptionObject}");

		TaskScheduler.UnobservedTaskException += (s, e) =>
		{
			System.Diagnostics.Debug.WriteLine($"Unobserved task: {e.Exception}");
			e.SetObserved();
		};
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}
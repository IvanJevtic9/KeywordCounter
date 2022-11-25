using KeyWordCounterApp;

// Application start

var source = new CancellationTokenSource();

CLI.Instance.RunApplicationCLI();

CLI.Instance.StopApplication += Instance_StopApplication;

void Instance_StopApplication(object sender, StopApplicationArgs e)
{
    source.Cancel();
    // Dispose objects
}
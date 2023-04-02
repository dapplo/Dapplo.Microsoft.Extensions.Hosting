using Dapplo.Microsoft.Extensions.Hosting.WinUI;
using Hosting.Sample.WinUI;
using Microsoft.Extensions.Hosting;

var host = Host.CreateDefaultBuilder()
    .ConfigureWinUI<App, MainWindow>()
    .Build();

host.Run();

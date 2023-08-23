using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using TwoHundred.Server;

using var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webHostBuilder =>
    {
        webHostBuilder.UseStartup<Startup>();
    }).Build();

await host.StartAsync();
await host.WaitForShutdownAsync();
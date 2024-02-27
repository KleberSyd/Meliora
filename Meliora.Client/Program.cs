using Meliora.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped<IMelioraService, MelioraService>();

await builder.Build().RunAsync();

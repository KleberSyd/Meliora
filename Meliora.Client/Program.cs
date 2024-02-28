using Meliora.Services.CookiesKristen;
using Meliora.Services.CookiesKristen.Interfaces;
using Meliora.Services.Petz;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
// Add services to the container.
//Petz Services
builder.Services.AddTransient<IPetzService, PetzService>();

// Cookie Kristen Services
builder.Services.AddTransient<IMelioraService, MelioraService>();
builder.Services.AddTransient<IMailHoglService, MailHogService>();

builder.Services.AddSingleton<IMailNotificationService>(new MailNotificationService());

await builder.Build().RunAsync();

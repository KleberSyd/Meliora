using Meliora.Services.CookiesKristen;
using Meliora.Services.CookiesKristen.Interfaces;
using Meliora.Services.Petz;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
// Add services to the container.
//Petz Services
builder.Services.AddScoped<IPetzService, PetzService>();

// Cookie Kristen Services
builder.Services.AddScoped<IMelioraService, MelioraService>();
builder.Services.AddScoped<IMailHoglService, MailHogService>();

builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddSingleton<IMailNotificationService>(new MailNotificationService());

await builder.Build().RunAsync();

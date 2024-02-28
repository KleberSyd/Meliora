using Meliora.Components;
using Meliora.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IMelioraService, MelioraService>();
builder.Services.AddScoped<IEmailService, EmailService>();

//builder.Services.AddDbContext<PetzDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("PetzConnection")));

//builder.Services.AddDbContext<CookieKristenDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("CookieKristenConnection")));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Meliora.Client._Imports).Assembly);

app.Run();

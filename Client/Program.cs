using Aon_PMS.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Aon_PMS.Services;
using RCL.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after"); 
builder.Services.AddMudServices();

builder.Services.AddScoped<Notify, Notify>();
builder.Services.AddScoped<HttpService, HttpService>();
builder.Services.AddScoped<AccessSvc, AccessSvc>();
builder.Services.AddScoped<LoginServices>();
builder.Services.AddScoped<Aon_PMS.Services.LocalStorageAccessor>();

builder.Services.ConfigAServ(a =>
{
    a.ApplicationID = Guid.Parse("CC730BCB-4FA1-4FAC-A6EC-617E381C1C35");
    a.BaseAddress = new Uri("https://localhost:7122/");
});


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "api/") });

await builder.Build().RunAsync();

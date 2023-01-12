using ClientAPI;
using FFSmartPlus_Website;
using MatBlazor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMatBlazor();
builder.Services.AddScoped<FFSBackEnd>(sp => new FFSBackEnd(builder.Configuration["APIBase"], new HttpClient()));
await builder.Build().RunAsync();

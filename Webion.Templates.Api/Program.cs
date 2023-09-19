using Kaire.Templates.Api.Config;
using Kaire.Templates.Api.Extensions;
using Webion.Templates.Infrastructure.Extensions;
using Webion.Firestore.Extensions;
using Webion.Templates.Firestore.Context;
using Webion.Templates.Mustache.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.Add<ControllersConfig>();

services.AddFirestore<TemplatesFirestoreDbContext>(
    Environment.GetEnvironmentVariable("project-id") ?? "",
    false
);
services.AddRepositories();
services.AddTemplateProcess();

var app = builder.Build();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://0.0.0.0:{port}");

app.UseRouting();
app.UseDeveloperExceptionPage();

app.Use<ControllersConfig>();

app.Run();
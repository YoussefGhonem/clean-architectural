using Elearninig.Getway.Helpers;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.ConfigureServices();

app.Configure(app.Environment);


app.Run();

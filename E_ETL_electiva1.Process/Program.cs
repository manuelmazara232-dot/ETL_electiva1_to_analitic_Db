using System.Configuration;
using E_ETL_electiva1.api.context;
using E_ETL_electiva1.Data.Repositories;
using E_ETL_electiva1.Entities.interfaces;
using E_ETL_electiva1.Entities.interfaces.Iservices;
using E_ETL_electiva1.Process;
using E_ETL_electiva1.Process.services;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

// --- Base de datos transaccional (origen), leída desde App.config ---
var connStringTrans = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStringBdTrans"].ConnectionString;
builder.Services.AddDbContext<opiniones_de_clientesDBContext>(options =>
    options.UseSqlServer(connStringTrans));

// --- Fuente CSV ---
builder.Services.AddScoped<ICsvRepository, CsvRepo>();

// --- Fuente API REST ---
builder.Services.AddHttpClient<IApiConsRepository, apiRepo>();

// --- Servicios de carga por fuente ---
builder.Services.AddScoped<ICsvService, CsvService>();
builder.Services.AddScoped<ITransDbService, DbTransService>();
builder.Services.AddScoped<IApiService, apiService>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
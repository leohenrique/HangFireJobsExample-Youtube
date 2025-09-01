using Hangfire;
using HangFireJobsExample_Youtube;
using HangFireJobsExample_Youtube.Repositorio_NH;
using HangFireJobsExample_Youtube.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Serilog;
using System.Data;
using System.Data.SqlClient;
using Serilog;
using Serilog.Sinks.Graylog;
using Serilog.Sinks.Graylog.Core.Transport;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HangFireJobsExample_Youtube.Repositorio_NH.Interfaces;

//// graylog - admin/admin

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
  c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
  {
      Title = "Minha API",
      Version = "v1"
  })
);

builder.Services.AddHangfire(config => config.UseSqlServerStorage(connectionString));
builder.Services.AddHangfireServer();

var sessionFactory = NHibernateHelper.CreateSessionFactory();
builder.Services.AddSingleton(sessionFactory);
builder.Services.AddScoped(provider => sessionFactory.OpenSession());

builder.Services.AddSingleton<IDbConnection>(sp => new Microsoft.Data.SqlClient.SqlConnection(connectionString));
builder.Services.AddSingleton<IUserRepository, UserRepository>();

/// Com NHibernate
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>(); /// Uma única instância de UsuarioRepository será criada para toda a aplicação. Essa instância será reutilizada em todas as requisições e injeções.
builder.Services.AddScoped<ContatoRepository>(); ///  Uma nova instância de ContatoRepository será criada para cada requisição HTTP.

/////////// Memory Cache
builder.Services.AddMemoryCache();
//using IHost host = builder.Build();


var app = builder.Build();
IMemoryCache cache =
    app.Services.GetRequiredService<IMemoryCache>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseHangfireDashboard();

app.UseAuthorization();

app.MapControllers();


app.MapPost("usuarios/agendar", (IUserRepository userRepository, [FromBody] User user) =>
{
    BackgroundJob.Schedule(() => userRepository.UserInsert(user.Nome, user.Email), TimeSpan.FromMinutes(1));
    return Results.Ok("Usuário agendado para inserção em 1 minuto!"); 
});


app.MapPost("usuarios/enfileirar", (IUserRepository userRepository, [FromBody] User user) =>
{
    BackgroundJob.Enqueue(() => userRepository.UserInsert(user.Nome, user.Email));
    return Results.Ok("Usuário enfileirado!");
});


//////////// Serilog
var log = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Graylog(new GraylogSinkOptions
    {
        HostnameOrAddress = "127.0.0.1",
        Port = 12201,
        TransportType = TransportType.Udp
    })
    .CreateLogger();

log.Information("Teste sem appsettings.json");


app.Run();

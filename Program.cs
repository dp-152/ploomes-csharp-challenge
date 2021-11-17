using Microsoft.EntityFrameworkCore;

using Newtonsoft.Json.Serialization;

using PloomesCsharpChallenge.Contexts;
using PloomesCsharpChallenge.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(s =>
    s.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<MainContext>(options =>
    options.UseSqlServer(builder.Configuration["PloomesChallenge:ConnectionString"]));
builder.Services.AddScoped<IUserRepository, SqlUserRepository>();
// builder.Services.AddSingleton<IUserRepository, MockUserRepository>();
builder.Services.AddScoped<IMessageRepository, SqlMessageRepository>();
// builder.Services.AddSingleton<IMessageRepository, MockMessageRepository>();
builder.Services.AddScoped<IChatRepository, SqlChatRepository>();
// builder.Services.AddSingleton<IChatRepository, MockChatRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

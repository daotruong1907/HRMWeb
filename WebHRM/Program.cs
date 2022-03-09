using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Text.Json;
using WebHRM.Interface;
using WebHRM.Models;
using WebHRM.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//dynamic CnString = JObject.Parse(ConnectionStrings);
//string conn = CnString.ContainsKey("Data Source=PM-TRUONGDV;Initial Catalog=HRM;Integrated Security=True;");
//FIXME
builder.Services.AddDbContext<HRMContext>(
        options => options.UseSqlServer("Data Source=PM-TRUONGDV;Initial Catalog=HRM;Integrated Security=True;"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IAccountsService, AccountsService>();

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

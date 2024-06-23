using DatabaseApi.Services;
using System.Data;
using ClassLibrary.Entities;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<TableItem>();
builder.Services.AddScoped<IMySqlService,MySqlService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add CORS policy

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorApp",
        builder =>
        {
            builder.WithOrigins("https://localhost:7002") // URL of your Blazor app
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowBlazorApp");

app.MapControllers();

app.Run();





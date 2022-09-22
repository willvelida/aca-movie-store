using Microsoft.EntityFrameworkCore;
using MovieStore.Catalog.Repository;
using MovieStore.Catalog.Repository.Interfaces;
using MovieStore.Catalog.Services;
using MovieStore.Catalog.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddDbContext<MovieContext>(opt => opt.UseInMemoryDatabase("MovieDB"));
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

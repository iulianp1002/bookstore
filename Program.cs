using BookstoreAPI.Repository;
using BookstoreAPI.Service;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Build.Framework;
using Microsoft.Extensions.FileProviders;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//Serilog setup
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "BookstorePolicy",
                      policy =>
                      {
                          policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                      });
});
builder.Services.Configure<FormOptions>(options =>
{
    options.ValueLengthLimit = int.MaxValue;
    options.MultipartBodyLengthLimit = int.MaxValue;
    options.MemoryBufferThreshold = int.MaxValue;
});

//register custom Interfaces and their implementation
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IBookAuthorsRepository, BookAuthorsRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
    RequestPath = new PathString("/Resources")
});

app.UseCors("BookstorePolicy");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDataAccess()
    .AddDomainServices()
    .AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost44356", policy =>
    {
        policy.WithOrigins("https://localhost:44356")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowLocalhost44356");

app.UseAuthorization();

app.MapControllers();

app.Run();

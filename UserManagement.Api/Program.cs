var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDataAccess()
    .AddDomainServices()
    .AddControllers();


var app = builder.Build();


app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();

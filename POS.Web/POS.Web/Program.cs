//using POS.Web.Services;

//var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllersWithViews();

//builder.Services.AddHttpClient<ApiService>();

//var app = builder.Build();

//app.UseStaticFiles();
//app.UseRouting();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Pages}/{action=Index}/{id?}");

//app.Run();
using POS.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7022/");
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();
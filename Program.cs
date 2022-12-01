using Transit;

Notifications notifications = new Notifications();
notifications.Initialize();

Logic.Initialize();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseHttpsRedirection();
app.UseAuthorization();

app.Run();


// 5*67Kym1f
// u1720650_transit
// u1720650_admin
// 31.31.196.202

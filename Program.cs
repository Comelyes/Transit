using Transit;
using Transit.Scripts;

Notifications notifications = new Notifications();
notifications.Initialize();

_ = Logic.Initialize(); // Инициализация БД


ControlSystem controlSystem = new ControlSystem();
Task.Run(controlSystem.Start);

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseHttpsRedirection();
app.UseAuthorization();

app.Run();

// Сервер - 31.31.196.202
// База - u1720650_transit
// Логин - u1720650_admin
// Пароль - 5*67Kym1f


using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=database.db")); // <----- ההוספה הזו
var app = builder.Build();



app.MapGet("/", () => "Hello World!");

app.MapGet("/signup", (string name, string password, AppDbContext db) =>
{
        User? user = db.Users.FirstOrDefault((e) => e.Username == name);


    // לבדוק אם המשתמש קיים ולהחזיר תשובה בהתאם
    if (user != null)
    {
        return Results.BadRequest("Username already taken");
    }

    // במקרה והשם לא תפוס - ליצור משתמש חדש
    user = new User()
    {
        Username = name,
        Password = password,
    };


    // להוסיף לשרת ולעדכן
    db.Users.Add(user);
    db.SaveChanges();


    // json להחזיר את המשתמש שנוצר כ
    return Results.Ok(user);

});


app.MapGet("/login", (string name, string password, AppDbContext db) =>
{
        User? user = db.Users.FirstOrDefault((e) => e.Username == name && e.Password == password);


    // לבדוק אם המשתמש קיים ולהחזיר תשובה בהתאם
    if (user == null)
    {
        return Results.BadRequest("Incorrect password or username");
    }


    // (json להחזיר את המשתמש אם הוא קיים (כ
    return Results.Ok(user);

});


app.Run();

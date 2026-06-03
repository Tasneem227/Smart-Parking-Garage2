using Smart_Parking_Garage;
using Smart_Parking_Garage.Seeding;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDependencies(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
var app = builder.Build();


// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseSerilogRequestLogging();

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
//    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
//    var context = services.GetRequiredService<ApplicationDbContext>();

//    await DefaultUsersSeeding.SeedAsync(userManager, roleManager, context);
//    await DefaultUsersSeeding.SeedPermissionsAsync(roleManager);
//}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Smart Parking API V1");
    c.RoutePrefix = "swagger";
});


app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization(); 

app.MapControllers();

app.Run();

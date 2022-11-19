using ACTech.CI.Services.CarEvaluation;

var builder = WebApplication.CreateBuilder(args);

var r = builder.Configuration.GetSection("CarEvaluationService");

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy  =>
                      {
                          policy.WithOrigins("http://localhost:8080", "http://localhost:5173")
                        //   policy.WithOrigins("*/*", "*", "XMLHttpRequest", "localhost", "http://localhost:8080", "https://localhost")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                      });
});

// Add services to the container.
//builder.Services.AddScoped<ICarEvaluationService, CarEvaluationService>(); 
builder.Services.AddSingleton<ICarEvaluationService, CarEvaluationService>(); 


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// using (var serviceScope = app.Services.CreateScope())
// {
//     var services = serviceScope.ServiceProvider;

//     var myDependency = services.GetRequiredService<ICarEvaluationService>();
//     var st = myDependency.WelcomeEquinox();
// }


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();

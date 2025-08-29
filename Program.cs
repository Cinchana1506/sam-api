using employee_confirmation_api_final.Data;
using employee_confirmation_api_final.Interfaces;
using employee_confirmation_api_final.Repositories;
using employee_confirmation_api_final.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DI
builder.Services.AddScoped<DbHelper>();
builder.Services.AddScoped<IEmployeeConfirmationRepository, EmployeeConfirmationRepository>();
builder.Services.AddScoped<EmployeeConfirmationService>();

var app = builder.Build();

// Configure HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
 

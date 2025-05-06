using Business;
using Data;
using Entity.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agregar DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("name=DefaultConnection"));

// Registro de dependencias
builder.Services.AddScoped<RolData>();
builder.Services.AddScoped<RolBusiness>();
builder.Services.AddScoped<RolFromPermissionData>();
builder.Services.AddScoped<RolFromPermisionBusiness>();
builder.Services.AddScoped<PermissionData>();
builder.Services.AddScoped<PermissionBusiness>();
builder.Services.AddScoped<UserData>();
builder.Services.AddScoped<UserBusiness>();
builder.Services.AddScoped<UserRolData>();
builder.Services.AddScoped<UserRolBusiness>();
builder.Services.AddScoped<HistoryExperienceData>();
builder.Services.AddScoped<HistoryExperienceBusiness>();
builder.Services.AddScoped<DocumentData>();
builder.Services.AddScoped<DocumentBusiness>();
builder.Services.AddScoped<ExperiencieData>();
builder.Services.AddScoped<ExperiencieBusiness>();
builder.Services.AddScoped<ExperiencieLineThematicData>();
builder.Services.AddScoped<ExperiencieLineThematicBusiness>();
builder.Services.AddScoped<ExperiencePopulationData>();
builder.Services.AddScoped<ExperiencePopulationBusiness>();
builder.Services.AddScoped<ExperiencieGradeData>();
builder.Services.AddScoped<ExperiencieGradeBusiness>();
builder.Services.AddScoped<PopulationGradeData>();
builder.Services.AddScoped<PopulationGradeBusiness>();
builder.Services.AddScoped<LineThematicData>();
builder.Services.AddScoped<LineThematicBusiness>();
builder.Services.AddScoped<InstitucionData>();
builder.Services.AddScoped<InstitucionBusiness>();
builder.Services.AddScoped<VerificationData>();
builder.Services.AddScoped<VerificationBusiness>();
builder.Services.AddScoped<GradeData>();
builder.Services.AddScoped<GradeBusiness>();
builder.Services.AddScoped<StateData>();
builder.Services.AddScoped<StateBusiness>();
builder.Services.AddScoped<CriteriaData>();
builder.Services.AddScoped<CriteriaBusiness>();
builder.Services.AddScoped<EvaluationData>();
builder.Services.AddScoped<EvaluationBusiness>();
builder.Services.AddScoped<EvaluationCriteriaData>();
builder.Services.AddScoped<EvaluationCriteriaBusiness>();
builder.Services.AddScoped<ObjectiveData>();
builder.Services.AddScoped<ObjectiveBusiness>();
builder.Services.AddScoped<PersonData>();
builder.Services.AddScoped<PersonBusiness>();
builder.Services.AddScoped<FormModuleData>();
builder.Services.AddScoped<FormModuleBusiness>();
builder.Services.AddScoped<FromData>();
builder.Services.AddScoped<FromBusiness>();
builder.Services.AddScoped<ModuleData>();
builder.Services.AddScoped<ModuleBusiness>();
builder.Services.AddScoped<RolFromPermissionData>();
builder.Services.AddScoped<RolFromPermisionBusiness>();

// Configurar CORS
var OrigenesPermitidos = builder.Configuration
    .GetValue<string>("OrigenesPermitidos")!
    .Split(",");

builder.Services.AddCors(opciones =>
{
    opciones.AddPolicy("AllowSpecificOrigins", politica =>
    {
        politica.WithOrigins(OrigenesPermitidos.Concat(new[] { "http://127.0.0.1:5500" }).ToArray())
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});

// Configurar Autenticación JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "tuapp.com",
            ValidAudience = "tuapp.com",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("SuperClaveJWT2025$%&")) // ⚠️ Cambia esto por una clave segura
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configurar Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigins");

app.UseAuthentication(); // JWT debe ir antes de Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();

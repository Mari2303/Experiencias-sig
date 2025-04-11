using Business;
using Data;
using Entity.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Registrar clases de Rol
builder.Services.AddScoped<RolData>();
builder.Services.AddScoped<RolBusiness>();

builder.Services.AddScoped<RolPermissionData>();
builder.Services.AddScoped<RolPermisionBusiness>();

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

//Agregar CORS
var OrigenesPermitidos = builder.Configuration.GetValue<string>("OrigenesPermitidos")!.Split(",");
builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(politica =>
    {
        politica.WithOrigins(OrigenesPermitidos)
         .AllowAnyHeader()
         .AllowAnyMethod();
    });
});

//Agregar DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("name=DefaultConnection"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();


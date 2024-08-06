using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PatientAdministrationSystem.Application.Interfaces;
using PatientAdministrationSystem.Application.Mapping;
using PatientAdministrationSystem.Application.Repositories.Interfaces;
using PatientAdministrationSystem.Application.Services;
using PatientAdministrationSystem.Application.Utilities;
using PatientAdministrationSystem.Infrastructure;
using PatientAdministrationSystem.Infrastructure.Repositories;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy
                .WithOrigins(builder.Configuration.GetSection("AllowedHosts").Get<string>()!)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
});
builder.Services.AddScoped<IPatientsRepository, PatientsRepository>();
builder.Services.AddScoped<IPatientsService, PatientsService>();

builder.Services.AddDbContext<HciDataContext>(options =>
    options.UseInMemoryDatabase("InMemoryDatabase").EnableSensitiveDataLogging());


builder.Services.AddResponseCompression(options => { options.EnableForHttps = true; });

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "HCI Home Api"
    });

    options.TagActionsBy(api =>
    {
        if (api.GroupName != null) return new[] { api.GroupName };

        if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            return new[] { controllerActionDescriptor.ControllerName };

        throw new InvalidOperationException("Unable to determine tag for endpoint.");
    });

    options.DocInclusionPredicate((_, _) => true);
});

builder.Services.AddHealthChecks();

var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<HciDataContext>();

    // In real world do a proper migration, but here's the test data

    // Read and deserialize JSON file
    var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "sample-data.json");
    if (File.Exists(jsonFilePath))
    {
        var jsonData = await File.ReadAllTextAsync(jsonFilePath);
        var dataSeed = JsonSerializer.Deserialize<DataSeed>(jsonData);

        if (dataSeed != null)
        {
            // Seed hospitals
            if (!dbContext.Hospitals.Any())
            {
                dbContext.Hospitals.AddRange(dataSeed.Hospitals);
            }

            // Seed patients
            foreach (var patient in dataSeed.Patients)
            {
                var existingPatient = dbContext.Patients.Local.FirstOrDefault(p => p.Id == patient.Id)
                    ?? dbContext.Patients.AsNoTracking().FirstOrDefault(p => p.Id == patient.Id);

                if (existingPatient == null)
                {
                    dbContext.Patients.Add(patient);
                }
                else
                {
                    // Update the existing patient entity if needed
                    dbContext.Entry(existingPatient).CurrentValues.SetValues(patient);
                }
            }

            // Seed visits
            foreach (var visit in dataSeed.Visits)
            {
                var existingVisit = dbContext.Visits.Local.FirstOrDefault(v => v.Id == visit.Id)
                    ?? dbContext.Visits.AsNoTracking().FirstOrDefault(v => v.Id == visit.Id);

                if (existingVisit == null)
                {
                    dbContext.Visits.Add(visit);
                }
                else
                {
                    // Update the existing visit entity if needed
                    dbContext.Entry(existingVisit).CurrentValues.SetValues(visit);
                }
            }

            // Seed patient hospital relations
            // Assuming that relationships are correctly set in the `dataSeed` object.
            // If not, you'll need to handle these separately.

            await dbContext.SaveChangesAsync();
        }
    }
    else
    {
        // Log or handle the missing JSON file scenario
        Console.WriteLine("Data file not found.");
    }
}


app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseResponseCompression();

app.MapControllers();

app.Run();
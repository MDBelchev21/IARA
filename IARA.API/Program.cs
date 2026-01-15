using dotenv.net;
using IARA.BusinessLogic.Services.Auth;
using IARA.BusinessLogic.Services.CommercialFishing;
using IARA.BusinessLogic.Services.Inspections;
using IARA.BusinessLogic.Services.RecreationalFishing;
using IARA.BusinessLogic.Services.Registry;
using IARA.Infrastructure.Interfaces;
using IARA.Infrastructure.Interfaces.Auth;
using IARA.Infrastructure.Interfaces.CommercialFishing;
using IARA.Infrastructure.Interfaces.Inspections;
using IARA.Infrastructure.Interfaces.RecreationalFishing;
using IARA.Infrastructure.Interfaces.Registry;
using IARA.Persistance.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var envPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, ".env");
if (File.Exists(envPath))
{
    DotEnv.Load(new DotEnvOptions(envFilePaths: new[] { envPath }));
}

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddOpenApi();

var envConn = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
var connectionString = string.IsNullOrWhiteSpace(envConn)
    ? builder.Configuration.GetConnectionString("DefaultConnection")
    : envConn;

builder.Services.AddDbContext<IARADbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddScoped<BaseServiceInjector>();

var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") 
    ?? builder.Configuration["Jwt:SecretKey"] 
    ?? throw new InvalidOperationException("JWT_SECRET_KEY not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrator", policy => policy.RequireRole("Administrator"));
    options.AddPolicy("Inspector", policy => policy.RequireRole("Inspector", "Administrator"));
    options.AddPolicy("ShipOwner", policy => policy.RequireRole("ShipOwner", "Administrator"));
    options.AddPolicy("ShipOwnerOrInspector", policy => policy.RequireRole("ShipOwner", "Inspector", "Administrator"));
    options.AddPolicy("RecreationalFisherman", policy => policy.RequireRole("RecreationalFisherman", "User", "Administrator"));
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IAdministratorService, AdministratorService>();
builder.Services.AddScoped<IInspectorService, InspectorService>();
builder.Services.AddScoped<IShipOwnerService, ShipOwnerService>();
builder.Services.AddScoped<ILegalEntityService, LegalEntityService>();

builder.Services.AddScoped<IShipService, ShipService>();
builder.Services.AddScoped<IShipEquipmentService, ShipEquipmentService>();
builder.Services.AddScoped<IFishingPermitService, FishingPermitService>();
builder.Services.AddScoped<IFishingTripService, FishingTripService>();
builder.Services.AddScoped<ILandingService, LandingService>();
builder.Services.AddScoped<ILandingLineService, LandingLineService>();
builder.Services.AddScoped<ITransportDocumentService, TransportDocumentService>();
builder.Services.AddScoped<ITransportLineService, TransportLineService>();

builder.Services.AddScoped<IRecreationalFishermanService, RecreationalFishermanService>();
builder.Services.AddScoped<IRecreationalTicketService, RecreationalTicketService>();
builder.Services.AddScoped<IRecreationalCatchService, RecreationalCatchService>();
builder.Services.AddScoped<IRecreationalTicketTypeService, RecreationalTicketTypeService>();
builder.Services.AddScoped<IQualificationService, QualificationService>();
builder.Services.AddScoped<IShipCrewService, ShipCrewService>();

builder.Services.AddScoped<IInspectionService, InspectionService>();
builder.Services.AddScoped<IViolationService, ViolationService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "IARA API",
        Version = "v1",
        Description = "API for Bulgarian Fisheries and Aquaculture Executive Agency"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<IARADbContext>();
    await DbSeeder.SeedAsync(context);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

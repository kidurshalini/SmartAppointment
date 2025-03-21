//using MediatR;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using SmartAppointment.Application.Interfaces;
//using SmartAppointment.Infrastructure.Data;
//using SmartAppointment.Infrastructure.Persistence;
//using System.Reflection;
//using System.Text;
//using AutoMapper;
//using SmartAppointment.Application.Mappings;
//var builder = WebApplication.CreateBuilder(args);

//// 🔹 1️⃣ Configure Database (SQL Server + EF Core)
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
//b => b.MigrationsAssembly("SmartAppointment.Infrastructure")));

//// 🔹 2️⃣ Configure Identity for User Authentication
//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();

//// 🔹 3️⃣ Configure JWT Authentication (Fix RequireHttpsMetadata)
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.RequireHttpsMetadata = false; // ✅ Allow HTTP for local testing
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidateIssuerSigningKey = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
//        };
//    });

//// 🔹 4️⃣ Enable CORS (Fix: Allow Both HTTP & HTTPS)
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAll",
//        policy => policy.AllowAnyOrigin()
//                        .AllowAnyMethod()
//                        .AllowAnyHeader());
//});
//// 🔹 5️⃣ Register Application Services & Repositories
//builder.Services.AddScoped<IAppointmentService, AppointmentRepository>();

//// 🔹 6️⃣ Register MediatR for Application Layer (CQRS)
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

//// 🔹 Register AutoMapper
//builder.Services.AddAutoMapper(typeof(MappingProfile));

//// 🔹 7️⃣ Add Controllers & Swagger
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// 🔹 8️⃣ Create Default Roles on App Startup (Fix: Use `app.Services`)
//using (var scope = app.Services.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
//    string[] roles = { "Admin", "Professional", "User" };

//    foreach (var role in roles)
//    {
//        if (!await roleManager.RoleExistsAsync(role))
//        {
//            await roleManager.CreateAsync(new IdentityRole(role));
//        }
//    }
//}

//// 🔹 9️⃣ Configure Middleware (Request Pipeline)
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();
//app.UseCors("AllowAll");  // ✅ Enable CORS for frontend
//app.UseAuthentication();        // ✅ Enable JWT Authentication
//app.UseAuthorization();         // ✅ Enable Authorization
//app.MapControllers();           // ✅ Map API Endpoints

//app.Run();

using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using SmartAppointment.Infrastructure.Data;

using System.Reflection;
using System.Text;
using AutoMapper;

using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// 🔹 1️⃣ Configure Database (SQL Server + EF Core)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("SmartAppointment.Infrastructure")));

// 🔹 2️⃣ Configure Identity for User Authentication
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 🔹 3️⃣ Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // Allow HTTP for local testing
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"], // e.g., "your-issuer"
            ValidAudience = builder.Configuration["Jwt:Audience"], // e.g., "your-audience"
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"])),
            // Secret key for signing JWT
             NameClaimType = JwtRegisteredClaimNames.Sub,
        };
    });

// 🔹 4️⃣ Enable CORS (Allow All origins for now, restrict in production)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// 🔹 5️⃣ Register Application Services & Repositories


// 🔹 6️⃣ Register MediatR for Application Layer (CQRS)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


// 🔹 7️⃣ Add Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// 🔹 8️⃣ Create Default Roles on App Startup (Use `app.Services`)
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roles = { "Admin", "Professional", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

// 🔹 9️⃣ Configure Middleware (Request Pipeline)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");  // Enable CORS for frontend
app.UseAuthentication();        // Enable JWT Authentication
app.UseAuthorization();         // Enable Authorization
app.MapControllers();           // Map API Endpoints

app.Run();

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Shah_Traveling_Agency_API.Areas.TicketHubArea.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Automatic Dependency Injection
builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>()
    .AddClasses(classes => classes
        .Where(type =>
            type.Name.EndsWith("Repo") ||
            type.Name.EndsWith("Service") ||
            type.Name.EndsWith("Context")))
    .AsSelf()
    .WithScopedLifetime()
);
builder.Services.AddSignalR();

// started
// jwt Bearer Token.
builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme
)
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

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                builder.Configuration["Jwt:Key"]!
            )
        )
    };
});
// Ended

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();
app.MapHub<TicketHub>("/hubs/tickets");
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseCors("AngularPolicy");

app.UseStaticFiles();

// Create Uploads folder if it doesn't exist
var uploadsPath = Path.Combine(
    builder.Environment.ContentRootPath,
    "Uploads"
);

Directory.CreateDirectory(uploadsPath);


// Serve Uploads folder
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});



app.UseAuthorization();

app.MapControllers();

app.Run();
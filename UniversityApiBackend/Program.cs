// 1. Usings to work with EntityFrameWork
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UniversityApiBackend;
using UniversityApiBackend.DataAccess;
using UniversityApiBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// 2. Connection with SQL Server
const string connectionName = "UniversityDB";
var connectionString = builder.Configuration.GetConnectionString(connectionName);

// 3. Add Context
builder.Services.AddDbContext<UniversityDBContext>(options => options.UseSqlServer(connectionString));


// 7. Add Service of JWT Authorization
builder.Services.AddJwtTokenService(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers();
// 4. Add Custom Services (Folder Services)Type, value
builder.Services.AddScoped<IStudentsService, StudentsService>();
//To do: Add rest of services

//8. Add Authorization 
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserOnlyPolicy", policy => policy.RequireClaim("UserOnly","User1"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// 9.Config Swagger to take care of authorization JWT 
builder.Services.AddSwaggerGen(options => 
    {//We Define the security for authorization
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "JWT Authorization Header using Bearer Scheme"
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
                new string[] {}
            }
        });
    }
);

// 5. CORS Configuration
builder.Services.AddCors(options => 
    {
        options.AddPolicy(name: "CorsPolicy", builder =>
            {//Normal configuration
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            }
        );
    }
);


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
// 6. Tell "app" use  CORS
app.UseCors("CorsPolicy");

app.Run();

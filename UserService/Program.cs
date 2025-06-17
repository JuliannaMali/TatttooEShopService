using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Cryptography;
using UserApplication.Producer;
using UserApplication.Services.JWT;
using UserApplication.Services.Login;
using UserApplication.Services.User;
using UserDomain.Models;
using UserDomain.Profiles;
using UserDomain.Repository;
using UserDomain.Seeders;




var builder = WebApplication.CreateBuilder(args);


//connection
var connectionString = builder.Configuration.GetConnectionString("TattooDB");
//var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
builder.Services.AddDbContext<UserDomain.Repository.DbContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Transient);
//-----




//DI
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IUserService, UserApplication.Services.User.UserService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<ISeeder, Seeder>();
builder.Services.AddScoped<IKafkaProducer, KafkaProducer>();
//-----



//jwt
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(jwtSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var rsa = RSA.Create();
    rsa.ImportFromPem(File.ReadAllText("/app/data/public.key"));
    var publicKey = new RsaSecurityKey(rsa);

    var jwtConfig = jwtSettings.Get<JwtSettings>();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig.Issuer,
        ValidAudience = jwtConfig.Audience,
        IssuerSigningKey = publicKey
    };
});
//-----




//auth
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Administrator"));
    options.AddPolicy("Managerial", policy =>
        policy.RequireRole("Administrator", "Employee"));
    options.AddPolicy("LoggedIn", policy =>
        policy.RequireRole("Administrator", "Employee", "Client"));
});

//-----


builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();



//bearer
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Wpisz token w formacie: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
          {
            {
              new OpenApiSecurityScheme
              {
                Reference = new OpenApiReference
                  {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                  },
                  Scheme = "oauth2",
                  Name = "Bearer",
                  In = ParameterLocation.Header,

                },
                new List<string>()
              }
            });
});
//-----




var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();




//seeder
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserDomain.Repository.DbContext>();
    await db.Database.MigrateAsync();
    var seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();
    await seeder.Seed();
}
//-----


app.Run();

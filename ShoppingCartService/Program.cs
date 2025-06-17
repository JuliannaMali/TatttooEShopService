using ShoppingCartApplication.Services;
using ShoppingCartDomain.Interfaces;
using ShoppingCartInfrastructure.Repositories;
using ShoppingCartInfrastructure.Services;
using ShoppingCartInfrastructure.Kafka;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


//connection
var connectionString = builder.Configuration.GetConnectionString("TattooDB");
builder.Services.AddDbContext<UserDomain.Repository.DbContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Transient);
//---


builder.Services.AddControllers();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ShoppingCartApplication.Services.CartService).Assembly));



//DI
builder.Services.AddSingleton<ICartRepository, InMemoryCartRepository>();
builder.Services.AddSingleton<ICartAdder, CartService>();
builder.Services.AddSingleton<ICartRemover, CartService>();
builder.Services.AddSingleton<ICartReader, CartService>();
builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();
builder.Services.AddSingleton<IOrderRepository, InMemoryOrderRepository>();
builder.Services.AddHttpClient<IProductInfoService, ProductInfoService>(client =>
{
    client.BaseAddress = new Uri("http://tattooeshopservice:5000/");
});
//---


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
//---


//rsa
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

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "TattooEShop",
        ValidAudience = "TattooEShopCustomer",
        IssuerSigningKey = publicKey
    };
});
//----


//policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("LoggedIn", policy =>
        policy.RequireRole("Client"));
    options.AddPolicy("Managerial", policy =>
        policy.RequireRole("Administrator","Employee"));
});
//--


//auth
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
//---


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

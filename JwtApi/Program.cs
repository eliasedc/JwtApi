using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Adicionar autenticação
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true, //Não utilizar em apis públicas
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "sua-api",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"] ?? "chave-secreta-muito-segura"
                    )
                ),
            //ValidAudience = null
            AudienceValidator = (audiences, token, parameters) =>
            {
                var validAudiences = new[] { "ClientOne", "ClientTwo" }; // Audiências permitidas
                return audiences.Any(audience => validAudiences.Contains(audience));
            }
        };
    });

builder.Services.AddAuthorization(
        options =>
        {
            options.AddPolicy("ApiClientOnePolicy", policy =>
            {
                policy.RequireClaim("aud", "ClientOne");
                //posso adicionar aqui também qualquer tipo de nome, exemplo:
                //policy.RequireClaim("permission", "admin-method-access")
                //E definir esse permission na hora de fazer o login
            });
            options.AddPolicy("ApiClientTwoPolicy", policy =>
            {
                policy.RequireClaim("aud", "ClientTwo");
            });
        }
    );


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

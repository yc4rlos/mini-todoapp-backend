using System.Text;
using mesha_test_backend.Data;
using mesha_test_backend.Filters;
using mesha_test_backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddDbContext<TasksDatabaseContext>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TasksService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", 
    new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header usando o Bearer Schema. \r\n\r\n 
                      Dgiite 'Bearer' [espaço] e então o seu token abaixo.
                      \r\n\r\nExemplo: 'Bearer 12345abcdefg'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey, 
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    
    options.OperationFilter<AuthOperationFilter>();
});

var app = builder.Build();

// Configure the JWT Authentication
app.UseAuthentication();
app.UseAuthorization();

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

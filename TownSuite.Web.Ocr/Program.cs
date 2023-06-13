using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using TownSuite.Web.Ocr;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<Settings>((s) => builder.Configuration.GetSection("Settings").Get<Settings>());
builder.Services.AddScoped<JwtSettings>((s) => builder.Configuration.GetSection("JWT").Get<JwtSettings>());

if (!System.IO.Directory.Exists(Settings.GetTempFolder()))
{
    System.IO.Directory.CreateDirectory(Settings.GetTempFolder());
}

var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]));
builder.Services.AddAuthentication(options => { options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            IssuerSigningKey = symmetricSecurityKey
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ValidAccessToken", policy =>
    {
        policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });
    options.DefaultPolicy = options.GetPolicy("ValidAccessToken");
    options.FallbackPolicy = options.DefaultPolicy;
});


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
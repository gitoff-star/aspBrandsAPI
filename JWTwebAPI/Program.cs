using JWTwebAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

//jwt authentication service 

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,  //validate server that generates token
                
            ValidateIssuer = true,   //validate recipt that is authorize to recieve
            ValidateLifetime = true,  // expiration or valididty time
            ValidIssuer = builder.Configuration["jwt:Issuer"],  //
            ValidAudience = builder.Configuration["jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:Key"])) //validates signature of token
        };
    }
    );

//Allow CORS
            builder.Services.AddDbContext<DBContextcs>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
               ) ) ;

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("corspolicy", build => build.WithOrigins("https://localhost:3000/")
                .AllowAnyOrigin()
                .AllowAnyMethod().AllowAnyHeader());
            });
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("corspolicy");
app.UseHttpsRedirection();
// adding authentication pipeline for jwt authentication
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

using DataAccessEF.Data;
using DataAccessEF.Repositories;
using DataAccessEF.UnitOfWorks;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ConfigurationManager = ChatAppApi.ConfigurationManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ChatAppApi.Chats;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "some description",
        Type = SecuritySchemeType.Http
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List < string > ()
        }
    });
});

builder.Services.AddDbContext<ChatDbContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("SmarterDB"),
    b => b.MigrationsAssembly(typeof(ChatDbContext).Assembly.FullName)));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ChatDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepo<>));
builder.Services.AddTransient<IContactsRepository, ContactRepo>();
builder.Services.AddTransient<IConversationRepository, ConversationRepo>();
builder.Services.AddTransient<IMessageRepository, MessageRepo>();
builder.Services.AddTransient<IUsersRepository, UserRepo>();
builder.Services.AddTransient<IUnitOfWorks, UnitOfWorks>();
builder.Services.AddSignalR();



builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = ConfigurationManager.AppSettings["JWT:ValidIssuer"],

        ValidateAudience = true,
        ValidAudience = ConfigurationManager.AppSettings["JWT:ValidAudience"],

        ValidateLifetime = true,

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["JWT:Secret"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseCors("AllowAll");

app.MapHub<ChatHub>("/chat");
app.Run();

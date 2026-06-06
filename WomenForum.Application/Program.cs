using System.Security;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using WomenForum.Business;
using WomenForum.Business.Interfaces;
using WomenForum.Database;
using WomenForum.Helpers;
using WomenForum.Helpers.Interfaces;
using WomenForum.Middlewares;
using WomenForum.Repository;
using WomenForum.Repository.Repositories;
using WomenForum.Repository.Repositories.Interfaces;
using WomenForum.Validators;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

//Swagger
builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description = "Enter JWT in the field",
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
    });

//Database
builder.Services.AddDbContext<WomenForumDbContext>(options =>
    options
        .UseLazyLoadingProxies()
        .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//Repositories
builder.Services.AddTransient<ICategoriesRepository,CategoriesRepository>();
builder.Services.AddTransient<ICommentsRepository,CommentsRepository>();
builder.Services.AddTransient<ICommunitiesRepository,CommunitiesRepository>();
builder.Services.AddTransient<ICommunityJoinRequestsRepository,CommunityJoinRequestsRepository>();
builder.Services.AddTransient<ICommunityMembersRepository,CommunityMembersRepository>();
builder.Services.AddTransient<IDiscussionThreadsRepository,DiscussionThreadsRepository>();
builder.Services.AddTransient<ILikesRepository,LikesRepository>();
builder.Services.AddTransient<IMessagesRepository,MessagesRepository>();
builder.Services.AddTransient<INotificationsRepository,NotificationsRepository>();
builder.Services.AddTransient<IPostsRepository,PostsRepository>();
builder.Services.AddTransient<IReportsRepository,ReportsRepository>();
builder.Services.AddTransient<ISubscriptionsRepository,SubscriptionsRepository>();
builder.Services.AddTransient<IUserActivitiesRepository,UserActivitiesRepository>();
builder.Services.AddTransient<IUserSettingsRepository,UserSettingsRepository>();
builder.Services.AddTransient<IUsersRepository,UsersRepository>();
builder.Services.AddTransient<IWarningsRepository,WarningsRepository>();

//Unit of work
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

//Business
builder.Services.AddTransient<IAuthorizationBusinessService, AuthorizationBusinessService>();
builder.Services.AddTransient<ICategoriesBusinessService, CategoriesBusinessService>();
builder.Services.AddTransient<ICommentsBusinessService, CommentsBusinessService>();
builder.Services.AddTransient<ICommunitiesBusinessService, CommunitiesBusinessService>();
builder.Services.AddTransient<ICommunityJoinRequestsBusinessService, CommunityJoinRequestsBusinessService>();
builder.Services.AddTransient<ICommunityMembersBusinessService, CommunityMembersBusinessService>();
builder.Services.AddTransient<IDiscussionThreadsBusinessService, DiscussionThreadsBusinessService>();
builder.Services.AddTransient<ILikesBusinessService, LikesBusinessService>();
builder.Services.AddTransient<IMessagesBusinessService, MessagesBusinessService>();
builder.Services.AddTransient<INotificationsBusinessService, NotificationsBusinessService>();
builder.Services.AddTransient<IPostsBusinessService, PostsBusinessService>();
builder.Services.AddTransient<IReportsBusinessService, ReportsBusinessService>();
builder.Services.AddTransient<IUserActivitiesBusinessService, UserActivitiesBusinessService>();
builder.Services.AddTransient<IUsersBusinessService, UsersBusinessService>();
builder.Services.AddTransient<IUserSettingsBusinessService, UserSettingsBusinessService>();
builder.Services.AddTransient<IPermissionsService, PermissionsService>();
builder.Services.AddTransient<IWarningsBusinessService, WarningsBusinessService>();

//Jwt helper
builder.Services.AddTransient<JWTHelper>();

//Automapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

//Validators
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();

//Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()               
    .WriteTo.Console()         
    .CreateLogger();

builder.Host.UseSerilog();

//Authorization
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,      
            ValidateIssuerSigningKey = true,  
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularDev");
app.UseRouting();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHttpsRedirection();

app.Run();
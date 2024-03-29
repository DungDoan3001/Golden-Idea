using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Web.Api.Configuration;
using Web.Api.Data.Context;
using Web.Api.Data.Queries;
using Web.Api.Data.UnitOfWork;
using Web.Api.Entities;
using Web.Api.Extensions;
using Web.Api.Services.Authentication;
using Web.Api.Services.Category;
using Web.Api.Services.Chart;
using Web.Api.Services.Comment;
using Web.Api.Services.DepartmentService;
using Web.Api.Services.EmailService;
using Web.Api.Services.FakeData;
using Web.Api.Services.FileService;
using Web.Api.Services.FileUploadService;
using Web.Api.Services.IdeaService;
using Web.Api.Services.ReactionService;
using Web.Api.Services.ResetPassword;
using Web.Api.Services.Role;
using Web.Api.Services.Topic;
using Web.Api.Services.User;
using Web.Api.Services.View;
using Web.Api.Services.ZipFile;
using Web.Api.SignalR;
using static Web.Api.Configuration.CacheKey;

namespace Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(config =>
            {
                config.CacheProfiles.Add("120SecondsDuration", new CacheProfile
                {
                    Duration = 120
                });
            });

            services.Configure<ApiBehaviorOptions>(options
                => options.SuppressModelStateInvalidFilter = true);

            // Auto mapper service
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Swagger service
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "COMP1640 - GoldenIdea", Description = "This is a list of APIs we use to manage the GoldenIdea Application" });
                // Generating api description via xml;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                // Add authentication button
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Enter your token here.<br><br>",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Connect Database
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
            });

            // Add Identity
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // CORS
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed(_ => true)
                        .AllowCredentials();
                });
            });

            //Authentication + JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwt =>
            {
                var key = Encoding.UTF8.GetBytes(Configuration["JwtSettings:Secret"]);
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(0),

                    ValidIssuer = Configuration["JwtSettings:Issuer"],
                    ValidAudience = Configuration["JwtSettings:Audience"]
                };
                jwt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/chat")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.Configure<JwtConfig>(Configuration.GetSection("JwtSettings"));
            //services.Configure<IdentityOptions>(options =>
            //{
            //    options.Password.RequiredLength = 6;
            //    options.Password.RequireUppercase = false;
            //    options.Password.RequireLowercase = false;
            //    options.Password.RequireDigit = false;
            //    options.Password.RequireNonAlphanumeric = false;
            //});

            //Set expiration for identity token
            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(1));
            // Authorization
            services.AddAuthorization();

            // Dependency Injections
            services.AddScoped<ValidationFilterAttribute>();
            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IResetPasswordService, ResetPasswordService>();
            services.AddScoped<IFileUploadService, FileUploadService>();
            services.AddScoped<IIdeaService, IdeaService>();
            services.AddScoped<IReactionService, ReactionService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IViewService, ViewService>();
            services.AddScoped<IChartService, ChartService>();
            services.AddScoped<IFakeDataService, FakeDataService>();
            services.AddScoped<IZipFileService, ZipFileService>();
            
            // Queries
            services.AddScoped<ITopicQuery, TopicQuery>();
            services.AddScoped<IIdeaQuery, IdeaQuery>();
            services.AddScoped<ICategoryQuery, CategoryQuery>();
            services.AddScoped<IDepartmentQuery, DepartmentQuery>();
            services.AddScoped<IViewQuery, ViewQuery>();
            services.AddScoped<IReactionQuery, ReactionQuery>();
            services.AddScoped<ICommentQuery, CommentQuery>();

            //
            services.AddSingleton<CacheKey>();
            //SignalR
            services.AddSignalR();

            //Caching
            services.AddMemoryCache();
            services.AddResponseCaching();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web.Api v1");
                c.DefaultModelsExpandDepth(-1);
            });

            app.UseCors(x => x.AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials()
                            .SetIsOriginAllowed(origin => true));

            app.UseHttpsRedirection();

            app.UseResponseCaching();
            //app.UseHttpCacheHeaders();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Web.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using Web.Api.Data.Context;
using Web.Api.Data.UnitOfWork;
using Web.Api.Services.DepartmentService;
using System.IO;
using System.Reflection;
using System;
using Web.Api.Entities;
using Microsoft.AspNetCore.Identity;
using Web.Api.Services.Topic;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Services.Category;
using Web.Api.Services.Role;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Web.Api.Configuration;
using Web.Api.Services.Authentication;
using Web.Api.Services.User;
using Web.Api.Services.EmailService;
using Web.Api.Services.ResetPassword;
using Web.Api.Services.UploadFileService;
using Web.Api.Data.Queries;
using Web.Api.Services.ImageService;

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
            services.AddControllers();

            services.Configure<ApiBehaviorOptions>(options
                => options.SuppressModelStateInvalidFilter = true);

            // Auto mapper service
            services.AddAutoMapper(typeof(MappingProfile).Assembly);

            // Swagger service
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo {Title = "COMP1640 - GoldenIdea", Description = "This is a list of APIs we use to manage the GoldenIdea Application"});
                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });


            // Connect Database
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName));
            });

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            // CORS
            services.AddCors();

            //Authentication + JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme= JwtBearerDefaults.AuthenticationScheme;
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
            services.AddScoped<IUploadFileService, UploadFileService>();
            services.AddScoped<IImageService, ImageService>();
            
            // Queries
            services.AddScoped<ITopicQuery, TopicQuery>();
            
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

            app.UseCors(option =>
            {
                option.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins("http://localhost:5000");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

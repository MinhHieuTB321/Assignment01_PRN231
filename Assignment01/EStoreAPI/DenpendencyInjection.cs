using BusinessObject.IRepo;
using BusinessObject.Models;
using BusinessObject.Repositories;
using DataAccess.IServices;
using DataAccess.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace EStoreAPI
{
    public static class DenpendencyInjection
    {
        public static IServiceCollection AddApiService(this IServiceCollection services,string connectionString,string secretKey)
        {
            services.AddDbContext<FstoreDbContext>(opt =>
            {
                opt.UseSqlServer(connectionString);
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddCors(options
                => options.AddDefaultPolicy(policy
                    => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
            #region DI_REPO

            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IMemberRepository,MemberRepository>();
            services.AddScoped<IProductRepository,ProductRepository>();
            services.AddScoped<IOrderRepository,OrderRepository>();
            services.AddScoped<IOrderDetailRepository,OrderDetailRepository>();
            services.AddScoped<ICategoryRepository,CategoryRepository>();
            #endregion

            #region DI_Service
            services.AddScoped<IMemberService,MemberService>();
            services.AddScoped<IOrderService,OrderService>();
            services.AddScoped<IProductService,ProductService>();
            services.AddScoped<ICategoryService, CategoryService>();
            #endregion
            #region App
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Backend API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opt.IncludeXmlComments(xmlPath);

                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[]{}
                    }
                });
            });
            #endregion

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = secretKey,
                        ValidAudience = secretKey,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                        ClockSkew = TimeSpan.FromSeconds(1)
                    };
                });

            return services;
        }
    }
}

using JOIEnergy.Domain;
using JOIEnergy.Generator;
using JOIEnergy.RepositoryFolder;
using JOIEnergy.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOIEnergy
{
    public class Startup
    {
        private const string MOST_EVIL_PRICE_PLAN_ID = "price-plan-0";
        private const string RENEWABLES_PRICE_PLAN_ID = "price-plan-1";
        private const string STANDARD_PRICE_PLAN_ID = "price-plan-2";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var readings =
                GenerateMeterElectricityReadings();

            var pricePlans = new List<PricePlan> {
                new PricePlan{
                    PlanName = MOST_EVIL_PRICE_PLAN_ID,
                    EnergySupplier = Enums.Supplier.DrEvilsDarkEnergy,
                    UnitRate = 10m,
                    PeakTimeMultiplier = new List<PeakTimeMultiplier>()
                },
                new PricePlan{
                    PlanName = RENEWABLES_PRICE_PLAN_ID,
                    EnergySupplier = Enums.Supplier.TheGreenEco,
                    UnitRate = 2m,
                    PeakTimeMultiplier = new List<PeakTimeMultiplier>()
                },
                new PricePlan{
                    PlanName = STANDARD_PRICE_PLAN_ID,
                    EnergySupplier = Enums.Supplier.PowerForEveryone,
                    UnitRate = 1m,
                    PeakTimeMultiplier = new List<PeakTimeMultiplier>()
                }
            };

            services.AddSingleton<TokenService>();
            services.AddSingleton<IMeterReadingRepository>(new MeterReadingRepository(readings));
            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IMeterReadingService, MeterReadingService>();
            services.AddTransient<IPricePlanService, PricePlanService>();
            services.AddSingleton((IServiceProvider arg) => readings);
            services.AddSingleton((IServiceProvider arg) => pricePlans);
            services.AddSingleton((IServiceProvider arg) => SmartMeterToPricePlanAccounts);

            var key = Encoding.ASCII.GetBytes("dasdiabdbadibdbifbiuafuiasnfndusanduasnduasdfuaf");

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = false,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false
                    };
                    x.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
                            logger.LogError("OnAuthenticationFailed: " + context.Exception.Message);
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Startup>>();
                            var token = context.SecurityToken as JwtSecurityToken;
                            if (token != null)
                            {
                                logger.LogInformation("OnTokenValidated: " + token.RawData);
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseMvc();
            logger.LogInformation("Application started.");
        }

        private Dictionary<string, List<ElectricityReading>> GenerateMeterElectricityReadings()
        {
            var readings = new Dictionary<string, List<ElectricityReading>>();
            var generator = new ElectricityReadingGenerator();
            var smartMeterIds = SmartMeterToPricePlanAccounts.Select(mtpp => mtpp.Key);

            foreach (var smartMeterId in smartMeterIds)
            {
                readings.Add(smartMeterId, generator.Generate(20));
            }
            return readings;
        }

        public Dictionary<string, string> SmartMeterToPricePlanAccounts
        {
            get
            {
                Dictionary<string, string> smartMeterToPricePlanAccounts = new Dictionary<string, string>();
                smartMeterToPricePlanAccounts.Add("smart-meter-0", MOST_EVIL_PRICE_PLAN_ID);
                smartMeterToPricePlanAccounts.Add("smart-meter-1", RENEWABLES_PRICE_PLAN_ID);
                smartMeterToPricePlanAccounts.Add("smart-meter-2", MOST_EVIL_PRICE_PLAN_ID);
                smartMeterToPricePlanAccounts.Add("smart-meter-3", STANDARD_PRICE_PLAN_ID);
                smartMeterToPricePlanAccounts.Add("smart-meter-4", RENEWABLES_PRICE_PLAN_ID);
                return smartMeterToPricePlanAccounts;
            }
        }
    }
}

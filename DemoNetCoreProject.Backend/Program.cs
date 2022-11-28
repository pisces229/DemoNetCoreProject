using DemoNetCoreProject.Backend.Csp;
using DemoNetCoreProject.Backend;
using DemoNetCoreProject.Backend.Filters;
using DemoNetCoreProject.Backend.Services;
using DemoNetCoreProject.BusinessLayer;
using DemoNetCoreProject.Common.Options;
using DemoNetCoreProject.DataLayer;
using DemoNetCoreProject.DataLayer.Services;
using DemoNetCoreProject.DataLayer.IServices;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Builder;
using System.Net;
using StackExchange.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using DemoNetCoreProject.Common.Dtos;
using Microsoft.Extensions.Configuration;
using Polly.Extensions.Http;
using Polly;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine(EnvironmentVariable.ASPNETCORE_ENVIRONMENT);

#region WebApplicationBuilder
var webApplicationBuilder = WebApplication.CreateBuilder(args);

//webApplicationBuilder.Services.Configure<HostOptions>(options => options.ShutdownTimeout = TimeSpan.FromSeconds(10));

webApplicationBuilder.Host.ConfigureAppConfiguration((hostBuilder, configurationBuilder) =>
{
    configurationBuilder.SetBasePath(webApplicationBuilder.Environment.ContentRootPath)
        .AddJsonFile(path: "appsettings.json", optional: false, reloadOnChange: true);
});

webApplicationBuilder.Host.ConfigureLogging((hostContext, loggingBuilder) =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
    loggingBuilder.UseDefaultSerilog();
});

// KestrelHttpServer
//webApplicationBuilder.WebHost.UseKestrel(options =>
//{
//    options.AddServerHeader = false;
//    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(1);
//    options.Limits.MaxConcurrentConnections = 100;
//    options.Limits.MaxConcurrentUpgradedConnections = 100;
//    options.Limits.MaxRequestBodySize = 10 * 1024;
//    options.Limits.MinRequestBodyDataRate =
//        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
//    options.Limits.MinResponseDataRate =
//        new MinDataRate(bytesPerSecond: 100, gracePeriod: TimeSpan.FromSeconds(10));
//    options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(5);
//    options.Listen(IPAddress.Loopback, 5080);
//    options.Listen(IPAddress.Loopback, 5443, listenOptions =>
//    {
//        listenOptions.UseHttps("localhost.pfx", "Password");
//    });
//});

//webApplicationBuilder.Services.AddSingleton(provider => configurationRoot);
webApplicationBuilder.Services.AddHttpContextAccessor();
// 註冊 Options Pattern 服務，將配置內容註冊到容器裡，來獲取對應的服務 Provider 對象
webApplicationBuilder.Services.AddOptions();
webApplicationBuilder.Services.AddCors(options =>
{
    // CorsPolicy 是自訂的 Policy 名稱
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .AllowAnyOrigin()
            //.WithOrigins("https://localhost:44387", "")
            //.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
            .AllowAnyHeader()
            .WithExposedHeaders("content-disposition")
            .AllowAnyMethod();
            //.AllowCredentials();
    });
});

// Make HTTP requests using IHttpClientFactory in ASP.NET Core
webApplicationBuilder.Services.AddHttpClient("Default", client =>
{
    client.BaseAddress = new Uri("https://localhost:9110/api/default/");
})
//.SetHandlerLifetime(TimeSpan.FromSeconds(5))
.AddTransientHttpErrorPolicy(policy =>
    policy.OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
    .RetryAsync(1))
.AddTransientHttpErrorPolicy(policy =>
    policy.OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
    .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30)));
//.AddPolicyHandler(HttpPolicyExtensions
//    .HandleTransientHttpError()
//    .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
//    .WaitAndRetryAsync(1, retryAttempt => TimeSpan.FromSeconds(Math.Pow(1, retryAttempt))));

#region DbContext
webApplicationBuilder.Services.AddDbContext<DefaultDbContext>(option =>
{
    var connectionName = "Default";
    option.UseInMemoryDatabase(databaseName: connectionName);
    //option.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString(connectionName),
    //    sqlServerOption =>
    //    {
    //        sqlServerOption.MinBatchSize(10);
    //        sqlServerOption.MaxBatchSize(1000);
    //        sqlServerOption.CommandTimeout(0);
    //        sqlServerOption.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
    //    });
    option.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    option.EnableSensitiveDataLogging();
    option.EnableDetailedErrors();
});
webApplicationBuilder.Services.AddDbContext<DataProtectionDbContext>(option =>
{
    var connectionName = "DataProtection";
    //option.UseInMemoryDatabase(databaseName: connectionName);
    option.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString(connectionName));
    option.EnableSensitiveDataLogging();
    option.EnableDetailedErrors();
});
#endregion

#region Cache
{
    #region Memory
    webApplicationBuilder.Services.AddSingleton((Func<IServiceProvider, IMemoryCache>)(factory =>
    {
        var cache = new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());
        return cache;
    }));
    webApplicationBuilder.Services.AddSingleton<ICache, DemoNetCoreProject.DataLayer.Services.MemoryCache>();
    #endregion

    #region Redis
    //webApplicationBuilder.Services.AddSingleton<ICache, RedisCache>();
    #endregion

    #region Database
    //webApplicationBuilder.Services.AddDistributedSqlServerCache(options =>
    //{
    //    options.ConnectionString = webApplicationBuilder.Configuration.GetConnectionString("Redis");
    //    options.SchemaName = "dbo";
    //    options.TableName = "DataCache";
    //    options.ExpiredItemsDeletionInterval = TimeSpan.FromMinutes(5);
    //});
    //webApplicationBuilder.Services.AddSingleton<ICache, DatabaseCache>();
    #endregion
}
#endregion

#region JWT
{
    var jwtSigning = webApplicationBuilder.Configuration.GetSection("JwtSigning");
    var symmetricSecurityKey = default(SymmetricSecurityKey);
    //var publicRsaSecurityKey = default(RsaSecurityKey);
    //var privateRsaSecurityKey = default(RsaSecurityKey);
    #region String
    {
        symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSigning.GetValue<string>("StringSecretKey")));
    }
    #endregion

    #region Pem
    //{
    //    var rsaPublicKey = RSA.Create();
    //    var pemCertificateContents = File.ReadAllText(jwtSigning.GetValue<string>("PemPublicFile"));
    //    var pemCertificateHeader = "-----BEGIN PUBLIC KEY-----";
    //    var pemCertificateFooter = "-----END PUBLIC KEY-----";
    //    var endIdx = pemCertificateContents.IndexOf(
    //        pemCertificateFooter,
    //        pemCertificateHeader.Length,
    //        StringComparison.Ordinal);
    //    var base64 = pemCertificateContents.Substring(
    //        pemCertificateHeader.Length,
    //        endIdx - pemCertificateHeader.Length);
    //    var binary = Convert.FromBase64String(base64);
    //    rsaPublicKey.ImportSubjectPublicKeyInfo(binary, out _);
    //    publicRsaSecurityKey = new RsaSecurityKey(rsaPublicKey.ExportParameters(false));
    //}
    //{
    //    var rsaPrivateKey = RSA.Create();
    //    var pemPrivateContents = File.ReadAllText(jwtSigning.GetValue<string>("PemPrivateFile"));
    //    var pemPrivateKeyHeader = "-----BEGIN PRIVATE KEY-----";
    //    var pemPrivateKeyFooter = "-----END PRIVATE KEY-----";
    //    var endIdx = pemPrivateContents.IndexOf(
    //        pemPrivateKeyFooter,
    //        pemPrivateKeyHeader.Length,
    //        StringComparison.Ordinal);
    //    var base64 = pemPrivateContents.Substring(
    //        pemPrivateKeyHeader.Length,
    //        endIdx - pemPrivateKeyHeader.Length);
    //    var binary = Convert.FromBase64String(base64);
    //    rsaPrivateKey.ImportPkcs8PrivateKey(binary, out _);
    //    privateRsaSecurityKey = new RsaSecurityKey(rsaPrivateKey.ExportParameters(true));
    //}
    #endregion
    var jwtOption = webApplicationBuilder.Configuration.GetSection(nameof(JwtOption));
    webApplicationBuilder.Services.Configure<JwtOption>(options =>
    {
        options.NameClaimType = jwtOption.GetValue<string>(nameof(JwtOption.NameClaimType));
        options.RoleClaimType = jwtOption.GetValue<string>(nameof(JwtOption.RoleClaimType));
        options.Issuer = jwtOption.GetValue<string>(nameof(JwtOption.Issuer));
        options.Subject = jwtOption.GetValue<string>(nameof(JwtOption.Subject));
        options.Audience = jwtOption.GetValue<string>(nameof(JwtOption.Audience));
        options.ValidFor = TimeSpan.FromSeconds(jwtOption.GetValue<double>(nameof(JwtOption.ValidFor)));
        #region String
        {
            options.SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            options.SecurityKey = symmetricSecurityKey;
        }
        #endregion

        #region Pem
        //options.SigningCredentials = new SigningCredentials(privateRsaSecurityKey, SecurityAlgorithms.RsaSha256)
        //{
        //    CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
        //};
        //options.SecurityKey = publicRsaSecurityKey;
        #endregion
    });
    webApplicationBuilder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        // 當驗證失敗時，回應標頭會包含 WWW-Authenticate 標頭，這裡會顯示失敗的詳細錯誤原因
        // 預設值為 true，有時會特別關閉
        options.IncludeErrorDetails = true;
        var tokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = jwtOption.GetValue<string>(nameof(JwtOption.NameClaimType)),
            RoleClaimType = jwtOption.GetValue<string>(nameof(JwtOption.RoleClaimType)),
            ValidateIssuer = true,
            ValidIssuer = jwtOption.GetValue<string>(nameof(JwtOption.Issuer)),
            ValidateAudience = true,
            ValidAudience = jwtOption.GetValue<string>(nameof(JwtOption.Audience)),
            RequireExpirationTime = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = symmetricSecurityKey
        };
        options.TokenValidationParameters = tokenValidationParameters;
    });
}
#endregion

#region DataProtection
{
    var configurationSection = webApplicationBuilder.Configuration.GetSection("DataProtection");
    var dataProtectionBuilder = webApplicationBuilder.Services.AddDataProtection()
        .SetApplicationName(configurationSection.GetValue<string>("ApplicationName"));
    if (configurationSection.GetValue<bool>("AutomaticKeyGeneration"))
    {
        dataProtectionBuilder.SetDefaultKeyLifetime(TimeSpan.FromDays(configurationSection.GetValue<int>("KeyLifetime")));
    }
    else
    {
        dataProtectionBuilder.DisableAutomaticKeyGeneration();
    }
    #region FileSystem
    {
        dataProtectionBuilder.PersistKeysToFileSystem(Directory.CreateDirectory(webApplicationBuilder.Configuration.GetValue<string>("Path:Temp")));
    }
    #endregion

    #region Redis
    //{
    //    var configurationOptions = ConfigurationOptions.Parse(webApplicationBuilder.Configuration.GetValue<string>("RedisConnectionStrings:Default"));
    //    configurationOptions.CertificateValidation += (request, certificate, chain, sslPolicyErrors) =>
    //    {
    //        return true;
    //    };
    //    var connectionMultiplexer = ConnectionMultiplexer.Connect(configurationOptions);
    //    dataProtectionBuilder.PersistKeysToStackExchangeRedis(connectionMultiplexer, "DataProtection-Keys");
    //}
    #endregion

    #region Database
    //{
    //    dataProtectionBuilder.PersistKeysToDbContext<DataProtectionDbContext>();
    //}
    #endregion
}
#endregion
// Background
//webApplicationBuilder.Services.AddHostedService<DefaultHostedService>();
//webApplicationBuilder.Services.AddHostedService<DefaultBackgroundService>();
// Service
webApplicationBuilder.Services.AddScoped<IUserService, UserBackendService>();
LoadBusinessLayerRegister.LoadServices(webApplicationBuilder.Services);
LoadDataLayerRegister.LoadServices(webApplicationBuilder.Services);
webApplicationBuilder.Services.AddAutoMapper(configure =>
{
    //configure.AllowNullDestinationValues = false;
    LoadBackendRegister.LoadAutoMappers(configure);
    LoadBusinessLayerRegister.LoadAutoMappers(configure);
    LoadDataLayerRegister.LoadAutoMappers(configure);
});
webApplicationBuilder.Services.AddControllers();
    //.AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
    //.ConfigureApiBehaviorOptions(options =>
    //{
    //    options.SuppressConsumesConstraintForFormFileParameters = true;
    //    options.SuppressInferBindingSourcesForParameters = true;
    //    options.SuppressModelStateInvalidFilter = true;
    //    options.SuppressMapClientErrors = true;
    //    options.ClientErrorMapping[StatusCodes.Status404NotFound].Link = "https://httpstatuses.com/404";
    //});
webApplicationBuilder.Services.AddScoped<JwtAuthorizationFilter>();
webApplicationBuilder.Services.AddControllersWithViews(options =>
{
    //options.Filters.Add(typeof(DefaultAuthorizationFilter));
    //options.Filters.Add(typeof(DefaultResourceFilter));
    //options.Filters.Add(typeof(DefaultActionFilter));
    //options.Filters.Add(typeof(DefaultResultFilter));
    options.Filters.Add(typeof(DefaultExceptionFilter));
});

webApplicationBuilder.Services.AddMvcCore(options =>
{
    //options.MaxModelValidationErrors = 50;
})
.ConfigureApiBehaviorOptions(options =>
{
    //options.SuppressConsumesConstraintForFormFileParameters = false;
    //options.SuppressInferBindingSourcesForParameters = false;
    //options.SuppressModelStateInvalidFilter = false;
    //options.SuppressMapClientErrors = false;
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        //return new BadRequestObjectResult(new { Message = "Model binding occurs problem." });
        //var messgae = new List<string>();
        //foreach (var modelState in actionContext.ModelState)
        //{
        //    foreach (var errors in modelState.Value.Errors)
        //    {
        //        messgae.Add(errors.ErrorMessage);
        //    }
        //}
        //return new OkObjectResult(new CommonOutputDto<string>()
        //{
        //    Success = false,
        //    Message = string.Join(Environment.NewLine, messgae),
        //});
        return new OkObjectResult(new CommonOutputDto<string>()
        {
            Success = false,
            Message = actionContext.ModelState.Last().Value!.Errors.Last().ErrorMessage,
        });
    };
});

//webApplicationBuilder.Services.AddSpaStaticFiles(configuration =>
//{
//    configuration.RootPath = "wwwroot";
//});

if (webApplicationBuilder.Environment.IsDevelopment())
{ 
    // do something
}
else
{
    //webApplicationBuilder.Services.AddHsts(options =>
    //{
    //    options.Preload = true;
    //    options.IncludeSubDomains = true;
    //    options.MaxAge = TimeSpan.FromDays(60);
    //    options.ExcludedHosts.Add("example.com");
    //    options.ExcludedHosts.Add("www.example.com");
    //});
    //webApplicationBuilder.Services.AddHttpsRedirection(options =>
    //{
    //    options.RedirectStatusCode = (int)HttpStatusCode.PermanentRedirect;
    //    options.HttpsPort = 443;
    //});
}
#endregion

#region WebApplication
var webApplication = webApplicationBuilder.Build();

#region Lifetime
webApplication.Lifetime.ApplicationStarted.Register(() =>
{
    webApplication.Logger.LogInformation("Started:[{time}]", DateTimeOffset.Now);
});
webApplication.Lifetime.ApplicationStopping.Register(() =>
{
    webApplication.Logger.LogInformation("Stopping:[{time}]", DateTimeOffset.Now);
});
webApplication.Lifetime.ApplicationStopped.Register(() =>
{
    webApplication.Logger.LogInformation("Stopped:[{time}]", DateTimeOffset.Now);
});
#endregion

#region 中介軟體
//名稱                                  描述                          API
//Authentication                        認證中介軟體                  app.UseAuthentication()
//Authorization                         授權中介軟體.                 app.UseAuthorization()
//CORS                                  跨域中介軟體.                 app.UseCors()
//Exception Handler                     全域性異常處理中介軟體.       app.UseExceptionHandler()
//Forwarded Headers                     代理頭資訊轉發中介軟體.       app.UseForwardedHeaders()
//HTTPS Redirection                     Https重定向中介軟體.          app.UseHttpsRedirection()
//HTTP Strict Transport Security (HSTS) 特殊響應頭的安全增強中介軟體. app.UseHsts()
//Request Logging                       HTTP請求和響應日誌中介軟體.   app.UseHttpLogging()
//Response Caching                      輸出快取中介軟體.             app.UseResponseCaching()
//Response Compression                  響應壓縮中介軟體.             app.UseResponseCompression()
//Session                               Session中介軟體               app.UseSession()
//Static Files                          靜態檔案中介軟體.             app.UseStaticFiles(), app.UseFileServer()
//WebSockets                            WebSocket支援中介軟體.        app.UseWebSockets()
#endregion

// Configure the HTTP request pipeline.

if (webApplicationBuilder.Environment.IsDevelopment())
{
    webApplication.UseCors("CorsPolicy");
    webApplication.UseDeveloperExceptionPage();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //webApplication.UseHsts();
    //app.UseHttpsRedirection();
}
// before UseRouting
//webApplication.UseDefaultFiles();
//webApplication.UseStaticFiles();
//webApplication.UseSpaStaticFiles();
// need UseRouting
//webApplication.UseRouting();
//webApplication.UseMiddleware<DefaultMiddleware>();
// before Middleware
//webApplication.UseCors();
// before UseEndpoints
webApplication.UseAuthentication();
//webApplication.UseAuthorization();
// X-XSS-Protection (Use: Content-Security-Policy)
//webApplication.Use(async (context, next) =>
//{
//    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
//    await next();
//});
// Content-Security-Policy
webApplication.UseCsp(options =>
{
    options.ScriptSrc.AllowSelf().UnsafeInline().UnsafeEval();
    options.StyleSrc.AllowSelf().UnsafeInline();
    options.ImageSrc.AllowSelf().Allow("data:");
    options.ChildSrc.Disallow();
    options.FrameAncestors.Disallow();
});
// X-Frame-Options
webApplication.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    await next();
});
// X-Content-Type-Options
webApplication.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    await next();
});
// Strict-Transport-Security (UseHsts)
//webApplication.Use(async (context, next) =>
//{
//    context.Response.Headers.Add("Strict-Transport-Security", "max-age=31536000");
//    await next();
//});

webApplication.MapControllers();

Console.WriteLine("app Start");
webApplication.Run();
Console.WriteLine("app End");
#endregion
public partial class Program { }
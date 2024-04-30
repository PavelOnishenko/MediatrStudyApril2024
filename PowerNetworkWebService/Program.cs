using PowerNetworkWebService;
using Microsoft.IdentityModel.Tokens;
using PowerNetworkWebService.Commands;
using PowerNetworkWebService.Db;

var builder = WebApplication.CreateBuilder(args);
RegisterServices(builder, builder.Services);
var app = builder.Build();
ConfigureApp(app);
using (var db = app.Services.GetRequiredService<IDb>())
{
    db.ClearTables();
    var liveDb = db as LiveDb;
    liveDb?.SeedTestData();
}
app.Run();

static void RegisterServices(WebApplicationBuilder builder, IServiceCollection services)
{
    services.AddControllers();
    RegisterAuthorization(services);
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddTransient<IDb, LiveDb>(provider => new LiveDb(builder.Configuration.GetConnectionString("PostgreSQLConnection")!));
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplyEfficiencyMeasureCommand).Assembly));
    services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.AddConsole();
        loggingBuilder.AddDebug();
    });
}

static void RegisterAuthorization(IServiceCollection services)
{
    services.AddIdentityServer()
            .AddInMemoryApiScopes(IdentityServerConfig.ApiScopes)
            .AddInMemoryClients(IdentityServerConfig.Clients)
            .AddDeveloperSigningCredential();
    services.AddAuthentication("Bearer")
       .AddJwtBearer("Bearer", options =>
       {
           options.Authority = "http://localhost:5099";
           options.RequireHttpsMetadata = false;
           options.TokenValidationParameters = new TokenValidationParameters { ValidateAudience = false };
       });
    services.AddAuthorizationBuilder()
        .AddPolicy("theAPI", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("scope", "theAPI");
        });
}

static void ConfigureApp(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseIdentityServer();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
}
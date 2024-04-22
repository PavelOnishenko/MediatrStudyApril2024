using Api_MediatrStudyApril2024;
using Library_MediatrStudyApril2024;
using Library_MediatrStudyApril2024.Commands;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
RegisterServices(builder, builder.Services);
var app = builder.Build();
ConfigureApp(app);
using (var db = app.Services.GetRequiredService<IDb>())
{
    db.ClearTables();
    db.SeedTestData();
}
app.Run();

static void RegisterServices(WebApplicationBuilder builder, IServiceCollection services)
{
    services.AddControllers();
    RegisterAuthorization(services);
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.AddTransient<IDb, LiveDb>(provider => new LiveDb(builder.Configuration.GetConnectionString("PostgreSQLConnection")!));
    services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplyEfficiencyMeasuresCommand).Assembly));
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
            .AddDeveloperSigningCredential();  // not recommended for production
    services.AddAuthentication("Bearer")
       .AddJwtBearer("Bearer", options =>
       {
           options.Authority = "http://localhost:5099"; // The URL of the IdentityServer
           options.RequireHttpsMetadata = false; // False for development, true in production

           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateAudience = false // Often set to false when the API is the only audience
           };
       });
    services.AddAuthorization(options =>
    {
        options.AddPolicy("writeAPI", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireClaim("scope", "writeAPI");
        });
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
    //app.UseEndpoints(x => x.MapControllers());
    app.MapControllers();
}
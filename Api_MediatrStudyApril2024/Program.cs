using Library_MediatrStudyApril2024;
using Library_MediatrStudyApril2024.Commands;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddTransient<IDb, LiveDb>(provider => new LiveDb(builder.Configuration.GetConnectionString("PostgreSQLConnection")!));
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ApplyEfficiencyMeasuresCommand).Assembly));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var db = app.Services.GetRequiredService<IDb>())
{
    db.ClearTables();
    db.SeedTestData();
}

app.Run();

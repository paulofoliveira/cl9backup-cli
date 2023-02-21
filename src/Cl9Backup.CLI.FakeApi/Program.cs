using Cl9Backup.CLI.FakeApi;
using Cl9Backup.CLI.FakeApi.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = options.JsonSerializerOptions.DictionaryKeyPolicy = null;
    });

builder.Services.AddScoped<UserProfileRepository>();
builder.Services.AddScoped<SessionKeyGenerator>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UsePathBase(new PathString("/api/v1"));
app.UseRouting();

app.MapControllers();

app.Run();

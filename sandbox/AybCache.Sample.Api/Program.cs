var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddHttpClient();

// 1. Must initialized IDistributedCache implementation
var redisConnectionString = builder.Configuration.GetValue<string>("Redis:ConnectionString");
builder.Services.AddDistributedRedisCache(options =>
{
    options.Configuration = redisConnectionString;
});

builder.Services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConnectionString));

// 2. Add Ayb Cache
builder.Services.AddAybCache();

// 3.1 Repository example
builder.Services.AddProxiedScoped<IProductRepository, ProductRepository>();

// 3.2 HttpClient example
builder.Services.AddProxiedScoped<IAgifyHttpClient, AgifyHttpClient>();

// 3.3 Mediator example
builder.Services.AddProxiedScoped<IRequestHandler<GetNameQuery, string>, GetNameQueryHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
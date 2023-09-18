using EStoreAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var con = builder.Configuration["ConnectionStrings:FStoreDB"];
var secretKey = builder.Configuration["SecretKey"];
builder.Services.AddApiService(con!,secretKey!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();

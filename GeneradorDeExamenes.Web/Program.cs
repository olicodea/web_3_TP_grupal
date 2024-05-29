using GeneradorDeExamanes.Configurations;
using GeneradorDeExamanes.Logica.Services;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure ApiSettings
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<ApiSettings>>().Value);

// Register services
builder.Services.AddHttpClient<IApiService, ApiService>();
builder.Services.AddSingleton<IIaService, IaService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Ia}/{action=GeneradorPreguntas}/{id?}");

app.Run();

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.ResolveConflictingActions(y => y.First());
    x.IgnoreObsoleteActions();
    x.IgnoreObsoleteProperties();
    x.CustomSchemaIds(y => y.FullName);

    x.IncludeXmlComments(@$"{System.AppDomain.CurrentDomain.BaseDirectory}/LawenconTestAPI.XML");

    x.SwaggerDoc(
        name: "v1",
        info: new OpenApiInfo
        {
            Version = "v1",
            Title = "Lawencon Test App",
            Description = $@"Lawencon Test App - Open API Documentation",
            TermsOfService = new Uri("https://github.com/fadhly-permata/"),
            Contact = new OpenApiContact
            {
                Name = "Fadhly Permata",
                Email = "fadhly.permata@gmail.com",
                Url = new Uri("https://github.com/fadhly-permata/")
            }
        }
    );
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(x =>
{
    x.DocExpansion(DocExpansion.List);
    x.SwaggerEndpoint("/swagger/v1/swagger.json", "LawenconTestAPI");
});

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
    pattern: "{controller=Home}/{action=Index}/{id?}")
;

app.Run();

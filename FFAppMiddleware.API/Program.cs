using FFappMiddleware.Application.Services.Real;
using FFappMiddleware.DataBase.Logger;
using FFAppMiddleware.API.DependencyInjection;
using FFAppMiddleware.API.Security;
using FFAppMiddleware.Model.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

ConnectionStringSettings.InitializeConnectionString(builder.Configuration);
builder.Services.RegisterApplicationServices();


builder.Services.AddSingleton<ICustomAuthenticationManager, CustomAuthenticationManager>();

builder.Services.AddAuthentication(CustomBearerAuthenticationOptions.DefaultScheme)
    .AddScheme<CustomBearerAuthenticationOptions, CustomAuthenticationHandler>(CustomBearerAuthenticationOptions.DefaultScheme, null);

builder.Services.AddControllers();
builder.Services.AddScoped<UserManagementService>();

WriteLog.InitLoggers();

#region Swagger/OpenAPI Configuration

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Dita EstFarm",
        Description = "FFAppMiddleware Project. ASP.NET Web API",
        Contact = new OpenApiContact
        {
            Name = ".Net Developers: Denis Polomarenco, Eugen Cojocaru, Mihai Tamazlîcaru",
            Email = string.Empty,
            Url = new Uri("https://twitter.com/spboyer"),
        },
        License = new OpenApiLicense
        {
            Name = "Dita EstFarm Licence",
            Url = new Uri("https://example.com/license"),
        },
    });

    //c.SchemaFilter<NullableDefaultSchemaFilter>();
    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. <br /><br />" +
                      @"Enter 'Bearer' [space] and then your token in the text input below. <br />" +
                      @"DitaEstFarm Authorization - Root. <br />",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement(){
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id =  JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "oauth2",
                Name =  JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();


}

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


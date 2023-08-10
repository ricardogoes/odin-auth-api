using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Odin.Auth.Api;
using Odin.Auth.Api.Attributes;
using Odin.Auth.Api.IoC;
using Odin.Auth.Domain.Models;

var builder = WebApplication.CreateBuilder(args);


var appSettings = new AppSettings
{
    AWSCognitoSettings = new AWSCognitoSettings
    {
        AccessKeyId = Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__AccessKeyId"),
        AccessSecretKey = Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__AccessSecretKey"),
        AppClientId = Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__AppClientId"),
        CognitoAuthorityUrl = Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__CognitoAuthorityUrl"),
        CognitoIdpUrl = Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__CognitoIdpUrl"),
        Region = Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__Region"),
        UserPoolId = Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__UserPoolId")
    }
};

// Add services to the container.
builder.Services.AddSingleton(appSettings);
builder.Services.AddScoped<ValidationFilterAttribute>();
ServiceBase.GetInstance<ServiceCognito>().Add(builder.Services);
ServiceBase.GetInstance<ServiceServices>().Add(builder.Services);

builder.Services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen();

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.AddCognitoIdentity();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = "";// appSettings.AWSCognitoSettings.CognitoAuthorityUrl;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false
    };
});

builder.Services.AddApiVersioning(options =>
{
    // Retorna os headers "api-supported-versions" e "api-deprecated-versions"
    // indicando versoes suportadas pela API e o que esta como deprecateds
    options.ReportApiVersions = true;

    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddVersionedApiExplorer(options =>
{
    // Agrupar por numero de versao
    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
    // note: the specified format code will format the version as "'v'major[.minor][-status]"
    options.GroupNameFormat = "'v'VVV";

    // Necessario para o correto funcionamento das rotas
    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
    // can also be used to control the format of the API version in route templates
    options.SubstituteApiVersionInUrl = true;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware(typeof(ErrorHandlingMiddleware));

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseResponseCompression();

app.MapControllers();

app.Run();

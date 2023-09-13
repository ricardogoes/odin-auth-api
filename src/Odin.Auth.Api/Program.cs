using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Odin.Auth.Api.Configurations;
using Odin.Auth.Api.Filters;
using Odin.Auth.Application.Common;
using Odin.Auth.Infra.Cognito.Models;
using Odin.Auth.Infra.Messaging.Policies;
using Odin.Baseline.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy => 
    { 
        policy.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var cognitoSettings = new CognitoSettings
(
    Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__AccessKeyId"),
    Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__AccessSecretKey"),
    Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__AppClientId"),
    Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__CognitoAuthorityUrl"),
    Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__CognitoIdpUrl"),
    Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__Region"),
    Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__UserPoolId")
);

var appSettings = new AppSettings(cognitoSettings);

// Add services to the container.
builder.Services.AddSingleton(appSettings);
builder.Services.AddSingleton(cognitoSettings);
builder.Services.AddApplications();
builder.Services.AddRepositories();

builder.Services
    .AddControllers(options =>
    {
        options.Filters.Add(typeof(ApiExceptionFilter));
    })
    .AddJsonOptions(jsonOptions =>
    {
        jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = new JsonSnakeCasePolicy();
        jsonOptions.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

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
    options.Authority = appSettings.AWSCognitoSettings.CognitoAuthorityUrl;
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

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.UseResponseCompression();

app.MapControllers();

app.Run();

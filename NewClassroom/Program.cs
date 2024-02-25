using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using NewClassroom.Serialization.Formatters;
using NewClassroom.Services;
using NewClassroom.Wrappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true;
    options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
    options.OutputFormatters.Add(new PlainTextOutputFormatter());
});

builder.Services.Add(new ServiceDescriptor(typeof(IUserStatsService), x => new UserStatsService(), ServiceLifetime.Transient));
builder.Services.Add(new ServiceDescriptor(typeof(IHttpClient), x => new HttpClientWrapper(), ServiceLifetime.Singleton));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(@"bin\NewClassroom.xml");
});

builder.Services.Configure<MvcOptions>(options =>
{
    var serializerSettings = options.OutputFormatters
        .OfType<XmlDataContractSerializerOutputFormatter>()
        .Single()
        .SerializerSettings;

    serializerSettings.SerializeReadOnlyTypes = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseAuthorization();

app.MapControllers();

app.Run();

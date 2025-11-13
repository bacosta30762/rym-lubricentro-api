using Aplicacion.Extensiones;
using Microsoft.OpenApi.Models;
using System.Text.Json;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    // Configuración de servicios
    public void ConfigureServices(IServiceCollection services)
    {        
        // Agregar servicios al contenedor
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "RyM Lubricentro API",
                Version = "v1",
                Description = "Comprehensive RESTful API for mechanic shop service management system",
                Contact = new OpenApiContact
                {
                    Name = "RyM Lubricentro",
                    Email = "support@lubricentrorym.com"
                }
            });

            // Configuración para incluir Bearer Token
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Introduce el token en el formato: Bearer {token}"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
        });

        services.AddInfraestructura(Configuration);
        services.AddAplicacion();
        services.AddHttpContextAccessor();

        // Configurar CORS
        services.AddCors(options =>
        {
            options.AddPolicy("PermitirFrontend", builder =>
            {
                builder.WithOrigins("http://localhost:3000", //local
                    "http://localhost:3001", //local
                    "https://bacosta30762.github.io") //GitHub Origen del frontend
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials(); // Si estás manejando cookies o autenticación
            });
        });

        // Configuración de JSON
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                // Configuración adicional para manejar caracteres especiales
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            });
}

    // Configuración del pipeline HTTP
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseStaticFiles();

        app.UseCors("PermitirFrontend"); // Aplicar política de CORS

        // Asegúrate de poner UseRouting antes de UseEndpoints
        app.UseRouting();  

        app.UseAuthentication();  // Manejo de autenticación
        app.UseAuthorization();   // Manejo de autorización

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers(); 
        });

    }
}

using System.Text.Json.Serialization;
using Taller_1_Herramientas.InyeccionDependencias;

namespace Taller_1_Herramientas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuracion = builder.Configuration;

            builder.Services.AgregarDependencias(configuracion);
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
            builder.Services.AddSwaggerGen();

            // CORS: permite que el frontend Angular llame a la API.
            // Para producción agrega el dominio en appsettings.json -> "Cors": { "Origenes": [ "https://tu-dominio" ] }
            var origenes = configuracion.GetSection("Cors:Origenes").Get<string[]>()
                           ?? new[] { "http://localhost:4200" };
            builder.Services.AddCors(opciones =>
                opciones.AddPolicy("Frontend", politica =>
                    politica.WithOrigins(origenes).AllowAnyHeader().AllowAnyMethod()));

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseCors("Frontend");   // debe ir antes de MapControllers

            app.MapControllers();

            app.Run();
        }
    }
}

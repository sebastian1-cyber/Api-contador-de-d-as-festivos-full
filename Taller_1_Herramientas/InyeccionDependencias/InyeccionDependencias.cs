using apiFestivos.aplicacion;
using apiFestivos.core.repositorio;
using apiFestivos.core.servicios;
using Apifestivos.infraestructura.Persistencia;
using Apifestivos.infraestructura.repositorios;
using Microsoft.EntityFrameworkCore;

namespace Taller_1_Herramientas.InyeccionDependencias
{
    public static class InyeccionDependencias
    {
        public static IServiceCollection AgregarDependencias(this IServiceCollection servicios,
                                               IConfiguration configuracion)
        {
            servicios.AddDbContext<festivosApiContext>(opciones =>
            {
                opciones.UseSqlServer(configuracion.GetConnectionString("ApiFestivos"));
            });

            servicios.AddTransient<IPaisRepositorio, PaisesRepositorio>();
            servicios.AddTransient<IFestivoRepositorio, FestivosRepositorio>();
            servicios.AddTransient<ITipoRepositorio, TipoRepositorio>();   // FALTABA: sin esto GET /api/tipo da error 500

            servicios.AddTransient<IPaisServicio, PaisServicio>();
            servicios.AddTransient<IFestivosServicio, FestivoServicio>();
            servicios.AddTransient<ITipoServicio, TipoServicio>();         // FALTABA
            servicios.AddTransient<ICalendarioServicio, CalendarioServicio>();

            return servicios;
        }
    }
}

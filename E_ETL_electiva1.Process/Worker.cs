using E_ETL_electiva1.Entities.interfaces.Iservices;

namespace E_ETL_electiva1.Process
{
    public class Worker(IServiceScopeFactory scopeFactory, ILogger<Worker> logger) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = scopeFactory.CreateScope();
            var csvService = scope.ServiceProvider.GetRequiredService<ICsvService>();
            var dbTransService = scope.ServiceProvider.GetRequiredService<ITransDbService>();
            var apiService = scope.ServiceProvider.GetRequiredService<IApiService>();

            logger.LogInformation("Iniciando ciclo de extraccion/carga: {time}", DateTimeOffset.Now);

            await EjecutarFuente("CSV", async () =>
            {
                await csvService.upload_Clientes();
                await csvService.upload_Productos();
                await csvService.upload_Fuentes();
            }, logger);

            await EjecutarFuente("Base de datos transaccional", async () =>
            {
                await dbTransService.upload_Clientes();
                await dbTransService.upload_Productos();
                await dbTransService.upload_Fuentes();
            }, logger);

            await EjecutarFuente("API REST", async () =>
            {
                await apiService.upload_Clientes();
                await apiService.upload_Productos();
                await apiService.upload_Fuentes();
                await apiService.upload_Redes();
            }, logger);

            logger.LogInformation("Ciclo finalizado: {time}", DateTimeOffset.Now);
        }

        private static async Task EjecutarFuente(string nombre, Func<Task> accion, ILogger logger)
        {
            try
            {
                await accion();
                logger.LogInformation("Fuente {Fuente} procesada correctamente", nombre);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error procesando la fuente {Fuente}", nombre);
            }
        }
    }
}
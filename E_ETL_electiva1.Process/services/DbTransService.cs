using E_ETL_electiva1.api.context;
using E_ETL_electiva1.api.Models;
using E_ETL_electiva1.Data.Repositories;
using E_ETL_electiva1.Entities.interfaces;
using E_ETL_electiva1.Entities.interfaces.Iservices;
using E_ETL_electiva1.Entities.Models;
using E_ETL_electiva1.Entities.Models.csv;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Globalization;
namespace E_ETL_electiva1.Process.services
{
    internal class DbTransService:ITransDbService
    {
        private readonly string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStringBdAnalit"].ConnectionString;       
        private readonly opiniones_de_clientesDBContext _Context;
        private readonly DbSet<Clientes> _DbSetClientes;
        private readonly DbSet<Productos> _DbSetProductos;
        private readonly DbSet<Tipos_Fuente> _DbSettipoFuente;
        public DbTransService(opiniones_de_clientesDBContext opinionesContext)
        {
            _Context = opinionesContext;
           _DbSetClientes = _Context.Set<Clientes>();
           _DbSetProductos = _Context.Set<E_ETL_electiva1.api.Models.Productos>();
           _DbSettipoFuente = _Context.Set<E_ETL_electiva1.api.Models.Tipos_Fuente>();

        
        }

        public async Task<bool> upload_Clientes() {
            DataTable Clientes = new DataTable();
            Clientes.Columns.Add("ID", typeof(string));


            var ClientesList = await _DbSetClientes.AsNoTracking().Select(c => c.IdCliente).ToListAsync();

            foreach (string IdCliente in ClientesList)
            {


                if (String.IsNullOrEmpty(IdCliente)) { continue; }

                Clientes.Rows.Add(IdCliente);

            }


            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();
                using (SqlBulkCopy bulk = new SqlBulkCopy(conn))
                {
                    bulk.DestinationTableName = "Clientes";
                    bulk.ColumnMappings.Add("ID", "IdCliente");
                    try
                    {
                        await bulk.WriteToServerAsync(Clientes);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }

            }
            return true;

        }
        public async Task<bool> upload_Productos() {
            DataTable Productos = new DataTable();
            Productos.Columns.Add("ID", typeof(string));


            var ProductossList = await _DbSetProductos.AsNoTracking().Select(c => c.IdProducto).ToListAsync();

            foreach (string IdProducto in ProductossList)
            {


                if (String.IsNullOrEmpty(IdProducto)) { continue; }

                Productos.Rows.Add(IdProducto);

            }


            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();
                using (SqlBulkCopy bulk = new SqlBulkCopy(conn))
                {
                    bulk.DestinationTableName = "Productos";
                    bulk.ColumnMappings.Add("ID", "IdProducto");
                    try
                    {
                        await bulk.WriteToServerAsync(Productos);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }

            }
            return true;

        }
        public async Task<bool> upload_Fuentes() {

            DataTable Fuentes = new DataTable();
            Fuentes.Columns.Add("ID", typeof(string));

            Fuentes.Rows.Add("Web Reviews");
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();
                using (SqlBulkCopy bulk = new SqlBulkCopy(conn))
                {
                    bulk.DestinationTableName = "Fuentes";
                    bulk.ColumnMappings.Add("ID", "Fuente");
                    try
                    {
                        await bulk.WriteToServerAsync(Fuentes);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }

            }
            return true;

        }
        public async Task<bool> upload_Opiniones() { return true; }
    }
}

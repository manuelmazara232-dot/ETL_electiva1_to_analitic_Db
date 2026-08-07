using E_ETL_electiva1.Data.Repositories;
using E_ETL_electiva1.Entities.interfaces;
using E_ETL_electiva1.Entities.interfaces.Iservices;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace E_ETL_electiva1.Process.services
{
    internal class apiService : IApiService 
    {
        private readonly string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStringBdAnalit"].ConnectionString;

        private readonly IApiConsRepository _apiConsRepository;
        public apiService(IApiConsRepository apiConsRepository)
        {
            _apiConsRepository = apiConsRepository;
        }

        public async Task<bool> upload_Clientes()
        {
            DataTable Clientes = new DataTable();
            Clientes.Columns.Add("ID", typeof(string));


            var ClientesList = await _apiConsRepository.GetClientes() ;

            foreach (var Cliente in ClientesList)
            {

             
                if (String.IsNullOrEmpty(Cliente.IdCliente)) { continue; }

                Clientes.Rows.Add(Cliente.IdCliente);

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
                        await bulk.WriteToServerAsync(Clientes); return true;
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message);  return false;  }
                }

            }
            return true;
        }
        public async Task<bool> upload_Productos() {
            DataTable Productos = new DataTable();
            Productos.Columns.Add("ID", typeof(string));


            var ProductosList = await _apiConsRepository.GetProductos();

            foreach (var Producto in ProductosList)
            {


                if (String.IsNullOrEmpty(Producto.IdProducto)) { continue; }

                Productos.Rows.Add(Producto.IdProducto);

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
                        await bulk.WriteToServerAsync(Productos); return true;
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message);  return false; }
                }

            }
            return true;
        }
        public async Task<bool> upload_Fuentes()
        {

            DataTable Fuentes = new DataTable();
            Fuentes.Columns.Add("ID", typeof(string));

            Fuentes.Rows.Add("Social Comments");
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
                        return true;
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message);
                        return false;
                    }
                }

            }
            return true;
        }
        public async Task<bool> upload_Redes()
        {
            DataTable Redes = new DataTable();
            Redes.Columns.Add("Nombre", typeof(string));


            var RedesList = await _apiConsRepository.GetRedesSociales();

            foreach (var Red in RedesList)
            {


                if (String.IsNullOrEmpty(Red.NombreRedSocial)) { continue; }

                Redes.Rows.Add(Red.NombreRedSocial);

            }


            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();
                using (SqlBulkCopy bulk = new SqlBulkCopy(conn))
                {
                    bulk.DestinationTableName = "Redes_Sociales";
                    bulk.ColumnMappings.Add("Nombre", "NombreRedSocial");
                    try
                    {
                        await bulk.WriteToServerAsync(Redes);
                        return true;
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message);
                        return false;
                    }
                }

            }
            
        }

        //public Task<bool> upload_Opiniones();
    }
}

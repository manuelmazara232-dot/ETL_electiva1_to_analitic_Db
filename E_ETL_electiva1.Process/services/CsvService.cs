using E_ETL_electiva1.api.Models;
using E_ETL_electiva1.Data.Repositories;
using E_ETL_electiva1.Entities.interfaces;
using E_ETL_electiva1.Entities.interfaces.Iservices;
using E_ETL_electiva1.Entities.Models.csv;
using E_ETL_electiva1.Entities.Models.Dwh;
using E_ETL_electiva1.Entities.Models.Dwh.Dims;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System.Data;
namespace E_ETL_electiva1.Process.services
{
    internal class CsvService:ICsvService
    {//ConnStringBdAnalit

        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnStringBdAnalit"].ConnectionString;

        private readonly ICsvRepository _csvRepository;


        public CsvService(ICsvRepository csvRepository) { 
        _csvRepository = csvRepository;
        }





        public async Task<bool> upload_Clientes() {
            
            DataTable Clientes = new DataTable();
            Clientes.Columns.Add("ID", typeof(string));


            var csv = _csvRepository.GetAll();

            foreach (surveys_part1 entrevista in csv)
            {

                if (String.IsNullOrEmpty($"{entrevista.IdCliente}")) { continue; }

                Clientes.Rows.Add(entrevista.IdCliente);

            }

     
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();
                using (SqlBulkCopy bulk = new SqlBulkCopy(conn))
                {
                    bulk.DestinationTableName ="Clientes";
                    bulk.ColumnMappings.Add("ID", "IdCliente");
                    try {
                        await bulk.WriteToServerAsync(Clientes);
                    }
                    catch(Exception ex) { Console.WriteLine(ex.Message); }
                }

            }
            return true; }




        public async Task<bool> upload_Productos()
        {
            DataTable productos = new DataTable();
            productos.Columns.Add("IdProducto", typeof(string));
            var csv = _csvRepository.GetAll();


            foreach (surveys_part1 entrevista in csv)
            {
                if (String.IsNullOrEmpty($"{entrevista.IdProducto}")) { continue; }

                productos.Rows.Add(entrevista.IdProducto);



            }
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();
                using (SqlBulkCopy bulk = new SqlBulkCopy(conn))
                {
                    {
                        bulk.DestinationTableName = "Productos";

                        bulk.ColumnMappings.Add("IdProducto", "IdProducto");
                        
                        try{await bulk.WriteToServerAsync(productos);}
                        
                        catch(Exception ex) { Console.WriteLine(ex.Message); }
                   
                    }
                }
                return true;
            }
        }
        
        
        
        
        
        public async Task<bool> upload_Fuentes() {
            DataTable Fuentes = new DataTable();
            Fuentes.Columns.Add("Fuente", typeof(string));
            var csv = _csvRepository.GetAll();
            HashSet<String> FuentesProcesadas = new HashSet<string>();

            foreach (surveys_part1 entrevista in csv)
            {
                if (String.IsNullOrEmpty($"{entrevista.Fuente}")) { continue; }
                if(!FuentesProcesadas.Add(entrevista.Fuente)) { continue; } 
                Fuentes.Rows.Add(entrevista.Fuente);



            }
            using (SqlConnection conn = new SqlConnection(connString))
            {
                await conn.OpenAsync();
                using (SqlBulkCopy bulk = new SqlBulkCopy(conn))
                {
                    {
                        bulk.DestinationTableName = "Fuentes";

                        bulk.ColumnMappings.Add("Fuente", "Fuente");

                        try { await bulk.WriteToServerAsync(Fuentes); }

                        catch (Exception ex) { Console.WriteLine(ex.Message); }

                    }
                }
                return true;
            }
        }





        public async Task<bool> upload_Opiniones() { return true; }
    }
}

using Azure;
using E_ETL_electiva1.api.Models;
using E_ETL_electiva1.Entities.interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace E_ETL_electiva1.Data.Repositories
{
    public class apiRepo : IApiConsRepository
    {
        private readonly HttpClient _httpClient;
        public apiRepo(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Solo subira las dimensiones relevantes para los comentarios en redes sociales.
        public async Task<IEnumerable<Clientes>> GetClientes()
        {
            var respuesta = await _httpClient.GetAsync("https://localhost:7163/api/Clientes");
            respuesta.EnsureSuccessStatusCode();
            var json = await respuesta.Content.ReadAsStringAsync();

            List<Clientes> clientes = JsonSerializer.Deserialize<List<Clientes>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return clientes;


        }
        public async Task<IEnumerable<Productos>> GetProductos()
        {
            var respuesta = await _httpClient.GetAsync("https://localhost:7163/api/Productos");
            respuesta.EnsureSuccessStatusCode();
            var json = await respuesta.Content.ReadAsStringAsync();

            List<Productos> Productos = JsonSerializer.Deserialize<List<Productos>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return Productos;
        }
        public string GetFuentes()
        {
            return "Social Comments";
        }
        public async Task<IEnumerable<Redes_Sociales>> GetRedesSociales()
        {
            var respuesta = await _httpClient.GetAsync("https://localhost:7163/api/Redes_Sociales");
            respuesta.EnsureSuccessStatusCode();
            var json = await respuesta.Content.ReadAsStringAsync();

            List<Redes_Sociales> Redes_Sociales = JsonSerializer.Deserialize<List<Redes_Sociales>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return Redes_Sociales;
        }

    }
}

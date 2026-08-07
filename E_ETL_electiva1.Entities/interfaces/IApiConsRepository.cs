using E_ETL_electiva1.api.Models;
using E_ETL_electiva1.Entities.Models;
namespace E_ETL_electiva1.Entities.interfaces

{
    public interface IApiConsRepository
    {
        public Task<IEnumerable<Clientes>> GetClientes();
        public Task<IEnumerable<Productos>> GetProductos();
        public string GetFuentes();
        public Task<IEnumerable<Redes_Sociales>> GetRedesSociales();

    }
}

namespace E_ETL_electiva1.Entities.interfaces.Iservices
{
    public interface ICsvService

    {
        public Task<bool> upload_Clientes();
        public Task<bool> upload_Productos();
        public Task<bool> upload_Fuentes();
        //public Task<bool> upload_Opiniones();



    }
}

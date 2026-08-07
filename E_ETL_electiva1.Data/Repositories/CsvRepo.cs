using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using E_ETL_electiva1.Entities.Models.csv;
using E_ETL_electiva1.Entities.interfaces;
using System.Data;
namespace E_ETL_electiva1.Data.Repositories
{
    public class CsvRepo : ICsvRepository
    {
        //Solo subira las dimensiones relevantes para encuestas internas de satisfacción
        private readonly string _filePath = "C:\\Users\\manue\\source\\repos\\E_ETL_electiva1\\E_ETL_electiva1.Data\\Csv_Archives\\surveys_part1.csv";


        public IEnumerable<surveys_part1> GetAll()
        {

            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException($"El archivo CSV no fue encontrado en: {_filePath}");
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ","
            };

            using var reader = new StreamReader(_filePath);
            using var csv = new CsvReader(reader, config);


            return csv.GetRecords<surveys_part1>().ToList();
        }
    }
}

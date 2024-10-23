using CsvHelper;
using CsvHelper.Configuration;
using ExcelDataReader;
using KoiCareSystem.Common.DTOs.Request;
using System.Data;
using System.Globalization;

namespace KoiCareSystem.Service.Helper
{
    public interface IFileService
    {
        List<ImportDataDto> ReadFromCsv(Stream fileStream);
        List<ImportDataDto> ReadFromExcel(Stream fileStream);
    }
    public class FileService : IFileService
    {
        public List<ImportDataDto> ReadFromCsv(Stream fileStream)
        {
            using (var reader = new StreamReader(fileStream))
            using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null, // Bỏ qua lỗi kiểm tra tiêu đề
                MissingFieldFound = null // Bỏ qua lỗi khi thiếu trường
            }))
            {
                return csv.GetRecords<ImportDataDto>().ToList();
            }
        }

        public List<ImportDataDto> ReadFromExcel(Stream fileStream)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var reader = ExcelReaderFactory.CreateReader(fileStream))
            {
                var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                {
                    UseColumnDataType = false,
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                });

                DataTable dataTable = dataSet.Tables[0];

                var dtos = new List<ImportDataDto>();

                foreach (DataRow row in dataTable.Rows)
                {
                    var dto = new ImportDataDto
                    {
                        WaterVolume = GetValueOrDefault<decimal>(row, 0),
                        Temperature = GetValueOrDefault<decimal>(row, 1),
                        Salinity = GetValueOrDefault<decimal>(row, 2),
                        Ph = GetValueOrDefault<decimal>(row, 3),
                        O2 = GetValueOrDefault<decimal>(row, 4),
                        No2 = GetValueOrDefault<decimal>(row, 5),
                        No3 = GetValueOrDefault<decimal>(row, 6),
                        Po4 = GetValueOrDefault<decimal>(row, 7)
                    };

                    dtos.Add(dto);
                }

                return dtos;
            }
        }

        private T GetValueOrDefault<T>(DataRow row, int index)
        {
            object? value = row.ItemArray[index];

            if (value == null || value == DBNull.Value)
            {
                return default(T); // Return default value
            }

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (InvalidCastException ex)
            {
                throw new InvalidCastException($"Cannot cast value '{value}' of type '{value.GetType()}' to type '{typeof(T)}'.", ex);
            }
        }
    }
}


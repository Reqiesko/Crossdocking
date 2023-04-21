using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ExcelDataReader;

namespace Crossdocking.Services
{
    public class ExcelParserService
    {
        private readonly string _filePath;
        public List<DateTime> Date { get; set; }
        public List<double> Weight { get; set; }

        public string DeliveryDate { get; set; }
        public string DeliveryWeight { get; set; }
        public ExcelParserService(string filePath)
        {
            _filePath = filePath;
        }

        public void ParseExcelFile()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding utf8 = Encoding.GetEncoding("UTF-8");
            var excelReaderConfiguration = new ExcelReaderConfiguration();
            excelReaderConfiguration.FallbackEncoding = utf8;

            Date = new List<DateTime>();
            Weight = new List<double>();

            using (var stream = File.Open(_filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream, excelReaderConfiguration))
                {
                    reader.Read();
                    DeliveryDate = reader.GetValue(0).ToString() ?? string.Empty;
                    DeliveryWeight = reader.GetValue(1).ToString() ?? string.Empty;
                    while (reader.Read())
                    {
                        var date = reader.GetValue(0);
                        var weight = reader.GetValue(1);
                        if (date != null || weight != null)
                        {
                            Date.Add((DateTime)date);
                            Weight.Add((double)weight);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}

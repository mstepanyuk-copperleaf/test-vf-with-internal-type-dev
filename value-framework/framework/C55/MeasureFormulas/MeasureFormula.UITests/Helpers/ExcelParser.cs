using System;
using System.Data;
using System.IO;
using ExcelDataReader;
using static System.Decimal;

namespace MeasureFormula.UITests.Helpers
{
    public class ExcelParser
    {
        private readonly DataTable _dataTable;
        private readonly IExcelDataReader _reader;

        public ExcelParser(string fileName)
        {
            var pathToSampleCalcFiles = AssemblyPathManager.SetupAssemblyPath() + @"\..\..\..\..\..\..\tests\";
            var originalFileName = pathToSampleCalcFiles += $"{fileName}";

            using (var stream = File.Open(originalFileName, FileMode.Open, FileAccess.Read))
            {
                //create Reader
                var file = new FileInfo(originalFileName);
                if (file.Extension.Equals(".xls")) //old version until 3.4+
                    _reader = ExcelReaderFactory.CreateBinaryReader(stream);
                else if (file.Extension.Equals(".xlsx")) //in 3.4+
                    _reader = ExcelReaderFactory.CreateReader(stream);
                else
                    throw new Exception("Invalid FileName");

                var dataSetConfiguration = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = false
                    }
                };
                var dataSet = _reader.AsDataSet(dataSetConfiguration);
                _dataTable = dataSet.Tables[0];
            }
        }

        public void Close()
        {
            _reader.Close();
        }

        public string GetTabName()
        {
            return _dataTable.TableName;
        }

        public decimal? GetTotalValue(int indexOffset)
        {
            var cellCoordinates = GetCellCoordinates("Total Value");

            //read next cell to the right
            var colIndex = cellCoordinates.Item2 + indexOffset;
            var value = _dataTable.Rows[cellCoordinates.Item1][colIndex].ToString();
            return value == "n/a" ? (decimal?) null : Parse(value);
        }

        private Tuple<int, int> GetCellCoordinates(string cellValue)
        {
            for (var rowIndex = 0; rowIndex < _dataTable.Rows.Count; rowIndex++)
            {
                for (var colIndex = 0; colIndex < _dataTable.Columns.Count; colIndex++)
                {
                    var foundCellValue = _dataTable.Rows[rowIndex][colIndex].ToString().Trim();
                    if (foundCellValue == cellValue.Trim())
                    {
                        return new Tuple<int, int>(rowIndex, colIndex);
                    }
                }
            }
            throw new ApplicationException("No cell found.");
        }

        public string GetFiscalYear()
        {
            var cellCoordinates = GetCellCoordinates("Fiscal Year:");

            //read 2nd cell to the right
            var colIndex = cellCoordinates.Item2 + 2;
            return _dataTable.Rows[cellCoordinates.Item1][colIndex].ToString().Substring(2);
        }
    }
}

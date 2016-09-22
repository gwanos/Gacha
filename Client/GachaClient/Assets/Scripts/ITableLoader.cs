using System.Collections.Generic;

public interface ITableLoader
{
    List<string[]> ReadCSVFile(string filePath, string encodingType);
    Dictionary<int, HeroInfo> CsvToDictionary(List<string[]> lines);
    Dictionary<int, HeroInfo> Load(string filePath, string encodingType);
}
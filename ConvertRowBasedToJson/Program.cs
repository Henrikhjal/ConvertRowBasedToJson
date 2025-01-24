using System.Text.Encodings.Web;
using System.Text.Json;

namespace hjalhen.ConvertRowbasedFileFormatToJson;

public class Program
{
    public static void Main()
    {
        const string inputFilePath = @"C:\testdata\persons.txt";
        const string outputFilePath = @"C:\testdata\persons.json";

        var persons = new List<Person>();

        if (File.Exists(inputFilePath))
        {
            var lines = File.ReadAllLines(inputFilePath);
            ProcessLines(lines, persons);

            WritePersonsToJsonFile(new People(persons), outputFilePath);
        }
        else
        {
            Console.WriteLine($"File not found {inputFilePath}");
        }
    }

    public static void ProcessLines(IReadOnlyList<string> lines, List<Person> persons)
    {
        ArgumentNullException.ThrowIfNull(lines);
        Person? currentPerson = null;

        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];
            var fields = line.Split('|');

            if (fields.Length < 1)
            {
                Console.WriteLine($"Line {i + 1}: Invalid line format: {line}");
                continue;
            }

            switch (fields[0])
            {
                case "P":
                    currentPerson = PersonDataProcessor.ProcessPerson(fields);
                    if (currentPerson != null)
                    {
                        persons.Add(currentPerson);
                    }
                    break;

                case "A":
                    PersonDataProcessor.ProcessAddress(fields, currentPerson);
                    break;

                case "T":
                    PersonDataProcessor.ProcessPhone(fields, currentPerson);
                    break;

                case "F":
                    PersonDataProcessor.ProcessFamilyMember(fields, currentPerson);
                    break;
            }
        }
    }

    public static void WritePersonsToJsonFile(People people, string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        var jsonString = JsonSerializer.Serialize(people, options);
        File.WriteAllText(filePath, jsonString);
        Console.WriteLine($"Persons written to JSON file: {filePath}");
    }
}
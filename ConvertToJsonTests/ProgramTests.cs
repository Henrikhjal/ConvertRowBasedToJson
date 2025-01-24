using hjalhen.ConvertRowbasedFileFormatToJson;
using System.Text.Json;

namespace hjalhen.Tests;

public class ProgramTests
{
    [Fact]
    public void ProcessLines_ValidLines_AddsPersons()
    {
        // Arrange
        var lines = new[]
        {
            "P|Anders|Berg",
            "A|Gatan 1|Stockholm|12345",
            "T|070-102030|08-102030",
            "F|Johan|1990"
        };
        var persons = new List<Person>();

        // Act
        Program.ProcessLines(lines, persons);

        // Assert
        Assert.Single(persons);
        var person = persons.First();
        Assert.Equal("Anders", person.FirstName);
        Assert.Equal("Berg", person.LastName);
        Assert.NotNull(person.Address);
        Assert.Equal("Gatan 1", person.Address.Street);
        Assert.Equal("Stockholm", person.Address.City);
        Assert.Equal("12345", person.Address.Zip);
        Assert.NotNull(person.Phone);
        Assert.Equal("070-102030", person.Phone.Mobile);
        Assert.Equal("08-102030", person.Phone.Landline);
        Assert.Single(person.Family);
        var familyMember = person.Family.First();
        Assert.Equal("Johan", familyMember.Name);
        Assert.Equal("1990", familyMember.Born);
    }

    [Fact]
    public void ProcessLines_InvalidLines_DoesNotAddPersons()
    {
        // Arrange
        var lines = new[]
        {
            "P|John",
            "A|Gatan 1|Stockholm",
            "T|08-102030",
            "F|Jimmy"
        };
        var persons = new List<Person>();

        // Act
        Program.ProcessLines(lines, persons);

        // Assert
        Assert.Empty(persons);
    }

    [Fact]
    public void WritePersonsToJsonFile_WritesToFile()
    {
        // Arrange
        var persons = new List<Person>
        {
            new Person("Anders", "Berg")
            {
                Address = new Address
                {
                    Street = "Gatan 1",
                    City = "Stockholm",
                    Zip = "12345"
                },
                Phone = new Phone
                {
                    Mobile = "070-102030",
                    Landline = "08-102030"
                },
                Family = new List<FamilyMember>
                {
                    new FamilyMember("Jimmy", "1990")
                }
            }
        };
        var people = new People(persons);
        var filePath = Path.Combine(Path.GetTempPath(), "persons.json");

        // Act
        Program.WritePersonsToJsonFile(people, filePath);

        // Assert
        Assert.True(File.Exists(filePath));
        var jsonString = File.ReadAllText(filePath);
        var deserializedPeople = JsonSerializer.Deserialize<People>(jsonString);
        Assert.NotNull(deserializedPeople);
        Assert.Single(deserializedPeople.Person);
        var person = deserializedPeople.Person.First();
        Assert.Equal("Anders", person.FirstName);
        Assert.Equal("Berg", person.LastName);
        Assert.NotNull(person.Address);
        Assert.Equal("Gatan 1", person.Address.Street);
        Assert.Equal("Stockholm", person.Address.City);
        Assert.Equal("12345", person.Address.Zip);
        Assert.NotNull(person.Phone);
        Assert.Equal("070-102030", person.Phone.Mobile);
        Assert.Equal("08-102030", person.Phone.Landline);
        Assert.Single(person.Family);
        var familyMember = person.Family.First();
        Assert.Equal("Jimmy", familyMember.Name);
        Assert.Equal("1990", familyMember.Born);
    }
}
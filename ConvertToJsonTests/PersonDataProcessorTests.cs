using hjalhen.ConvertRowbasedFileFormatToJson;

namespace hjalhen.ConvertRowbasedFileFormatToJsonTests;

public class PersonDataProcessorTests
{
    [Fact]
    public void ProcessPerson_ValidFields_ReturnsPerson()
    {
        // Arrange
        var fields = new[] { "P", "Anders", "Berg" };

        // Act
        var person = PersonDataProcessor.ProcessPerson(fields);

        // Assert
        Assert.NotNull(person);
        Assert.Equal("Anders", person.FirstName);
        Assert.Equal("Berg", person.LastName);
    }

    [Fact]
    public void ProcessPerson_InvalidFields_ReturnsNull()
    {
        // Arrange
        var fields = new[] { "P", "Anders" };

        // Act
        var person = PersonDataProcessor.ProcessPerson(fields);

        // Assert
        Assert.Null(person);
    }

    [Fact]
    public void ProcessAddress_InvalidFields_DoesNotAssignAddress()
    {
        // Arrange
        var fields = new[] { "A", "123 Main St", "Cityville" };
        var person = new Person("John", "Doe");

        // Act
        PersonDataProcessor.ProcessAddress(fields, person);

        // Assert
        Assert.NotNull(person.Address);
        Assert.Null(person.Address.Street);
        Assert.Null(person.Address.City);
        Assert.Null(person.Address.Zip);
    }


    [Fact]
    public void ProcessPhone_InvalidFields_DoesNotAssignPhone()
    {
        // Arrange
        var fields = new[] { "T", "070-102030" };
        var person = new Person("Anders", "Berg");

        // Act
        PersonDataProcessor.ProcessPhone(fields, person);

        // Assert
        Assert.NotNull(person.Phone);
        Assert.Null(person.Phone.Mobile);
        Assert.Null(person.Phone.Landline);
    }

    [Fact]
    public void ProcessFamilyMember_ValidFields_AddsFamilyMember()
    {
        // Arrange
        var fields = new[] { "F", "Johan", "1990" };
        var person = new Person("Anders", "Berg");

        // Act
        PersonDataProcessor.ProcessFamilyMember(fields, person);

        // Assert
        Assert.Single(person.Family);
        var familyMember = person.Family.First();
        Assert.Equal("Johan", familyMember.Name);
        Assert.Equal("1990", familyMember.Born);
    }

    [Fact]
    public void ProcessFamilyMember_InvalidFields_DoesNotAddFamilyMember()
    {
        // Arrange
        var fields = new[] { "F", "Johan" };
        var person = new Person("Anders", "Berg");

        // Act
        PersonDataProcessor.ProcessFamilyMember(fields, person);

        // Assert
        Assert.Empty(person.Family);
    }
}
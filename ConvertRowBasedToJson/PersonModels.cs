namespace hjalhen.ConvertRowbasedFileFormatToJson;
public class People
{
    public List<Person> Person { get; set; }

    public People()
    {
        Person = new List<Person>();
    }

    public People(List<Person> persons)
    {
        Person = persons;
    }
}

public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public Phone? Phone { get; set; }
    public Address? Address { get; set; }
    public List<FamilyMember> Family { get; set; }

    public Person(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("First name cannot be null or empty.", nameof(firstName));
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Last name cannot be null or empty.", nameof(lastName));
        }

        FirstName = firstName;
        LastName = lastName;
        Phone = new Phone();
        Address = new Address();
        Family = new List<FamilyMember>();
    }
}

public class Address
{
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? Zip { get; set; }
}

public class Phone
{
    public string? Mobile { get; set; }
    public string? Landline { get; set; }
}

public class FamilyMember
{
    public string Name { get; set; }
    public string? Born { get; set; }
    public Address? Address { get; set; }
    public Phone? Phone { get; set; }

    public FamilyMember(string name, string born)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        }

        Name = name;
        Born = born;
        Address = new Address();
        Phone = new Phone();
    }
}
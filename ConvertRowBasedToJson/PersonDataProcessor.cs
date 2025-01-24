namespace hjalhen.ConvertRowbasedFileFormatToJson;
public static class PersonDataProcessor
{
    private const int PersonFieldCount = 3;
    private const int AddressFieldCount = 4;
    private const int PhoneFieldCount = 3;
    private const int FamilyMemberFieldCount = 3;

    private const int FirstNameIndex = 1;
    private const int LastNameIndex = 2;
    private const int StreetIndex = 1;
    private const int CityIndex = 2;
    private const int ZipIndex = 3;
    private const int MobileIndex = 1;
    private const int LandlineIndex = 2;
    private const int FamilyMemberNameIndex = 1;
    private const int FamilyMemberBornIndex = 2;

    private static FamilyMember? _currentFamilyMember;
    public static Person? ProcessPerson(string[] fields)
    {
        if (fields.Length == PersonFieldCount)
        {
            _currentFamilyMember = null;
            return new Person(fields[FirstNameIndex], fields[LastNameIndex]);
        }

        Console.WriteLine($"Invalid person format: {string.Join('|', fields)}");
        return null;
    }

    public static void ProcessAddress(string[] fields, Person? currentPerson)
    {
        if (currentPerson != null)
        {
            if (fields.Length == AddressFieldCount)
            {
                if (_currentFamilyMember == null)
                {
                    if (currentPerson.Address != null)
                    {
                        currentPerson.Address.Street = fields[StreetIndex];
                        currentPerson.Address.City = fields[CityIndex];
                        currentPerson.Address.Zip = fields[ZipIndex];
                    }
                }
                else
                {
                    if (_currentFamilyMember.Address != null)
                    {
                        _currentFamilyMember.Address.Street = fields[StreetIndex];
                        _currentFamilyMember.Address.City = fields[CityIndex];
                        _currentFamilyMember.Address.Zip = fields[ZipIndex];
                    }
                }
            }
            else
            {
                Console.WriteLine($"Invalid address format: {string.Join('|', fields)}");
            }
        }
        else
        {
            Console.WriteLine($"No current person to assign address: {string.Join('|', fields)}");
        }
    }
    public static void ProcessPhone(string[] fields, Person? currentPerson)
    {
        if (currentPerson != null)
        {
            if (fields.Length == PhoneFieldCount)
            {
                if (_currentFamilyMember == null)
                {
                    if (currentPerson.Phone != null)
                    {
                        currentPerson.Phone.Mobile = fields[MobileIndex];
                        currentPerson.Phone.Landline = fields[LandlineIndex];
                    }
                }
                else
                {
                    if (_currentFamilyMember.Phone != null)
                    {
                        _currentFamilyMember.Phone.Mobile = fields[MobileIndex];
                        _currentFamilyMember.Phone.Landline = fields[LandlineIndex];
                    }
                }
            }
            else
            {
                Console.WriteLine($"Invalid phone format: {string.Join('|', fields)}");
            }
        }
        else
        {
            Console.WriteLine($"No current person to assign phone: {string.Join('|', fields)}");
        }
    }
    public static void ProcessFamilyMember(string[] fields, Person? currentPerson)
    {
        if (currentPerson != null)
        {
            if (fields.Length >= FamilyMemberFieldCount)
            {
                var familyMember = new FamilyMember(fields[FamilyMemberNameIndex], fields[FamilyMemberBornIndex]);

                currentPerson.Family.Add(familyMember);
                _currentFamilyMember = familyMember;
            }
            else
            {
                Console.WriteLine($"Invalid family member format: {string.Join('|', fields)}");
            }
        }
        else
        {
            Console.WriteLine($"No current person to assign family member: {string.Join('|', fields)}");
        }
    }
}
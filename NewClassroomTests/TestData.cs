using Bogus;
using Bogus.Extensions.UnitedStates;
using NewClassroom.Enums;
using NewClassroom.Models;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace NewClassroomTests;

[ExcludeFromCodeCoverage]
public class TestData
{
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public static Faker<AgeDate> AgeDateFaker {get;} = new Faker<AgeDate>()
        .CustomInstantiator(f =>
        {
            var age = f.Random.Int(0, 110);
            return new AgeDate(f.Date.PastOffset(age), age);
        });

    public static Faker<Coordinates> CoordinateFaker {get;} = new Faker<Coordinates>()
        .CustomInstantiator(f => new Coordinates(f.Random.Double(-180, 180), f.Random.Double(-90, 90)));

    public static Faker<Identification> IdentificationFaker {get;} = new Faker<Identification>()
        .CustomInstantiator(f => new Identification("SSN", f.Person.Ssn()));

    public static Faker<Street> StreetFaker {get;} = new Faker<Street>()
        .CustomInstantiator(f => new Street(f.Address.BuildingNumber(), f.Address.StreetName()));

    public static Faker<Timezone> TimezoneFaker {get;} = new Faker<Timezone>()
        .CustomInstantiator(f => new Timezone(f.Date.TimeZoneString(), f.Date.TimeZoneString()));

    public static Faker<Location> LocationFaker {get;} = new Faker<Location>()
        .CustomInstantiator(f => new Location(StreetFaker.Generate(), f.Address.City(), f.Address.State(), "United States", (int?)f.Random.UInt(10000, 99999),
            CoordinateFaker.Generate(), TimezoneFaker.Generate()));

    public static Faker<Login> LoginFaker {get;} = new Faker<Login>()
        .CustomInstantiator(f => new Login(Guid.NewGuid(), f.Person.UserName, f.Random.String2(10, 10), f.Random.String2(10, 10),
            f.Random.String2(10, 10), f.Random.String2(10, 10), f.Random.String2(10, 10)));

    public static Faker<Name> NameFaker {get;} = new Faker<Name>()
        .CustomInstantiator(f => new Name("M", f.Person.FirstName, f.Person.LastName));

    public static Faker<Picture> PictureFaker {get;} = new Faker<Picture>()
        .CustomInstantiator(f => new Picture(f.Internet.Url(), f.Internet.Url(), f.Internet.Url()));

    public static Faker<User> UserFaker {get;} = new Faker<User>()
        .CustomInstantiator(f => new User(NameFaker.Generate(), IdentificationFaker.Generate()))
        .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
        .RuleFor(u => u.Location, LocationFaker.Generate())
        .RuleFor(u => u.Email, f => f.Person.Email)
        .RuleFor(u => u.Login, LoginFaker.Generate())
        .RuleFor(u => u.DateOfBirth, AgeDateFaker.Generate())
        .RuleFor(u => u.Registered, AgeDateFaker.Generate())
        .RuleFor(u => u.Phone, f => f.Person.Phone)
        .RuleFor(u => u.Cell, f => f.Person.Phone)
        .RuleFor(u => u.Picture, PictureFaker.Generate())
        .RuleFor(u => u.Nationality, "US");

    public static Name GetCustomName(
        char firstNameCharMin, char firstNameCharMax,
        char lastNameCharMin, char lastNameCharMax)
    {
        Name result;

        do
        {
            result = NameFaker.Generate();
        } while (result.First![0] < firstNameCharMin || result.First[0] > firstNameCharMax ||
                 result.Last![0] < lastNameCharMin || result.Last[0] > lastNameCharMax);

        return result;
    }

    public static AgeDate GetCustomAgeDate(int minAge, int maxAge)
    {
        var faker = new Faker<AgeDate>()
            .CustomInstantiator(f =>
            {
                var age = f.Random.Int(minAge, maxAge);
                return new AgeDate(f.Date.PastOffset(age), age);
            });

        return faker.Generate();
    }
}

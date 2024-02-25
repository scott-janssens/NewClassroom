using NewClassroom.Enums;
using NewClassroom.Models;
using NewClassroom.Services;
using System.Diagnostics.CodeAnalysis;

namespace NewClassroomTests;

[ExcludeFromCodeCoverage]
public class UserStatsServiceTests
{
    private UserStatsService _service;
    private const double _threshold = .00000001;

    private readonly StateSetupItem[] _stateSetup =
    [
        new("Indiana", 11),
        new("Iowa", 8),
        new("Illinois", 12),
        new("Vermont", 1),
        new("Michigan", 7),
        new("New York", 4),
        new("Minnesota", 9),
        new("Pennsylvania", 5),
        new("Ohio", 6),
        new("Rhode Island", 2),
        new("Wisconsin", 10),
        new("Connecticut", 3)
    ];

    [SetUp]
    public void Setup()
    {
        _service = new UserStatsService();
    }

    [Test]
    public void GenderStats()
    {
        var users = new List<User>();

        users.AddRange(TestData.UserFaker
            .RuleFor(u => u.Gender, Gender.Male)
            .Generate(10));

        users.AddRange(TestData.UserFaker
            .RuleFor(u => u.Gender, Gender.Female)
            .Generate(5));

        _service.AddDefaultQueries();
        var actual = _service.GetStatistics(users).Stats.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(actual.Single(x => x.Name == UserStatsService.StatMalePct).Items.First().Pct,
                Is.EqualTo(2 / 3.0).Within(_threshold));
            Assert.That(actual.Single(x => x.Name == UserStatsService.StatFemalePct).Items.First().Pct,
                Is.EqualTo(1 / 3.0).Within(_threshold));
        });
    }

    [Test]
    public void FirstNameStats()
    {
        var users = new List<User>();

        users.AddRange(TestData.UserFaker
            .CustomInstantiator(f => new User(TestData.GetCustomName('A', 'M', 'A', 'Z'), TestData.IdentificationFaker.Generate()))
            .Generate(10));

        users.AddRange(TestData.UserFaker
            .CustomInstantiator(f => new User(TestData.GetCustomName('N', 'Z', 'A', 'Z'), TestData.IdentificationFaker.Generate()))
            .Generate(4));

        users.Add(TestData.UserFaker
            .CustomInstantiator(f => new User(new Name(null, null, null), TestData.IdentificationFaker.Generate()))
            .Generate());

        _service.AddDefaultQueries();
        var actual = _service.GetStatistics(users).Stats.ToList();

        Assert.That(actual.Single(x => x.Name == UserStatsService.StatFirstNameA_M).Items.First().Pct, Is.EqualTo(2 / 3.0).Within(_threshold));
    }

    [Test]
    public void LastNameStats()
    {
        var users = new List<User>();

        users.AddRange(TestData.UserFaker
            .CustomInstantiator(f => new User(TestData.GetCustomName('A', 'Z', 'A', 'M'), TestData.IdentificationFaker.Generate()))
            .Generate(10));

        users.AddRange(TestData.UserFaker
            .CustomInstantiator(f => new User(TestData.GetCustomName('A', 'Z', 'N', 'Z'), TestData.IdentificationFaker.Generate()))
            .Generate(4));

        users.Add(TestData.UserFaker
            .CustomInstantiator(f => new User(new Name(null, null, null), TestData.IdentificationFaker.Generate()))
            .Generate());

        _service.AddDefaultQueries();
        var actual = _service.GetStatistics(users).Stats.ToList();

        Assert.That(actual.Single(x => x.Name == UserStatsService.StatLastNameA_M).Items.First().Pct,
            Is.EqualTo(2 / 3.0).Within(_threshold));
    }

    [Test]
    public void StatePeopleStats()
    {
        var users = new List<User>();

        foreach (var item in _stateSetup)
        {
            users.AddRange(TestData.UserFaker
                .RuleFor(x => x.Location, TestData.LocationFaker.RuleFor(x => x.State, item.State).Generate())
                .Generate(item.Count));
        }

        users.Add(TestData.UserFaker
            .RuleFor(x => x.Location, TestData.LocationFaker.RuleFor(x => x.State, (string?)null).Generate())
            .Generate());

        users.Add(TestData.UserFaker
            .RuleFor(x => x.Location, (Location?)null)
            .Generate());

        _service.AddDefaultQueries();
        var actual = _service.GetStatistics(users).Stats.Single(x => x.Name == UserStatsService.StatStatePeople).Items.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(actual, Has.Count.EqualTo(10));
            Assert.That(actual[0].Pct, Is.EqualTo(12 / (double)users.Count).Within(_threshold));
            Assert.That(actual[0].Description, Does.Contain("Illinois"));
            Assert.That(actual[1].Pct, Is.EqualTo(11 / (double)users.Count).Within(_threshold));
            Assert.That(actual[1].Description, Does.Contain("Indiana"));
            Assert.That(actual[2].Pct, Is.EqualTo(10 / (double)users.Count).Within(_threshold));
            Assert.That(actual[2].Description, Does.Contain("Wisconsin"));
            Assert.That(actual[3].Pct, Is.EqualTo(9 / (double)users.Count).Within(_threshold));
            Assert.That(actual[3].Description, Does.Contain("Minnesota"));
            Assert.That(actual[4].Pct, Is.EqualTo(8 / (double)users.Count).Within(_threshold));
            Assert.That(actual[4].Description, Does.Contain("Iowa"));
            Assert.That(actual[5].Pct, Is.EqualTo(7 / (double)users.Count).Within(_threshold));
            Assert.That(actual[5].Description, Does.Contain("Michigan"));
            Assert.That(actual[6].Pct, Is.EqualTo(6 / (double)users.Count).Within(_threshold));
            Assert.That(actual[6].Description, Does.Contain("Ohio"));
            Assert.That(actual[7].Pct, Is.EqualTo(5 / (double)users.Count).Within(_threshold));
            Assert.That(actual[7].Description, Does.Contain("Pennsylvania"));
            Assert.That(actual[8].Pct, Is.EqualTo(4 / (double)users.Count).Within(_threshold));
            Assert.That(actual[8].Description, Does.Contain("New York"));
            Assert.That(actual[9].Pct, Is.EqualTo(3 / (double)users.Count).Within(_threshold));
            Assert.That(actual[9].Description, Does.Contain("Connecticut"));
        });
    }

    [Test]
    public void StatePeopleStatsLessThan10States()
    {
        var users = new List<User>();

        users.AddRange(TestData.UserFaker
            .RuleFor(x => x.Location, TestData.LocationFaker.RuleFor(x => x.State, "Illinois").Generate())
            .Generate(5));

        users.AddRange(TestData.UserFaker
            .RuleFor(x => x.Location, TestData.LocationFaker.RuleFor(x => x.State, "Wisconsin").Generate())
            .Generate(2));

        _service.AddDefaultQueries();
        var actual = _service.GetStatistics(users).Stats.Single(x => x.Name == UserStatsService.StatStatePeople).Items.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(actual, Has.Count.EqualTo(2));
            Assert.That(actual[0].Pct, Is.EqualTo(5 / (double)users.Count).Within(_threshold));
            Assert.That(actual[0].Description, Does.Contain("Illinois"));
            Assert.That(actual[1].Pct, Is.EqualTo(2 / (double)users.Count).Within(_threshold));
            Assert.That(actual[1].Description, Does.Contain("Wisconsin"));
        });
    }

    [Test]
    public void StateFemaleStats()
    {
        var users = new List<User>();

        foreach (var item in _stateSetup)
        {
            users.AddRange(TestData.UserFaker
                .RuleFor(x => x.Location, TestData.LocationFaker.RuleFor(x => x.State, item.State).Generate())
                .RuleFor(x => x.Gender, Gender.Female)
                .Generate(item.Count));

            users.AddRange(TestData.UserFaker
                .RuleFor(x => x.Location, TestData.LocationFaker.RuleFor(x => x.State, item.State).Generate())
                .RuleFor(x => x.Gender, Gender.Male)
                .Generate(22 - item.Count));

            users.Add(TestData.UserFaker
                .RuleFor(x => x.Location, TestData.LocationFaker.RuleFor(x => x.State, item.State).Generate())
                .RuleFor(x => x.Gender, (Gender?)null)
                .Generate());
        }

        users.Add(TestData.UserFaker
            .RuleFor(x => x.Location, TestData.LocationFaker.RuleFor(x => x.State, (string?)null).Generate())
            .Generate());

        users.Add(TestData.UserFaker
            .RuleFor(x => x.Location, (Location?)null)
            .Generate());

        _service.AddDefaultQueries();
        var actual = _service.GetStatistics(users).Stats.Single(x => x.Name == UserStatsService.StatStateFemale).Items.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(actual, Has.Count.EqualTo(10));
            Assert.That(actual[0].Pct, Is.EqualTo(12 / 23.0).Within(_threshold));
            Assert.That(actual[0].Description, Does.Contain("Illinois"));
            Assert.That(actual[1].Pct, Is.EqualTo(11 / 23.0).Within(_threshold));
            Assert.That(actual[1].Description, Does.Contain("Indiana"));
            Assert.That(actual[2].Pct, Is.EqualTo(10 / 23.0).Within(_threshold));
            Assert.That(actual[2].Description, Does.Contain("Wisconsin"));
            Assert.That(actual[3].Pct, Is.EqualTo(9 / 23.0).Within(_threshold));
            Assert.That(actual[3].Description, Does.Contain("Minnesota"));
            Assert.That(actual[4].Pct, Is.EqualTo(8 / 23.0).Within(_threshold));
            Assert.That(actual[4].Description, Does.Contain("Iowa"));
            Assert.That(actual[5].Pct, Is.EqualTo(7 / 23.0).Within(_threshold));
            Assert.That(actual[5].Description, Does.Contain("Michigan"));
            Assert.That(actual[6].Pct, Is.EqualTo(6 / 23.0).Within(_threshold));
            Assert.That(actual[6].Description, Does.Contain("Ohio"));
            Assert.That(actual[7].Pct, Is.EqualTo(5 / 23.0).Within(_threshold));
            Assert.That(actual[7].Description, Does.Contain("Pennsylvania"));
            Assert.That(actual[8].Pct, Is.EqualTo(4 / 23.0).Within(_threshold));
            Assert.That(actual[8].Description, Does.Contain("New York"));
            Assert.That(actual[9].Pct, Is.EqualTo(3 / 23.0).Within(_threshold));
            Assert.That(actual[9].Description, Does.Contain("Connecticut"));
        });
    }

    [Test]
    public void StateMaleStats()
    {
        var users = new List<User>();

        foreach (var item in _stateSetup)
        {
            users.AddRange(TestData.UserFaker
                .RuleFor(x => x.Location, TestData.LocationFaker.RuleFor(x => x.State, item.State).Generate())
                .RuleFor(x => x.Gender, Gender.Female)
                .Generate(15 - item.Count));

            users.AddRange(TestData.UserFaker
                .RuleFor(x => x.Location, TestData.LocationFaker.RuleFor(x => x.State, item.State).Generate())
                .RuleFor(x => x.Gender, Gender.Male)
                .Generate(item.Count));

            users.Add(TestData.UserFaker
                .RuleFor(x => x.Location, TestData.LocationFaker.RuleFor(x => x.State, item.State).Generate())
                .RuleFor(x => x.Gender, (Gender?)null)
                .Generate());
        }

        users.Add(TestData.UserFaker
            .RuleFor(x => x.Location, TestData.LocationFaker.RuleFor(x => x.State, (string?)null).Generate())
            .Generate());

        users.Add(TestData.UserFaker
            .RuleFor(x => x.Location, (Location?)null)
            .Generate());

        _service.AddDefaultQueries();
        var actual = _service.GetStatistics(users).Stats.Single(x => x.Name == UserStatsService.StatStateMale).Items.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(actual, Has.Count.EqualTo(10));
            Assert.That(actual[0].Pct, Is.EqualTo(12 / 16.0).Within(_threshold));
            Assert.That(actual[0].Description, Does.Contain("Illinois"));
            Assert.That(actual[1].Pct, Is.EqualTo(11 / 16.0).Within(_threshold));
            Assert.That(actual[1].Description, Does.Contain("Indiana"));
            Assert.That(actual[2].Pct, Is.EqualTo(10 / 16.0).Within(_threshold));
            Assert.That(actual[2].Description, Does.Contain("Wisconsin"));
            Assert.That(actual[3].Pct, Is.EqualTo(9 / 16.0).Within(_threshold));
            Assert.That(actual[3].Description, Does.Contain("Minnesota"));
            Assert.That(actual[4].Pct, Is.EqualTo(8 / 16.0).Within(_threshold));
            Assert.That(actual[4].Description, Does.Contain("Iowa"));
            Assert.That(actual[5].Pct, Is.EqualTo(7 / 16.0).Within(_threshold));
            Assert.That(actual[5].Description, Does.Contain("Michigan"));
            Assert.That(actual[6].Pct, Is.EqualTo(6 / 16.0).Within(_threshold));
            Assert.That(actual[6].Description, Does.Contain("Ohio"));
            Assert.That(actual[7].Pct, Is.EqualTo(5 / 16.0).Within(_threshold));
            Assert.That(actual[7].Description, Does.Contain("Pennsylvania"));
            Assert.That(actual[8].Pct, Is.EqualTo(4 / 16.0).Within(_threshold));
            Assert.That(actual[8].Description, Does.Contain("New York"));
            Assert.That(actual[9].Pct, Is.EqualTo(3 / 16.0).Within(_threshold));
            Assert.That(actual[9].Description, Does.Contain("Connecticut"));
        });
    }

    [Test]
    public void AgeStats()
    {
        var users = new List<User>();

        users.AddRange(TestData.UserFaker
            .RuleFor(x => x.DateOfBirth, TestData.GetCustomAgeDate(0, 20))
            .Generate(2));

        users.AddRange(TestData.UserFaker
            .RuleFor(x => x.DateOfBirth, TestData.GetCustomAgeDate(21, 40))
            .Generate(8));

        users.AddRange(TestData.UserFaker
            .RuleFor(x => x.DateOfBirth, TestData.GetCustomAgeDate(41, 60))
            .Generate(7));

        users.AddRange(TestData.UserFaker
            .RuleFor(x => x.DateOfBirth, TestData.GetCustomAgeDate(61, 80))
            .Generate(5));

        users.AddRange(TestData.UserFaker
            .RuleFor(x => x.DateOfBirth, TestData.GetCustomAgeDate(81, 100))
            .Generate(3));

        users.Add(TestData.UserFaker
            .RuleFor(x => x.DateOfBirth, new AgeDate(DateTimeOffset.Now.AddDays(-36915), 101))
            .Generate());

        users.Add(TestData.UserFaker
            .RuleFor(x => x.DateOfBirth, new AgeDate(DateTimeOffset.Now.AddDays(-40555), 121))
            .Generate());

        users.Add(TestData.UserFaker
            .RuleFor(x => x.DateOfBirth, new AgeDate(DateTimeOffset.Now, 0))
            .Generate());

        users.Add(TestData.UserFaker
            .RuleFor(x => x.DateOfBirth, (AgeDate?)null)
            .Generate());

        _service.AddDefaultQueries();
        var actual = _service.GetStatistics(users).Stats.Single(x => x.Name == UserStatsService.StatStateAge).Items.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(actual, Has.Count.EqualTo(6));
            Assert.That(actual[0].Pct, Is.EqualTo(3 / (double)users.Count).Within(_threshold));
            Assert.That(actual[0].Description, Does.Contain("0-20"));
            Assert.That(actual[1].Pct, Is.EqualTo(8 / (double)users.Count).Within(_threshold));
            Assert.That(actual[1].Description, Does.Contain("21-40"));
            Assert.That(actual[2].Pct, Is.EqualTo(7 / (double)users.Count).Within(_threshold));
            Assert.That(actual[2].Description, Does.Contain("41-60"));
            Assert.That(actual[3].Pct, Is.EqualTo(5 / (double)users.Count).Within(_threshold));
            Assert.That(actual[3].Description, Does.Contain("61-80"));
            Assert.That(actual[4].Pct, Is.EqualTo(3 / (double)users.Count).Within(_threshold));
            Assert.That(actual[4].Description, Does.Contain("81-100"));
            Assert.That(actual[5].Pct, Is.EqualTo(2 / (double)users.Count).Within(_threshold));
            Assert.That(actual[5].Description, Does.Contain("100+"));
        });
    }

    [Test]
    public void MissingAgeStats()
    {
        var users = new List<User>();

        users.AddRange(TestData.UserFaker
            .RuleFor(x => x.DateOfBirth, TestData.GetCustomAgeDate(21, 40))
            .Generate(8));

        users.AddRange(TestData.UserFaker
            .RuleFor(x => x.DateOfBirth, TestData.GetCustomAgeDate(41, 60))
            .Generate(7));

        users.AddRange(TestData.UserFaker
            .RuleFor(x => x.DateOfBirth, TestData.GetCustomAgeDate(81, 100))
            .Generate(3));

        users.Add(TestData.UserFaker
            .RuleFor(x => x.DateOfBirth, new AgeDate(DateTimeOffset.Now.AddDays(-36915), 101))
            .Generate());

        users.Add(TestData.UserFaker
            .RuleFor(x => x.DateOfBirth, new AgeDate(DateTimeOffset.Now.AddDays(-40555), 121))
            .Generate());

        users.Add(TestData.UserFaker
            .RuleFor(x => x.DateOfBirth, new AgeDate(DateTimeOffset.Now, 0))
            .Generate());

        users.Add(TestData.UserFaker
            .RuleFor(x => x.DateOfBirth, (AgeDate?)null)
            .Generate());

        _service.AddDefaultQueries();
        var actual = _service.GetStatistics(users).Stats.Single(x => x.Name == UserStatsService.StatStateAge).Items.ToList();

        Assert.Multiple(() =>
        {
            Assert.That(actual, Has.Count.EqualTo(5));
            Assert.That(actual[0].Pct, Is.EqualTo(1 / (double)users.Count).Within(_threshold));
            Assert.That(actual[0].Description, Does.Contain("0-20"));
            Assert.That(actual[1].Pct, Is.EqualTo(8 / (double)users.Count).Within(_threshold));
            Assert.That(actual[1].Description, Does.Contain("21-40"));
            Assert.That(actual[2].Pct, Is.EqualTo(7 / (double)users.Count).Within(_threshold));
            Assert.That(actual[2].Description, Does.Contain("41-60"));
            Assert.That(actual[3].Pct, Is.EqualTo(3 / (double)users.Count).Within(_threshold));
            Assert.That(actual[3].Description, Does.Contain("81-100"));
            Assert.That(actual[4].Pct, Is.EqualTo(2 / (double)users.Count).Within(_threshold));
            Assert.That(actual[4].Description, Does.Contain("100+"));
        });
    }

    private record StateSetupItem(string State, int Count);
}
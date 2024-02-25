using NewClassroom.Models;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace NewClassroomTests;

[ExcludeFromCodeCoverage]
public class UserTests
{
    [Test]
    public void User_Serialization()
    {
        var expected = TestData.UserFaker.Generate();
        var json = JsonSerializer.Serialize(expected, TestData.JsonOptions);
        var actual = JsonSerializer.Deserialize<User>(json, TestData.JsonOptions);

        Assert.That(actual, Is.EqualTo(expected));
    }
}

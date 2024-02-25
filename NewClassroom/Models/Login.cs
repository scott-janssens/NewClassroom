namespace NewClassroom.Models;

/// <summary>
/// A User's login information.
/// </summary>
/// <param name="Uuid">A Guid value</param>
/// <param name="Username">Username</param>
/// <param name="Password">Password</param>
/// <param name="Salt">Hash salt</param>
/// <param name="Md5">MD5 Hash value</param>
/// <param name="Sha1">SHA1 Hash value</param>
/// <param name="Sha256">SH256 Hash value</param>
public record Login(
    Guid Uuid,
    string Username,
    string Password,
    string? Salt,
    string? Md5,
    string? Sha1,
    string? Sha256
);

using System.ComponentModel.DataAnnotations;

namespace CourseMusic.Models;

public class User
{
    [Key]
    public string? Name { get; set; }
    public string? Password { get; set; }

    public User(string name, string password)
    {
        Name = name;
        Password = password;
    }

    public override string ToString()
    {
        return @$"{Name}{Password}";
    }
}
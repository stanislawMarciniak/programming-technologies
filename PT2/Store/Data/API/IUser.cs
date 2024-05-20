namespace Data.API;

public interface IUser
{
    int Id { get; set; }

    string Nickname { get; set; }

    string Email { get; set; }

    double Balance { get; set; }

    DateTime DateOfBirth { get; set; }
}

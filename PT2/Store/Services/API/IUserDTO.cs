namespace Service.API;

public interface IUserDTO
{
    int Id { get; set; }

    string Nickname { get; set; }

    string Email { get; set; }

    double Balance { get; set; }

    DateTime DateOfBirth { get; set; }
}

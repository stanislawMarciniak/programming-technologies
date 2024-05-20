using Service.API;

namespace Service.Implementation;

internal class UserDTO : IUserDTO
{
    public int Id { get; set; }

    public string Nickname { get; set; }

    public string Email { get; set; }

    public double Balance { get; set; }

    public DateTime DateOfBirth { get; set; }

    public UserDTO(int id, string nickname, string email, double balance, DateTime date)
    {
        this.Id = id;
        this.Nickname = nickname;
        this.Email = email;
        this.Balance = balance;
        this.DateOfBirth = date;
    }
}

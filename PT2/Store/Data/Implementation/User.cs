using Data.API;

namespace Data.Implementation;

internal class User : IUser
{
    public User(int id, string nickname, string email, double balance, DateTime dateOfBirth)
    {
        this.Id = id;
        this.Nickname = nickname;
        this.Email = email;
        this.Balance = balance;
        this.DateOfBirth = dateOfBirth;
    }

    public int Id { get; set; }

    public string Nickname { get; set; }

    public string Email { get; set; }

    public double Balance { get; set; } = 0;

    public DateTime DateOfBirth { get; set; }
}

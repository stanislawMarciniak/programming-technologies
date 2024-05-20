using System;

namespace Presentation.Model.API;

public interface IUserModel
{
    public int Id { get; set; }

    public string Nickname { get; set; }

    public string Email { get; set; }

    public double Balance { get; set; }

    public DateTime DateOfBirth { get; set; }
}

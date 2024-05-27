using Service.API;

namespace PresentationTests.Mocks.DTO
{
    internal class MockUserDTO : IUserDTO
    {
        public MockUserDTO(int id, string nickname, string email, double balance, DateTime dateOfBirth)
        {
            Id = id;
            Nickname = nickname;
            Email = email;
            Balance = balance;
            DateOfBirth = dateOfBirth;
        }

        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public double Balance { get; set; } = 0;
        public DateTime DateOfBirth { get; set; }
    }
}
using Service.API;

namespace PresentationTests.Mocks.DTO
{
    internal class MockMovieDTO : IMovieDTO
    {
        public MockMovieDTO(int id, string name, double price, int ageRestriction)
        {
            Id = id;
            Name = name;
            Price = price;
            AgeRestriction = ageRestriction;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int AgeRestriction { get; set; }
    }
}
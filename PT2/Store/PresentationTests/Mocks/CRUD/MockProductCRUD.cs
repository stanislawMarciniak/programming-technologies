using Service.API;

namespace PresentationTests.Mocks.CRUD
{
    internal class MockMovieCRUD : IMovieCRUD
    {
        private readonly MockDataRepository _repository = new MockDataRepository();

        public MockMovieCRUD()
        {

        }

        public async Task AddMovieAsync(int id, string name, double price, int ageRestriction)
        {
            await _repository.AddMovieAsync(id, name, price, ageRestriction);
        }

        public async Task<IMovieDTO> GetMovieAsync(int id)
        {
            return await _repository.GetMovieAsync(id);
        }

        public async Task UpdateMovieAsync(int id, string name, double price, int ageRestriction)
        {
            await _repository.UpdateMovieAsync(id, name, price, ageRestriction);
        }

        public async Task DeleteMovieAsync(int id)
        {
            await _repository.DeleteMovieAsync(id);
        }

        public async Task<Dictionary<int, IMovieDTO>> GetAllMoviesAsync()
        {
            Dictionary<int, IMovieDTO> result = new Dictionary<int, IMovieDTO>();

            foreach (IMovieDTO movie in (await _repository.GetAllMoviesAsync()).Values)
            {
                result.Add(movie.Id, movie);
            }

            return result;
        }

        public async Task<int> GetMoviesCountAsync()
        {
            return await _repository.GetMoviesCountAsync();
        }
    }
}
﻿using Data.API;

namespace ServiceTests.Mocks.DTO;

internal class MockMovieDTO : IMovie
{
    public MockMovieDTO(int id, string movieName, double price, int ageRestriction)
    {
        Id = id;
        MovieName = movieName;
        Price = price;
        AgeRestriction = ageRestriction;
    }

    public int Id { get; set; }
    public string MovieName { get; set; }
    public double Price { get; set; }
    public int AgeRestriction { get; set; }
}
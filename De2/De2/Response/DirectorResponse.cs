using De2.Models;

namespace De2.Response
{
    public class DirectorResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? Gender { get; set; }
        public DateTime? Dob { get;set; }
        public string? DobString { get; set; }
        public string Nationality { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
    public class DirectorDetailResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? DobString { get; set; }
        public string Nationality { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<MovieResponse> Movies { get; set; } = new List<MovieResponse>();
    }

    public class MovieResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public DateTime? ReleaseDate { get; set; }
        public string? Description { get; set; }
        public string Language { get; set; } = null!;
        public int? ProducerId { get; set; }
        public int? DirectorId { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Star> Stars { get; set; }
    }

    public class DirectorRequest
    {
        public string FullName { get; set; } = null!;
        public bool Male { get; set; }
        public DateTime Dob { get; set; }
        public string Nationality { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}

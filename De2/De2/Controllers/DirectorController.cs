using De2.Models;
using De2.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace De2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorController : ControllerBase
    {
        [HttpGet("getAllDirectors")]
        public IActionResult GetAllDirectors()
        {

            var _context = new PE_PRN_Fall22B1Context();

            var directors = _context.Directors.ToList();
            return Ok(directors);
        }

        [HttpGet("Getdirectors/{Nationality}/{Gender}")]
        public IActionResult GetDirectors(string Nationality, string Gender)
        {
            var _context = new PE_PRN_Fall22B1Context();

            bool isMale = Gender.ToLower().Equals("male");

            var directors = _context.Directors
            .Where(x => x.Nationality.ToLower().Equals(Nationality.ToLower()) && x.Male == isMale)
            .ToList();

            var response = new List<DirectorResponse>();
            foreach (var director in directors)
            {
                response.Add(new DirectorResponse
                {
                    Id = director.Id,
                    FullName = director.FullName,
                    Nationality = director.Nationality,
                    Dob = director.Dob,
                    DobString =director.Dob.ToString("M/d/yyyy"),
                    Gender = director.Male ? "Male" : "Female",
                    Description = director.Description
                });
            }

            return Ok(response);
        }

        [HttpGet("Getdirector/{id}")]
        public IActionResult GetDirector(int id)
        {
            var _context = new PE_PRN_Fall22B1Context();

        

            var director = _context.Directors.Include(x => x.Movies)
            .FirstOrDefault(x=> x.Id == id);
            if(director == null)
            {
                return NotFound();
            }
          

            var response = new DirectorDetailResponse
            {
                    Id = director.Id,
                    FullName = director.FullName,
                    Nationality = director.Nationality,
                    Dob = director.Dob,
                    DobString = director.Dob.ToString("M/d/yyyy"),
                    Gender = director.Male ? "Male" : "Female",
                    Description = director.Description,
                    
            };

            foreach (var item in director.Movies)
            {
                var movie = new MovieResponse();
                movie.Id = item.Id;
                movie.Title = item.Title;
                movie.ReleaseDate = item.ReleaseDate;
                movie.Description = item.Description;
                movie.Language = item.Language;
                movie.ProducerId = item.ProducerId;
                movie.DirectorId = item.DirectorId;
                movie.Genres = item.Genres.ToList();
                movie.Stars = item.Stars.ToList();
                response.Movies.Add(movie);
            }


            return Ok(response);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] DirectorRequest request)
        {
            try
            {
                var _context = new PE_PRN_Fall22B1Context();

                var director = new Director();
                director.FullName = request.FullName;
                director.Male = request.Male;
                director.Dob = request.Dob;
                director.Nationality = request.Nationality;
                director.Description = request.Description;
                _context.Directors.Add(director);
                _context.SaveChanges();
                return Ok(1);
            }
            catch (Exception e)
            {
                return Conflict("There is an error while adding.");
            }
        }

    }
}

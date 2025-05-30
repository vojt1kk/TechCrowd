using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projekt___TechCrowd.Data;
using Projekt___TechCrowd.Dtos;
using Articles = Projekt___TechCrowd.Models.Articles;


namespace Projekt___TechCrowd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ArticleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var articles = await _context.Articles.ToListAsync();

            var dtoList = articles.Select(a => new OutputDto
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                Author = a.Author,
                Genre = a.Genre,
                Date = a.Date
            });

            return Ok(dtoList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var article = await _context.Articles.FindAsync(id);

            if(article == null)
            {
                return NotFound();
            }
            return Ok(article);
        }

        [HttpGet("genres")]
        public IActionResult getGenres()
        {
            var genres = new List<string>
            {
                "Tech",
                "Lifestyle", 
                "Health", 
                "Finance", 
                "Travel", 
                "Education", 
                "Entertainment"
            };
            return Ok(genres);
        }

        [HttpGet("bygenre")]
        public async Task<IActionResult> GetByGenre([FromQuery] string genre)
        {
            if (string.IsNullOrWhiteSpace(genre))
            {
                return BadRequest("Musíte zadat hodnotu parametru 'genre'.");
            }

            genre = genre.Trim();

            var articles = await _context.Articles
                .Where(a => !string.IsNullOrWhiteSpace(a.Genre) && a.Genre.Trim() == genre)
                .ToListAsync();

            if (!articles.Any())
            {
                return NotFound($"Žádné články nebyly nalezeny pro žánr '{genre}'.");
            }

            var dtoList = articles.Select(a => new OutputDto
            {
                Id = a.Id,
                Title = a.Title,
                Content = a.Content,
                Author = a.Author,
                Genre = a.Genre,
                Date = a.Date
            });

            return Ok(dtoList);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] InputDto dto)
        {
            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound($"Článek s ID {id} nebyl nalezen.");
            }
            article.Author = dto.Author;
            article.Title = dto.Title;
            article.Content = dto.Content;
            article.Genre = dto.Genre;
            article.Date = dto.Date;

            await _context.SaveChangesAsync();
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] InputDto dto)
        {
            var allowedGenres = new List<string>
            {
                "Tech",
                "Lifestyle",
                "Health",
                "Finance",
                "Travel",
                "Education",
                "Entertainment"
            };

            if (!allowedGenres.Contains(dto.Genre))
            {
                return BadRequest($"Neplatný žánr '{dto.Genre}'. Povolené žánry jsou: {string.Join(", ", allowedGenres)}.");
            }

            var article = new Articles
            {
                Title = dto.Title,
                Content = dto.Content,
                Author = dto.Author,
                Genre = dto.Genre,
                Date = dto.Date
            };

            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAll), article);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            var hasAny = await _context.Articles.AnyAsync();
            if (!hasAny)
            {
                await _context.Database.ExecuteSqlRawAsync("DELETE FROM sqlite_sequence WHERE name = 'Articles'");
            }

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAll()
        {
            var articles = await _context.Articles.ToListAsync();

            if (!articles.Any())
            {
                return NotFound("V databázi nejsou položky ke smazání.");
            }

            _context.Articles.RemoveRange(articles);
            await _context.SaveChangesAsync();

            await _context.Database.ExecuteSqlRawAsync("DELETE FROM sqlite_sequence WHERE name = 'Articles'");

            return NoContent();
        }
    }
}
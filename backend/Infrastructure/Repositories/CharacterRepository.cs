using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly ApplicationContext _context;

        public CharacterRepository(ApplicationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Character>> GetAllCharacters(CancellationToken ct)
        {
            return await _context.Characters
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<Character?> GetCharacterById(int id, CancellationToken ct)
        {
            return await _context.Characters
                .Include(c => c.Civilization)
                .Include(c => c.Age)
                .Include(c => c.Battles) //Tabla intermedia
                    .ThenInclude(cb => cb.Battle)
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public async Task<Character> CreateCharacter(Character character, CancellationToken ct)
        {
            await _context.Characters.AddAsync(character, ct);
            await _context.SaveChangesAsync(ct);
            return character;
        }

        public async Task UpdateCharacter(Character character, CancellationToken ct)
        {
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteCharacter(Character character, CancellationToken ct)
        {
            _context.Characters.Remove(character);
            await _context.SaveChangesAsync(ct);
        }
    }
}
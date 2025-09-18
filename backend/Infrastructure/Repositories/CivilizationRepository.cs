using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;//Mirar usings

namespace Infrastructure.Repositories
{
    public class CivilizationRepository(ApplicationContext context) : ICivilizationRepository
    {
        private readonly ApplicationContext _context = context;

        public async Task<IEnumerable<Civilization>> GetAllCivilization(CancellationToken ct)
        {
            return await _context.Civilizations
                .AsNoTracking()
                .ToListAsync(ct);
        }

        public async Task<Civilization?> GetCivizlizationById(int id, CancellationToken ct)
        {
            return await _context.Civilizations
                .Include(c => c.Characters).ThenInclude(ch => ch.Age) //Ver mas de los pj
                .Include(c => c.Ages).ThenInclude(ca => ca.Age) //Tabla intermedia
                .Include(c => c.Battles).ThenInclude(cb => cb.Battle) //Tabla intermedia
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id, ct);
        }

        public async Task<Civilization> CreateCivilization(Civilization civilization, CancellationToken ct)
        {
            await _context.Civilizations.AddAsync(civilization);
            await _context.SaveChangesAsync(ct);
            return civilization;
        }

        public async Task UpdateCivilization(Civilization civilization, CancellationToken ct)
        {
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteCivilization(Civilization civilization, CancellationToken ct)
        {
            _context.Civilizations.Remove(civilization);
            await _context.SaveChangesAsync(ct);
        }
    }
}
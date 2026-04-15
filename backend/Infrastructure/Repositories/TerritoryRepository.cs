using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TerritoryRepository : ITerritoryRepository
    {
        private readonly ApplicationContext _context;

        public TerritoryRepository(ApplicationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Territory?> GetTerritoryByName(string name, CancellationToken ct)
        {
            return await _context.Territories
                .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower(), ct);
        }

        public async Task<List<Territory>> GetTerritoriesByNames(IEnumerable<string> names, CancellationToken ct)
        {
            var normalizedNames = names
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => name.Trim().ToLower())
                .Distinct()
                .ToList();

            return await _context.Territories
                .Where(t => normalizedNames.Contains(t.Name.ToLower()))
                .ToListAsync(ct);
        }

        public async Task<Territory> CreateTerritory(Territory territory, CancellationToken ct)
        {
            await _context.Territories.AddAsync(territory, ct);
            await _context.SaveChangesAsync(ct);
            return territory;
        }
    }
}

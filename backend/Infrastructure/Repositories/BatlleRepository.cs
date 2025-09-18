using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BatlleRepository : IBattleRepository
    {
        private readonly ApplicationContext _context;

        public BatlleRepository(ApplicationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Battle>> GetAllBattles(CancellationToken ct)
        {
            return await _context.Battles
            .AsNoTracking()
            .ToListAsync(ct);
        }

        public async Task<Battle?> GetByIdBattle(int id, CancellationToken ct)
        {
            return await _context.Battles
            .Include(b => b.Age)
            .Include(b => b.Character) //Tabla intermedia
                .ThenInclude(cb => cb.Character)
            .Include(b => b.Civilization)//Tabla intermedia
                .ThenInclude(cb => cb.Civilization)
            .FirstOrDefaultAsync(b => b.Id == id, ct);
        }

        public async Task<Battle> CreateBattle(Battle battle, CancellationToken ct)
        {
            await _context.Battles.AddAsync(battle, ct);
            await _context.SaveChangesAsync(ct);
            return battle;
        }

        public async Task UpdateBattle(Battle battle, CancellationToken ct)
        {
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteBattle(Battle battle, CancellationToken ct)
        {
            _context.Battles.Remove(battle);
            await _context.SaveChangesAsync(ct);
        }
    }
}
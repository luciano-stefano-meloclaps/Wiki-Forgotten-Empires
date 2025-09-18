using Domain.Entities;
using Domain.Interfaces;
using Domain.Relations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AgeRepository : IAgeRepository
    {
        private readonly ApplicationContext _context;

        public AgeRepository(ApplicationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Age>> GetAllAges(CancellationToken ct)
        {
            return await _context.Ages
           .AsNoTracking()
           .ToListAsync(ct);
        }

        public async Task<Age?> GetAgeDetailById(int id, CancellationToken ct)
        {
            return await _context.Ages

                .Include(a => a.Battles)
                .Include(a => a.Characters)
                 .Include(a => a.Civilizations)
            .ThenInclude(ca => ca.Civilization)
        .FirstOrDefaultAsync(a => a.Id == id, ct);
        }

        public async Task<Age?> GetTrackedAgeById(int id, CancellationToken ct)
        {
            return await _context.Ages.FirstOrDefaultAsync(a => a.Id == id, ct);
        }

        public async Task<Age> CreateAge(Age age, CancellationToken ct)
        {
            await _context.Ages.AddAsync(age, ct);
            await _context.SaveChangesAsync(ct);
            return age;
        }

        public async Task UpdateAge(Age age, CancellationToken ct)
        {
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAge(Age age, CancellationToken ct)
        {
            _context.Ages.Remove(age);
            await _context.SaveChangesAsync(ct);
        }

        /////// METODOS para VINCULAR relaciones por ID unico \\\\\\
        public async Task<bool> LinkBattleAsync(int ageId, int battleId, CancellationToken ct)
        {
            // Buscar la entidad
            var battle = await _context.Battles.FindAsync(battleId);

            if (battle is null)
            {
                return false;
            }

            // Asignar y guardar
            battle.AgeId = ageId;
            _context.Battles.Update(battle);
            await _context.SaveChangesAsync(ct);

            return true;
        }

        public async Task<bool> LinkCharacterAsync(int ageId, int characterId, CancellationToken ct)
        {
            var character = await _context.Characters.FindAsync(characterId);

            if (character is null)
            {
                return false;
            }

            character.AgeId = ageId;
            _context.Characters.Update(character);
            await _context.SaveChangesAsync(ct);

            return true;
        }

        public async Task<bool> LinkCivilizationAsync(int ageId, int civilizationId, CancellationToken ct)
        {
            // Validar civilización
            var civilizationExists = await _context.Civilizations.AnyAsync(c => c.Id == civilizationId, ct);
            if (!civilizationExists)
            {
                return false;
            }

            // Garantiza idempotencia y no crea duplicados
            var relationExists = await _context.CivilizationAges
                .AnyAsync(ca => ca.AgeId == ageId && ca.CivilizationId == civilizationId, ct);

            if (relationExists)
            {
                return true;
            }

            //  Asignar y guardar
            var newRelation = new CivilizationAge { AgeId = ageId, CivilizationId = civilizationId };
            _context.CivilizationAges.Add(newRelation);
            await _context.SaveChangesAsync(ct);

            return true;
        }

        /////// METODOS para DESVINCULAR relaciones por ID unico \\\\\\
        public async Task<bool> UnlinkBattleAsync(int ageId, int battleId, CancellationToken ct)
        {
            var battle = await _context.Battles.FirstOrDefaultAsync(b => b.Id == battleId, ct);

            //  Validar que exista y que la relación sea correcta
            if (battle is null || battle.AgeId != ageId)
            {
                return false;
            }

            //  Asignar y guardar
            battle.AgeId = null;
            _context.Battles.Update(battle);
            await _context.SaveChangesAsync(ct);

            return true;
        }

        public async Task<bool> UnlinkCharacterAsync(int ageId, int characterId, CancellationToken ct)
        {
            var character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == characterId, ct);

            if (character is null || character.AgeId != ageId)
            {
                return false;
            }

            character.AgeId = null;
            _context.Characters.Update(character);
            await _context.SaveChangesAsync(ct);

            return true;
        }

        public async Task<bool> UnlinkCivilizationAsync(int ageId, int civilizationId, CancellationToken ct)
        {
            //Buscar la entidad de unión
            var relation = await _context.CivilizationAges
                .FirstOrDefaultAsync(ca => ca.AgeId == ageId && ca.CivilizationId == civilizationId, ct);

            if (relation is null)
            {
                return false;
            }

            //Asignar y guardar
            _context.CivilizationAges.Remove(relation);
            await _context.SaveChangesAsync(ct);

            return true;
        }
    }
}
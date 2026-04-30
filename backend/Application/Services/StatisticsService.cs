using Application.Interfaces;
using Application.Models.Dto;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ApplicationContext _context;

        public StatisticsService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<GlobalStatsDto> GetGlobalStatsAsync(CancellationToken ct)
        {
            return new GlobalStatsDto
            {
                TotalBattles = await _context.Battles.CountAsync(ct),
                TotalCharacters = await _context.Characters.CountAsync(ct),
                TotalCivilizations = await _context.Civilizations.CountAsync(ct),
                TotalAges = await _context.Ages.CountAsync(ct)
            };
        }
    }
}

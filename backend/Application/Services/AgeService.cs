using System;
using Application.Interfaces;
using Application.Models.Dto;
using Application.Models.Request;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Relations;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ForgottenEmpires.Application.Services;

public class AgeService : IAgeService
{
    private readonly IAgeRepository _ageRepository;
    private readonly ApplicationContext _context;

    public AgeService(IAgeRepository ageRepository, ApplicationContext context)
    {
        _ageRepository = ageRepository;
        _context = context;
    }

    public async Task<IEnumerable<AgeAccordionDto>> GetAllAges(CancellationToken ct)
    {
        var ages = await _ageRepository.GetAllAges(ct);
        return ages.Select(AgeAccordionDto.ToDto);
    }

    public async Task<AgeDetailDto?> GetAgeById(int id, CancellationToken ct)
    {
        var age = await _ageRepository.GetAgeDetailById(id, ct);
        return age is null ? null : AgeDetailDto.ToDto(age);
    }

    public async Task<AgeDetailDto> CreateAge(CreateAgeDto request, CancellationToken ct)
    {
        var age = CreateAgeDto.ToEntity(request);
        await _ageRepository.CreateAge(age, ct);
        return AgeDetailDto.ToDto(age);
    }

    public async Task<bool> UpdateAge(int id, UpdateAgeDto request, CancellationToken ct)
    {
        var age = await _ageRepository.GetAgeDetailById(id, ct);
        if (age is null)
        {
            return false;
        }

        UpdateAgeDto.ApplyToEntity(request, age);
        await _ageRepository.UpdateAge(age, ct);
        return true;
    }

    public async Task<bool> DeleteAge(int id, CancellationToken ct)
    {
        var age = await _ageRepository.GetAgeDetailById(id, ct);
        if (age is null)
        {
            return false;
        }
        await _ageRepository.DeleteAge(age, ct);
        return true;
    }

    /*public async Task<(bool Success, string ErrorMessage)> UpdateAgeRelations(int ageId, UpdateAgeRelationsDto dto, CancellationToken ct)
    {
        //Validar que exista la Age
        var ageExists = await _context.Ages.AnyAsync(a => a.Id == ageId, ct);
        if (!ageExists)
        {
            return (false, $"No se encontró la Age con id {ageId}.");
        }

        //Buscar y asignar las rel Battle
        if (dto.BattleId.HasValue)
        {
            var battle = await _context.Battles.FindAsync(dto.BattleId.Value);
            if (battle is null) return (false, $"No se encontró la Battle con id {dto.BattleId.Value}.");

            battle.AgeId = ageId; // Asigna la relación
            _context.Update(battle);
        }

        //Buscar y asignar las rel Character
        if (dto.CharacterId.HasValue)
        {
            private var character = await _context.Characters.FindAsync(dto.CharacterId.Value);

            if (character is null) return (false, $"No se encontró el Character con id {dto.CharacterId.Value}.");

            character.AgeId = ageId; // Asigna la relación
            _context.Update(character);
        }

        await _context.SaveChangesAsync(ct);

        return (true, string.Empty);
    }*/

    /////// METODOS para VINCULAR relaciones por ID unico \\\\\\

    public async Task<(bool Success, string ErrorMessage)> UpdateAgeBattleRelation(int ageId, int battleId, CancellationToken ct)
    {
        var age = await _ageRepository.GetTrackedAgeById(ageId, ct);
        if (age is null)
        {
            return (false, $"No se encontró la Age con id {ageId}.");
        }

        // Delegar la operación de vinculación al repositorio.
        var success = await _ageRepository.LinkBattleAsync(ageId, battleId, ct);

        if (!success)
        {
            return (false, $"No se encontró la Battle con id {battleId}.");
        }

        return (true, string.Empty);
    }

    public async Task<(bool Success, string ErrorMessage)> UpdateAgeCharacterRelation(int ageId, int characterId, CancellationToken ct)
    {
        var age = await _ageRepository.GetTrackedAgeById(ageId, ct);
        if (age is null)
        {
            return (false, $"No se encontró la Age con id {ageId}.");
        }

        var success = await _ageRepository.LinkCharacterAsync(ageId, characterId, ct);

        if (!success)
        {
            return (false, $"No se encontró el Character con id {characterId}.");
        }

        return (true, string.Empty);
    }

    public async Task<(bool Success, string ErrorMessage)> UpdateAgeCivilizationRelation(int ageId, int civilizationId, CancellationToken ct)
    {
        var age = await _ageRepository.GetTrackedAgeById(ageId, ct);
        if (age is null)
        {
            return (false, $"No se encontró la Age con id {ageId}.");
        }

        var success = await _ageRepository.LinkCivilizationAsync(ageId, civilizationId, ct);

        if (!success)
        {
            return (false, $"No se encontró la Civilization con id {civilizationId}.");
        }

        return (true, string.Empty);
    }

    /* public async Task<(bool Success, string ErrorMessage)> RemoveAgeRelationsAsync(int ageId, UpdateAgeRelationsDto dto, CancellationToken ct)
     {
         // Validar que exista la Age
         var ageExists = await _context.Ages.AnyAsync(a => a.Id == ageId, ct);
         if (!ageExists)
         {
             return (false, $"No se encontró la Age con id {ageId}.");
         }

         // Buscart y desvincular Character
         if (dto.CharacterId.HasValue)
         {
             var character = await _context.Characters.FindAsync(dto.CharacterId.Value);

             if (character is null) return (false, $"No se encontró el Character con id {dto.CharacterId.Value}.");

             character.AgeId = null; // Elimina la relación
             _context.Update(character);
         }

         // Buscart y desvincular Battle
         if (dto.BattleId.HasValue)
         {
             var battle = await _context.Battles.FindAsync(dto.BattleId.Value);
             if (battle is null) return (false, $"No se encontró la Battle con id {dto.BattleId.Value}.");

             battle.AgeId = null; // Elimina la relación
             _context.Update(battle);
         }

         await _context.SaveChangesAsync(ct);
         return (true, string.Empty);
     }*/

    /////// METODOS para DESVINCULAR relaciones por ID unico \\\\\\
    public async Task<(bool Success, string ErrorMessage)> RemoveAgeBattleRelation(int ageId, int battleId, CancellationToken ct)
    {
        var age = await _ageRepository.GetTrackedAgeById(ageId, ct);
        if (age is null)
        {
            return (false, $"No se encontró la Age con id {ageId}.");
        }

        // Delegar la op desvinculación al repositorio
        var success = await _ageRepository.UnlinkBattleAsync(ageId, battleId, ct);

        if (!success)
        {
            return (false, $"No se pudo remover la relación: la Batalla con id {battleId} no existe o no está relacionada con la Edad con id {ageId}.");
        }

        return (true, string.Empty);
    }

    public async Task<(bool Success, string ErrorMessage)> RemoveAgeCharacterRelation(int ageId, int characterId, CancellationToken ct)
    {
        var age = await _ageRepository.GetTrackedAgeById(ageId, ct);
        if (age is null)
        {
            return (false, $"No se encontró la Age con id {ageId}.");
        }

        var success = await _ageRepository.UnlinkCharacterAsync(ageId, characterId, ct);

        if (!success)
        {
            return (false, $"No se pudo remover la relación: el Personaje con id {characterId} no existe o no está relacionado con la Edad con id {ageId}.");
        }

        return (true, string.Empty);
    }

    public async Task<(bool Success, string ErrorMessage)> RemoveAgeCivilizationRelation(int ageId, int civilizationId, CancellationToken ct)
    {
        var age = await _ageRepository.GetTrackedAgeById(ageId, ct);
        if (age is null)
        {
            return (false, $"No se encontró la Age con id {ageId}.");
        }

        var success = await _ageRepository.UnlinkCivilizationAsync(ageId, civilizationId, ct);

        if (!success)
        {
            return (false, $"No se encontró una relación entre la Age con id {ageId} y la Civilization con id {civilizationId}.");
        }

        return (true, string.Empty);
    }
}
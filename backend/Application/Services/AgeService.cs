using System;
using Application.Interfaces;
using Application.Models.Dto;
using Application.Models.Request;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Relations;

namespace ForgottenEmpires.Application.Services;

public class AgeService : IAgeService
{
    private readonly IAgeRepository _ageRepository;
    private readonly INotionDataStore _notionStore;

    public AgeService(IAgeRepository ageRepository, INotionDataStore notionStore)
    {
        _ageRepository = ageRepository;
        _notionStore = notionStore;
    }

    public async Task<IEnumerable<AgeAccordionDto>> GetAllAges(CancellationToken ct)
    {
        if (!_notionStore.IsInitialized)
        {
            throw new InvalidOperationException("Los datos de Notion no están disponibles. Por favor, sincronice primero.");
        }

        var ages = await _ageRepository.GetAllAges(ct);
        return ages.Select(AgeAccordionDto.ToDto);
    }

    public async Task<AgeDetailDto?> GetAgeById(int id, CancellationToken ct)
    {
        if (!_notionStore.IsInitialized)
        {
            throw new InvalidOperationException("Los datos de Notion no están disponibles. Por favor, sincronice primero.");
        }

        var age = await _ageRepository.GetAgeDetailById(id, ct);
        return age is null ? null : AgeDetailDto.ToDto(age);
    }

    public async Task<AgeDetailDto> CreateAge(CreateAgeDto request, CancellationToken ct)
    {
        throw new NotSupportedException("No se permite crear edades desde la API. Los datos deben gestionarse en Notion.");
    }

    public async Task<bool> UpdateAge(int id, UpdateAgeDto request, CancellationToken ct)
    {
        throw new NotSupportedException("No se permite actualizar edades desde la API. Los datos deben gestionarse en Notion.");
    }

    public async Task<bool> DeleteAge(int id, CancellationToken ct)
    {
        throw new NotSupportedException("No se permite eliminar edades desde la API. Los datos deben gestionarse en Notion.");
    }

    /////// METODOS para VINCULAR relaciones por ID unico \\\\\\

    public async Task<(bool Success, string ErrorMessage)> UpdateAgeBattleRelation(int ageId, int battleId, CancellationToken ct)
    {
        throw new NotSupportedException("No se permiten modificaciones de relaciones desde la API. Los datos deben gestionarse en Notion.");
    }

    public async Task<(bool Success, string ErrorMessage)> UpdateAgeCharacterRelation(int ageId, int characterId, CancellationToken ct)
    {
        throw new NotSupportedException("No se permiten modificaciones de relaciones desde la API. Los datos deben gestionarse en Notion.");
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
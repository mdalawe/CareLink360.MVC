using CareLink360.Application.DTOs;
using CareLink360.Application.Interfaces;
using CareLink360.Domain.Entities;
using CareLink360.Domain.Interface;

namespace CareLink360.Application.Services;

public class RatingService : IRatingService
{
    private readonly IRatingRepository _repository;
    private readonly ICaregiverRepository _caregiverRepository;

    public RatingService(
        IRatingRepository repository,
        ICaregiverRepository caregiverRepository)
    {
        _repository = repository;
        _caregiverRepository = caregiverRepository;
    }

    public async Task<Rating> CreateAsync(CreateRatingDto dto)
    {
        if (dto.Score < 1 || dto.Score > 5)
            throw new Exception("Score must be between 1 and 5");

        var rating = new Rating
        {
            CaregiverId = dto.CaregiverId,
            ClientId = dto.ClientId,
            Score = dto.Score,
            Comment = dto.Comment
        };

        await _repository.CreateAsync(rating);

        var caregiver =
            await _caregiverRepository.GetByIdAsync(dto.CaregiverId);

        if (caregiver != null)
        {
            var ratings =
                await _repository.GetByCaregiverIdAsync(dto.CaregiverId);

            caregiver.AverageRating =
                ratings.Any()
                    ? Convert.ToDecimal(ratings.Average(x => x.Score))
                    : 0;

            await _caregiverRepository.UpdateAsync(caregiver);
        }

        return rating;
    }

    public async Task<List<Rating>> GetByCaregiverIdAsync(Guid caregiverId)
    {
        return await _repository.GetByCaregiverIdAsync(caregiverId);
    }
}
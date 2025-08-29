using FinStack.Application.DTOs;
using FinStack.Common;
using FinStack.Domain.Entities;
using FinStack.Domain.Repositories;
using MediatR;

using static FinStack.Common.Result;

namespace FinStack.Application.Commands;

public record CreateUpdateUserPreferenceCommand(Guid userGuid, CreateUpdateUserPreferenceDto userDto) : IRequest<Result<Unit>>;

public class CreateUserPreferenceCommandHandler(IUserRepository userRepo, IUserPreferenceRepository userPrefsRepo) 
    : IRequestHandler<CreateUpdateUserPreferenceCommand, Result<Unit>>
{
    public async Task<Result<Unit>> Handle(CreateUpdateUserPreferenceCommand request, CancellationToken cancellationToken)
    {
        var dto = request.userDto;
        Option<AppUser> option = await userRepo.GetByIdAsync(request.userGuid);

        if (option.IsNone) {
            return Failure<Unit>(Error.UserNotFound(request.userGuid));
        }

        var user = option.Value;
        var userPrefs = new UserPreference
        {
            User = user,
            RiskLevel = dto.RiskLevel,
            PreferredMarkets = dto.PreferredMarkets,
            DefaultCurrency = dto.DefaultCurrency,
            EmailNotifications = dto.EmailNotifications,
            PushNotifications = dto.PushNotifications,
            Theme = dto.Theme,
        };

        return (await userPrefsRepo.AddOrUpdateAsync(userPrefs)).Match(
            success => Success(success),
            failure => Failure<Unit>(failure)
        );
    }
}

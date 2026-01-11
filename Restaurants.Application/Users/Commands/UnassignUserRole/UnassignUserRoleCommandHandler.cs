using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.UnassignUserRole;

public class UnassignUserRoleCommandHandler(
    ILogger<UnassignUserRoleCommandHandler> logger,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager) : IRequestHandler<UnassignUserRoleCommand>
{
    public async Task Handle(UnassignUserRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Unassigning user {UserEmail} from role {RoleName}", request.UserEmail, request.RoleName);

        var user = await userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new NotFoundException($"User with email '{request.UserEmail}' not found");

        var role = await roleManager.FindByNameAsync(request.RoleName)
            ?? throw new NotFoundException($"Role '{request.RoleName}' not found");

        await userManager.RemoveFromRoleAsync(user, role.Name!);
    }
}

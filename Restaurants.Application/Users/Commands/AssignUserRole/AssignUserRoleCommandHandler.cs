using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommandHandler(
    ILogger<AssignUserRoleCommandHandler> logger,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager) : IRequestHandler<AssignUserRoleCommand>
{
    public async Task Handle(AssignUserRoleCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Assigning user {UserEmail} to role {RoleName}", request.UserEmail, request.RoleName);

        var user = await userManager.FindByEmailAsync(request.UserEmail)
            ?? throw new NotFoundException($"User with email '{request.UserEmail}' not found");

        var role = await roleManager.FindByNameAsync(request.RoleName)
            ?? throw new NotFoundException($"Role '{request.RoleName}' not found");

        await userManager.AddToRoleAsync(user, role.Name!);
    }
}
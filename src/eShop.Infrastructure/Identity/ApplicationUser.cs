using eShop.Domain.SharedKernel.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace eShop.Infrastructure.Identity;

public sealed class ApplicationUser : IdentityUser<UserId> { }

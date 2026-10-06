using Desafio3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Desafio3.Identity;

public class UsuarioManager(
    IUserStore<Usuario> store,
    IOptions<IdentityOptions> optionsAccessor,
    IPasswordHasher<Usuario> passwordHasher,
    IEnumerable<IUserValidator<Usuario>> userValidators,
    IEnumerable<IPasswordValidator<Usuario>> passwordValidators,
    ILookupNormalizer keyNormalizer,
    IdentityErrorDescriber errors,
    IServiceProvider services,
    ILogger<UserManager<Usuario>> logger,
    RoleManager<IdentityRole> roleManager)
    : UserManager<Usuario>(
        store,
        optionsAccessor,
        passwordHasher,
        userValidators,
        passwordValidators,
        keyNormalizer,
        errors,
        services,
        logger)
{
    public override async Task<IdentityResult> CreateAsync(Usuario user, string password)
    {
        var result = await base.CreateAsync(user, password);
        if (!result.Succeeded || !await roleManager.RoleExistsAsync(Roles.Usuario))
        {
            return result;
        }

        return await AddToRoleAsync(user, Roles.Usuario);
    }
}

public static class Roles
{
    public const string Administrador = "Administrador";
    public const string Usuario = "Usuario";
}

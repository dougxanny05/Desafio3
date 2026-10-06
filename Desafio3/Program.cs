using Desafio3.Data;
using Desafio3.DTOs;
using Desafio3.Identity;
using Desafio3.Models;
using Desafio3.SSRS;
using Desafio3.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("No se encontró ConnectionStrings:DefaultConnection.");

builder.Services.AddDbContext<MiDbContext> ( options =>
    options.UseSqlServer ( connectionString ) );

builder.Services
    .AddIdentityCore<Usuario> ( options =>
    {
        options.User.RequireUniqueEmail = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
    } )
    .AddRoles<IdentityRole> ()
    .AddEntityFrameworkStores<MiDbContext> ()
    .AddApiEndpoints ();

builder.Services.AddScoped<UserManager<Usuario>, UsuarioManager> ();

builder.Services.AddAuthentication ( options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
} )
    .AddIdentityCookies ();

builder.Services.AddAuthorization ();
builder.Services.AddControllers ();
builder.Services.AddRazorPages ();
builder.Services.AddOpenApi ();
builder.Services
    .AddOptions<AdminSeedOptions> ()
    .Bind ( builder.Configuration.GetSection ( "IdentitySeed" ) )
    .ValidateDataAnnotations ()
    .ValidateOnStart ();

var app = builder.Build();

await InicializarBaseDeDatosAsync ( app.Services );

if ( app.Environment.IsDevelopment () )
{
    app.MapOpenApi ();
    app.MapScalarApiReference ();
}

app.UseHttpsRedirection ();
app.UseAuthentication ();
app.UseAuthorization ();

app.MapControllers ();
app.MapRazorPages ();
app.MapIdentityApi<Usuario> ();

app.Run ();

static async Task InicializarBaseDeDatosAsync ( IServiceProvider services )
{
    using var scope = services.CreateScope();
    var serviceProvider = scope.ServiceProvider;
    var adminSeed = serviceProvider.GetRequiredService<IOptions<AdminSeedOptions>>().Value;
    var dbContext = serviceProvider.GetRequiredService<MiDbContext>();

    if ( dbContext.Database.IsRelational () )
    {
        await dbContext.Database.MigrateAsync ();
    }
    else
    {
        await dbContext.Database.EnsureCreatedAsync ();
    }

    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    foreach ( var role in new[] { Roles.Administrador, Roles.Usuario } )
    {
        if ( !await roleManager.RoleExistsAsync ( role ) )
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
            if ( !roleResult.Succeeded )
            {
                throw new InvalidOperationException (
                    $"No se pudo crear el rol {role}: {string.Join ( ", ", roleResult.Errors.Select ( error => error.Description ) )}" );
            }
        }
    }

    var userManager = serviceProvider.GetRequiredService<UserManager<Usuario>>();
    var admin = await userManager.FindByEmailAsync(adminSeed.AdminEmail);
    if ( admin is null )
    {
        if ( string.IsNullOrWhiteSpace ( adminSeed.AdminPassword ) )
        {
            throw new InvalidOperationException (
                "Falta IdentitySeed:AdminPassword para crear el usuario administrador inicial." );
        }

        admin = new Usuario
        {
            UserName = adminSeed.AdminEmail,
            Email = adminSeed.AdminEmail,
            EmailConfirmed = true
        };

        var userResult = await userManager.CreateAsync(admin, adminSeed.AdminPassword);
        if ( !userResult.Succeeded )
        {
            throw new InvalidOperationException (
                $"No se pudo crear el administrador: {string.Join ( ", ", userResult.Errors.Select ( error => error.Description ) )}" );
        }
    }

    if ( !await userManager.IsInRoleAsync ( admin, Roles.Administrador ) )
    {
        var roleResult = await userManager.AddToRoleAsync(admin, Roles.Administrador);
        if ( !roleResult.Succeeded )
        {
            throw new InvalidOperationException (
                $"No se pudo asignar el rol Administrador: {string.Join ( ", ", roleResult.Errors.Select ( error => error.Description ) )}" );
        }
    }
}

public partial class Program { }
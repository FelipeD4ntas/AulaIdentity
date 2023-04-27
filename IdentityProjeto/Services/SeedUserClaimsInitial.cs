using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityProjeto.Services;

public class SeedUserClaimsInitial : ISeedUserClaimsInitial
{
    private readonly UserManager<IdentityUser> _userManager;

    public SeedUserClaimsInitial(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task SeedUserClaims()
    {
        try
        {
            // Procura um usuário com o email "admin@localhost" e armazena em "user1"
            IdentityUser user1 = await _userManager.FindByEmailAsync("admin@localhost");

            // Verifica se "user1" não é nulo
            if (user1 is not null)
            {
                // Obtém a lista de reivindicações para "user1" e seleciona apenas o tipo de cada reivindicação, armazenando em "claimList"
                var claimList = (await _userManager.GetClaimsAsync(user1)).Select(p => p.Type);

                // Verifica se a lista de reivindicações não contém a reivindicação com o tipo "CadastradoEm"
                if (!claimList.Contains("CadastradoEm"))
                {
                    // Adiciona uma nova reivindicação com o tipo "CadastradoEm" para "user1" com a data atual em formato de string e armazena o resultado em "claimResult1"
                    var claimResult1 = await _userManager.AddClaimAsync(user1, new Claim("CadastradoEm", DateTime.Now.Date.ToString()));
                }

                // Verifica se a lista de reivindicações não contém a reivindicação com o tipo "IsAdmin"
                if (!claimList.Contains("IsAdmin"))
                {
                    // Adiciona uma nova reivindicação com o tipo "IsAdmin" para "user1" com o valor "true" em formato de string e armazena o resultado em "claimResult1"
                    var claimResult1 = await _userManager.AddClaimAsync(user1, new Claim("IsAdmin", "true"));
                }
            }

            IdentityUser user2 = await _userManager.FindByEmailAsync("usuario@localhost");

            if (user2 is not null)
            {
                var claimList = (await _userManager.GetClaimsAsync(user2)).Select(p => p.Type);

                if (!claimList.Contains("IsFuncionario"))
                {
                    var claimResult2 = await _userManager.AddClaimAsync(user2, new Claim("IsFuncionario", "true"));
                }

                if (!claimList.Contains("IsAdmin"))
                {
                    var claimResult2 = await _userManager.AddClaimAsync(user2, new Claim("IsAdmin", "false"));
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
}

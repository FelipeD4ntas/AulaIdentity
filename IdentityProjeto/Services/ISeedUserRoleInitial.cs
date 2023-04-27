namespace IdentityProjeto.Services;

public interface ISeedUserRoleInitial
{
	Task SeedRolesAsync();
	Task SeedUsersAync();
}

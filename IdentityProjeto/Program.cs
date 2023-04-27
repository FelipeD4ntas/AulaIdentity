using IdentityProjeto.Context;
using IdentityProjeto.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SqlServerConnection");

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(connectionString);
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
	.AddEntityFrameworkStores<AppDbContext>();

builder.Services.Configure<IdentityOptions>(opt =>
{
	opt.Password.RequiredLength = 10;
	opt.Password.RequireNonAlphanumeric = false;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(opt =>
	{
		opt.Cookie.Name = "AspNetCore.Cookies";
		opt.ExpireTimeSpan = TimeSpan.FromHours(5);
		opt.SlidingExpiration = true;
	});

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("RequireUserAdminGerenteRole", policy => policy.RequireRole("User", "Admin", "Gerente"));
    options.AddPolicy("IsAdminAcess", policy => policy.RequireClaim("CadastradoEm"));
    options.AddPolicy("IsAdminAcess", policy => policy.RequireClaim("IsAdmin", "true"));
    options.AddPolicy("IsFuncionarioAcess", policy => policy.RequireRole("IsFuncionario", "true"));
});

builder.Services.AddScoped<ISeedUserRoleInitial, SeedUserRoleInitial>();
builder.Services.AddScoped<ISeedUserClaimsInitial, SeedUserClaimsInitial>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
await CriarPerfilUsuarioAsync(app);

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
	  name: "MinhaArea",
	  pattern: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
	);
});


app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// Define um m�todo ass�ncrono chamado CriarPerfilUsuarioAsync que recebe uma inst�ncia de WebApplication como par�metro
async Task CriarPerfilUsuarioAsync(WebApplication app)
{
	// Obt�m uma inst�ncia de IServiceScopeFactory do cont�iner de servi�os da aplica��o
	var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

	// Cria um novo escopo de servi�o usando a IServiceScopeFactory
	using (var scope = scopedFactory.CreateScope())
	{
        //// Obt�m uma inst�ncia do servi�o ISeedUserRoleInitial a partir do provedor de servi�os do escopo
        //var service = scope.ServiceProvider.GetService<ISeedUserRoleInitial>();

        //// Chama o m�todo ass�ncrono SeedRolesAsync do servi�o ISeedUserRoleInitial
        //await service.SeedRolesAsync();

        //// Chama o m�todo ass�ncrono SeedUsersAync do servi�o ISeedUserRoleInitial
        //await service.SeedUsersAync();



        // Obt�m o servi�o "ISeedUserClaimsInitial" do provedor de servi�os atrav�s da vari�vel "service" criada
        var service = scope.ServiceProvider.GetService<ISeedUserClaimsInitial>();

        // Chama o m�todo "SeedUserClaims" do servi�o "ISeedUserClaimsInitial" e espera sua conclus�o
        await service.SeedUserClaims();
    }
}

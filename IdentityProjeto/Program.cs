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

// Define um método assíncrono chamado CriarPerfilUsuarioAsync que recebe uma instância de WebApplication como parâmetro
async Task CriarPerfilUsuarioAsync(WebApplication app)
{
	// Obtém uma instância de IServiceScopeFactory do contêiner de serviços da aplicação
	var scopedFactory = app.Services.GetService<IServiceScopeFactory>();

	// Cria um novo escopo de serviço usando a IServiceScopeFactory
	using (var scope = scopedFactory.CreateScope())
	{
        //// Obtém uma instância do serviço ISeedUserRoleInitial a partir do provedor de serviços do escopo
        //var service = scope.ServiceProvider.GetService<ISeedUserRoleInitial>();

        //// Chama o método assíncrono SeedRolesAsync do serviço ISeedUserRoleInitial
        //await service.SeedRolesAsync();

        //// Chama o método assíncrono SeedUsersAync do serviço ISeedUserRoleInitial
        //await service.SeedUsersAync();



        // Obtém o serviço "ISeedUserClaimsInitial" do provedor de serviços através da variável "service" criada
        var service = scope.ServiceProvider.GetService<ISeedUserClaimsInitial>();

        // Chama o método "SeedUserClaims" do serviço "ISeedUserClaimsInitial" e espera sua conclusão
        await service.SeedUserClaims();
    }
}

using IdentityProjeto.Entities;
using IdentityProjeto.Entities.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProjeto.Context;

public class AppDbContext : IdentityDbContext
{
	public DbSet<Aluno> Alunos { get; set; }
	public DbSet<Produto> Produtos { get; set; }

	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfiguration(new AlunoConfiguration());
		modelBuilder.ApplyConfiguration(new ProdutoConfiguration());
		
		modelBuilder.Entity<Aluno>().HasData(
			new Aluno
			{
				AlunoId = 1,
				Nome = "Felipe Dantas",
				Email = "felipedantas@example.com",
				Idade = 28,
				Curso = "Sistemas de Informação"
			},
			new Aluno
			{
				AlunoId = 2,
				Nome = "Ana Silva",
				Email = "anasilva@example.com",
				Idade = 23,
				Curso = "Engenharia de Computação"
			},
			new Aluno
			{
				AlunoId = 3,
				Nome = "João Pedro",
				Email = "joaopedro@example.com",
				Idade = 20,
				Curso = "Administração"
			});
	}
}

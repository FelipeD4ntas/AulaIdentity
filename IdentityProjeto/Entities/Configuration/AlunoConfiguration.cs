using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProjeto.Entities.Configuration;

public class AlunoConfiguration : IEntityTypeConfiguration<Aluno>
{
	public void Configure(EntityTypeBuilder<Aluno> builder)
	{
		builder.Property(aluno => aluno.Nome)
			.IsRequired()
			.HasMaxLength(80)
			.HasAnnotation("MaxLengthErrorMessage", "O nome não pode ter mais que 80 caracteres");


		builder.Property(aluno => aluno.Email)
			.IsRequired()
			.HasMaxLength(120)
			.HasAnnotation("MaxLengthErrorMessage", "O Email não pode ter mais que 120 caracteres");

		builder.Property(aluno => aluno.Curso)
			.HasMaxLength(80)
			.HasAnnotation("MaxLengthErrorMessage", "O nome do curso não pode ter mais que 80 caracteres");
	}
}

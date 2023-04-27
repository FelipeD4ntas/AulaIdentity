using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdentityProjeto.Entities.Configuration;

public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
	public void Configure(EntityTypeBuilder<Produto> builder)
	{
		builder.Property(produto => produto.Nome)
			.HasMaxLength(80)
			.HasAnnotation("MaxLengthErrorMessage", "O nome não pode ter mais que 80 caracteres");
	}
}

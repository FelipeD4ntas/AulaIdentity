using System.ComponentModel.DataAnnotations;

namespace IdentityProjeto.Entities;

public class Aluno
{
	public int AlunoId { get; set; }
	public string? Nome { get; set; }
	[EmailAddress]
	public string? Email { get; set; }
	public int Idade { get; set; }
	public string? Curso { get; set; }
}

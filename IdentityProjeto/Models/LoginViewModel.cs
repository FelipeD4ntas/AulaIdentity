﻿using System.ComponentModel.DataAnnotations;

namespace IdentityProjeto.Models;

public class LoginViewModel
{
	[Required]
	public string UserName { get; set; }

	[Required]
	[DataType(DataType.Password)]
	public string Password { get; set; }

	public bool RememberMe { get; set; } = true;

}

using System.ComponentModel.DataAnnotations;

namespace task4.Models
{
	public class LoginViewModel
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		public string Password { get; set; }
	}
}

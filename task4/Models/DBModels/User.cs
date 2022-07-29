using System;
using System.ComponentModel.DataAnnotations;

namespace task4.Models.DBModels
{
	public class User
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public string UserName { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		[MaxLength(64)]
		public string Password { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public DateTime LastLogin { get; set; }

        [Required]
		public bool IsBlocked { get; set; }

		[Required]
		public bool IsDelete { get; set; }
	}
}

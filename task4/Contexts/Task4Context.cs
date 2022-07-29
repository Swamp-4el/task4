using Microsoft.EntityFrameworkCore;
using task4.Models.DBModels;

namespace task4.Contexts
{
	public class Task4Context : DbContext
	{
		public DbSet<User> Users { get; set; }

		public Task4Context(DbContextOptions<Task4Context> options) : base(options)
		{
			Database.EnsureCreated();
		}
	}
}

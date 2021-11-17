using Microsoft.EntityFrameworkCore;

using PloomesCsharpChallenge.Models;

namespace PloomesCsharpChallenge.Contexts
{
    public class MainContext : DbContext
    {
        public MainContext(DbContextOptions<MainContext> options)
          : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMembership> ChatMemberships { get; set; }
    }
}

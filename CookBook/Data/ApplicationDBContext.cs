using CookBook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Data;

public class ApplicationDBContext : IdentityDbContext<AppUser>
{
    
    public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
    {
        
    }
    
    public DbSet<AppUser> User { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<IngredientGroup> IngredientGroups { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<RefreshTokens> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Recipe>()
            .HasOne(r => r.User)
            .WithMany(u => u.Recipes)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<IngredientGroup>()
            .HasOne(ig => ig.Recipe)
            .WithMany(r => r.IngredientGroups)
            .HasForeignKey(ig => ig.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Ingredient>()
            .HasOne(i => i.IngredientGroup)
            .WithMany(ig => ig.Ingredients)
            .HasForeignKey(i => i.IngredientGroupId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<RefreshTokens>()
            .HasOne(r => r.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        
        List<IdentityRole> roles = new List<IdentityRole>
        {
            new IdentityRole
            {
                Id = "1",
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole
            {
                Id = "2",
                Name = "User",
                NormalizedName = "USER"
            },
            new IdentityRole
            {
                Id = "3",
                Name = "Pending",
                NormalizedName = "PENDING"
            }
        };
        modelBuilder.Entity<IdentityRole>().HasData(roles);
    }
}
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
    public DbSet<UserSettings> UserSettings { get; set; }
    public DbSet<FavoriteRecipe> FavoriteRecipes { get; set; }
    public DbSet<RecipeStep> RecipeSteps { get; set; }
    public DbSet<ShoppingList> ShoppingLists { get; set; }
    public DbSet<ShoppingListItem> ShoppingListItems { get; set; }

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

        modelBuilder.Entity<AppUser>()
            .HasOne(a => a.UserSettings)
            .WithOne(b => b.User)
            .HasForeignKey<UserSettings>(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FavoriteRecipe>()
            .HasKey(fr => new { fr.UserId, fr.RecipeId });

        modelBuilder.Entity<FavoriteRecipe>()
            .HasOne(fr => fr.User)
            .WithMany(u => u.FavoriteRecipes)
            .HasForeignKey(fr => fr.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FavoriteRecipe>()
            .HasOne(fr => fr.Recipe)
            .WithMany(r => r.FavoritedBy)
            .HasForeignKey(fr => fr.RecipeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ShoppingList>()
            .HasOne(sl => sl.User)
            .WithMany(u => u.ShoppingLists)
            .HasForeignKey(sl => sl.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ShoppingListItem>()
            .HasOne(sli => sli.ShoppingList)
            .WithMany(sl => sl.Items)
            .HasForeignKey(sli => sli.ShoppingListId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.Tags)
            .WithMany(t => t.Recipes)
            .UsingEntity<Dictionary<string, object>>(
                "RecipeTag",
                r => r.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                t => t.HasOne<Recipe>().WithMany().HasForeignKey("RecipeId"));


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
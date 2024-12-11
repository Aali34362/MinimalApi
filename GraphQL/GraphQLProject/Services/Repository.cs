using GraphQLProject.DatabaseConnections.DbContexts;
using GraphQLProject.Interfaces;
using GraphQLProject.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphQLProject.Services;

public class MenuRepository(GraphqlDbContext dbContext) : IMenuRepository
{
    private readonly GraphqlDbContext _dbContext = dbContext;
    private static List<Menu> MenuList = new List<Menu>()
        {
            new Menu() { Id = Guid.NewGuid(), Name = "Classic Burger", Description="A juicy chicken burger with lettuce and cheese" , Price = 8.99},
            new Menu() { Id = Guid.NewGuid(), Name = "Margherita Pizza", Description = "Tomato, mozzarella, and basil pizza", Price = 10.50 },
            new Menu() { Id = Guid.NewGuid(), Name = "Grilled Chicken Salad", Description = "Fresh garden salad with grilled chicken", Price = 7.95 },
            new Menu() { Id = Guid.NewGuid(), Name = "Pasta Alfredo", Description = "Creamy Alfredo sauce with fettuccine pasta", Price = 12.75 },
            new Menu() { Id = Guid.NewGuid(), Name = "Chocolate Brownie Sundae", Description = "Warm chocolate brownie with ice cream and fudge", Price = 6.99 },

        };

    public List<Menu> GetAllMenu()
    {
        return MenuList;
    }

    public Menu GetMenuById(Guid id)
    {
        return MenuList.FirstOrDefault(x=>x.Id == id)!;
    }

    public Menu AddMenu(Menu menu)
    {
        MenuList.Add(menu);
        return menu;
    }

    public void DeleteMenu(Guid id)
    {
        var index = FindListIndex(MenuList, id);
        //Menu menu = MenuList[index];
        //MenuList.Remove(menu);
        MenuList.RemoveAt(index);
    }

    public Menu UpdateMenu(Guid id, Menu menu)
    {

        var index = FindListIndex(MenuList, id);
        // Update the menu at the found index
        MenuList[index] = menu;
        return menu;
    }

    private int FindListIndex(List<Menu> obj, Guid id)
    {
        // Find the index of the menu item with the given ID
        int index = obj.FindIndex(m => m.Id == id);

        if (index == -1) // If no menu with the given ID is found
            throw new KeyNotFoundException($"Menu with ID {id} not found.");
        return index;
    }


    public List<Menu> GetAllDBMenu()
    {
        return _dbContext.Menus.ToList();
    }

    public Menu GetDBMenuById(Guid id)
    {
        return _dbContext.Menus.FirstOrDefault(x => x.Id == id)!;
    }

    public async Task<Menu> AddDBMenu(Menu menu)
    {
        await _dbContext.AddAsync(menu);
        await _dbContext.SaveChangesAsync();
        return menu;
    }

    public async Task DeleteDBMenu(Guid id)
    {
        // Find the entity using the provided ID
        var menu = await _dbContext.Menus!.FindAsync(id); // Replace 'Menus' with your DbSet name

        // Check if the entity exists
        if (menu == null)
        {
            throw new KeyNotFoundException($"Menu with ID {id} not found.");
        }

        // Remove the entity from the DbContext
        _dbContext.Menus.Remove(menu);

        // Save changes to the database
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Menu> UpdateDBMenu(Guid id, Menu menu)
    {
        menu.Id = id;
         _dbContext.Update(menu);
        await _dbContext.SaveChangesAsync();
        return menu;
    }
}

public class CategoryRepository(GraphqlDbContext dbContext) : ICategoryRepository
{
    private readonly GraphqlDbContext _dbContext = dbContext;

    public List<Category> GetCategoryList()
    {
        return _dbContext.Category.ToList();
    }

    public Category GetCategoryById(Guid id)
    {
        return _dbContext.Category.FirstOrDefault(x => x.Id == id)!;
    }

    public async Task<Category> AddCategory(Category Category)
    {
        await _dbContext.AddAsync(Category);
        await _dbContext.SaveChangesAsync();
        return Category;
    }

    public async Task DeleteCategory(Guid id)
    {
        // Find the entity using the provided ID
        var Category = await _dbContext.Category!.FindAsync(id); // Replace 'Categorys' with your DbSet name

        // Check if the entity exists
        if (Category == null)
        {
            throw new KeyNotFoundException($"Category with ID {id} not found.");
        }

        // Remove the entity from the DbContext
        _dbContext.Category.Remove(Category);

        // Save changes to the database
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Category> UpdateCategory(Guid id, Category Category)
    {
        Category.Id = id;
        _dbContext.Update(Category);
        await _dbContext.SaveChangesAsync();
        return Category;
    }
}
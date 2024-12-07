using GraphQLProject.Interfaces;
using GraphQLProject.Models;

namespace GraphQLProject.Services;

public class MenuRepository : IMenuRepository
{
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
}

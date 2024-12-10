using GraphQLProject.Models;

namespace GraphQLProject.Interfaces;

public interface IMenuRepository
{
    List<Menu> GetAllMenu();
    Menu GetMenuById(Guid id);
    Menu AddMenu(Menu menu);
    Menu UpdateMenu(Guid id, Menu menu);
    void DeleteMenu(Guid id);

    List<Menu> GetAllDBMenu();
    Menu GetDBMenuById(Guid id);
    Task<Menu> AddDBMenu(Menu menu);
    Task<Menu> UpdateDBMenu(Guid id, Menu menu);
    Task DeleteDBMenu(Guid id);
}

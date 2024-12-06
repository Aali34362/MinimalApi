using GraphQLProject.Models;

namespace GraphQLProject.Interfaces;

public interface IMenuRepository
{
    List<Menu> GetAllMenu();
    Menu GetMenuById(Guid id);
    Menu AddMenu(Menu menu);
    Menu UpdateMenu(Guid id, Menu menu);
    void DeleteMenu(Guid id);
}

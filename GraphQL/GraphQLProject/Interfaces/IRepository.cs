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

public interface ICategoryRepository
{
    Category GetCategoryById(Guid id);
    List<Category> GetCategoryList();
    Task<Category> AddCategory(Category category);
    Task<Category> UpdateCategory(Guid id, Category category);
    Task DeleteCategory(Guid id);
}

public interface IReservationRepository
{
    Reservation GetReservationById(Guid id);
    List<Reservation> GetReservationList();
    Task<Reservation> AddReservation(Reservation reservation);
    Task<Reservation> UpdateReservation(Guid id, Reservation reservation);
    Task DeleteReservation(Guid id);
}
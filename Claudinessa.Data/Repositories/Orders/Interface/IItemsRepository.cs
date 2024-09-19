using Claudinessa.Model;

namespace Claudinessa.Data.Repositories.Orders.Interface
{
    public interface IItemsRepository
    {
        Task<Dictionary<int, IEnumerable<Item.Extra>>> CreateItems(IEnumerable<Item> products, int IdOrder);
        Task<bool> DeleteItem(int IdItem);
        Task<bool> AddItemExtras(Dictionary<int, IEnumerable<Item.Extra>> productDictionary, int IdOrder);
        Task<IEnumerable<Item.Extra>> GetItemExtras(int IdItem);
        Task<IEnumerable<Item>> GetItems(int IdItem);
    }
}
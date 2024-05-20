
namespace Service
{
    public class ItemModel
    {
        public int _id { get; set; }

        public string _itemName { get; set; }

        public double _price { get; set; }

        public Category _category { get; set; }
    }

    public enum Category
    {
        electronics,
        home,
        garden,
        health,
        books,
        games,
        food
    }

}

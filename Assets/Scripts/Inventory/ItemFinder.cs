namespace Project.Inventory
{
    public class ItemFinder
    {
        private static ItemFinder _instance;

        private ItemFinder() { }

        public static ItemFinder Instance
        {
            get { return _instance ??= new ItemFinder(); }
        }

        public bool TryGetItem(string itemId, out Item item)
        {
            item = null;
            return false;
        }
    }
}

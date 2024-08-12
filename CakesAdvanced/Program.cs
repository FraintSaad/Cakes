using CakesLibrary.Models;

namespace CakesAdvanced
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var store = new Store("Название");
            store.Open();
        }
    }
}

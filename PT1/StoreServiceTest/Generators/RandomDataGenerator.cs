using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using StoreService.Data;

namespace StoreService.Tests
{
    class RandomDataGenerator : IDataGenerator
    {

        public DataContext GenerateData()
        {
            DataContext context = new DataContext();

            int ClientsNumber = 10;
            int ItemsNumber = 20;
            int EventsNumber = 5;

            List<int> clientIDs = RandomIntList(ClientsNumber);
            List<int> itemIDs = RandomIntList(ItemsNumber);

            List<Category> categories = new List<Category> {
                    Category.electronics,
                    Category.home,
                    Category.garden,
                    Category.health,
                    Category.books,
                    Category.games,
                    Category.food
                };

            // Generate  clients
            for (int i = 0; i < ClientsNumber; i++)
            {
                int id = itemIDs[i];
                context.clients.Add(new Client(id, RandomString(5), RandomString(5), RandomString(10)));
            }
          
            // Generate items 
            for (int i = 0; i < ItemsNumber; i++)
            {
                int id = itemIDs[i];
                Category category = categories[RandomInt(3) % categories.Count];
                context.items.Add(id, new Item(id, RandomInt(3), category));
            }

            // Generate purchases
            for (int i = 0; i < EventsNumber; i++)
            {
                Item item = context.items[
                    itemIDs[RandomInt(3) % itemIDs.Count]];

                Client client = context.clients
                    .Find(c => c.ClientID == RandomInt(3) % clientIDs.Count);

                context.events.Add(new EventPurchase(new State(item, RandomInt(2)), client, RandomInt(1)));
            }

            // Generate returns 
            for (int i = 0; i < EventsNumber; i++)
            {
                Item item = context.items[
                    itemIDs[RandomInt(3) % itemIDs.Count]];

                Client client = context.clients
                    .Find(c => c.ClientID == RandomInt(3) % clientIDs.Count);

                context.events.Add(new EventReturn(new State(item, RandomInt(2)), client, RandomInt(1), RandomString(10)));
            }

            return context;
        }

        public string RandomString(int length)
        {
            const string chars = "abcdefghijklmnropqrstuvwxyz";
            var random = new Random();
            var stringBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(chars[random.Next(chars.Length)]);
            }

            return stringBuilder.ToString();
        }

        public List<string> RandomStringList(int stringsNumber, int length)
        {
            List<string> strings = new List<string>();
            string str = RandomString(length);

            for (int i = 0; i < stringsNumber - 1; i++)
            {
                strings.Add(RandomString(length));
            }
            return strings;
        }


        public int RandomInt(int length)
        {
            var random = new Random();
            int minValue = (int)Math.Pow(10, length - 1);
            int maxValue = (int)Math.Pow(10, length);
            int number = random.Next(minValue, maxValue);
            return number;
        }

        List<int> RandomIntList(int howMany)
        {
            List<int> numbers = new List<int>();
            int number = RandomInt(2);
            numbers.Add(number);

            for (int i = 0; i < howMany - 1; i++)
            {
                while (numbers.Contains(number))
                {
                    number = RandomInt(3);
                }
                numbers.Add(number);
            }
            return numbers;
        }
    }
}
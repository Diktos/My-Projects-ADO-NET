using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Models
{
    public class ClientTable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // => - Екземпляри створюються лише за потреби, що економить пам'ять, якщо об'єкти використовуються рідко.
        // Підходить, якщо об'єкти потребують створення на запит або можуть змінюватися при кожному зверненні
        public static ClientTable Table1 => new ClientTable { Id = 2, Name = "Столик 1" };
        public static ClientTable Table2 => new ClientTable { Id = 3, Name = "Столик 2" };
        public static ClientTable Table3 => new ClientTable { Id = 1, Name = "Столик біля вікна" };

        public static IEnumerable<ClientTable> DefaultClientTables()
        {
            // yield - Table1, Table2, і Table3 будуть створені тільки тоді,
            // коли їх потрібно повернути викликаючому коду.
            // У поєднанні з yield, екземпляри генеруються поступово під час ітерації, а не всі одразу.
            // Завдяки yield об’єкти повертаються по одному, а не як повністю сформована колекція.
            // Це важливо, якщо список містить багато елементів або об'єкти вимагають багато пам’яті

            yield return Table1;
            yield return Table2;
            yield return Table3;
        }
    }
}

using System.Reflection;
using Coin_Wave_Lib;

namespace sfgfsg
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly externalAssembly = Assembly.LoadFrom(@"D:\Семестр 4 (полигон)\Курсовая работа\coin wave\Coin Wave (sln)\Coin Wave Lib\bin\Debug\net8.0\Coin Wave Lib.dll");
            
            var types = externalAssembly.GetTypes();

            // Фильтруем типы, чтобы найти те, которые являются наследниками GameObject
            var gameObjectTypes = types.Where(t => t.IsSubclassOf(typeof(GameObject)));

            List<GameObject> gameObjects = new List<GameObject>();
            // Выводим информацию о найденных классах
            foreach (var type in gameObjectTypes)
            {
                Console.WriteLine(type.Name);
                GameObject instance = (GameObject)Activator.CreateInstance(type, new object[] { } );
                gameObjects.Add(instance);
            }
            Console.WriteLine();
            Console.WriteLine();
            foreach (var gameObject in gameObjects)
            {
                Console.WriteLine(gameObject.GetType().Name);
            }
            Console.WriteLine("End...");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Coin_Wave_Lib
{
    public static class FileSave
    {
        public static bool SerializeObjectsToXml<T>(T objects, string filePath)
        {
            // Создаем объект XmlSerializer для класса Obj
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            // Создаем поток для записи в файл
            using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                try
                {
                    // Сериализуем список объектов в поток
                    serializer.Serialize(stream, objects);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при сериализации объектов: {ex.Message}");
                    return false;
                }
            }
        }

    }
}

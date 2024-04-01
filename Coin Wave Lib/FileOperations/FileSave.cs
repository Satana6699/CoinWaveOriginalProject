using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            XmlSerializer serializer;
            // Создаем объект XmlSerializer для класса Obj
            try
            {
                serializer = new XmlSerializer(typeof(T));
            }
            catch(Exception ex)
            {
                Debug.WriteLine(typeof(T));
                Debug.WriteLine($"Ошибка при сериализации объектов: {ex.Message}");
                return false;
            }

            // Создаем поток для записи в файл
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                try
                {
                    // Сериализуем список объектов в поток
                    serializer.Serialize(stream, objects);
                    return true;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Ошибка при сериализации объектов: {ex.Message}");
                    return false;
                }
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Coin_Wave_Lib
{
    public static class FileRead
    {
        public static GameObjectData[] DeserializeObjectsToXml(string path)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(GameObjectData[]));
            
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                GameObjectData[]? gameObjects = formatter.Deserialize(fs) as GameObjectData[];

                return gameObjects;
            }

        }

    }
}

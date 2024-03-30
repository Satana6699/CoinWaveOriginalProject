using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;


namespace Coin_Wave_Lib
{
    // Промежуточный класс для сериализации/десериализации
    [XmlInclude(typeof(BackWall))] // Здесь добавьте все классы-наследники GameObject
    [XmlInclude(typeof(Coin))]
    public class GameObjectData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Дополнительные свойства, если необходимо
    }
}
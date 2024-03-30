using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Coin_Wave_Lib
{
    [XmlRoot("GameObjects")]
    public class GameObjectDataList
    {
        [XmlElement("GameObject")]
        public List<GameObjectData> Objects { get; set; }
    }
}

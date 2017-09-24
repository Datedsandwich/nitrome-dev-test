using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("Levels")]
public class DeserializedLevels {
    [XmlElement("Developer")]
    public LevelDebug developer;
    public struct LevelDebug {
        [XmlAttribute("StartLevel")]
        public string startLevel;
    }

    [XmlElement("Level")]
    public Level[] levels;
    public struct Level {
        [XmlElement("Item")]
        public Item[] items;
    }

    public class Item {
        [XmlAttribute("prefab")]
        public string prefab;

        [XmlAttribute("x")]
        public string x;

        [XmlAttribute("y")]
        public string y;

        [XmlAttribute("rotation")]
        public string rotation;

        [XmlAttribute("scale_x")]
        public string scaleX;

        [XmlAttribute("scale_y")]
        public string scaleY;

        public Item() { }

        public Item(Transform item) {
            prefab = item.name;
            x = Optional.ToStringOrElseNull(item.transform.position.x);
            y = Optional.ToStringOrElseNull(item.transform.position.y);
            rotation = Optional.ToStringOrElseNull(item.localRotation.eulerAngles.x);
            scaleX = Optional.ToStringOrElseOne(item.localScale.x);
            scaleY = Optional.ToStringOrElseOne(item.localScale.y);
        }
    }
}


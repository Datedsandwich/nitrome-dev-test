using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class DeserializedLevelsSaver {
    public const string xmlItemsToExportGOName = "XmlItemsToExport";

    public void SaveExportItems() {
        GameObject xmlItemsToExportGO;

        // Create XmlItemsToExport or find existing
        if (GameObject.Find(xmlItemsToExportGOName) == null) {
            xmlItemsToExportGO = new GameObject(xmlItemsToExportGOName);
            //we have nothing to save so skip execution
            return;
        } else {
            xmlItemsToExportGO = GameObject.Find(xmlItemsToExportGOName);
        }

        Transform[] xmlItemsToExportGOchildren = xmlItemsToExportGO.GetComponentsInChildren<Transform>();

        // Check if there are actually any objects childed to our export game object
        if (xmlItemsToExportGOchildren.Length == 1) {
            Debug.LogError("Add the prefabs to " + xmlItemsToExportGOName);
            return;
        }

        List<DeserializedLevels.Item> itemList = new List<DeserializedLevels.Item>();

        foreach (Transform item in xmlItemsToExportGOchildren) {
            if (item.parent == xmlItemsToExportGO.transform) {
                itemList.Add(new DeserializedLevels.Item(item));
            }
        }

        DeserializedLevels.Level levelXml = new DeserializedLevels.Level();
        levelXml.items = new DeserializedLevels.Item[itemList.Count];
        itemList.CopyTo(levelXml.items);

        // Export just one level, this is a simple XML level loading pipeline for now
        DeserializedLevels levelsXmlToExport = new DeserializedLevels();
        levelsXmlToExport.levels = new DeserializedLevels.Level[1];
        levelsXmlToExport.levels[0] = levelXml;

        string outputFilePath = "./Assets/Resources/" + xmlItemsToExportGOName + ".xml";
        XmlIO.SaveXml<DeserializedLevels>(levelsXmlToExport, outputFilePath);
    }
}

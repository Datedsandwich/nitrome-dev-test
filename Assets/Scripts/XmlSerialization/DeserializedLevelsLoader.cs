using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class DeserializedLevelsLoader {
    // Levels deserialized
    private DeserializedLevels deserializedLevels;
    private const string prefabsFolder = "Prefabs/";

    struct ItemStruct {
        public GameObject prefab;
        public float x;
        public float y;
        public float rotation;
        public float scaleX;
        public float scaleY;

        public ItemStruct(GameObject prefab, DeserializedLevels.Item deserializedItem) {
            this.prefab = prefab;
            x = Optional.ToFloatOrElseNull(deserializedItem.x);
            y = Optional.ToFloatOrElseNull(deserializedItem.y);
            rotation = Optional.ToFloatOrElseNull(deserializedItem.rotation);
            scaleX = Optional.ToFloatOrElseOne(deserializedItem.scaleX);
            scaleY = Optional.ToFloatOrElseOne(deserializedItem.scaleY);
        }
    }
    // Cache prefabs in prefabPool
    private Dictionary<string, GameObject> prefabPool;
    // Cache all items with locations
    private List<ItemStruct> sceneItemsList;
    Transform parentOfXmlItems;
    public const string xmlItemsGOName = "XmlItems";

    private void Init() {
        prefabPool = new Dictionary<string, GameObject>();
        sceneItemsList = new List<ItemStruct>();

        // if the XmlItems gameobject folder remained in the Hierarcy, then delete it
        while (GameObject.Find(xmlItemsGOName) != null) {
            MonoBehaviour.DestroyImmediate(GameObject.Find(xmlItemsGOName));
        }

        parentOfXmlItems = new GameObject(xmlItemsGOName).transform;
    }

    public void GenerateItems() {
        Init();
        CreateSceneItemsList();
        InstantiateItems();
    }

    private DeserializedLevels.Level GetCurrentLevel() {
        deserializedLevels = XmlIO.LoadXml<DeserializedLevels>("Levels");

        // if startlevel is in the XML i.e. <Developer StartLevel="3" /> then get level from there
        // otherwise start with level 1
        int startLevel = int.Parse(deserializedLevels.developer.startLevel);

        // 0 indexed
        return deserializedLevels.levels[startLevel - 1]; ;
    }


    private void InstantiateItems() {
        foreach (ItemStruct item in sceneItemsList) {
            GameObject newGameObject = MonoBehaviour.Instantiate(item.prefab) as GameObject;
            SetPosition2D(newGameObject, new Vector2(item.x, item.y));
            SetRotation2D(newGameObject, item.rotation);
            newGameObject.transform.localScale = new Vector3(item.scaleX, item.scaleY, 1);
            newGameObject.transform.parent = parentOfXmlItems;
        }
    }

    private void CreateSceneItemsList() {
        foreach (DeserializedLevels.Item deserializedItem in GetCurrentLevel().items) {
            string prefabName = deserializedItem.prefab;

            // if the prefab in the item XmlNode has not been loaded then add it to the prefabPool
            if (!prefabPool.ContainsKey(prefabName)) {
                GameObject prefabObject = GlobalPrefabs.GetPrefab(prefabName);

                if (prefabObject == null) {
                    // If we couldn't find the object, break out and continue the loop
                    continue;
                }

                prefabPool.Add(prefabName, prefabObject);
            }

            ItemStruct item = new ItemStruct(prefabPool[prefabName], deserializedItem);

            sceneItemsList.Add(item);
        }
    }

    private void SetPosition2D(GameObject g, Vector2 pos) {
        g.transform.position = new Vector3(pos.x, pos.y, g.transform.position.z);
    }

    private void SetRotation2D(GameObject g, float rot) {
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = new Vector3(0, 0, rot);
        g.transform.localRotation = rotation;
    }
}

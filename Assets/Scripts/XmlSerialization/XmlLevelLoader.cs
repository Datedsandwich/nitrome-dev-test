using UnityEngine;
using System.Collections;

public class XmlLevelLoader : MonoBehaviour {
    void Start() {
        GlobalPrefabs.LoadAll("Characters");
        GlobalPrefabs.LoadAll("Environment");
        GlobalPrefabs.LoadAll("Game Dynamics");

        DeserializedLevelsLoader deserializedLevelsLoader = new DeserializedLevelsLoader();
        deserializedLevelsLoader.GenerateItems();

        DontDestroyOnLoad(this);
    }
}

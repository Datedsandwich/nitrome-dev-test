using UnityEngine;
using UnityEditor;

public class XmlLevelEditor : EditorWindow {
    DeserializedLevelsLoader deserializedLevelsLoader;
    DeserializedLevelsSaver deserializedLevelsSaver;

    private string importGOName = DeserializedLevelsLoader.xmlItemsGOName;
    private string exportGOName = DeserializedLevelsSaver.xmlItemsToExportGOName;

    [MenuItem("Window/Xml Level Editor")]
    public static void ShowWindow() {
        EditorWindow.GetWindow(typeof(XmlLevelEditor));
    }

    void OnGUI() {
        Init();
        CreateImportButton();
        CreateExportButton();
        CreateDeleteButton();
    }

    private void Init() {
        if (deserializedLevelsLoader == null) deserializedLevelsLoader = new DeserializedLevelsLoader();
        if (deserializedLevelsSaver == null) deserializedLevelsSaver = new DeserializedLevelsSaver();
    }

    private void CreateImportButton() {
        GUILayout.Label("Import", EditorStyles.boldLabel);
        GUILayout.Label("Import Levels.xml into the scene");
        if (GUILayout.Button("Import Levels.xml")) {
            deserializedLevelsLoader.GenerateItems();
        }
    }

    private void CreateExportButton() {
        GUILayout.Label("Export", EditorStyles.boldLabel);
        GUILayout.Label("Export children of \"" + exportGOName + "\" GameObject into " + exportGOName + ".xml", EditorStyles.wordWrappedLabel);
        if (GUILayout.Button("Export " + exportGOName)) {
            deserializedLevelsSaver.SaveExportItems();
        }
    }

    private void CreateDeleteButton() {
        GUILayout.Label("Delete", EditorStyles.boldLabel);
        GUILayout.Label("Delete " + importGOName + " and " + exportGOName + " GameObjects from scene", EditorStyles.wordWrappedLabel);
        if (GUILayout.Button("Delete")) {
            DestroyImmediate(GameObject.Find(importGOName));
            DestroyImmediate(GameObject.Find(exportGOName));
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class ensures we only load each prefab from Resources once, on game load, and then they can be reused.
public class GlobalPrefabs : ScriptableObject {
	public static Dictionary<int, Object> objectList = new Dictionary<int, Object>();

	public static void LoadAll(string path) {
		Object[] ObjectArray = Resources.LoadAll("Prefabs/" + path);

		foreach (Object obj in ObjectArray) {
			objectList.Add (obj.name.GetHashCode(), obj as GameObject);
		}
	}

	public static GameObject GetPrefab(string prefabName) {
		Object obj;

		if (objectList.TryGetValue(prefabName.GetHashCode(), out obj)) {
			return obj as GameObject;
		} else {
			Debug.Log("Could not find Prefab: " + prefabName);
			return null;
		}
	}
}

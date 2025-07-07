using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class FindNestedPrefabs : EditorWindow {
    [MenuItem("Tools/Find Nested Prefabs")]
    public static void ShowWindow() {
        GetWindow<FindNestedPrefabs>("Find Nested Prefabs");
    }

    private GameObject prefabToFind;
    private List<string> containingPrefabs = new List<string>();

    private void OnGUI() {
        GUILayout.Label("Select the Prefab to find:", EditorStyles.boldLabel);
        prefabToFind = (GameObject)EditorGUILayout.ObjectField("Prefab:", prefabToFind, typeof(GameObject), false);

        if (GUILayout.Button("Find")) {
            if (prefabToFind != null) {
                FindPrefabsContainingPrefab();
            }
            else {
                Debug.LogWarning("Please select a prefab.");
            }
        }

        EditorGUILayout.LabelField("Prefabs containing the target:", EditorStyles.boldLabel);

        foreach (string path in containingPrefabs) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(path);
            if (GUILayout.Button("Open", GUILayout.Width(60))) {
                GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                EditorGUIUtility.PingObject(go);
                AssetDatabase.OpenAsset(go);
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    private void FindPrefabsContainingPrefab() {
        containingPrefabs.Clear();
        string[] allPrefabs = AssetDatabase.FindAssets("t:Prefab");

        foreach (string prefab in allPrefabs) {
            string path = AssetDatabase.GUIDToAssetPath(prefab);
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (PrefabUtility.GetPrefabAssetType(go) == PrefabAssetType.Regular) {
                Transform[] allChildren = go.GetComponentsInChildren<Transform>(true);
                foreach (Transform child in allChildren) {
                    if (PrefabUtility.GetPrefabInstanceHandle(child.gameObject) != null && child.name == prefabToFind.name) {
                        containingPrefabs.Add(path);
                        break;
                    }
                }
            }
        }
    }
}

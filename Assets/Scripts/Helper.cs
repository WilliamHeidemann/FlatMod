using UnityEditor;
using UnityEngine;

public class Helper : Editor
{
    [MenuItem("Tools/Select FBX Models")]
    private static void SelectFBXModels()
    {
        string[] guids = AssetDatabase.FindAssets("t:Model", new[] { "Assets/3D Road Tiles" });
        Object[] fbxModels = new Object[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            fbxModels[i] = AssetDatabase.LoadAssetAtPath<Object>(path);
        }

        Selection.objects = fbxModels;
    }
    
    [MenuItem("Tools/Layout Road Tiles")]
    private static void LayoutRoadTiles()
    {
        var selection = Selection.objects;
        Debug.Log(selection.Length);
        
        GameObject[] models = new GameObject[selection.Length];
        for (int i = 0; i < selection.Length; i++)
        {
            models[i] = selection[i] as GameObject;
        }
        
        Debug.Log(models[0].name);
        
        if (models.Length == 0)
        {
            Debug.LogWarning("No models selected.");
            return;
        }
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(models.Length));
        for (int i = 0; i < models.Length; i++)
        {
            int row = i / gridSize;
            int col = i % gridSize;
            Vector3 position = new Vector3(col * 4.0f, 0, row * 4.0f);
            models[i].transform.position = position;
        }
    }
}
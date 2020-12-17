using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor
{
    /// <summary>
    /// Randomly selects a tile to paint. 
    /// <para>Adapted from https://github.com/Unity-Technologies/2d-techdemos.</para>
    /// </summary>
    [CustomGridBrush(false, false, false, "Prefab Brush")]
    public class PrefabBrush : GridBrush
    {
        /// <summary>
        /// Prefab to create.
        /// </summary>
        public GameObject Prefab;

        /// <summary>
        /// Disables flood fill.
        /// </summary>
        /// <param name="gridLayout"></param>
        /// <param name="brushTarget"></param>
        /// <param name="position"></param>
        public override void FloodFill(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            return;
        }

        /// <summary>
        /// Paint spawn tile.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="brushTarget"></param>
        /// <param name="position"></param>
        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            Erase(grid, brushTarget, position);
            Tilemap tilemap = brushTarget.GetComponent<Tilemap>();
            GameObject gameObject = GameObject.Instantiate(Prefab);
            gameObject.transform.SetParent(brushTarget.transform, false);
            gameObject.transform.position = tilemap.CellToWorld(position);

            Undo.RegisterCreatedObjectUndo(gameObject, "Instantiated prefab");
        }

        /// <summary>
        /// Removes instantiated objects at position.
        /// </summary>
        /// <param name="gridLayout"></param>
        /// <param name="brushTarget"></param>
        /// <param name="position"></param>
        public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            Tilemap tilemap = brushTarget.GetComponent<Tilemap>();

            foreach (Transform childTransform in brushTarget.transform)
            {
                if (tilemap.WorldToCell(childTransform.position) == position)
                {
                    //GameObject.DestroyImmediate(childTransform.gameObject);
                    Undo.DestroyObjectImmediate(childTransform.gameObject);
                }
            }
        }

        /// <summary>
        /// Create new random brush asset.
        /// </summary>
        [MenuItem("Assets/Create/Prefab Brush")]
        public static void CreateBrush()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Prefab Brush", "New Prefab Brush", "asset", "Save Prefab Brush", "Assets");

            if (path == "")
                return;

            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<PrefabBrush>(), path);
        }
    }
}
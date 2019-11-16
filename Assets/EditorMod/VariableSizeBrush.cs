using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor.Tilemaps
{
    [CustomGridBrush(true, false, false, "Variable Size Brush")]
    public class VariableSizeBrush : GridBrush
    {
        public Vector3Int snapTo = new Vector3Int(2, 2, 0);
        public Vector3Int snapOffset = new Vector3Int(0, 0, 0);

        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            Vector3Int pos = position - pivot - snapOffset;
            for (int i = 0; i < Mathf.Max(1, size.x); i++)
            {
                for (int j = 0; j < Mathf.Max(1, size.y); j++)
                {
                    Vector3Int drawTo = new Vector3Int(IntFloor(pos.x + i, snapTo.x), IntFloor(pos.y + j, snapTo.y), IntFloor(pos.z, snapTo.z));
                    base.Paint(grid, brushTarget, drawTo + pivot + snapOffset);
                }
            }
        }

        public static int IntFloor(int value, int multiple)
        {
            multiple = Math.Max(1, Math.Abs(multiple));
            return multiple * (int)Math.Floor((float)value / multiple);
        }

        [MenuItem("Assets/Create/Variable Size Brush")]
        public static void CreateBrush()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Line Brush", "New Line Brush", "Asset", "Save Line Brush", "Assets");
            if (path == "")
                return;
            AssetDatabase.CreateAsset(CreateInstance<VariableSizeBrush>(), path);
        }
    }
    [CustomEditor(typeof(VariableSizeBrush))]
    public class VariableSizeBrushEditor : GridBrushEditor
    {
        private VariableSizeBrush customBrush { get { return target as VariableSizeBrush; } }
        public override void OnPaintSceneGUI(GridLayout grid, GameObject brushTarget, BoundsInt position, GridBrushBase.Tool tool, bool executing)
        {
            base.OnPaintSceneGUI(grid, brushTarget, position, tool, executing);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Collisions))]
public class CollisionInspector : Editor {

    private static bool showState = true;
    private static bool baseInspector;

    public override void OnInspectorGUI()
    {
        Collisions col = (Collisions)target;

        GUIStyle boolean = new GUIStyle();
        boolean.margin = new RectOffset(15, 5, 5, 5);
        GUIStyle bgTextArea = EditorStyles.textArea;
        //bgTextArea.margin = new RectOffset(20,50, 5, 5);
        //bgTextArea.normal.background = MakeTex(1, 1, new Color(0,0,0,1.0f));

        EditorGUI.indentLevel = 1;
        EditorGUILayout.Space();
        showState = EditorGUILayout.Foldout(showState, "State", true, EditorStyles.toolbarDropDown);
        if(showState)
        {
            EditorGUILayout.BeginVertical(bgTextArea);

            if(col.isGrounded) boolean.normal.textColor = Color.green;
            else boolean.normal.textColor = Color.red;
            GUILayout.Label("IsGrounded", boolean);

            if(col.isTouchingCeiling) boolean.normal.textColor = Color.green;
            else boolean.normal.textColor = Color.red;
            GUILayout.Label("IsTouchingCeiling", boolean);

            if(col.isTouchingWall) boolean.normal.textColor = Color.green;
            else boolean.normal.textColor = Color.red;
            GUILayout.Label("IsTouchingWall", boolean);

            if(col.isFalling) boolean.normal.textColor = Color.green;
            else boolean.normal.textColor = Color.red;
            GUILayout.Label("IsFalling", boolean);

            if(col.wasGroundedLastFrame) boolean.normal.textColor = Color.green;
            else boolean.normal.textColor = Color.red;
            GUILayout.Label("WasGroundedLastFrame", boolean);

            if(col.wasTouchingCeilingLastFrame) boolean.normal.textColor = Color.green;
            else boolean.normal.textColor = Color.red;
            GUILayout.Label("WasTouchingCeilingLastFrame", boolean);

            if(col.wasTouchingWallLastFrame) boolean.normal.textColor = Color.green;
            else boolean.normal.textColor = Color.red;
            GUILayout.Label("WasTouchingWallLastFrame", boolean);

            if(col.justGotGrounded) boolean.normal.textColor = Color.green;
            else boolean.normal.textColor = Color.red;
            GUILayout.Label("JustGotGrounded", boolean);

            if(col.justNotGrounded) boolean.normal.textColor = Color.green;
            else boolean.normal.textColor = Color.red;
            GUILayout.Label("JustNOTGrounded", boolean);

            if(col.justTouchCeiling) boolean.normal.textColor = Color.green;
            else boolean.normal.textColor = Color.red;
            GUILayout.Label("JustTouchCeiling", boolean);

            if(col.justTouchWall) boolean.normal.textColor = Color.green;
            else boolean.normal.textColor = Color.red;
            GUILayout.Label("JustTouchWall", boolean);
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Space();
        baseInspector = EditorGUILayout.Foldout(baseInspector, "Base Inspector", true, EditorStyles.toolbarDropDown);
        if(baseInspector)  base.OnInspectorGUI();
        EditorGUILayout.Space();
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];

        for(int i = 0; i < pix.Length; i++)
            pix[i] = col;

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();

        return result;
    }
}

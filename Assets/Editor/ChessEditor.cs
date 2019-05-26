using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine.Networking;
using UnityStandardAssets.ImageEffects;

public class ChessEditor : EditorWindow
{
    GameObject blackField;
    GameObject whiteField;

    GameObject easyHexField;
    GameObject mediumHexField;
    GameObject hardHexField;
    
    GameObject resizebleObject;
    
    AnimBool m_ShowExtraFields;

    Transform fieldParent;
    int x1 = 0, x2 = 0, y1 = 0, y2 = 0;
    float xScale = 0, yScale = 0, zScale = 0;
    bool initialized = false;

    void initialization()
    {
        fieldParent = GameObject.Find("Fields").transform;
        initialized = true;
        m_ShowExtraFields = new AnimBool(true);
        m_ShowExtraFields.valueChanged.AddListener(Repaint);
    }

    [MenuItem("Window/Chess Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<ChessEditor>();
    }


    void OnGUI()
    {
        if (!initialized)
            initialization();
        Object tempBlackField = blackField;
        Object tempWhiteField = whiteField;
        tempBlackField = EditorGUILayout.ObjectField("Black field", tempBlackField, typeof(GameObject), GUILayout.MinWidth(15));
        tempWhiteField = EditorGUILayout.ObjectField("White field", tempWhiteField, typeof(GameObject), GUILayout.MinWidth(15));

        blackField = tempBlackField as GameObject;
        whiteField = tempWhiteField as GameObject;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("x1", GUILayout.MinWidth(4));
        x1 = EditorGUILayout.IntField(x1);
        EditorGUILayout.LabelField("y1", GUILayout.MinWidth(4));
        y1 = EditorGUILayout.IntField(y1);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("x2", GUILayout.MinWidth(4));
        x2 = EditorGUILayout.IntField(x2);
        EditorGUILayout.LabelField("y2", GUILayout.MinWidth(4));
        y2 = EditorGUILayout.IntField(y2);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Create field square"))
        {
            createFieldsSquare(x1, y1, x2, y2);    
        }

        Object tempEasyHexField = easyHexField;
        Object tempMediumHexField = mediumHexField;
        Object tempHardHexField = hardHexField;
        tempEasyHexField = EditorGUILayout.ObjectField("easyHexField", tempEasyHexField, typeof(GameObject), GUILayout.MinWidth(15));
        tempMediumHexField = EditorGUILayout.ObjectField("easyHexField", tempMediumHexField, typeof(GameObject), GUILayout.MinWidth(15));
        tempHardHexField = EditorGUILayout.ObjectField("easyHexField", tempHardHexField, typeof(GameObject), GUILayout.MinWidth(15));

        easyHexField = tempEasyHexField as GameObject;
        mediumHexField = tempMediumHexField as GameObject;
        hardHexField = tempHardHexField as GameObject;

        if (GUILayout.Button("Create field Hex"))
        {
            createFieldsHex();
        }

        if (GUILayout.Button("Delete all fields"))
        {
            DeleteAllFields();
        }
        
        Object resizebleObjectTemp = resizebleObject;
        resizebleObjectTemp = EditorGUILayout.ObjectField("Resizable object", resizebleObjectTemp, typeof(GameObject), GUILayout.MinWidth(15));

        resizebleObject = resizebleObjectTemp as GameObject;

        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("x scale", GUILayout.MinWidth(4));
        xScale = EditorGUILayout.FloatField(xScale);
        EditorGUILayout.LabelField("y scale", GUILayout.MinWidth(4));
        yScale = EditorGUILayout.FloatField(yScale);
        EditorGUILayout.LabelField("z scale", GUILayout.MinWidth(4));
        zScale = EditorGUILayout.FloatField(zScale);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Resize"))
        {
            resize(resizebleObject, xScale, yScale, zScale);    
        }
        
        if (GUILayout.Button("Add network id"))
        {
            AddNetworkIdComponent();  
        }
    }

    void createFieldsSquare(int x1, int y1, int x2, int y2)
    {
        for (int i = x1; i <= x2; i++)
        {
            for (int j = y1; j <= y2; j++)
            {
                GameObject field;
                if ((i + j) % 2 == 0)
                    field = Instantiate(whiteField);
                else
                    field = Instantiate(blackField);

                field.transform.position = new Vector3(i, 0, j);
                field.name = "field " + i + " " + j;
                field.transform.parent = fieldParent;
                field.GetComponent<Field>().x = i;
                field.GetComponent<Field>().y = j;
            }
        }
    }

    void createFieldsHex()
    {
        int n = 16;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (i + j < 8)
                    continue;
                if (i + j > 22)
                    continue;
                if (i == 0)
                    continue;
                if (j == 15)
                    continue;

                GameObject field;
                if ((i + j*2) % 3 == 0)
                    field = Instantiate(easyHexField);
                else
                    if ((i + j*2) % 3 == 1)
                        field = Instantiate(mediumHexField);
                else
                    field = Instantiate(hardHexField);

                field.transform.position = new Vector3(0.5f * j + i, 0, Mathf.Sqrt(3) / 2 * j);
                field.name = "field " + i + " " + j;
                field.transform.parent = fieldParent;
                field.GetComponent<Field>().x = i;
                field.GetComponent<Field>().y = j;
            }
        }
    }

    void DeleteAllFields()
    {
        for (int i = 0; i < fieldParent.childCount; i++)
        {
            DestroyImmediate(fieldParent.GetChild(i).gameObject);
        }
    }

    void resize(GameObject obj, float xScale, float yScale, float zScale)
    {
        setScale(obj.transform, xScale,  yScale,  zScale);
    }

    void setScale(Transform t, float xScale, float yScale, float zScale)
    {
        t.localScale = new Vector3(xScale, yScale, zScale);
        for (int i = 0; i < t.GetChildCount(); i++)
        {
            setScale(t.GetChild(i), xScale, yScale, zScale);
        }
    }

    void AddNetworkIdComponent()
    {
        foreach (var obj in  GameObject.FindObjectsOfType<Piece>())
        {
            obj.gameObject.AddComponent<NetworkIdentity>();
        }
        
        foreach (var obj in  GameObject.FindObjectsOfType<Field>())
        {
            obj.gameObject.AddComponent<NetworkIdentity>();
        }
    }
}

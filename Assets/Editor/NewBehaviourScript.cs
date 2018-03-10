using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NewBehaviourScript : ScriptableWizard
{

    public GameObject Target;

    private static MeshFilter mf;
    private static GameObject[] vex;
    private void OnWizardUpdate()
    {
        if (!Target)//若未指定target 不能使用
        {
            isValid = false;
            return;
        }
        mf = Target.GetComponent<MeshFilter>();
        if (!mf)//若target没有meshfilter组件 不能使用
        {
            isValid = false;
            return;
        }
        if (!mf.sharedMesh)//若target没有sharedmesh 不能使用
        {
            isValid = false;
            return;
        }
        isValid = true;

    }

    private void OnWizardCreate()
    {
        GameObject root = new GameObject("P_" + Target.name);
        Mesh mesh = mf.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        Vector3[] normals = mesh.normals;
        vex = new GameObject[vertices.Length];
        GameObject prefab = (GameObject)Resources.Load("Point");
        

        for (int i = 0; i < vertices.Length; ++i)
        {
            //vex[i] = Instantiate(prefab, vertices[i], Quaternion.Euler(normals[i]), root.transform);
            vex[i] = Instantiate(prefab, vertices[i]+Target.transform.position, Quaternion.identity, root.transform);
            vex[i].name = i + "";
            //vex[i].transform.LookAt(vertices[i] + normals[i]);
        }

    }
    [MenuItem("Model/Points")]
    static void ShowPoints()
    {
        ScriptableWizard.DisplayWizard<NewBehaviourScript>("Points", "Show");
    }

    [MenuItem("Model/Apply")]
    static void Apply()
    {
        Vector3[] v = new Vector3[vex.Length];
        for (int i = 0; i < vex.Length; ++i)
        {
            v[i] = vex[i].transform.position;
        }
        mf.sharedMesh.vertices = v;
    }

    [MenuItem("Model/Extrude")]
    static void Extrude()
    {
        Vector3[] v = new Vector3[vex.Length];
        for (int i = 0; i < vex.Length; ++i)
        {
            v[i] = vex[i].transform.position + vex[i].transform.up;
        }
        mf.sharedMesh.vertices = v;
    }

}

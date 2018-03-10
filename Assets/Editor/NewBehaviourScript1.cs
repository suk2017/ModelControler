using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NewBehaviourScript1 : ScriptableWizard
{

    public Transform target;
    public Cubemap cup;
    private void OnWizardCreate()
    {
        GameObject g = new GameObject("CubemapCamera", typeof(Camera));
        g.transform.position = target.position;
        g.GetComponent<Camera>().RenderToCubemap(cup);
        DestroyImmediate(g);
    }
    private void OnWizardUpdate()
    {
        if (!target)
        {
            isValid = false;
            return;
        }
        if (!cup)
        {
            isValid = false;
            return;
        }
        isValid = true;
    }
    [MenuItem("Cubemap/RenderToCubemap")]
    static void RenderToCubemap()
    {
        DisplayWizard("RenderToCubemap", typeof(NewBehaviourScript1));
    }
}

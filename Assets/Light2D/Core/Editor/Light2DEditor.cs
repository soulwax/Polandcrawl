using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Light2DEmitter))]
public class Light2DEditor : Editor 
{
    private Light2DEmitter em;
    private GenericMenu menu = new GenericMenu();
    private static List<Camera> cameras = new List<Camera>();

    void OnEnable()
    {
        em = (Light2DEmitter)target;
        em.PreviewThis = true;
    }
    void OnDisable()
    {
        em.PreviewThis = false;
    }

    public override void OnInspectorGUI()
    {
        GetCameras();

        EditorGUILayout.Separator();

        em.lightType = (Light2DEmitter.Light2DType)EditorGUILayout.EnumPopup("Light Type", em.lightType);
        em.enableEvents = EditorGUILayout.Toggle("Enable Events", em.enableEvents);
        em.eventPassedFilter = EditorGUILayout.TextField("Event Filter", em.eventPassedFilter);

        EditorGUILayout.Separator();

        em.isStatic = EditorGUILayout.Toggle("Is Static", em.isStatic);
        em.lightDetail = EditorGUILayout.IntField("Detail", em.lightDetail);
        em.lightSize = EditorGUILayout.FloatField("Size", em.lightSize);
        em.coneAngle = EditorGUILayout.FloatField("Cone", em.coneAngle);

        EditorGUILayout.Separator();

        em.lightMaterial = (Material)EditorGUILayout.ObjectField("Material", em.lightMaterial, typeof(Material), true);
        em.lightColor = EditorGUILayout.ColorField("Color", em.lightColor);

        if (em.lightType == Light2DEmitter.Light2DType.Radial)
        {
            em.uvMapping = (Light2DEmitter.UVMapping)EditorGUILayout.EnumPopup("UV Map Type", em.uvMapping);
            
            if(em.uvMapping == Light2DEmitter.UVMapping.Planer)
                em.uvVariable = EditorGUILayout.FloatField("UV Rotation", em.uvVariable);
            else
                em.uvVariable = EditorGUILayout.FloatField("UV Start", em.uvVariable);
        }

        EditorGUILayout.Separator();

        if (GUILayout.Button(new GUIContent("Used On (" + em.assignedRenderers.Count + ") Camera(s)", "Shows a list of available cameras to add as renderers for this object.")))
        {
            menu.ShowAsContext();
        }

        LayerMaskDropdown("Shadow Mask List", em.shadowMask);

        Light2DEmitter.PreviewAll = EditorGUILayout.Toggle("Preview All", Light2DEmitter.PreviewAll);

        SceneView.RepaintAll();
    }

    void LayerMaskDropdown(string label, LayerMask selected)
    {
        if(GUILayout.Button(new GUIContent(label)))
        {
            GenericMenu menu = new GenericMenu();

            menu.AddItem(new GUIContent("Nothing"), (selected.value == 0) ? true : false, SelectLayer, new object[] { selected, 0 });
            menu.AddItem(new GUIContent("Everything"), (selected.value == -1) ? true : false, SelectLayer, new object[] { selected, -1 });

            string layerName = "";
            for (int i = 0; i < 32; i++)
            {
                layerName = LayerMask.LayerToName(i);

                if (layerName != "")
                {
                    menu.AddItem(new GUIContent(layerName), selected == (selected | (1 << i)), SelectLayer, new object[] { selected, i });
                }
            }

            menu.ShowAsContext();
        }
    }

    void SelectLayer(object o)
    {
        object[] oA = (object[])o;
        LayerMask selected = (LayerMask)oA[0];

        if ((int)oA[1] == 0)
        {
            selected = 0;
        }
        else if ((int)oA[1] == -1)
        {
            selected = -1;
        }
        else
        {
            if (selected == (selected | (1 << (int)oA[1])))
            {
                selected &= ~(1 << (int)oA[1]);
            }
            else
            {
                selected = selected | (1 << (int)oA[1]);
            }
        }

        em.shadowMask = selected;
    }

    void GetCameras()
    {
        Object[] c = GameObject.FindObjectsOfType(typeof(Camera));

        em.assignedRenderers.TrimExcess();
        //em.assignedRenderers.ForEach(i => { if (i == null) em.assignedRenderers.Remove(i); });
        cameras.Clear();
        foreach (Object o in c)
        {
            cameras.Add((Camera)o);
        }

        menu = new GenericMenu();
        for (int i = 0; i < cameras.Count; i++)
        {
            bool containsCam = ContainsCamera(cameras[i]);
            menu.AddItem(new GUIContent(cameras[i].name), containsCam, AddCamera, new object[] { cameras[i], containsCam });
        }
    }
    void AddCamera(object o)
    {
        object[] conv = (object[])o;

        if (System.Convert.ToBoolean(conv[1]) == false)
            em.assignedRenderers.Add(((Camera)conv[0]).gameObject.GetComponent<Light2DRenderer>() ? ((Camera)conv[0]).gameObject.GetComponent<Light2DRenderer>() : ((Camera)conv[0]).gameObject.AddComponent<Light2DRenderer>());
        else
            em.assignedRenderers.Remove(((Camera)conv[0]).gameObject.GetComponent<Light2DRenderer>());

        GetCameras();
        SceneView.RepaintAll();
    }
    bool ContainsCamera(Camera cam)
    {
        for (int j = 0; j < em.assignedRenderers.Count; j++)
        {
            if (em.assignedRenderers[j].GetComponent<Camera>() == cam)
                return true;
        }
        return false;
    }
}

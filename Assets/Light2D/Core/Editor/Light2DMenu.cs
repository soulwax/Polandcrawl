using UnityEngine;
using UnityEditor;
using System.Collections;

public class Light2DMenu : MonoBehaviour 
{
    [MenuItem("GameObject/Create Other/2D Lights/Directional Light")]
    static void Add2DDirectionalLight()
    {
        GameObject go = new GameObject("Directional 2D Light");
        Light2DEmitter emitter = go.AddComponent<Light2DEmitter>();

        go.transform.Rotate(0, 0, 45);
        emitter.assignedRenderers.Add((Camera.main.gameObject.GetComponent<Light2DRenderer>() != null) ? Camera.main.gameObject.GetComponent<Light2DRenderer>() : Camera.main.gameObject.AddComponent<Light2DRenderer>());
        emitter.lightType = Light2DEmitter.Light2DType.Directional;
        emitter.lightSize = 10;
        emitter.lightDetail = 1000;
        emitter.uvMapping = Light2DEmitter.UVMapping.Planer;
        emitter.lightMaterial = (Material)Resources.Load("GradientMaterial", typeof(Material));

        Selection.activeObject = go;
    }

    [MenuItem("GameObject/Create Other/2D Lights/Radial Light")]
    static void Add2DRadialLight()
    {
        GameObject go = new GameObject("Radial 2D Light");
        Light2DEmitter emitter = go.AddComponent<Light2DEmitter>();

        emitter.assignedRenderers.Add((Camera.main.gameObject.GetComponent<Light2DRenderer>() != null) ? Camera.main.gameObject.GetComponent<Light2DRenderer>() : Camera.main.gameObject.AddComponent<Light2DRenderer>());
        emitter.lightType = Light2DEmitter.Light2DType.Radial;
        emitter.lightSize = 10;
        emitter.uvMapping = Light2DEmitter.UVMapping.Planer;
        emitter.lightMaterial = (Material)Resources.Load("PlanerMaterial", typeof(Material));

        Selection.activeObject = go;
    }

    [MenuItem("GameObject/Create Other/2D Lights/Spot Light")]
    static void Add2DSpotLight()
    {
        GameObject go = new GameObject("Spot 2D Light");
        Light2DEmitter emitter = go.AddComponent<Light2DEmitter>();

        go.transform.Rotate(0, 0, 195);

        emitter.assignedRenderers.Add((Camera.main.gameObject.GetComponent<Light2DRenderer>() != null) ? Camera.main.gameObject.GetComponent<Light2DRenderer>() : Camera.main.gameObject.AddComponent<Light2DRenderer>());
        emitter.lightType = Light2DEmitter.Light2DType.Radial;
        emitter.lightSize = 10;
        emitter.coneAngle = 30;
        emitter.lightDetail = 50;

        emitter.uvMapping = Light2DEmitter.UVMapping.Planer;
        emitter.lightMaterial = (Material)Resources.Load("PlanerMaterial", typeof(Material));

        Selection.activeObject = go;
    }
}

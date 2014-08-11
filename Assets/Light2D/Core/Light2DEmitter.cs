using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public delegate void Light2DEvent(GameObject obj, Light2DEmitter hitObject);

[ExecuteInEditMode()]
public class Light2DEmitter : MonoBehaviour 
{
    public static event Light2DEvent OnBeamStay = null;
    public static event Light2DEvent OnBeamEnter = null;
    public static event Light2DEvent OnBeamExit = null;

    public static bool PreviewAll = false;
    public bool PreviewThis = false;

    public enum Light2DType
    {
        Radial,
        Directional
    }

    public enum UVMapping
    {
        Planer,
        Radial
    }

    public bool isStatic = false;
    public bool enableEvents = false;
    public List<Light2DRenderer> assignedRenderers = new List<Light2DRenderer>();
    public Light2DType lightType = Light2DType.Radial;
    public int lightDetail = 200;
    public float lightSize = 1;
    public float coneAngle = 360;
    public Material lightMaterial = null;
    public Color lightColor = new Color(1, 1, 1, 0.4f);
    public UVMapping uvMapping = UVMapping.Radial;
    public float uvVariable = 0;
    public LayerMask shadowMask = -1;
    public string eventPassedFilter = "";

    private Light2DMesh mesh = new Light2DMesh();
    private RaycastHit rh;
    private Vector3[] v3;
    private Vector2[] v2;

    void OnDrawGizmosSelected()
    {
        PreviewThis = true;

        Gizmos.color = new Color(lightColor.r, lightColor.g, lightColor.b, 0.75f);

        if (lightType == Light2DType.Directional)
        {
            Gizmos.color = Color.green;
            DrawGizmoArrow(transform.TransformPoint(new Vector3((-lightSize / 2) + ((lightSize / 2)), 0, 0)), transform);

            Gizmos.color = new Color(lightColor.r, lightColor.g, lightColor.b, 1.0f);
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(-lightSize / 2, lightSize / 2, 0)), transform.TransformPoint(new Vector3(lightSize / 2, lightSize / 2, 0)));
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(lightSize / 2, lightSize / 2, 0)), transform.TransformPoint(new Vector3(lightSize / 2, -lightSize / 2, 0)));
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(lightSize / 2, -lightSize / 2, 0)), transform.TransformPoint(new Vector3(-lightSize / 2, -lightSize / 2, 0)));
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(-lightSize / 2, -lightSize / 2, 0)), transform.TransformPoint(new Vector3(-lightSize / 2, lightSize / 2, 0)));
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(-lightSize / 2, -lightSize / 2, 0)), transform.TransformPoint(new Vector3(lightSize / 2, lightSize / 2, 0)));
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(-lightSize / 2, lightSize / 2, 0)), transform.TransformPoint(new Vector3(lightSize / 2, -lightSize / 2, 0)));
        }
        else
        {
            Vector3 curVec = Vector3.zero;
            Vector3 lastVec = Vector3.zero;
            for (int i = 0; i < 25 + 1; i++)
            {
                if (i == 0 || i == 25 && coneAngle < 360)
                    Gizmos.color = Color.black;
                else
                    Gizmos.color = new Color(lightColor.r, lightColor.g, lightColor.b, 0.75f);

                curVec = transform.TransformPoint(new Vector3(Mathf.Sin((coneAngle / 25) * Mathf.Deg2Rad * i) * lightSize / 2, Mathf.Cos((coneAngle / 25) * Mathf.Deg2Rad * i) * lightSize / 2, 0));

                if (i > 0)
                    Gizmos.DrawLine(curVec, lastVec);

                lastVec = curVec;
            }
        }
    }
    void OnDrawGizmos()
    {
        CheckSafeValue();

        Gizmos.color = new Color(lightColor.r, lightColor.g, lightColor.b, 0.3f);

        if (lightType == Light2DType.Directional)
        {
            Gizmos.color = new Color(lightColor.r, lightColor.g, lightColor.b, 0.3f);
            DrawGizmoArrow(transform.TransformPoint(new Vector3((-lightSize / 2) + ((lightSize / 2)), 0, 0)), transform);
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(-lightSize / 2, lightSize / 2, 0)), transform.TransformPoint(new Vector3(lightSize / 2, lightSize / 2, 0)));
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(lightSize / 2, lightSize / 2, 0)), transform.TransformPoint(new Vector3(lightSize / 2, -lightSize / 2, 0)));
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(lightSize / 2, -lightSize / 2, 0)), transform.TransformPoint(new Vector3(-lightSize / 2, -lightSize / 2, 0)));
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(-lightSize / 2, -lightSize / 2, 0)), transform.TransformPoint(new Vector3(-lightSize / 2, lightSize / 2, 0)));
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(-lightSize / 2, -lightSize / 2, 0)), transform.TransformPoint(new Vector3(lightSize / 2, lightSize / 2, 0)));
            Gizmos.DrawLine(transform.TransformPoint(new Vector3(-lightSize / 2, lightSize / 2, 0)), transform.TransformPoint(new Vector3(lightSize / 2, -lightSize / 2, 0)));
        }
        else
        {
            Vector3 curVec = Vector3.zero;
            Vector3 lastVec = Vector3.zero;
            for (int i = 0; i < 25 + 1; i++)
            {
                if (i == 0 || i == 25 && coneAngle < 360)
                    Gizmos.color = Color.black;
                else
                    Gizmos.color = new Color(lightColor.r, lightColor.g, lightColor.b, 0.3f);

                curVec = transform.TransformPoint(new Vector3(Mathf.Sin((coneAngle / 25) * Mathf.Deg2Rad * i) * lightSize / 2, Mathf.Cos((coneAngle / 25) * Mathf.Deg2Rad * i) * lightSize / 2, 0));
                Gizmos.DrawLine(transform.position, curVec);
                
                if(i>0)
                    Gizmos.DrawLine(curVec, lastVec);
                
                lastVec = curVec;
            }
        }
    }
    void DrawGizmoArrow(Vector3 pos, Transform direction)
    {
        Gizmos.DrawLine(pos, pos - direction.up * 2);
        Gizmos.DrawLine(pos - direction.up * 2, pos - direction.up + direction.right * 0.5f);
        Gizmos.DrawLine(pos - direction.up * 2, pos - direction.up - direction.right * 0.5f);
    }

    void Start()
    {
        if (assignedRenderers.Count == 0)
            return;

        CheckSafeValue();

        if(lightType == Light2DType.Radial)
            GenerateRadialMesh();
        else
            GenerateDirectionalMesh();
    }

    // Update is called once per frame
	void Update () 
    {
        if (!Application.isPlaying && !PreviewAll)
            if(!Application.isPlaying && !PreviewThis)
                return;

        if (assignedRenderers.Count == 0)
            return;

        CheckSafeValue();

        if (!isStatic)
        {
            if (lightType == Light2DType.Radial)
                GenerateRadialMesh();
            else
                GenerateDirectionalMesh();
        }
        else
        {
            RegenerateMesh();
        }

        PreviewThis = false;
	}

    void GenerateDirectionalMesh()
    {
        float secDis = (lightSize / lightDetail); ;
        Vector3 sPos = new Vector3(-lightSize / 2, lightSize / 2, 0);

        mesh.Clear();

        for (int i = 1; i < lightDetail+1; i++)
        {
            // == Triangle 1 =======================================
            v3 = new Vector3[3];

            v3[0] = transform.TransformPoint(sPos + new Vector3((i-1) * secDis, 0, 0));

            if (Physics.Raycast(transform.TransformPoint(sPos + new Vector3((i - 1) * secDis, 0, 0)), -transform.up, out rh, lightSize, shadowMask))
                v3[1] = rh.point;
            else
                v3[1] = transform.TransformPoint(sPos + new Vector3((i - 1) * secDis, -lightSize, 0));

            if (Physics.Raycast(transform.TransformPoint(sPos + new Vector3(i * secDis, 0, 0)), -transform.up, out rh, lightSize, shadowMask))
                v3[2] = rh.point;
            else
                v3[2] = transform.TransformPoint(sPos + new Vector3(i * secDis, -lightSize, 0));

            mesh.AddTriangle(v3, PlanerMap(v3, 90), lightColor);

            // == Triangle 2 =======================================
            v3 = new Vector3[3];
            v2 = new Vector2[3];

            v3[0] = transform.TransformPoint(sPos + new Vector3((i) * secDis, 0, 0));
            v3[1] = transform.TransformPoint(sPos + new Vector3((i - 1) * secDis, 0, 0));

            if (Physics.Raycast(transform.TransformPoint(sPos + new Vector3(i * secDis, 0, 0)), -transform.up, out rh, lightSize, shadowMask))
                v3[2] = rh.point;
            else
                v3[2] = transform.TransformPoint(sPos + new Vector3((i) * secDis, -lightSize, 0));

            mesh.AddTriangle(v3, PlanerMap(v3, 90), lightColor);
        }

        foreach (Light2DRenderer lr in assignedRenderers)
            mesh.SubmitForRender(lr, lightMaterial);
    }

    private List<GameObject> detectorList = new List<GameObject>();
    private List<GameObject> detectedList = new List<GameObject>();
    private List<GameObject> stayList = new List<GameObject>();
    void GenerateRadialMesh()
    {
        float ang1Sin, ang1Cos;
        float ang2Sin, ang2Cos;

        if (enableEvents)
        {
            for (int i = 0; i < detectorList.Count; i++)
            {
                if (!detectedList.Contains(detectorList[i]))
                {
                    if(OnBeamExit!=null)
                        OnBeamExit(detectorList[i], this);
                    
                    detectorList.RemoveAt(i);
                    //return;
                }
            }

            detectorList.TrimExcess();

            stayList.Clear();
            detectedList.Clear();
        }

        mesh.Clear();
        for (int i = 1; i < lightDetail + 1; i++)
        {
            v3 = new Vector3[3];

            ang1Sin = Mathf.Sin((coneAngle / lightDetail) * Mathf.Deg2Rad * (i));
            ang1Cos = Mathf.Cos((coneAngle / lightDetail) * Mathf.Deg2Rad * (i));
            ang2Sin = Mathf.Sin((coneAngle / lightDetail) * Mathf.Deg2Rad * (i - 1));
            ang2Cos = Mathf.Cos((coneAngle / lightDetail) * Mathf.Deg2Rad * (i - 1));

            bool prevDetected = false;

            // p1
            if (Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(ang1Sin * (lightSize / 2), ang1Cos * (lightSize / 2), 0)), out rh, (this.lightSize / 2), shadowMask))
            {
                if (enableEvents)
                {
                    if (!detectedList.Contains(rh.collider.gameObject))
                        detectedList.Add(rh.collider.gameObject);

                    if (!detectorList.Contains(rh.collider.gameObject))
                    {
                        detectorList.Add(rh.collider.gameObject);

                        if (OnBeamEnter != null)
                            OnBeamEnter(rh.collider.gameObject, this);
                    }
                    else
                    {
                        if (!stayList.Contains(rh.collider.gameObject))
                        {
                            stayList.Add(rh.collider.gameObject);

                            if (OnBeamStay != null)
                                OnBeamStay(rh.collider.gameObject, this);
                        }

                        prevDetected = true;
                    }
                }

                v3[0] = transform.TransformPoint(new Vector3(ang1Sin * (rh.distance), ang1Cos * (rh.distance), 0));
            }
            else
                v3[0] = transform.TransformPoint(new Vector3(ang1Sin * (lightSize / 2), ang1Cos * (lightSize / 2), 0));

            // p2
            if (Physics.Raycast(transform.position, transform.TransformDirection(new Vector3(ang2Sin * (lightSize / 2), ang2Cos * (lightSize / 2), 0)), out rh, (this.lightSize / 2), shadowMask))
            {
                if (enableEvents)
                {
                    if (!detectedList.Contains(rh.collider.gameObject))
                        detectedList.Add(rh.collider.gameObject);

                    if (!detectorList.Contains(rh.collider.gameObject))
                    {
                        detectorList.Add(rh.collider.gameObject);

                        if (OnBeamEnter != null)
                            OnBeamEnter(rh.collider.gameObject, this);
                    }
                    else
                    {
                        if (!prevDetected)
                        {
                            if (!stayList.Contains(rh.collider.gameObject))
                            {
                                stayList.Add(rh.collider.gameObject);

                                if (OnBeamStay != null)
                                    OnBeamStay(rh.collider.gameObject, this);
                            }
                        }
                    }
                }

                v3[1] = transform.TransformPoint(new Vector3(ang2Sin * (rh.distance), ang2Cos * (rh.distance), 0));
            }
            else
                v3[1] = transform.TransformPoint(new Vector3(ang2Sin * (lightSize / 2), ang2Cos * (lightSize / 2), 0));

            // p3 (center)
            v3[2] = transform.TransformPoint(Vector3.zero);

            mesh.AddTriangle(v3, (uvMapping == UVMapping.Radial) ? RadialMap(v3, uvVariable) : PlanerMap(v3, uvVariable), lightColor);
        }

        foreach (Light2DRenderer lr in assignedRenderers)
            mesh.SubmitForRender(lr, lightMaterial);
    }

    void RegenerateMesh()
    {
        for (int i = 0; i < mesh.triangles.Count; i++)
        {
            mesh.UpdateTriangleUV(i, (uvMapping == UVMapping.Radial) ? RadialMap(mesh.triangles[i].points, uvVariable) : PlanerMap(mesh.triangles[i].points, uvVariable), lightColor);
        }

        foreach (Light2DRenderer lr in assignedRenderers)
            mesh.SubmitForRender(lr, lightMaterial);
    }

    Vector3[] TransformTriangle(Vector3[] triangles, Transform trans)
    {
        v3 = new Vector3[triangles.Length];
        for(int i = 0; i < triangles.Length; i++)
            v3[i] = trans.TransformPoint(triangles[i]);

        return v3;
    }

    Vector2[] RadialMap(Vector3[] verts, float rotation)
    {
        v2 = new Vector2[verts.Length];

        for (int i = 0; i < verts.Length; i+=3)
        {
            v2[i + 0] = new Vector2(rotation + (Vector3.Distance(verts[i + 0], transform.position) / (lightSize/2)), 0);
            v2[i + 1] = new Vector2(rotation + (Vector3.Distance(verts[i + 1], transform.position) / (lightSize / 2)), 0);

            v2[i + 2] = new Vector2(rotation + 0, 0);
        }
        
        return v2;
    }

    Vector2[] PlanerMap(Vector3[] verts, float rotation)
    {
        v2 = new Vector2[verts.Length];

        for (int i = 0; i < verts.Length; i++)
        {
            Vector2 t = new Vector2((-transform.position.x / lightSize) + (verts[i].x / lightSize), (-transform.position.y / lightSize) + (verts[i].y / lightSize));

            v2[i] = new Vector2(0.5f + (t.x * Mathf.Cos((-transform.eulerAngles.z + rotation) * Mathf.Deg2Rad) - t.y * Mathf.Sin((-transform.eulerAngles.z + rotation) * Mathf.Deg2Rad)),
                                0.5f + (t.x * Mathf.Sin((-transform.eulerAngles.z + rotation) * Mathf.Deg2Rad) + t.y * Mathf.Cos((-transform.eulerAngles.z + rotation) * Mathf.Deg2Rad)));
        }

        return v2;
    }

    void CheckSafeValue()
    {
        if (transform.parent != null)
            transform.localScale = new Vector3(1 / transform.root.localScale.x, 1 / transform.root.localScale.y, 1 / transform.root.localScale.z);
        else
            transform.localScale = Vector3.one;

        coneAngle = Mathf.Clamp(coneAngle, 0.1f, 360);
        lightSize = Mathf.Clamp(lightSize, 0.001f, Mathf.Infinity);
        lightDetail = Mathf.Clamp(lightDetail, 4, 50000);

        if (uvMapping == UVMapping.Planer)
        {
            if (uvVariable > 360)
                uvVariable = 0;

            if (uvVariable < 0)
                uvVariable = 360;
        }
        else
        {
            if (uvVariable > 1)
                uvVariable = 0;
            if (uvVariable < 0)
                uvVariable = 1;
        }
    }
}
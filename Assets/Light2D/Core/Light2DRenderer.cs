using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public struct Light2DTriangle
{
    public Vector3[] points;
    public Vector2[] uvs;
    public Color color;
}

public class Light2DMesh
{
    public List<Light2DTriangle> triangles = new List<Light2DTriangle>();
    public Material material = null;

    public void SubmitForRender(Light2DRenderer light2DRenderer, Material material)
    {
        this.material = material;
        light2DRenderer.SubmitMesh(this);
    }

    public void ForceMeshOffRenderQue(Light2DRenderer light2DRenderer)
    {
        light2DRenderer.ForceClearMeshList();
    }

    public void AddTriangle(Vector3[] verts, Vector2[] uvs, Color color)
    {
        triangles.Add(new Light2DTriangle { points = verts, uvs = uvs, color = color });
    }

    public void UpdateTriangleUV(int index, Vector2[] uvs, Color color)
    {
        triangles[index] = new Light2DTriangle() { points = triangles[index].points, uvs = uvs, color = color };
    }

    public void Clear()
    {
        this.material = null;
        this.triangles = new List<Light2DTriangle>();
    }
}

[ExecuteInEditMode()]
[RequireComponent(typeof(Camera))]
public class Light2DRenderer : MonoBehaviour 
{
    private List<Light2DMesh> lightMeshs = new List<Light2DMesh>();

    void OnDrawGizmos()
    {
        //if(!Application.isPlaying)
        //    ForceRender();
    }

    public void SubmitMesh(Light2DMesh mesh)
    {
        lightMeshs.Add(mesh);
    }

    public void ForceClearMeshList()
    {
        lightMeshs.Clear();
        lightMeshs.TrimExcess();
    }

    void OnPostRender()
    {
        Render();
    }

    void CheckForNullMaterial(ref Material material)
    {
        if(material == null)
        {
            material = new Material("Shader \"Lines/Colored Blended\" {" +
                "SubShader { Pass { " +
                "    Blend SrcAlpha OneMinusSrcAlpha " +
                "    ZWrite Off Cull Off Fog { Mode Off } " +
                "    BindChannels {" +
                "      Bind \"vertex\", vertex Bind \"color\", color }" +
                "} } }");
            material.hideFlags = HideFlags.HideAndDontSave;
            material.shader.hideFlags = HideFlags.HideAndDontSave;
        }
    }

    public void ForceRender()
    {
        Render();
    }

    void Render()
    {
        foreach (Light2DMesh m in lightMeshs)
        {
            GL.PushMatrix();

            CheckForNullMaterial(ref m.material);
            m.material.SetPass(0);

            GL.Begin(GL.TRIANGLES);
            foreach (Light2DTriangle t in m.triangles)
            {
                GL.Color(t.color);

                GL.TexCoord2(t.uvs[0].x, t.uvs[0].y);
                GL.Vertex3(t.points[0].x, t.points[0].y, t.points[0].z);

                GL.TexCoord2(t.uvs[1].x, t.uvs[1].y);
                GL.Vertex3(t.points[1].x, t.points[1].y, t.points[1].z);

                GL.TexCoord2(t.uvs[2].x, t.uvs[2].y);
                GL.Vertex3(t.points[2].x, t.points[2].y, t.points[2].z);
            }
            GL.End();

            GL.PopMatrix();
        }

        lightMeshs.Clear();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OctasphereCreator {

    private static Vector3[] directions =
    {
        Vector3.left,
        Vector3.back,
        Vector3.right,
        Vector3.forward
    };



	public static Mesh Create(int subdivisions, float radius)
    {
        if(subdivisions < 0)
        {
            subdivisions = 0;
            Debug.LogWarning("Octasphere subdivisions was below minimum of 0, increased to 0");
        }
        else if (subdivisions > 6)
        {
            subdivisions = 6;
            Debug.LogWarning("Octasphere subdivisions was above maximum of 6, decreased to 6");
        }

        int resolution = 1 << subdivisions;
        Vector3[] verts = new Vector3[(resolution + 1) * (resolution + 1) * 4 - (resolution * 2 - 1) * 3];
        int[] tris = new int[(1 << (subdivisions * 2 + 3)) * 3];
        CreateOctasphere(verts, tris, resolution);

        Vector3[] norms = new Vector3[verts.Length];
        Normalize(verts, norms);

        Vector2[] uv = new Vector2[verts.Length];
        CreateUV(verts, uv);

        Vector4[] tangents = new Vector4[verts.Length];
        CreateTangents(verts, tangents);

        if (radius != 1f)
        {
            for(int i = 0; i < verts.Length; ++i)
            {
                verts[i] *= radius;
            }
        }

        Mesh mesh = new Mesh();
        mesh.name = "Octasphere";
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.normals = norms;
        mesh.uv = uv;
        mesh.tangents = tangents;
        return mesh;
    }

    private static void CreateOctasphere(Vector3[] verts, int[] tris, int resolution)
    {
        int v = 0, vBottom = 0, t = 0;

        for(int i = 0; i < 4; ++i)
        {
            verts[v++] = Vector3.down;
        }

        for(int i = 1; i <= resolution; ++i)
        {
            float progress = (float)i / resolution;
            Vector3 from, to;
            verts[v++] = to = Vector3.Lerp(Vector3.down, Vector3.forward, progress);

            for(int d = 0; d < 4; ++d)
            {
                from = to;
                to = Vector3.Lerp(Vector3.down, directions[d], progress);
                t = CreateLowerStrip(i, v, vBottom, t, tris);
                v = CreateVertexLine(from, to, i, v, verts);
                vBottom += i > 1 ? (i - 1) : 1;
            }

            vBottom = v - 1 - i * 4;
        }

        for (int i = resolution - 1; i >= 1; --i)
        {
            float progress = (float)i / resolution;
            Vector3 from, to;
            verts[v++] = to = Vector3.Lerp(Vector3.up, Vector3.forward, progress);

            for (int d = 0; d < 4; ++d)
            {
                from = to;
                to = Vector3.Lerp(Vector3.up, directions[d], progress);
                t = CreateUpperStrip(i, v, vBottom, t, tris);
                v = CreateVertexLine(from, to, i, v, verts);
                vBottom += i + 1;
            }

            vBottom = v - 1 - i * 4;
        }

        for (int i = 0; i < 4; ++i)
        {
            tris[t++] = vBottom;
            tris[t++] = v;
            tris[t++] = ++vBottom;
            verts[v++] = Vector3.up;
        }
    }

    private static void CreateTangents(Vector3[] verts, Vector4[] tangents)
    {
        for(int i = 0; i < verts.Length; ++i)
        {
            Vector3 v = verts[i];
            v.y = 0f;
            v = v.normalized;
            Vector4 tangent;
            tangent.x = -v.z;
            tangent.y = 0f;
            tangent.z = v.x;
            tangent.w = -1f;
            tangents[i] = tangent;
        }

        tangents[verts.Length - 4] = tangents[0] = new Vector3(-1f, 0f, -1f).normalized;
        tangents[verts.Length - 3] = tangents[1] = new Vector3(1f, 0f, -1f).normalized;
        tangents[verts.Length - 2] = tangents[2] = new Vector3(1f, 0f, 1f).normalized;
        tangents[verts.Length - 1] = tangents[3] = new Vector3(-1f, 0f, 1f).normalized;

        for(int i = 0; i < 4; ++i)
        {
            tangents[verts.Length - 1 - i].w = tangents[i].w = -1f;
        }
    }

    private static int CreateUpperStrip(int steps, int vTop, int vBottom, int t, int[] tris)
    {
        tris[t++] = vBottom;
        tris[t++] = vTop - 1;
        tris[t++] = ++vBottom;

        for (int i = 1; i <= steps; ++i)
        {
            tris[t++] = vTop - 1;
            tris[t++] = vTop;
            tris[t++] = vBottom;

            tris[t++] = vBottom;
            tris[t++] = vTop++;
            tris[t++] = ++vBottom;
        }

        return t;
    }

    private static int CreateLowerStrip(int steps, int vTop, int vBottom, int t, int[] tris)
    {
        for(int i = 1; i < steps; ++i)
        {
            tris[t++] = vBottom;
            tris[t++] = vTop - 1;
            tris[t++] = vTop;

            tris[t++] = vBottom++;
            tris[t++] = vTop++;
            tris[t++] = vBottom;
        }

        tris[t++] = vBottom;
        tris[t++] = vTop - 1;
        tris[t++] = vTop;
        return t;
    }

    private static int CreateVertexLine(Vector3 from, Vector3 to, int steps, int v, Vector3[] verts)
    {
        for(int i = 1; i <= steps; ++i)
        {
            verts[v++] = Vector3.Lerp(from, to, (float)i / steps);
        }
        return v;
    }

    private static void Normalize(Vector3[] vertices, Vector3[] normals)
    {
        for(int i = 0; i < vertices.Length; ++i)
        {
            normals[i] = vertices[i] = vertices[i].normalized;
        }
    }

    private static void CreateUV(Vector3[] verts, Vector2[] uv)
    {
        float previousX = 1f;

        for(int i = 0; i < verts.Length; ++i)
        {
            Vector3 v = verts[i];
            if(v.x == previousX)
            {
                uv[i - 1].x = 1f;
            }
            previousX = v.x;

            Vector2 texCoord;
            texCoord.x = Mathf.Atan2(v.x, v.z) / (-2f * Mathf.PI);
            if (texCoord.x < 0f)
                texCoord.x += 1f;
            texCoord.y = Mathf.Asin(v.y) / Mathf.PI + 0.5f;

            uv[i] = texCoord;
        }

        uv[verts.Length - 4].x = uv[0].x = 0.125f;
        uv[verts.Length - 3].x = uv[1].x = 0.375f;
        uv[verts.Length - 2].x = uv[2].x = 0.625f;
        uv[verts.Length - 1].x = uv[3].x = 0.875f;
    }

}

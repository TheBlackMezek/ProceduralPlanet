using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OctasphereCreator {

	public static Mesh Create(int subdivisions, float radius)
    {
        Vector3[] verts =
        {
            Vector3.down,
            Vector3.forward,
            Vector3.left,
            Vector3.back,
            Vector3.right,
            Vector3.up
        };

        int[] tris =
        {
            0, 1, 2,
            0, 2, 3,
            0, 3, 4,
            0, 4, 1,

            5, 2, 1,
            5, 3, 2,
            5, 4, 3,
            5, 1, 4
        };

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
        return mesh;
    }

}

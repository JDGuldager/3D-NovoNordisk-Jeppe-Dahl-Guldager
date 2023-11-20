using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace GradientTerrain
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]

    public class TerrainPlaneGenerator : MonoBehaviour
    {
        // This line declares a public field named material of type Material. This field will store the material that will be applied to the terrain.
        public Material material;
        // This line declares a public field named side of type int. This field will store the size of the terrain in terms of the number of vertices on each side.
        public int side = 100;
        // This field will store the mesh that will be used to represent the terrain.
        Mesh mesh;

        // This field will store an array of vertices that will be used to define the shape of the terrain.
        Vector3[] vertices;
        // This field will store an array of UV coordinates that will be used to map textures onto the terrain.
        Vector2[] uvCoords;
        // This field will store an array of triangles that will be used to define the faces of the terrain.
        int[] triangles;

        void Start()
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;

            // Checks if the material field is not null.
            if (material)
            // Assigns the material to the MeshRenderer component of the GameObject that this script is attached to.
            // The MeshRenderer component is responsible for applying the material to the mesh.
                GetComponent<MeshRenderer>().material = material;

            // Calls the CreateShape() method to create the shape of the terrain.
            CreateShape();
            // Calls the UpdateMesh() method to update the mesh with the new shape.
            UpdateMesh();
        }

        // This method creates the shape of the terrain.
        void CreateShape()
        {
            // Creates an array of vertices with the size of side * side.
            vertices = new Vector3[side * side];
            // Creates an array of UV coordinates with the size of side * side.
            uvCoords = new Vector2[side * side];
            // Creates an array of triangles with the size of (side - 1) * (side - 1) * 6.
            triangles = new int[(side - 1) * (side - 1) * 6];

            // Loop that iterates over the X-axis of the terrain.
            for (int x = 0; x < side; x++)
            {
                // A nested loop that iterates over the Z-axis of the terrain.
                for (int z = 0; z < side; z++)
                {
                    // Creates a new vertex object with the coordinates (x, 0, z)
                    Vector3 vtx = new Vector3(x, 0, z);
                    // This line adds the vertrices coordinate to the vertices array.
                    vertices[x + z * side] = vtx;
                    // Creates a new UV coordinate object with the coordinates(x, z) / (side - 1).
                    Vector2 uv = new Vector2(x, z) / (float)(side - 1);
                    // This line adds the UV coordinate to the uvCoords array.
                    uvCoords[x + z * side] = uv;
                }
            }
            // This line declares a variable named idx to keep track of the index of the current triangle.
            int idx = 0;
            // Loop that iterates over the X-axis of the terrain, excluding the last vertex on each row.
            for (int x = 0; x < side - 1; x++)
            {
                // A nested loop that iterates over the Z-axis of the terrain, excluding the last vertex on each column.
                for (int z = 0; z < side - 1; z++)
                {
                    // These lines assign the indices of the vertices that form a quad to variables named bl, br, tr, and tl.
                    int bl = x + z * side;
                    int br = x + 1 + z * side;
                    int tr = x + 1 + (z + 1) * side;
                    int tl = x + (z + 1) * side;

                    // These lines add the indices of the vertices to the triangles array in the order required to form triangles.
                    triangles[idx] = bl;
                    triangles[idx + 1] = tr;
                    triangles[idx + 2] = br;
                    triangles[idx + 3] = bl;
                    triangles[idx + 4] = tl;
                    triangles[idx + 5] = tr;
                    // This line increments the idx variable by 6, as each quad contributes 6 indices to the triangles array.
                    idx += 6;
                }
            }
        }
        // This method updates the mesh object with the newly created shape.
        void UpdateMesh()
        {
            // Clears the existing data from the mesh object.
            mesh.Clear();
            // Assigns the vertices array to the vertices property of the mesh object.
            mesh.vertices = vertices;
            // Assigns the triangles array to the triangles property of the mesh object.
            mesh.triangles = triangles;
            // Assigns the uvCoords array to the uv property of the mesh object.
            mesh.uv = uvCoords;
            // This line recalculates the normals for the mesh object.
            // Normals are vectors that indicate the direction a surface is facing.
            mesh.RecalculateNormals();
        }
    }
}
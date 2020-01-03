using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]

public class MapGenerator : MonoBehaviour {

    /*
     * Create a xLength * zLength mesh (map).
     * Use Perlin noise to create variations in terrain.
     */

    private Vector3[] vertices;
    private int[] triangles; //order in which vertices will be drawn

    //Default orientation is 100x100
    public int xLength = 100;
    public int zLength = 100;

    //Default height scale is 2.25
    public float heightScale = 2.25f;

    Mesh mesh;

	// Use this for initialization
	void Start () {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        vertices = new Vector3[(xLength + 1) * (zLength + 1)];
        triangles = new int[xLength * zLength * 6]; //six triangle points per square in grid

        //generate the vertices and triangle data that will be used in mesh
        GenerateMeshData();

        //update the values in the mesh to the data points generated
        UpdateMesh();

        //display the mesh
        OnDrawGizmos();

	}

    void GenerateMeshData() {

        //Generate vertices
        int vertLoc = 0;
        for (int i = 0; i < xLength + 1; i++){
            for (int j = 0; j < zLength + 1; j++) {
                //Assign random perlin noise value to y for height 
                //(side note: perlin noise needs non whole numbers as input, hence multiplying i and j by a random float value)
                vertices[vertLoc] = new Vector3(i, Mathf.PerlinNoise(i * .3f, j * .3f) * heightScale, j);
                vertLoc++;
            }
        }

        //Generate triangles
        //triangles = new int[6] { 1, 11, 0, 11, 1, 12 };

        
        int baseIndex = 0, topIndex = xLength + 1, triIndex = 0;
        for (int i = 0; i < zLength; i++)
        {
            for (int j = 0; j < xLength; j++)
            {
                triangles[triIndex] = baseIndex + 1;
                triangles[triIndex + 1] = topIndex;
                triangles[triIndex + 2] = baseIndex;
                triangles[triIndex + 3] = topIndex;
                triangles[triIndex + 4] = baseIndex + 1;
                triangles[triIndex + 5] = topIndex + 1;

                baseIndex++;
                topIndex++;
                triIndex += 6;
            }
            baseIndex++;
            topIndex++;
        }
        
    }

    void UpdateMesh() {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    void OnDrawGizmos() {

        //first check if vertices exist
        if(mesh.vertices == null){
            return;
        }

        const float drawRadius = .1f;
        foreach (Vector3 vector in mesh.vertices) {
            Gizmos.DrawSphere(vector, drawRadius);
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}

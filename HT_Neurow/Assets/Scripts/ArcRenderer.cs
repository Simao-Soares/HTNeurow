using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArcRenderer : MonoBehaviour
{

	[Range(0.001f, float.MaxValue)]
	//public float startAngle = 0.0f;

	public float arcHeight = 0.1f;

	[Range(0.0f, 2.0f)]
	public float sidesPerAngle = 0.1f;

	[Range(1, 360)]
	public int arcAngle = 360;

	[Range(0, 36)]
	public int arcSides = 0;

	//[Range(0.001f, float.MaxValue)]
	public float arcWidth = 0.5f;

	//[Range(0.001f, float.MaxValue)]
	public float arcRadius = 1.5f;

	MeshFilter viewMeshFilter;
	MeshRenderer meshRenderer;
	Mesh viewMesh;
	

	Material arcMaterial;

    public float xPos;
    public float zPos;


	// Start is called before the first frame update
	void Start()
    {
        DrawArcMesh();
    }



	public void DrawArcMesh()
	{
        gameObject.transform.position = new Vector3(xPos, 0.64f, zPos);
		int resSides = arcSides;
		if (arcSides < 3)
		{
			resSides = (int)Mathf.RoundToInt(arcAngle * sidesPerAngle);
		}

		if (viewMeshFilter == null)
		{
			viewMeshFilter = GetComponent<MeshFilter>();
		}

		if (meshRenderer == null)
		{
			meshRenderer = GetComponent<MeshRenderer>();
		}

		viewMesh = CreateArcMesh("Square", resSides, arcWidth, arcRadius, arcAngle);
		viewMeshFilter.mesh = viewMesh;

		arcMaterial = meshRenderer.sharedMaterial;
		

	}


	public Mesh CreateArcMesh(string name, int sides, float width, float radius, int arcAngle)
	{

		Mesh mesh = new Mesh();
		mesh.name = name;

		sides = (int)Mathf.Clamp((float)sides, 3, Mathf.Infinity); 

        ArcMeshInfo arcInfo = new ArcMeshInfo(sides, width, arcAngle, radius, arcHeight);

		mesh.vertices = arcInfo.vertices;
		mesh.triangles = arcInfo.triangles;

		mesh.RecalculateNormals();
		return mesh;
	}

    public struct ArcMeshInfo
    {
        public Vector3[] vertices;
        public int[] triangles;

        struct MeshInfo
        {
            public Vector3[] vertices;
            public int[] triangles;
        }

        public static Vector3 DirFromAngle(float angleInDegrees)
        {
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }

        public ArcMeshInfo(int sides, float width, int arcAngle, float radius, float arcHeight)
        {

            vertices = new Vector3[0];
            triangles = new int[0];

            bool isCircle = arcAngle == 360;
            float angleStep = arcAngle / sides;
            //int steps = isCircle ? sides : sides + 1;
            int steps = sides + 1;

            MeshInfo top = GetArcMesh(angleStep, sides, width, radius, steps, arcHeight, false, isCircle);
            MeshInfo bottom = GetArcMesh(angleStep, sides, width, radius, steps, 0, true, isCircle);

            for (int i = 0; i < bottom.triangles.Length; i++)
            {
                bottom.triangles[i] += top.vertices.Length;
            }

            List<int> tris = new List<int>();
            tris.AddRange(top.triangles);
            tris.AddRange(bottom.triangles);
            int[] _tris = tris.ToArray();

            List<Vector3> verts = new List<Vector3>();
            verts.AddRange(top.vertices);
            verts.AddRange(bottom.vertices);
            Vector3[] _verts = verts.ToArray();


            vertices = _verts;
            triangles = _tris;
        }

        static MeshInfo GetArcMesh(float angleStep,
                        int sides,
                        float width,
                        float radius,
                        int steps,
                        float yPos,
                        bool reverse,
                        bool isCircle)
        {

            int vertexCount = sides * 2;

            int arcVertex = sides * 2 + 2;

            Vector3[] _vertices = new Vector3[arcVertex];
            int[] _triangles = new int[vertexCount * 3];



            for (int i = 0; i < steps; i++)
            {

                float angle = i * angleStep;
                Vector3 dir = DirFromAngle(angle);

                Vector3 inner = dir * (width > radius ? 0 : radius - width);
                Vector3 outer = dir * radius;

                inner.y = yPos;
                outer.y = yPos;

                _vertices[i * 2] = inner;
                _vertices[i * 2 + 1] = outer;
                int ti = i * 6; //triangle index



                if (i < sides)
                {

                    if (reverse)
                    {

                        _triangles[ti] = i * 2 + 1;
                        _triangles[ti + 1] = i * 2 + 0;
                        _triangles[ti + 2] = i * 2 + 2;

                        _triangles[ti + 3] = i * 2 + 3;
                        _triangles[ti + 4] = i * 2 + 1;
                        _triangles[ti + 5] = i * 2 + 2;

                    }
                    else
                    {

                        _triangles[ti] = i * 2 + 1;
                        _triangles[ti + 1] = i * 2 + 2;
                        _triangles[ti + 2] = i * 2 + 0;

                        _triangles[ti + 3] = i * 2 + 3;
                        _triangles[ti + 4] = i * 2 + 2;
                        _triangles[ti + 5] = i * 2 + 1;
                    }
                }

                
            }

            MeshInfo info = new MeshInfo();
            info.triangles = _triangles;
            info.vertices = _vertices;
            return info;
        }
    }





}

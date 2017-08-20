using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGen {
	Mesh mesh;
	Vector3[] verts;
	int[] tris;
	Vector2[] uvs;

	int trisIndice;
	int vertsIndice;

	public MeshGen(){
		
	}

	public void nuevaMeshVertsDobles(int maxCantTriangulos){
		mesh = new Mesh ();
		tris = new int[maxCantTriangulos * 3];
		verts = new Vector3[maxCantTriangulos * 3];
		uvs = new Vector2[verts.Length];

		trisIndice = 0;
		vertsIndice = 0;
	}

	public void nuevaMesh(int maxCantVertices, int maxCantTriangulos){
		mesh = new Mesh ();
		tris = new int[maxCantTriangulos * 3];
		verts = new Vector3[maxCantVertices];
		uvs = new Vector2[verts.Length];

		trisIndice = 0;
		vertsIndice = 0;
	}

	public void vertice(Vector3 vert){
		verts [vertsIndice] = vert;
		vertsIndice++;
	}

	public void triangulo(int idVert1, int idVert2, int idVert3){
		tris [trisIndice++] = idVert1;
		tris [trisIndice++] = idVert2;
		tris [trisIndice++] = idVert3;
	}

	public void triangulo(int idVert1, int idVert2, int idVert3, Vector2 uv1, Vector2 uv2, Vector2 uv3){
		tris [trisIndice++] = idVert1;
		tris [trisIndice++] = idVert2;
		tris [trisIndice++] = idVert3;

		uvs [idVert1] = uv1;
		uvs [idVert2] = uv2;
		uvs [idVert3] = uv3;
	}

	public void trianguloAutoVert(Vector3 v1, Vector3 v2, Vector3 v3){
		tris [trisIndice  ] = vertsIndice  ;
		tris [trisIndice+1] = vertsIndice+1;
		tris [trisIndice+2] = vertsIndice+2;

		verts [vertsIndice  ] = v1;
		verts [vertsIndice+1] = v2;
		verts [vertsIndice+2] = v3;

		vertsIndice += 3;
		trisIndice += 3;
	}

	public void trianguloAutoVert (Vector3 v1, Vector3 v2, Vector3 v3, Vector2 uv1, Vector2 uv2, Vector2 uv3){
		tris [trisIndice  ] = vertsIndice  ;
		tris [trisIndice+1] = vertsIndice+1;
		tris [trisIndice+2] = vertsIndice+2;

		verts [vertsIndice  ] = v1;
		verts [vertsIndice+1] = v2;
		verts [vertsIndice+2] = v3;

		uvs [vertsIndice  ] = uv1;
		uvs [vertsIndice+1] = uv2;
		uvs [vertsIndice+2] = uv3;

		vertsIndice += 3;
		trisIndice += 3;
	}

	public void quadConvexoAutoVert(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4){
		trianguloAutoVert (v1, v2, v3);
		trianguloAutoVert (v3, v4, v1);
	}

	public void quadConvexoAutoVert(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4,
			Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4){
		trianguloAutoVert (v1, v2, v3, uv1, uv2, uv3);
		trianguloAutoVert (v3, v4, v1, uv3, uv4, uv1);
	}

	public Mesh getMesh(bool recalcularTodo){
		mesh.vertices = verts;
		mesh.uv = uvs;
		mesh.uv2 = uvs;
		mesh.triangles = tris;

		if (recalcularTodo) {
			mesh.RecalculateNormals ();
			mesh.RecalculateBounds ();
			mesh.RecalculateTangents();
		}
		return mesh;
	}

	public Mesh getMesh(bool recalcularNormales, bool recalcularCaja, bool recalcularTangentes){
		mesh.vertices = verts;
		mesh.uv = uvs;
		mesh.uv2 = uvs;
		mesh.triangles = tris;

		if(recalcularNormales)
			mesh.RecalculateNormals ();
		if (recalcularCaja)
			mesh.RecalculateBounds ();
		if (recalcularTangentes)
			mesh.RecalculateTangents();
		return mesh;
	}
}
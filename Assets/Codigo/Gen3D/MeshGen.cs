﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGen {
	static Vector3[] verts;
	static int[] tris;
	static Vector2[] uvs;

	static int submeshActual;
	static bool utilizaSubmeshes;

	static List<int[]> submeshTris;

	static int trisIndice;
	static int vertsIndice;

	public static void nuevaMeshVertsDobles(int maxCantTriangulos){
		tris = new int[maxCantTriangulos * 3];
		verts = new Vector3[maxCantTriangulos * 3];
		uvs = new Vector2[verts.Length];

		trisIndice = 0;
		vertsIndice = 0;

		submeshActual = 0;
		utilizaSubmeshes = false;
		submeshTris = new List<int[]> ();
	}

	public static void nuevaMesh(int maxCantVertices, int maxCantTriangulos){
		tris = new int[maxCantTriangulos * 3];
		verts = new Vector3[maxCantVertices];
		uvs = new Vector2[verts.Length];

		trisIndice = 0;
		vertsIndice = 0;

		submeshActual = 0;
		utilizaSubmeshes = false;
		submeshTris = new List<int[]> ();
	}

	public static void anadirSubmesh(int maxCantTriangulos){
		submeshTris.Add (tris);
		submeshActual++;
		tris = new int[maxCantTriangulos * 3];
		trisIndice = 0;
		utilizaSubmeshes = true;
	}

	public static void vertice(Vector3 vert){
		verts [vertsIndice] = vert;
		vertsIndice++;
	}

	public static void triangulo(int idVert1, int idVert2, int idVert3){
		tris [trisIndice++] = idVert1;
		tris [trisIndice++] = idVert2;
		tris [trisIndice++] = idVert3;
	}

	public static void triangulo(int idVert1, int idVert2, int idVert3, Vector2 uv1, Vector2 uv2, Vector2 uv3){
		tris [trisIndice++] = idVert1;
		tris [trisIndice++] = idVert2;
		tris [trisIndice++] = idVert3;

		uvs [idVert1] = uv1;
		uvs [idVert2] = uv2;
		uvs [idVert3] = uv3;
	}

	public static void trianguloAutoVert(Vector3 v1, Vector3 v2, Vector3 v3){
		tris [trisIndice  ] = vertsIndice  ;
		tris [trisIndice+1] = vertsIndice+1;
		tris [trisIndice+2] = vertsIndice+2;

		verts [vertsIndice  ] = v1;
		verts [vertsIndice+1] = v2;
		verts [vertsIndice+2] = v3;

		vertsIndice += 3;
		trisIndice += 3;
	}

	public static void trianguloAutoVert (Vector3 v1, Vector3 v2, Vector3 v3, Vector2 uv1, Vector2 uv2, Vector2 uv3){
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

	public static void quadConvexoAutoVert(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4){
		trianguloAutoVert (v1, v2, v3);
		trianguloAutoVert (v3, v4, v1);
	}

	public static void quadConvexoAutoVert(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4,
			Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4){
		trianguloAutoVert (v1, v2, v3, uv1, uv2, uv3);
		trianguloAutoVert (v3, v4, v1, uv3, uv4, uv1);
	}

	public static void quadConvexoAutoVertsCuatro(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4,
		Vector2 uv1, Vector2 uv2, Vector2 uv3, Vector2 uv4){
		verts [vertsIndice  ] = v1;
		verts [vertsIndice+1] = v2;
		verts [vertsIndice+2] = v3;
		verts [vertsIndice+3] = v4;
		triangulo (vertsIndice, vertsIndice + 1, vertsIndice + 2, uv1, uv2, uv3);
		triangulo (vertsIndice, vertsIndice + 2, vertsIndice + 3, uv1, uv3, uv4);
		vertsIndice += 4;
	}


	public static Mesh getMesh(bool recalcularTodo){
		Mesh mesh = new Mesh ();
		mesh.vertices = verts;
		mesh.uv = uvs;
		mesh.uv2 = uvs;
		if (utilizaSubmeshes) {
			mesh.subMeshCount = submeshActual + 1;
			for (int i = 0; i < submeshActual; i++) {
				mesh.SetTriangles (submeshTris [i], i);
			}
			mesh.SetTriangles (tris, submeshActual);
		} else {
			mesh.triangles = tris;
		}

		if (recalcularTodo) {
			mesh.RecalculateNormals ();
			mesh.RecalculateBounds ();
			mesh.RecalculateTangents();
		}
		return mesh;
	}

	public static Mesh getMesh(bool recalcularNormales, bool recalcularCaja, bool recalcularTangentes){
		Mesh mesh = new Mesh ();
		mesh.vertices = verts;
		mesh.uv = uvs;
		mesh.uv2 = uvs;
		if (utilizaSubmeshes) {
			mesh.subMeshCount = submeshActual + 1;
			for (int i = 0; i < submeshActual; i++) {
				mesh.SetTriangles (submeshTris [i], i);
			}
			mesh.SetTriangles (tris, submeshActual);
		} else {
			mesh.triangles = tris;
		}
		if(recalcularNormales)
			mesh.RecalculateNormals ();
		if (recalcularCaja)
			mesh.RecalculateBounds ();
		if (recalcularTangentes)
			mesh.RecalculateTangents();
		return mesh;
	}

	public static Mesh actualizarMesh(Mesh mesh, bool recalcularTodo){

		mesh.vertices = verts;
		mesh.uv = uvs;
		mesh.uv2 = uvs;
		if (utilizaSubmeshes) {
			mesh.subMeshCount = submeshActual + 1;
			for (int i = 0; i < submeshActual; i++) {
				mesh.SetTriangles (submeshTris [i], i);
			}
			mesh.SetTriangles (tris, submeshActual);
		} else {
			mesh.triangles = tris;
		}
		if (recalcularTodo) {
			mesh.RecalculateNormals ();
			mesh.RecalculateBounds ();
			mesh.RecalculateTangents();
		}
		return mesh;
	}

	public static Mesh actualizarMesh(Mesh mesh, bool updateVertices, bool updateUv, bool updateUv2, bool updateTriangles,
		bool recalcularNormales, bool recalcularCaja, bool recalcularTangentes){

		if(updateVertices)
			mesh.vertices = verts;
		if(updateUv)
			mesh.uv = uvs;
		if(updateUv2)
			mesh.uv2 = uvs;
		if (updateTriangles) {
			if (utilizaSubmeshes) {
				mesh.subMeshCount = submeshActual + 1;
				for (int i = 0; i < submeshActual; i++) {
					mesh.SetTriangles (submeshTris [i], i);
				}
				mesh.SetTriangles (tris, submeshActual);
			} else {
				mesh.triangles = tris;
			}
		}
		if(recalcularNormales)
			mesh.RecalculateNormals ();
		if (recalcularCaja)
			mesh.RecalculateBounds ();
		if (recalcularTangentes)
			mesh.RecalculateTangents();
		return mesh;
	}

	public static Mesh actualizarMesh(Mesh mesh, bool updateVertices, bool updateUv, bool updateUv2, bool updateTriangles,
		bool recalcularTodo){

		if(updateVertices)
			mesh.vertices = verts;
		if(updateUv)
			mesh.uv = uvs;
		if(updateUv2)
			mesh.uv2 = uvs;
		if (updateTriangles) {
			if (utilizaSubmeshes) {
				mesh.subMeshCount = submeshActual + 1;
				for (int i = 0; i < submeshActual; i++) {
					mesh.SetTriangles (submeshTris [i], i);
				}
				mesh.SetTriangles (tris, submeshActual);
			} else {
				mesh.triangles = tris;
			}
		}
		if (recalcularTodo) {
			mesh.RecalculateNormals ();
			mesh.RecalculateBounds ();
			mesh.RecalculateTangents();
		}
		return mesh;
	}
}
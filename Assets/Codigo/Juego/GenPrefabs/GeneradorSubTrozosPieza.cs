using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class GeneradorSubTrozosPieza : MonoBehaviour {

	public float umbral = 0.0001F;
	public Material materialEsquina;

	[ContextMenu("Quitar originales")]
	void quitarOriginales(){
		for (int i = 0; i < transform.childCount; i++) {
			Transform hijo = transform.GetChild (i);
			DestroyImmediate (hijo.GetComponent<MeshFilter> ());
			DestroyImmediate (hijo.GetComponent<MeshRenderer> ());
		}
	}

	[ContextMenu("Asignar Material")]
	void asignarMaterial(){
		int countHijos = transform.childCount;
		for (int i = 0; i < transform.childCount; i++) {
			Transform hijo = transform.GetChild (i);
			for(int j = 0; j < hijo.childCount; j++){
				GameObject subHijo = hijo.GetChild (j).gameObject;
				subHijo.GetComponent<MeshRenderer> ().sharedMaterial = materialEsquina;

			}
		}
	}
	[ContextMenu("Limpiar")]
	void limpiar(){
		int countHijos = transform.childCount;
		for (int i = 0; i < transform.childCount; i++) {
			Transform hijo = transform.GetChild (i);
			while (hijo.childCount > 0) {
				GameObject.DestroyImmediate(hijo.GetChild (0).gameObject);
			}
		}
	}

	[ContextMenu("Generar (Sobreescribir)")]
	void generarMuestrarioSobreescribir(){
		generarMuestrario (true);
	}

	[ContextMenu("Generar (Completar)")]
	void generarMuestrarioCompletar(){
		generarMuestrario (false);
	}

	void generarMuestrario(bool sobreescribir){
		limpiar ();

		int countHijos = transform.childCount;
		for (int i = 0; i < countHijos; i++) {
			Transform hijo = transform.GetChild (i);
			string nombre = hijo.gameObject.name;
			switch (nombre) {
			case "Esquinas":
			case "EjeX":
			case "EjeY":
			case "EjeZ":
			case "PlanoX":
			case "PlanoY":
			case "PlanoZ":
			case "DoblezX":
			case "DoblezY":
			case "DoblezZ":
			case "Union":
				hijo.localPosition = Vector3.right * i;
				hijo.localRotation = Quaternion.identity;
				hijo.gameObject.name = nombre;

				bool generar = true;
				if (!sobreescribir) {
					Mesh modelo = hijo.GetComponent<MeshFilter> ().sharedMesh;
					string ruta = AssetDatabase.GetAssetPath (modelo.GetInstanceID ());
					ruta = ruta.Substring (0, ruta.LastIndexOf ('/') + 1) + hijo.name;
					generar = !Directory.Exists (ruta);
				}
				if (generar) {
					subdividirCasoTrozo (hijo);
				} else {
					recargarCasoTrozo (hijo);
				}
				break;
			default:
				break;
			}
		}
		AssetDatabase.SaveAssets ();
	}

	string[] nombresSubTrozos = {"PPP","PPN","PNP","PNN","NPP","NPN","NNP","NNN"};

	/// <summary>
	/// Subdivide el trozo en 8 subtrozos según los 8 octantes del sistema cartesiano. (!) IMPORTANTE (!) Este algoritmo ASUME 2 cosas:
	/// -Que la Mesh contenida en el MeshFilter del objeto enviado está centrada en el origen [0,0,0]
	/// -Que los planos x=0, y=0 y z=0 seccionan los triángulos de la Mesh uniformemente, es decir, que no existen triángulos que ocupen más de un octante
	/// Se guardan los 8 resultados como assets en carpetas dentro de la ruta donde se aloja el modelo original, y se asignan como GameObjects hijos
	/// </summary>
	/// <param name="transform">Transform que contiene el modelo a dividir como parte de su MeshFilter y contendrá a los 8 subTrozos como hijos.</param>

	void subdividirCasoTrozo(Transform transf){
		Mesh modelo = transf.GetComponent<MeshFilter> ().sharedMesh;

		List<Vector3>[] vertsOctantes = new List<Vector3>[8];
		List<int>[] trisOctantes = new List<int>[8];
		List<Vector2>[] uvsOctantes = new List<Vector2>[8];
		List<Vector3>[] normsOctantes = new List<Vector3>[8];

		for (int i = 0; i < 8; i++) {
			vertsOctantes [i] = new List<Vector3> ();
			trisOctantes [i] = new List<int> ();
			uvsOctantes [i] = new List<Vector2> ();
			normsOctantes[i] = new List<Vector3> ();
		}

		int[] indiceVert = new int[8];

		int[] tris = modelo.triangles;
		Vector3[] verts = modelo.vertices;
		Vector2[] uvs = modelo.uv;
		Vector3[] norms = modelo.normals;

		for (int i = 0; i < modelo.triangles.Length; i += 3) {
			int indV1 = tris [i];
			int indV2 = tris [i + 1];
			int indV3 = tris [i + 2];
			Vector3 v1 = verts [indV1];
			Vector3 v2 = verts [indV2];
			Vector3 v3 = verts [indV3];
			bool[] octantesV1 = getExistenciaOctantes (v1);
			bool[] octantesV2 = getExistenciaOctantes (v2);
			bool[] octantesV3 = getExistenciaOctantes (v3);

			for (int j = 0; j < 8; j++) {
				if (octantesV1 [j] && octantesV2 [j] && octantesV3 [j]) { //Los 3 vértices existen en el mismo octante
					
					vertsOctantes [j].Add (v1);
					vertsOctantes [j].Add (v2);
					vertsOctantes [j].Add (v3);
					uvsOctantes [j].Add (uvs[indV1]);
					uvsOctantes [j].Add (uvs[indV2]);
					uvsOctantes [j].Add (uvs[indV3]);
					normsOctantes [j].Add (norms [indV1]);
					normsOctantes [j].Add (norms [indV2]);
					normsOctantes [j].Add (norms [indV3]);
					trisOctantes [j].Add (indiceVert[j]);
					trisOctantes [j].Add (indiceVert[j]+1);
					trisOctantes [j].Add (indiceVert[j]+2);
					indiceVert[j] += 3;
				}
			}
		}


		for (int i = 0; i < 8; i++) {
			Mesh meshSubTrozo = new Mesh ();
			meshSubTrozo.vertices = vertsOctantes [i].ToArray ();
			meshSubTrozo.triangles = trisOctantes [i].ToArray ();
			meshSubTrozo.uv = uvsOctantes [i].ToArray ();
			meshSubTrozo.normals = normsOctantes [i].ToArray ();
			MeshUtility.Optimize (meshSubTrozo);

			string ruta = AssetDatabase.GetAssetPath (modelo.GetInstanceID ());
			ruta = ruta.Substring (0, ruta.LastIndexOf ('/')+1) + transf.name;
			if (!Directory.Exists (ruta)) {
				Directory.CreateDirectory (ruta);
			}
			ruta = ruta + "/" + transf.name + "_" + nombresSubTrozos[i] + ".asset";
			AssetDatabase.CreateAsset (meshSubTrozo, ruta);
			GameObject goSubTrozo = new GameObject (transf.name + "_" + nombresSubTrozos [i]);

			MeshRenderer mr = goSubTrozo.AddComponent<MeshRenderer> ();
			MeshFilter mf = goSubTrozo.AddComponent<MeshFilter> ();
			mf.sharedMesh = meshSubTrozo;

			mr.sharedMaterial = transf.GetComponent<MeshRenderer> ().sharedMaterial;
			goSubTrozo.transform.SetParent (transf);
			goSubTrozo.transform.localPosition = Vector3.zero;
		}
	}

	/// <summary>
	/// Recarga los subTrozos a partir de los archivos de modelos existentes. No son recalculados.
	/// </summary>
	/// <param name="transf">Transform que contendrá a los 8 subTrozos como hijos.</param>
	void recargarCasoTrozo(Transform transf){
		Mesh modelo = transf.GetComponent<MeshFilter> ().sharedMesh;

		for (int i = 0; i < 8; i++) {
			string ruta = AssetDatabase.GetAssetPath (modelo.GetInstanceID ());
			ruta = ruta.Substring (0, ruta.LastIndexOf ('/')+1) + transf.name;
			if (!Directory.Exists (ruta)) {
				Directory.CreateDirectory (ruta);
			}
			ruta = ruta + "/" + transf.name + "_" + nombresSubTrozos[i] + ".asset";
			Mesh meshSubTrozo = AssetDatabase.LoadAssetAtPath<Mesh> (ruta);
			GameObject goSubTrozo = new GameObject (transf.name + "_" + nombresSubTrozos [i]);

			MeshRenderer mr = goSubTrozo.AddComponent<MeshRenderer> ();
			MeshFilter mf = goSubTrozo.AddComponent<MeshFilter> ();
			mf.sharedMesh = meshSubTrozo;

			mr.sharedMaterial = transf.GetComponent<MeshRenderer> ().sharedMaterial;
			goSubTrozo.transform.SetParent (transf);
			goSubTrozo.transform.localPosition = Vector3.zero;
		}
	}

	/// <summary>
	/// Devuelve un array con 8 booleanos indicando que a que octantes "pertenece" el vértice.
	/// Nótese que un v;értice puede "estar" en m;ás de un octante si se encuentra en la frontera entre dos o más,
	/// según la precisión indicada por el atributo "umbral" de este script.
	/// </summary>
	/// <returns>The existencia octantes.</returns>
	/// <param name="vertice">Vertice.</param>
	bool[] getExistenciaOctantes(Vector3 vertice){

		bool[] existenciaOctantes = new bool[8];

		bool dentro_Nx = vertice.x < umbral;
		bool dentro_Ny = vertice.y < umbral;
		bool dentro_Nz = vertice.z < umbral;
		bool dentro_Px = vertice.x > -umbral;
		bool dentro_Py = vertice.y > -umbral;
		bool dentro_Pz = vertice.z > -umbral;


		if (dentro_Px && dentro_Py && dentro_Pz)
			existenciaOctantes [0] = true;

		if (dentro_Px && dentro_Py && dentro_Nz)
			existenciaOctantes [1] = true;
		
		if (dentro_Px && dentro_Ny && dentro_Pz)
			existenciaOctantes [2] = true;

		if (dentro_Px && dentro_Ny && dentro_Nz)
			existenciaOctantes [3] = true;

		if (dentro_Nx && dentro_Py && dentro_Pz)
			existenciaOctantes [4] = true;
		
		if (dentro_Nx && dentro_Py && dentro_Nz)
			existenciaOctantes [5] = true;

		if (dentro_Nx && dentro_Ny && dentro_Pz)
			existenciaOctantes [6] = true;

		if (dentro_Nx && dentro_Ny && dentro_Nz)
			existenciaOctantes [7] = true;
		
		return existenciaOctantes;
	}
}

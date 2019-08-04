
#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class GeneradorSubTrozosPieza : MonoBehaviour {

	public float umbral = 0.0001F;
	public Material[] materialesEsquina;

	[ContextMenu("Cambiar Z por Y")]
	void cambiarZporY(){
		for (int i = 0; i < transform.childCount; i++) {
			Transform hijo = transform.GetChild (i);
			string nombre = hijo.gameObject.name;
			if (nombre.Substring (nombre.Length - 1) == "Y") {
				nombre = nombre.Substring (0, nombre.Length - 1) + "Z";
			} else if (nombre.Substring (nombre.Length - 1) == "Z") {
				nombre = nombre.Substring (0, nombre.Length - 1) + "Y";
			}
			hijo.gameObject.name = nombre;
		}
	}

	[ContextMenu("Ocultar originals")]
	void ocultarOriginales(){
		for (int i = 0; i < transform.childCount; i++) {
			Transform hijo = transform.GetChild (i);
			hijo.GetComponent<MeshRenderer> ().enabled = false;
		}
	}

	[ContextMenu("Mostrar originales")]
	void mostrarOriginales(){
		for (int i = 0; i < transform.childCount; i++) {
			Transform hijo = transform.GetChild (i);
			hijo.GetComponent<MeshRenderer> ().enabled = true;
		}
	}

	[ContextMenu("Asignar Materiales")]
	void asignarMateriales(){

		List<Material> materialesOrig = new List<Material> ();

		for (int i = 0; i < transform.childCount; i++) {
			Transform hijo = transform.GetChild (i);
			MeshRenderer mr = hijo.GetComponent<MeshRenderer> ();
			foreach (Material m in mr.sharedMaterials) {
				if (!materialesOrig.Contains (m)) {
					materialesOrig.Add (m);
				}
			}
		}

		Material[] arrayMats = materialesOrig.ToArray ();

		for (int i = 0; i < transform.childCount; i++) {
			Transform hijo = transform.GetChild (i);

			for (int j = 0; j < hijo.childCount; j++) {
				Transform subHijo = hijo.GetChild (j);
				MeshRenderer mrSubHijo = subHijo.GetComponent<MeshRenderer> ();
				List<Material> matsSubHijoNuevos = new List<Material> ();
				foreach (Material m in mrSubHijo.sharedMaterials) {
					for (int k = 0; k < arrayMats.Length; k++) {
						if (m == arrayMats [k]) {
							matsSubHijoNuevos.Add (materialesEsquina [k]);
						}
					}
				}
				mrSubHijo.sharedMaterials = matsSubHijoNuevos.ToArray ();
			}

			MeshRenderer mrHijo = hijo.GetComponent<MeshRenderer> ();
			List<Material> matsHijoNuevos = new List<Material> ();

			foreach (Material m in mrHijo.sharedMaterials) {
				for (int j = 0; j < arrayMats.Length; j++) {
					if (m == arrayMats [j]) {
						matsHijoNuevos.Add (materialesEsquina [j]);
					}
				}
			}
			mrHijo.sharedMaterials = matsHijoNuevos.ToArray ();
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
					ruta = ruta.Substring (0, ruta.LastIndexOf ('/') + 1) + nombre;
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

	string[] nombresSubTrozos = {"PPN","PNN","PPP","PNP","NPN","NNN","NPP","NNP"};

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
		List<int>[,] trisOctantes = new List<int>[8, modelo.subMeshCount];
		List<Vector2>[] uvsOctantes = new List<Vector2>[8];
		List<Vector3>[] normsOctantes = new List<Vector3>[8];
		bool[,] matsOctantes = new bool[8, modelo.subMeshCount];

		for (int i = 0; i < 8; i++) {
			vertsOctantes [i] = new List<Vector3> ();
			for (int j = 0; j < modelo.subMeshCount; j++) {
				trisOctantes [i, j] = new List<int> ();
			}
			uvsOctantes [i] = new List<Vector2> ();
			normsOctantes[i] = new List<Vector3> ();
		}

		int[] indiceVert = new int[8];
		Vector3[] verts = modelo.vertices;
		Vector2[] uvs = modelo.uv;
		Vector3[] norms = modelo.normals;
		for (int i = 0; i < modelo.subMeshCount; i++) {

			int[] tris = modelo.GetTriangles(i);

			for (int j = 0; j < tris.Length; j += 3) {
				int indV1 = tris [j];
				int indV2 = tris [j + 1];
				int indV3 = tris [j + 2];
				Vector3 v1 = verts [indV1];
				Vector3 v2 = verts [indV2];
				Vector3 v3 = verts [indV3];
				bool[] octantesV1 = getExistenciaOctantes (v1);
				bool[] octantesV2 = getExistenciaOctantes (v2);
				bool[] octantesV3 = getExistenciaOctantes (v3);

				for (int k = 0; k < 8; k++) {
					if (octantesV1 [k] && octantesV2 [k] && octantesV3 [k]) { //Los 3 vértices existen en el mismo octante
						matsOctantes[k,i] = true;
						vertsOctantes [k].Add (v1);
						vertsOctantes [k].Add (v2);
						vertsOctantes [k].Add (v3);
						uvsOctantes [k].Add (uvs[indV1]);
						uvsOctantes [k].Add (uvs[indV2]);
						uvsOctantes [k].Add (uvs[indV3]);
						normsOctantes [k].Add (norms [indV1]);
						normsOctantes [k].Add (norms [indV2]);
						normsOctantes [k].Add (norms [indV3]);
						trisOctantes [k,i].Add (indiceVert[k]);
						trisOctantes [k,i].Add (indiceVert[k]+1);
						trisOctantes [k,i].Add (indiceVert[k]+2);
						indiceVert[k] += 3;
					}
				}
			}
		}
			
		MeshRenderer meshrend = transf.GetComponent<MeshRenderer> ();

		for (int i = 0; i < 8; i++) {
			List<Material> mats = new List<Material> ();

            if(vertsOctantes[i].Count > 0)
            {
                Mesh meshSubTrozo = new Mesh();
                //meshSubTrozo.subMeshCount = modelo.subMeshCount;
                meshSubTrozo.vertices = vertsOctantes[i].ToArray();
                int k = 0;
                for (int j = 0; j < modelo.subMeshCount; j++)
                {
                    if (matsOctantes[i, j])
                    {
                        mats.Add(meshrend.sharedMaterials[j]);
                        k++;
                    }
                }
                meshSubTrozo.subMeshCount = mats.Count;
                k = 0;
                for (int j = 0; j < modelo.subMeshCount; j++)
                {
                    if (matsOctantes[i, j])
                    {
                        meshSubTrozo.SetTriangles(trisOctantes[i, j].ToArray(), k);
                        k++;
                    }
                }

                meshSubTrozo.uv = uvsOctantes[i].ToArray();
                meshSubTrozo.normals = normsOctantes[i].ToArray();
                MeshUtility.Optimize(meshSubTrozo);

                string ruta = AssetDatabase.GetAssetPath(modelo.GetInstanceID());
                ruta = ruta.Substring(0, ruta.LastIndexOf('/') + 1) + transf.name;
                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }
                ruta = ruta + "/" + transf.name + "_" + nombresSubTrozos[i] + ".asset";
                AssetDatabase.CreateAsset(meshSubTrozo, ruta);
                GameObject goSubTrozo = new GameObject(transf.name + "_" + nombresSubTrozos[i]);

                MeshRenderer mr = goSubTrozo.AddComponent<MeshRenderer>();
                MeshFilter mf = goSubTrozo.AddComponent<MeshFilter>();
                mf.sharedMesh = meshSubTrozo;

                mr.sharedMaterials = mats.ToArray();
                goSubTrozo.transform.SetParent(transf);
                goSubTrozo.transform.localPosition = Vector3.zero;
            }

			
		}
	}

	/// <summary>
	/// Recarga los subTrozos a partir de los archivos de modelos existentes. No son recalculados.
	/// </summary>
	/// <param name="transf">Transform que contendrá a los 8 subTrozos como hijos.</param>
	void recargarCasoTrozo(Transform transf){
        Mesh modelo = transf.GetComponent<MeshFilter>().sharedMesh;

        List<Vector3>[] vertsOctantes = new List<Vector3>[8];
        List<int>[,] trisOctantes = new List<int>[8, modelo.subMeshCount];
        bool[,] matsOctantes = new bool[8, modelo.subMeshCount];

        for (int i = 0; i < 8; i++)
        {
            vertsOctantes[i] = new List<Vector3>();
            for (int j = 0; j < modelo.subMeshCount; j++)
            {
                trisOctantes[i, j] = new List<int>();
            }
        }
        
        Vector3[] verts = modelo.vertices;
        for (int i = 0; i < modelo.subMeshCount; i++)
        {
            int[] tris = modelo.GetTriangles(i);

            for (int j = 0; j < tris.Length; j += 3)
            {
                int indV1 = tris[j];
                int indV2 = tris[j + 1];
                int indV3 = tris[j + 2];
                Vector3 v1 = verts[indV1];
                Vector3 v2 = verts[indV2];
                Vector3 v3 = verts[indV3];
                bool[] octantesV1 = getExistenciaOctantes(v1);
                bool[] octantesV2 = getExistenciaOctantes(v2);
                bool[] octantesV3 = getExistenciaOctantes(v3);

                for (int k = 0; k < 8; k++)
                {
                    if (octantesV1[k] && octantesV2[k] && octantesV3[k])
                    { //Los 3 vértices existen en el mismo octante
                        matsOctantes[k, i] = true;
                    }
                }
            }
        }

        int[] cantMatsOctantes = new int[8];
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < modelo.subMeshCount; j++)
            {
                if (matsOctantes[i, j])
                {
                    cantMatsOctantes[i]++;
                }
            }
        }

        for (int i = 0; i < 8; i++)
        {
            string ruta = AssetDatabase.GetAssetPath(modelo.GetInstanceID());
            ruta = ruta.Substring(0, ruta.LastIndexOf('/') + 1) + transf.name;
            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }
            ruta = ruta + "/" + transf.name + "_" + nombresSubTrozos[i] + ".asset";
            Mesh meshSubTrozo = AssetDatabase.LoadAssetAtPath<Mesh>(ruta);

            GameObject goSubTrozo = new GameObject(transf.name + "_" + nombresSubTrozos[i]);

            MeshRenderer mr = goSubTrozo.AddComponent<MeshRenderer>();
            MeshFilter mf = goSubTrozo.AddComponent<MeshFilter>();
            mf.sharedMesh = meshSubTrozo;

            Material[] matsTrozo = transf.GetComponent<MeshRenderer>().sharedMaterials;
            List<Material> matsSubTrozo = new List<Material>();
            for(int j = 0; j < modelo.subMeshCount; j++)
            {
                if (matsOctantes[i, j])
                {
                    matsSubTrozo.Add(matsTrozo[j]);
                }
            }
            mr.materials = matsSubTrozo.ToArray();

            goSubTrozo.transform.SetParent(transf);
            goSubTrozo.transform.localPosition = Vector3.zero;
        }

    }

	/// <summary>
	/// Devuelve un array con 8 booleanos indicando que a que octantes "pertenece" el vértice.
	/// Nótese que un vértice puede "estar" en más de un octante si se encuentra en la frontera entre dos o más,
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
#endif
using UnityEngine;
using System;

public class Pieza : MonoBehaviour {


	public enum TipoPieza {TERRENO, CARAMELO}

	//=======VARIABLES DE PRUEBA=======
	public GameObject prefabMuestrario;
	public int tamPruebaX = 5;
	public int tamPruebaY = 3;
	public int tamPruebaZ = 5;
	public float probabilidadGen = 0.7F;


	//=======PROPIEDADES=======
	public TipoPieza tipoPieza = TipoPieza.CARAMELO;

	bool[,,] grilla; //Matriz que contiene la existencia de la pieza. grilla[x,y,z] es verdadero si la pieza contiene un trozo en esas coordenadas (locales)
	int[,,] metadata; //Matriz que contiene la metadata de la pieza. Debería ser siempre del mismo tamaño que grilla.

	bool[,,] grilla_pad; //Contiene una versión paddeada con falses de la variable grilla
	int[,,] metadata_pad; //Contiene una versión paddeada con 0s de la variable metadata

	//=======GENERACIÓN=======
	[ContextMenu("Generar")]
	void generar(){
		MuestrarioPiezas.cargarMuestrario (tipoPieza, 0, prefabMuestrario);

		int minX = Int32.MaxValue;
		int minY = Int32.MaxValue;
		int minZ = Int32.MaxValue;

		int maxX = Int32.MinValue;
		int maxY = Int32.MinValue;
		int maxZ = Int32.MinValue;

		Transform hijo;
		for (int i = 0; i < transform.childCount; i++) {
			hijo = transform.GetChild (i);
			int x = Mathf.RoundToInt (hijo.localPosition.x);
			int y = Mathf.RoundToInt (hijo.localPosition.y);
			int z = Mathf.RoundToInt (hijo.localPosition.z);

			if (x < minX)
				minX = x;
			if (y < minY)
				minY = y;
			if (z < minZ)
				minZ = z;

			if (x > maxX)
				maxX = x;
			if (y > maxY)
				maxY = y;
			if (z > maxZ)
				maxZ = z;
		}

		int largoX = maxX - minX + 1;
		int largoY = maxY - minY + 1;
		int largoZ = maxZ - minZ + 1;

		grilla = new bool[largoX, largoY, largoZ];
		metadata = new int[largoX, largoY, largoZ];

		for (int i = 0; i < transform.childCount; i++) {
			hijo = transform.GetChild (i);
			int x = Mathf.RoundToInt (hijo.localPosition.x) - minX;
			int y = Mathf.RoundToInt (hijo.localPosition.y) - minY;
			int z = Mathf.RoundToInt (hijo.localPosition.z) - minZ;
			grilla [x, y, z] = true;
		}

		limpiar ();
		generarPadding ();
		recrearModeloCompleto ();
		transform.position += new Vector3 (minX, minY, minZ);
	}

	[ContextMenu("Limpiar")]
	void limpiar(){
		while (transform.childCount > 0) {
			DestroyImmediate (transform.GetChild (0).gameObject);
		}
	}


	[ContextMenu("PRUEBA")]
	void prueba(){
		limpiar ();

		MuestrarioPiezas.cargarMuestrario (tipoPieza, 0, prefabMuestrario);

		grilla = new bool[tamPruebaX, tamPruebaY, tamPruebaZ];
		metadata = new int[tamPruebaX, tamPruebaY, tamPruebaZ];

		for (int x = 0; x < tamPruebaX; x++) {
			for (int y = 0; y < tamPruebaY; y++) {
				for (int z = 0; z < tamPruebaZ; z++) {
					grilla [x, y, z] = UnityEngine.Random.value < probabilidadGen;
					metadata [x, y, z] = 0;
				}
			}
		}

		generarPadding ();
		recrearModeloCompleto ();
	}

	void generarPadding(){
		grilla_pad = new bool[grilla.GetLength(0)+2, grilla.GetLength(1)+2,grilla.GetLength(2)+2];
		metadata_pad = new int[metadata.GetLength(0)+2,metadata.GetLength(1)+2,metadata.GetLength(2)+2];

		for (int x = 0; x < grilla.GetLength (0); x++) {
			for (int y = 0; y < grilla.GetLength (1); y++) {
				for (int z = 0; z < grilla.GetLength (2); z++) {
					grilla_pad [x + 1, y + 1, z + 1] = grilla [x, y, z];
					metadata_pad [x + 1, y + 1, z + 1] = metadata [x, y, z];
				}
			}
		}
	}

	void recrearModeloCompleto(){
		Quaternion quat = Quaternion.Inverse(transform.localRotation);
		//transform.rotation = Quaternion.identity;

		for (int x = 0; x < grilla.GetLength(0); x++) {
			for (int y = 0; y < grilla.GetLength(1); y++) {
				for (int z = 0; z < grilla.GetLength(2); z++) {
					if (grilla [x, y, z]) {

						bool[,,] mapaVecindad = new bool[3, 3, 3];
						for (int xx = 0; xx <= 2; xx++) {
							for (int yy = 0; yy <= 2; yy++) {
								for (int zz = 0; zz <= 2; zz++) {
									mapaVecindad [xx, yy, zz] = grilla_pad [x + xx, y + yy, z + zz];
								}
							}
						}
						GameObject go = new GameObject ();

						TrozoPieza tp = go.AddComponent<TrozoPieza> ();
						tp.generarTrozoPieza (mapaVecindad, metadata_pad [x + 1, y + 1, z + 1], tipoPieza);
						go.transform.parent = transform;
						go.transform.localPosition = new Vector3 (x, y, z);
					}
				}
			}
		}

		//transform.rotation = quat;
	}
}

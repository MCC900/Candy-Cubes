using UnityEngine;
using System;

public class Pieza : MonoBehaviour {


	public enum TipoPieza {TERRENO_PASTO, CARAMELO_ROJO, CARAMELO_VERDE, CARAMELO_NARANJA}

	//=======VARIABLES DE PRUEBA=======
	public GameObject prefabMuestrario;
	public int tamPruebaX = 5;
	public int tamPruebaY = 3;
	public int tamPruebaZ = 5;
	public float probabilidadGen = 0.7F;

	//=======PROPIEDADES=======
	public TipoPieza tipoPieza = TipoPieza.CARAMELO_ROJO;
	public Vector3Int dimensiones;
	public Vector3Int posicion;

	public bool[,,] existencia; //Matriz que contiene la existencia de la pieza. grilla[x,y,z] es verdadero si la pieza contiene un trozo en esas coordenadas (locales)
	public int[,,] metadata; //Matriz que contiene la metadata de la pieza. Debería ser siempre del mismo tamaño que grilla.

	bool[,,] existencia_pad; //Contiene una versión paddeada con falses de la variable grilla
	int[,,] metadata_pad; //Contiene una versión paddeada con 0s de la variable metadata

	//=======GENERACIÓN=======
	[ContextMenu("Generar")]
	void generar(){
		DataJuego.i.cargarDataMuestrarios ();

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

		existencia = new bool[largoX, largoY, largoZ];
		metadata = new int[largoX, largoY, largoZ];

		for (int i = 0; i < transform.childCount; i++) {
			hijo = transform.GetChild (i);
			int x = Mathf.RoundToInt (hijo.localPosition.x) - minX;
			int y = Mathf.RoundToInt (hijo.localPosition.y) - minY;
			int z = Mathf.RoundToInt (hijo.localPosition.z) - minZ;
			existencia [x, y, z] = true;
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

		DataJuego.i.cargarDataMuestrarios ();

		existencia = new bool[tamPruebaX, tamPruebaY, tamPruebaZ];
		metadata = new int[tamPruebaX, tamPruebaY, tamPruebaZ];

		for (int x = 0; x < tamPruebaX; x++) {
			for (int y = 0; y < tamPruebaY; y++) {
				for (int z = 0; z < tamPruebaZ; z++) {
					existencia [x, y, z] = UnityEngine.Random.value < probabilidadGen;
					metadata [x, y, z] = 0;
				}
			}
		}

		generarPadding ();
		recrearModeloCompleto ();
	}

	void generarPadding(){
		existencia_pad = new bool[existencia.GetLength (0) + 2, existencia.GetLength (1) + 2, existencia.GetLength (2) + 2];
		if (metadata != null) {
			metadata_pad = new int[metadata.GetLength (0) + 2, metadata.GetLength (1) + 2, metadata.GetLength (2) + 2];
		}
		for (int x = 0; x < existencia.GetLength (0); x++) {
			for (int y = 0; y < existencia.GetLength (1); y++) {
				for (int z = 0; z < existencia.GetLength (2); z++) {
					existencia_pad [x + 1, y + 1, z + 1] = existencia [x, y, z];
					if (metadata != null) {
						metadata_pad [x + 1, y + 1, z + 1] = metadata [x, y, z];
					}
				}
			}
		}
	}
	void recrearModeloCompleto(){
		//Quaternion quat = Quaternion.Inverse(transform.localRotation);
		//transform.rotation = Quaternion.identity;

		for (int x = 0; x < existencia.GetLength(0); x++) {
			for (int y = 0; y < existencia.GetLength(1); y++) {
				for (int z = 0; z < existencia.GetLength(2); z++) {
					if (existencia [x, y, z]) {

						bool[,,] mapaVecindad = new bool[3, 3, 3];
						for (int xx = 0; xx <= 2; xx++) {
							for (int yy = 0; yy <= 2; yy++) {
								for (int zz = 0; zz <= 2; zz++) {
									mapaVecindad [xx, yy, zz] = existencia_pad [x + xx, y + yy, z + zz];
								}
							}
						}
						GameObject go = new GameObject ();

						TrozoPieza tp = go.AddComponent<TrozoPieza> ();
						int metadata_leido;
						if (metadata != null) {
							metadata_leido = metadata_pad [x + 1, y + 1, z + 1];
						} else {
							metadata_leido = 0;
						}
						tp.generarTrozoPieza (mapaVecindad, metadata_leido, tipoPieza);
						PintadorTrozos.pintarTrozo (tp, tipoPieza, mapaVecindad, metadata_leido);

						go.transform.parent = transform;
						go.transform.localPosition = new Vector3 (x, y, z);
					}
				}
			}
		}

		//transform.rotation = quat;
	}

	public void inicializar(TipoPieza tipoPieza, Vector3Int dimensiones, bool[,,] existencia, int[,,] metadata){
		DataJuego.i.cargarDataMuestrarios ();

		this.tipoPieza = tipoPieza;
		this.dimensiones = dimensiones;
		this.existencia = existencia;
		this.metadata = metadata;
		generarPadding ();
		recrearModeloCompleto ();
	}

	void Start(){
		generar ();
	}
}

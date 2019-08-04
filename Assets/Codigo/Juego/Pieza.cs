using UnityEngine;
using System;

public class Pieza : MonoBehaviour {


	public enum TipoPieza {TERRENO,
        CARAMELO_ROJO, CARAMELO_VERDE, CARAMELO_NARANJA, CARAMELO_AZUL, CARAMELO_AMARILLO, CARAMELO_VIOLETA,
        OBJETIVO_ROJO, OBJETIVO_VERDE, OBJETIVO_NARANJA, OBJETIVO_AZUL, OBJETIVO_AMARILLO, OBJETIVO_VIOLETA
    }

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

	public Array3DBool existencia; //Matriz que contiene la existencia de la pieza. grilla[x,y,z] es verdadero si la pieza contiene un trozo en esas coordenadas (locales)
	public Array3DInt metadata; //Matriz que contiene la metadata de la pieza. Debería ser siempre del mismo tamaño que grilla.

	[SerializeField] Array3DBool existencia_pad; //Contiene una versión paddeada con falses de la variable grilla
	[SerializeField] Array3DInt metadata_pad; //Contiene una versión paddeada con 0s de la variable metadata

	//=======GENERACIÓN=======
	[ContextMenu("Generar prueba terreno")]
	void pruebaTerreno(){
		limpiar ();
		for(int x = 0; x < this.metadata.largoX; x++){
			for (int y = 0; y < this.metadata.largoY; y++) {
				for (int z = 0; z < this.metadata.largoZ; z++) {
					if (this.existencia [x, y, z]) {
						if ((y < this.metadata.largoY - 2 &&
							(!this.existencia [x, y + 2, z] && this.existencia [x, y + 1, z])) ||
							((y == this.metadata.largoY - 2) && this.existencia[x,y+1,z])) {
							this.metadata [x, y, z] = 1;
						} else if(y < this.metadata.largoY - 2 && (this.existencia[x,y+2,z] && this.existencia[x,y+1,z])){
							this.metadata [x, y, z] = 2;
						}
					}
				}
			}
		}
		this.generarPadding ();

		recrearModeloCompleto ();
		//this.actualizarSegunMetadata ();
	}

	void actualizarSegunMetadata(){
		
	}

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
		dimensiones = new Vector3Int (largoX, largoY, largoZ);
		existencia = new Array3DBool(largoX, largoY, largoZ);
		metadata = new Array3DInt ();
		if (DataJuego.cantidadEstadosTipoPieza (tipoPieza) != 1) {
			metadata = new Array3DInt(largoX, largoY, largoZ);
		}

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

		existencia = new Array3DBool(tamPruebaX, tamPruebaY, tamPruebaZ);
		metadata = new Array3DInt(tamPruebaX, tamPruebaY, tamPruebaZ);

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
		existencia_pad = new Array3DBool(existencia.largoX + 2, existencia.largoY + 2, existencia.largoZ + 2);
		metadata_pad = new Array3DInt ();
		if (!metadata.esNull()) {
			metadata_pad = new Array3DInt(metadata.largoX + 2, metadata.largoY + 2, metadata.largoZ + 2);
		}
		for (int x = 0; x < existencia.largoX; x++) {
			for (int y = 0; y < existencia.largoY; y++) {
				for (int z = 0; z < existencia.largoZ; z++) {
					existencia_pad [x + 1, y + 1, z + 1] = existencia [x, y, z];
					if (!metadata.esNull()) {
						metadata_pad [x + 1, y + 1, z + 1] = metadata [x, y, z];
					}
				}
			}
		}
	}
	void recrearModeloCompleto(){
		//Quaternion quat = Quaternion.Inverse(transform.localRotation);
		//transform.rotation = Quaternion.identity;

		for (int x = 0; x < existencia.largoX; x++) {
			for (int y = 0; y < existencia.largoY; y++) {
				for (int z = 0; z < existencia.largoZ; z++) {
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
						if (!metadata.esNull()) {
							metadata_leido = metadata_pad [x + 1, y + 1, z + 1];
						} else {
							metadata_leido = 0;
						}
						tp.generarTrozoPieza (mapaVecindad, metadata_leido, tipoPieza);

						go.transform.parent = transform;
						go.transform.localPosition = new Vector3 (x, y, z);
					}
				}
			}
		}

		//transform.rotation = quat;
	}

	public void inicializar(TipoPieza tipoPieza, Vector3Int dimensiones, Vector3Int posicion, Array3DBool existencia, Array3DInt metadata){
		DataJuego.i.cargarDataMuestrarios ();

		this.tipoPieza = tipoPieza;
		this.dimensiones = dimensiones;
		this.existencia = existencia;
		this.metadata = metadata;
        this.posicion = posicion;
		generarPadding ();
		recrearModeloCompleto ();
	}
}

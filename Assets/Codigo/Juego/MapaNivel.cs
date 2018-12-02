using UnityEngine;
using System;

public class MapaNivel : MonoBehaviour {

	//=======VARIABLES PRIVADAS=======


	enum TipoMapaNivel {MAPA_2D, MAPA_3D};
	TipoMapaNivel tipoMapaNivel;

	//-------------------GENERACION----------------------

	string mapaPrueba = "1/10,10,10/1;3:4:5;010:111:111:101:011:110:100:111:000:010:000:111:000:010:000:111:000:110:101:011";
	string charsMetadata = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ#$";

	/// <summary>
	/// Genera el MapaNivel a partir de su código explícito.
	/// </summary>
	/// <returns><c>true</c>, si el código es correcto y se generó el MapaNivel correctamente, <c>false</c> en caso contrario.</returns>
	/// <param name="codigo">Código explícito del MapaNivel.</param>
	bool generarDesdeCodigo(string codigo){
		//	EN ESTA EXPLICACIÓN, SE ASUME QUE DataJuego.charsSeparadores =  "/,.;:"
		//
		//TODO EXPLANEISHON
		//

		//descomprimirCodigo (codigo);
		string[] split = codigo.Split (DataJuego.charsSeparadores[0]);
		if (split.Length <= 2) {
			logErrorConstruccion ("No hay suficientes datos separados, sólo hay " + split.Length, codigo);
			return false;
		}

		//------TIPO DE MAPA NIVEL (0 o 1)------
		string str_tipoMapaNivel = split [0];
		int int_tipoMapaNivel_leido;

		if (!int.TryParse (str_tipoMapaNivel, out int_tipoMapaNivel_leido)) {
			logErrorConstruccion ("Tipo de MapaNivel sólo puede ser 0 para Mapa_2D o 1 para Mapa_3D", codigo);
			return false;
		}

		TipoMapaNivel tipoMapaNivel_leido = (TipoMapaNivel)int_tipoMapaNivel_leido;

		//------DIMENSIONES DEL MAPA (x,y,z)------
		string str_dimensiones = split[1];
		string[] splitDimensiones = str_dimensiones.Split (DataJuego.charsSeparadores[1]);

		if ((tipoMapaNivel_leido == TipoMapaNivel.MAPA_2D && splitDimensiones.Length != 2) ||
			(tipoMapaNivel_leido == TipoMapaNivel.MAPA_3D && splitDimensiones.Length != 3)) {
			logErrorConstruccion ("El tipo de MapaNivel especificado (" + tipoMapaNivel_leido + ") no corresponde con la cantidad de dimensiones especificada (" + splitDimensiones.Length + ")", codigo);
			return false;
		}

		string str_dimX = splitDimensiones [0];
		string str_dimY = splitDimensiones [1];

		int dimX_leido;
		int dimY_leido;
		int dimZ_leido;

		if (!int.TryParse (str_dimX, out dimX_leido)) {
			logErrorConstruccion ("Dimensión X del mapa no se traduce como un número entero", codigo);
			return false;
		}
		if (!int.TryParse (str_dimY, out dimY_leido)) {
			logErrorConstruccion ("Dimensión Y del mapa no se traduce como un número entero", codigo);
			return false;
		}
		if (tipoMapaNivel_leido == TipoMapaNivel.MAPA_3D) {
			string str_dimZ = splitDimensiones [2];
			if (!int.TryParse (str_dimZ, out dimZ_leido)) {
				logErrorConstruccion ("Dimensión Z del mapa no se traduce como un número entero", codigo);
				return false;
			}
		}

		//-----PIEZAS (...)-----

		string str_piezas = split[2];
		string[] splitPiezas = str_piezas.Split (DataJuego.charsSeparadores [1]);
		foreach (string pieza in splitPiezas) {
			string[] datos = pieza.Split (DataJuego.charsSeparadores [2]);
			if (!decodificarPieza (datos, tipoMapaNivel_leido)) {
				logErrorConstruccion ("Error al decodificar la pieza a partir del código '" + pieza + "'", codigo);
				return false;
			}
		}

		return false;	
	}

	bool decodificarPieza(string[] datosPieza, TipoMapaNivel tipoMapa){
		//	EN ESTA EXPLICACIÓN, SE ASUME QUE DataJuego.charsSeparadores =  "/,;:"
		//
		//	Formato del código de una pieza:
		//	[tipoPieza;dimensiones;existencia;(opcional)metadata]
		//
		// dimensiones: largoX:largoY         (si tipoMapa es 2D)
		// dimensiones: largoX:largoY:largoZ  (si tipoMapa es 3D)
		//
		// existencia: fila1:fila2:fila3:...:filaN
		// fila de existencia (ejemplo): 100011011011111 (caracteres 0 y 1)
		//
		// metadata: fila1:fila2:fila3:...:filaN
		// fila de metadata (ejemplos) (usando caracteres necesarios de la constante estática "charsMetadata" de DataJuego):
		//                   >   011011101010  > utiliza los primeros 2 caracteres (0,1) para una pieza con 2 estados posibles por celda
		//                   >   012012110200  > utiliza los primeros 3 caracteres (0,1,2) para una pieza con 3 estados posibles por celda
		//                   >   0b983a354bab401293810112b  > utiliza los primeros 12 caracteres (0,1,2,3,4,5,6,7,8,9,a,b) para una pieza con 12 estados posibles por celda
		//                   >  etc.
		//  


		//---VERIFICACIÓN DATOS VACÍO---
		if (datosPieza.Length == 0) {
			logErrorPieza ("Array datosPieza vacío");
			return false;
		}

		//---TIPO PIEZA---
		string str_tipoPieza = datosPieza [0];
		int int_tipoPieza;
		if (!int.TryParse (str_tipoPieza, out int_tipoPieza)) {
			logErrorPieza ("TipoPieza leído no se traduce como un número entero");
			return false;
		}

		//---VERIFICACIÓN CANTIDAD DE DATOS---
		Pieza.TipoPieza tipoPieza_leido = (Pieza.TipoPieza)int_tipoPieza;
		int cantidadEstados = DataJuego.cantidadEstadosTipoPieza (tipoPieza_leido);
		if (cantidadEstados == 0) {
			logErrorPieza ("La cantidad de estados para el TipoPieza " + Enum.GetName (typeof(Pieza.TipoPieza), int_tipoPieza) + " no ha sido especificada");
			return false;
		}
		bool tieneMetadata = cantidadEstados != 1;

		if(tieneMetadata){
			if (datosPieza.Length != 4) {
				logErrorPieza ("DatosPieza provisto no contiene la cantidad de datos correcto (4 para el TipoPieza " + Enum.GetName(typeof(Pieza.TipoPieza), int_tipoPieza) + ")");
				return false;
			} else {
				logErrorPieza ("DatosPieza provisto no contiene la cantidad de datos correcto (3 para el TipoPieza " + Enum.GetName (typeof(Pieza.TipoPieza), int_tipoPieza) + ")");
				return false;
			}
		}

		//---LEER TIPO PIEZA---
		string str_dimensiones = datosPieza [1];
		string[] splitDimensiones = str_dimensiones.Split (DataJuego.charsSeparadores [3]);
		if ((tipoMapa == TipoMapaNivel.MAPA_2D && splitDimensiones.Length != 2) ||
		   (tipoMapa == TipoMapaNivel.MAPA_3D && splitDimensiones.Length != 3)) {
			logErrorPieza ("La cantidad de dimensiones especificadas para la pieza (" + splitDimensiones.Length + ") no corresponde con el TipoMapaNivel especificado (" + tipoMapa + ")");
			return false;
		}

		//---LEER DIMENSIONES---
		string str_largoX = splitDimensiones [0];
		string str_largoY = splitDimensiones [1];

		int largoX_leido;
		int largoY_leido;
		int largoZ_leido = 1;

		if (!int.TryParse (str_largoX, out largoX_leido)) {
			logErrorPieza ("Dimensión X de la pieza no se traduce como un número entero");
			return false;
		}
		if (!int.TryParse (str_largoY, out largoY_leido)) {
			logErrorPieza ("Dimensión Y de la pieza no se traduce como un número entero");
			return false;
		}

		if (tipoMapa == TipoMapaNivel.MAPA_3D) {
			string str_largoZ = splitDimensiones [2];
			if (!int.TryParse (str_largoZ, out largoZ_leido)) {
				logErrorPieza ("Dimensión Z de la pieza no se traduce como un número entero");
				return false;
			}
		}


		//---LEER EXISTENCIA---
		string str_existencia = datosPieza [2];
		string[] split_existencia = str_existencia.Split (DataJuego.charsSeparadores [3]);
		if (split_existencia.Length != largoY_leido * largoZ_leido) {
			logErrorPieza ("La cantidad de filas de la pieza no se corresponde con las dimensiones especificadas anteriormente (X:"+largoX_leido+", Y:"+largoY_leido+", Z:"+largoZ_leido+")");
			return false;
		}
		bool[,,] existencia_leido = new bool[largoX_leido, largoZ_leido, largoY_leido];
			
		int i = 0;
		for (int z = 0; z < largoZ_leido; z++) {
			for (int y = 0; y < largoY_leido; y++) {
				string fila = split_existencia [i];
				if (fila.Length != largoX_leido) {
					logErrorPieza ("(leyendo existencia) La fila en (Y:"+largoY_leido+", Z:"+largoZ_leido+") no tiene el largo especificado (X:"+largoX_leido+")");
					return false;
				}
				for (int x = 0; x < largoX_leido; x++) {
					existencia_leido [x, z, y] = fila [x] == '1';
				}

				i++;
			}
		}


		int[,,] metadata_leido = null;

		//---LEER METADATA---
		if (tieneMetadata) {
			
			string str_metadata = datosPieza [3];
			string[] split_metadata = str_metadata.Split (DataJuego.charsSeparadores [3]);
			if (split_metadata.Length != largoY_leido * largoZ_leido) {
				logErrorPieza ("La cantidad de filas de la metadata de la pieza no se corresponde con las dimensiones especificadas anteriormente (X:"+largoX_leido+", Y:"+largoY_leido+", Z:"+largoZ_leido+")");
				return false;
			}

			metadata_leido = new int[largoX_leido, largoZ_leido, largoY_leido];

			i = 0;
			for (int z = 0; z < largoZ_leido; z++) {
				for (int y = 0; y < largoY_leido; y++) {
					string fila = split_metadata [i];
					if (fila.Length != largoX_leido) {
						logErrorPieza ("(leyendo metadata) La fila en (Y:"+largoY_leido+", Z:"+largoZ_leido+") no tiene el largo especificado (X:"+largoX_leido+")");
						return false;
					}
					for (int x = 0; x < largoX_leido; x++) {
						int metadata = DataJuego.metadataAInt(fila [x]);
						if (metadata == -1) {
							logErrorPieza ("Caracter desconocido en metadata de la pieza");
							return false;
						}
						if (metadata >= cantidadEstados) {
							logErrorPieza ("El número de metadata " + metadata + ", cuyo caracter es '" + fila [x] + "' es mayor o igual que la cantidad de estados especificada (" + cantidadEstados + ") para el TipoPieza " + Enum.GetName (typeof(Pieza.TipoPieza), int_tipoPieza));
							return false;
						}
						metadata_leido [x, z, y] = metadata;
					}

					i++;
				}
			}
		}

		GameObject goPieza = new GameObject ();
		Pieza nuevaPieza = goPieza.AddComponent<Pieza> ();
		goPieza.transform.parent = transform;
		nuevaPieza.inicializar (tipoPieza_leido, new Vector3Int (largoX_leido, largoY_leido, largoZ_leido), existencia_leido, metadata_leido);

		return true;
	}


	/// <summary>
	/// Comprime el código literal del MapaNivel y devuelve el resultado.
	/// </summary>
	/// <returns>El código comprimido.</returns>
	/// <param name="codigo">Código explícito del mapaNivel.</param>
	string comprimirCodigo(string codigo){
		
		return null;	
	}

	/// <summary>
	/// Recupera el código literal a partir del código comprimido del MapaNivel, a partir del cual luego se podrán leer los datos.
	/// </summary>
	/// <returns>Código explícito del MapaNivel.</returns>
	/// <param name="codigo">Codigo comprimido del MapaNivel.</param>
	string descomprimirCodigo(string codigo){
		return null;	

	}

	void logErrorConstruccion(string mensaje, string codigo){
		Debug.LogError ("\nError al construír MapaNivel a partir del código " + codigo + ":"+mensaje);
	}
	void logErrorPieza(string mensaje){
		Debug.LogError ("\nError al decodificar pieza:"+mensaje);
	}

	[ContextMenu("Prueba")]
	void prueba(){
		string charsInput = "01";
		string charsOutput = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ#$";
		string test = Codificado.comprimirCodigo("0101101101110", charsInput, charsOutput);
		Debug.Log ("CODIFICADO: " + test);
		Debug.Log("DECODIFICADO: " + Codificado.descomprimirCodigo(test, charsInput, charsOutput));
	}

	[ContextMenu("Prueba 2")]
	void prueba2(){
		generarDesdeCodigo (mapaPrueba);
	}
	

	//---------------------------------------------------
}

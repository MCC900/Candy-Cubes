using System;
using TMPro;
using UnityEngine;

public class DisplayInputNumero : LabelUI
{
    public void cambiarNumero(int nuevoNumero)
    {
        this.cambiarTexto(nuevoNumero.ToString());
    }
}

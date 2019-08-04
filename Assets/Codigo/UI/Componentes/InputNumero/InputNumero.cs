using System;
using UnityEngine;

public class InputNumero : MonoBehaviour
{
    public LabelUI label;
    public DisplayInputNumero displayNumero;
    public BotonCambioInputNumero btnMas;
    public BotonCambioInputNumero btnMenos;
    public bool desactivado;

    public int minimo;
    public int maximo;

    public int numero = 8;

    private void Start()
    {
        string texto = this.displayNumero.tmproTexto.text.ToString();
        int outNumero;
        if (int.TryParse(texto, out outNumero))
        {
            this.numero = outNumero;
        }
    }

    public void incrementar(int incremento)
    {
        this.numero += incremento;

        if(this.numero <= this.minimo)
        {
            this.numero = this.minimo;
            this.desactivarBoton(this.btnMenos);
            this.activarBoton(this.btnMas);
        } else if(this.numero >= this.maximo)
        {
            this.numero = this.maximo;
            this.activarBoton(this.btnMenos);
            this.desactivarBoton(this.btnMas);
        } else
        {
            this.activarBoton(this.btnMenos);
            this.activarBoton(this.btnMas);
        }
        this.displayNumero.cambiarNumero(this.numero);
    }

    void desactivarBoton(BotonCambioInputNumero btn)
    {
        if (!btn.desactivado)
        {
            btn.meshRenderer.sharedMaterials = DataUI.i.matsOpcionDeseleccionada;
            btn.rendererTextoHijo.SetMaterial(DataUI.i.matTextoOpcionDeseleccionada, 0);
            btn.desactivado = true;
            btn.box2DCollider.enabled = false;
        }
    }

    void activarBoton(BotonCambioInputNumero btn)
    {
        btn.meshRenderer.sharedMaterials = DataUI.i.matsOpcionSeleccionada;
        btn.rendererTextoHijo.SetMaterial(DataUI.i.matTextoOpcionSeleccionada, 0);
        btn.desactivado = false;
        btn.box2DCollider.enabled = true;
    }

    public void desactivar()
    {
        if(!this.desactivado)
        {
            this.desactivado = true;
            desactivarBoton(btnMas);
            desactivarBoton(btnMenos);
            displayNumero.desactivar();
            label.desactivar();
        }
    }

    public void activar()
    {
        if (this.desactivado)
        {
            this.desactivado = false;

            if (this.numero <= this.minimo)
                desactivarBoton(btnMenos);
            else
                activarBoton(btnMenos);

            if (this.numero >= this.maximo)
                desactivarBoton(btnMas);
            else
                activarBoton(btnMas);

            displayNumero.activar();
            label.activar();
        }
    }
}

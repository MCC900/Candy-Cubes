using System;
using UnityEngine;

public class PresetPanelUI : MonoBehaviour
{
    public Material material;
    public Sprite spriteReferencia;
    public PanelUI.TipoRepeticionPanelUI tipoRepeticion = PanelUI.TipoRepeticionPanelUI.AJUSTAR;
    public bool pixelPerfect = true;

    public ValorUI tamanoEsquinaTopLeftX = new ValorUI(ValorUI.TipoValorUI.FIJO_PX, 10F);
    public ValorUI tamanoEsquinaTopLeftY = new ValorUI(ValorUI.TipoValorUI.FIJO_PX, 10F);
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using StaserSDK;
using StaserSDK.Utilities;
using UnityEngine.Events;
using Zenject;

public partial class MaterialFade : MonoBehaviour
{
    [SerializeField] private ShaderType _shaderType;

    [Inject] private Timer _timer;
    
    public void Fade(Renderer renderer, Material destinationMaterial, float fadeDuration, bool returnStartMaterial = false, float returnDelay = 0.0f)
    {
        Fade(material => renderer.material = material, renderer.material, destinationMaterial, fadeDuration, returnStartMaterial, returnDelay);
    }

    public void Fade(UnityAction<Material> action, Material startMaterial, Material destinationMaterial, float fadeDuration, bool returnStartMaterial = false, float returnDelay = 0.0f)
    {
        Material tempMaterial = new Material(startMaterial);
        Color startColor = startMaterial.GetColor(GetColorParameter());
        Color destinationColor = destinationMaterial.GetColor(GetColorParameter());
        tempMaterial.SetColor(GetColorParameter(), startColor);
        action(tempMaterial);
        Fade(tempMaterial, startColor, destinationColor, fadeDuration);
        if(returnStartMaterial == false) return;   
        
        _timer.ExecuteWithDelay(() => Fade(tempMaterial,destinationColor, startColor, fadeDuration), returnDelay);
        _timer.ExecuteWithDelay(() => action(tempMaterial), returnDelay + fadeDuration);
    }

    private void Fade(Material tempMaterial, Color startColor, Color endColor, float duration)
    {
        Animations.ColorFade(this, startColor, endColor, color=>tempMaterial.SetColor(GetColorParameter(), color), duration);
    }

    private string GetColorParameter()
    {
        switch (_shaderType)
        {
            case ShaderType.MKToon:
                return "_AlbedoColor";
            default:
                return "_BaseColor";
        }
    }
    
}

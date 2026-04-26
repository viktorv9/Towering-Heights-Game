using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRandomizer : MonoBehaviour
{
    Renderer rend;
    MaterialPropertyBlock propBlock;
    
    void Awake()
    {
        rend = GetComponent<Renderer>();
        propBlock = new MaterialPropertyBlock();
    }
    
    void Start()
    {
        rend.GetPropertyBlock(propBlock);
    
        // Slight random color variation
        Color baseColor = rend.sharedMaterial.GetColor("_BaseColor");
        float newColorR = Random.Range(0.8f, 1f) * baseColor.r;
    
        propBlock.SetColor("_BaseColor", new Color(newColorR, baseColor.g, baseColor.b, baseColor.a));
    
        rend.SetPropertyBlock(propBlock);
    }
}

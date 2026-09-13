using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTransition : MonoBehaviour
{
    private static readonly int PixelSize = Shader.PropertyToID("_PixelSize");

    [SerializeField] private FullScreenPassRendererFeature fsprf;
    [SerializeField] private float transitionDuration = 0.5f;
    
    private const float maxPixelSize = 200;
    private const float minPixelSize = 1;
    
    void Start()
    {
        StartCoroutine(PixelTransition());
    }

    private IEnumerator PixelTransition() {
        fsprf.SetActive(true);
        fsprf.passMaterial.SetFloat(PixelSize, maxPixelSize);
        
        float startTime = Time.time;

        while (Time.time - startTime < transitionDuration) {
            float t = (Time.time - startTime) / transitionDuration;
            fsprf.passMaterial.SetFloat(PixelSize, Mathf.Lerp(maxPixelSize, minPixelSize, t));
            yield return null;
        }
        
        fsprf.SetActive(false);
    }
}

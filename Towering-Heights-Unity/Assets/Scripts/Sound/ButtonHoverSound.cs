using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;

public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private EventReference hoverSound;

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.PlayOneShot(hoverSound, Camera.main.transform.position);
    }
}
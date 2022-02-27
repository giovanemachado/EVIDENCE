using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        LeanTween.cancel(gameObject);
        transform.LeanScale(new Vector2(1.2f, 1.2f), 0.30f);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(gameObject);
        transform.LeanScale(new Vector2(1.0f, 1.0f), 0.07f);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PhotoController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Photo Photo;

    TextMeshProUGUI title;
    TextMeshProUGUI description;
    Sprite sprite;
    bool isMouseOver = false;
    bool isDragging = false;
    Vector3 cachedScale;
    int cachedIndex;

    RectTransform rectTransform;
    CanvasGroup canvasGroup;

    Vector2 originalPosition;
    public Canvas canvas;

    public GameObject GameController;
    GameController gameController;

    void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        gameController = GameController.GetComponent<GameController>();
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;

        if (!isDragging)
        {
            cachedIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();

            cachedScale = transform.localScale;
            LeanTween.cancel(gameObject);
            transform.LeanScale(new Vector2(1.8f, 1.8f), 0.30f);
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (!isDragging)
        {
            LeanTween.cancel(gameObject);
            transform.LeanScale(new Vector2(1.0f, 1.0f), 0.07f);
            transform.SetSiblingIndex(cachedIndex);
        }

        isMouseOver = false;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.LeanScale(new Vector2(1.0f, 1.0f), 0.07f);
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        int boardIndex = eventData.hovered.FindIndex(hoveredGO => hoveredGO.name == "Board");
        int backToEndIndex = eventData.hovered.FindIndex(hoveredGO => hoveredGO.name == "BackToEnd");

        if (boardIndex != -1)  gameController.PhotoMovedToBoard(Photo, gameObject);
        if (backToEndIndex != -1) gameController.PhotoMovedToEnd(Photo, gameObject);

        if (backToEndIndex == -1 && boardIndex == -1) rectTransform.anchoredPosition = originalPosition;

        isDragging = false;
    }
}

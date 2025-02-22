using UnityEngine;
using UnityEngine.UI;

public abstract class BasePanel : MonoBehaviour
{
    protected Canvas canvas;
    protected CanvasGroup canvasGroup;
    public bool isVisible { get; protected set; }

    protected virtual void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        
        if (canvas == null)
            canvas = gameObject.AddComponent<Canvas>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        Hide();
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
        isVisible = true;
    }

    public virtual void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        isVisible = false;
        gameObject.SetActive(false);
    }

    public virtual void Toggle()
    {
        if (isVisible)
            Hide();
        else
            Show();
    }
} 
using DG.Tweening;
using UnityEngine;

public static class Helper
{
    public const float RoadWidth = 270;
    public const float MaxInitialSpeed = 350; // to show in progress bar
    public const float MaxAcceleration = 15; // to show in progress bar
    public const float MaxSpeedLimit = 1200;
    public const float MaxCarRotateLimit = 20;
    public const float PanelFadingDuration = 0.15f;
    public const float CarImageTransitionDuration = 0.4f;

    public static void FadeObjectView(GameObject mainPanelObj, GameObject innerPanelObj, bool show, float duration = -1, System.Action callback = null)
    {
        if (duration < 0) duration = Helper.PanelFadingDuration;
        float startScale, endScale;
        (startScale, endScale) = (0.8f, 1); //to show
        CanvasGroup cg = mainPanelObj.GetComponent<CanvasGroup>();
        if (cg == null) cg = mainPanelObj.AddComponent<CanvasGroup>();
        if (show)
        {
            mainPanelObj.SetActive(true);
            innerPanelObj.SetActive(true);
            cg.alpha = 0;
            cg.DOFade(1, duration).SetEase(Ease.Linear).OnComplete(() => callback?.Invoke());
        }
        else
        {
            (startScale, endScale) = (endScale, startScale);
            cg.alpha = 1;
            cg.DOFade(0, duration).SetEase(Ease.Linear).OnComplete(() =>
            {
                mainPanelObj.SetActive(false);
                innerPanelObj.SetActive(false);
                callback?.Invoke();
            });
        }
        Transform t = innerPanelObj.transform;
        t.localScale = startScale * Vector3.one;
        t.DOScale(endScale, duration);
    }
}

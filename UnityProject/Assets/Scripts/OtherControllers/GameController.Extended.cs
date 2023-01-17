using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public partial class GameController
{
    private void ShowInitialCarSelectionView()
    {
        carImages[1].color = Color.clear;
        displayCarIndex = 1;
        AnimateCarView(false);
    }

    private void AnimateCarView(bool showNext)
    {
        float centeredImageTargetPosX = 100;
        if (showNext) displayCarIndex++;
        else displayCarIndex--;
        int showImageIndex = this.displayCarIndex % 2;
        int hideImageIndex = (showImageIndex + 1) % 2;
        carPrevButton.interactable = displayCarIndex > 0;
        carNextButton.interactable = displayCarIndex < cars.Length-1;
        if (showNext)
        {
            centeredImageTargetPosX = -centeredImageTargetPosX;
        }
        carImages[showImageIndex].sprite = cars[displayCarIndex].sprite;
        carImages[showImageIndex].color = Color.clear;
        carImages[hideImageIndex].color = Color.white;
        carImages[showImageIndex].DOColor(Color.white, Helper.CarImageTransitionDuration);
        carImages[hideImageIndex].DOColor(Color.clear, Helper.CarImageTransitionDuration);
        RectTransform rtShow = carImages[showImageIndex].GetComponent<RectTransform>();
        RectTransform rtHide = carImages[hideImageIndex].GetComponent<RectTransform>();
        rtShow.anchoredPosition = centeredImageTargetPosX * Vector3.left;
        rtHide.anchoredPosition = Vector3.zero;
        DOTween.Kill(rtShow);
        rtShow.DOAnchorPos(Vector3.zero, Helper.CarImageTransitionDuration);
        rtHide.DOAnchorPos(centeredImageTargetPosX * Vector3.right, Helper.CarImageTransitionDuration);

        float halfTime = Helper.CarImageTransitionDuration / 2;
        carSpeedBarImage.DOFillAmount(0, halfTime);
        carAccelerationBarImage.DOFillAmount(0, halfTime).OnComplete(() =>
        {
            carSpeedBarImage.DOFillAmount(cars[displayCarIndex].initialSpeed / 200, halfTime);
            carAccelerationBarImage.DOFillAmount(cars[displayCarIndex].acceleration / 30, halfTime);
        });
    }

    private void ShowInitialScoreText()
    {
        popupPanelObj.GetComponent<Image>().color = Color.clear;
        popupPanelObj.SetActive(true);
        scoreText.gameObject.SetActive(true);
        scoreText.text = "0";
        scoreText.fontSize = 30;
        scoreText.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    private void AnimateScorePanel()
    {
        float animationDuration = 0.8f;
        restartButton.interactable = false;
        backToLobbyButton.interactable = false;
        popupPanelObj.GetComponent<Image>().color = (new Vector4(0, 0, 0, 0.5f));
        Helper.FadeObjectView(popupPanelObj, scorePanelObj, true);

        scoreText.transform.DOMove(scoreTextTargetOnScorePanelTransform.position, animationDuration);
        int val = 30, endSize = 50;
        DOTween.To(() => val, x => val = x, endSize, animationDuration).OnUpdate(() =>
        {
            scoreText.fontSize = val;
        }).OnComplete(() =>
        {
            restartButton.interactable = true;
            backToLobbyButton.interactable = true;
        });
    }
}

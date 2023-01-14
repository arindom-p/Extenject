using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public partial class GameController
{
    private void ShowInitialCarSelectionView()
    {
        displayCarIndex = 1;
        carImages[0].sprite = cars[0].sprite;
        carImages[1].sprite = cars[1].sprite;
        AnimateCarView(false);
    }

    private void AnimateCarView(bool showNext)
    {
        if (showNext) displayCarIndex++;
        else displayCarIndex--;

        carPrevButton.interactable = displayCarIndex > 0;
        carNextButton.interactable = displayCarIndex < 1;
        Transform t1 = carImages[0].transform;
        Transform t2 = carImages[1].transform;
        Image img1 = carImages[0];
        Image img2 = carImages[1];
        float imgTargetPosX = 100;
        if (displayCarIndex == 1)
        {
            (img1, img2) = (img2, img1);
            (t1, t2) = (t2, t1);
            imgTargetPosX = -imgTargetPosX;
        }
        t1.DOLocalMoveX(0, Helper.CarImageTransitionDuration);
        t2.DOLocalMoveX(imgTargetPosX, Helper.CarImageTransitionDuration);
        img1.color = Color.clear;
        img2.color = Color.white;
        img1.DOColor(Color.white, Helper.CarImageTransitionDuration);
        img2.DOColor(Color.clear, Helper.CarImageTransitionDuration);

        float halfTime = Helper.CarImageTransitionDuration / 2;
        carSpeedBarImage.DOFillAmount(0, halfTime);
        carAccelerationBarImage.DOFillAmount(0, halfTime).OnComplete(() =>
        {
            carSpeedBarImage.DOFillAmount(cars[displayCarIndex].initialSpeed / 100, halfTime);
            carAccelerationBarImage.DOFillAmount(cars[displayCarIndex].acceleration / 50, halfTime);
        });
    }
}

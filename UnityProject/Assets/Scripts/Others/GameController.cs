using UnityEngine;
using UnityEngine.UI;
using Zenject;

public partial class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject gameCanvasObj,
        uiCanvasObj,
        gamePanelObj,
        lobbyPanelObj,
        popupPanelObj,
        scorePanelObj,
        otherPanelsObj,
        carSelectionPenlObj,
        settingPanelObj;

    [SerializeField]
    private Button startButton,
        exitButton,
        settingButton,
        carPrevButton,
        carNextButton,
        carSelectButton,
        commonBackButton,
        restartButton,
        backToLobbyButton;

    [SerializeField] private Image[] carImages;
    [SerializeField]
    private Image carSpeedBarImage,
        carAccelerationBarImage;
    [SerializeField] private Text scoreText;
    [SerializeField] private Transform scoreTextTargetOnScorePanelTransform;

    private SignalBus signalBus;

    private int displayCarIndex;
    private CarData[] cars;

    [Inject]
    private void Construct(CarData[] cars, SignalBus signalBus)
    {
        this.cars = cars;
        this.signalBus = signalBus;
    }

    private void Start()
    {
        if (carImages.Length != 2)
            Debug.LogError("Implementation will not work for car selection. 2 Image ref is needed");
        ResgisterButtons();
        CommonLobbyAction();
    }

    private void CommonLobbyAction()
    {
        DisableAllPanels();
        uiCanvasObj.SetActive(true);
        lobbyPanelObj.SetActive(true);
    }

    private void DisableAllPanels()
    {
        gameCanvasObj.SetActive(false);
        uiCanvasObj.SetActive(false);
        gamePanelObj.SetActive(false);
        lobbyPanelObj.SetActive(false);
        popupPanelObj.SetActive(false);
        scorePanelObj.SetActive(false);
        otherPanelsObj.SetActive(false);
        carSelectionPenlObj.SetActive(false);
        settingPanelObj.SetActive(false);
    }

    private void ResgisterButtons()
    {
        startButton.onClick.AddListener(OnPressedStartButton);
        exitButton.onClick.AddListener(OnPressedExitButton);
        carPrevButton.onClick.AddListener(OnPressedCarPrevButton);
        carNextButton.onClick.AddListener(OnPressedCarNextButton);
        settingButton.onClick.AddListener(OnPressedSettingButton);
        carSelectButton.onClick.AddListener(OnPressedCarSelectButton);
        commonBackButton.onClick.AddListener(OnPressedCommonBackButton);
        restartButton.onClick.AddListener(OnPressedRestartButton);
        backToLobbyButton.onClick.AddListener(OnPressedBackToLobbyButton);
    }

    private void OnPressedStartButton()
    {
        ShowInitialCarSelectionView();
        Helper.FadeObjectView(otherPanelsObj, carSelectionPenlObj, true);
    }

    private void OnPressedExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
    }

    private void OnPressedSettingButton()
    {
        Helper.FadeObjectView(otherPanelsObj, settingPanelObj, true);
    }

    private void OnPressedCarPrevButton()
    {
        AnimateCarView(false);
    }

    private void OnPressedCarNextButton()
    {
        AnimateCarView(true);
    }

    private void OnPressedCarSelectButton()
    {
        Helper.FadeObjectView(otherPanelsObj, carSelectionPenlObj, false);
        Helper.FadeObjectView(gameCanvasObj, gamePanelObj, true);
        lobbyPanelObj.SetActive(false);
        ShowInitialScoreText();
        signalBus.Fire(new MatchStartedSignal(displayCarIndex));
    }

    private void OnPressedCommonBackButton()
    {
        GameObject obj = settingPanelObj.activeSelf ? settingPanelObj : carSelectionPenlObj;
        Helper.FadeObjectView(otherPanelsObj, obj, false);
    }

    private void OnPressedRestartButton()
    {
        scorePanelObj.SetActive(false);
        OnPressedCarSelectButton();
    }

    private void OnPressedBackToLobbyButton()
    {
        Helper.FadeObjectView(popupPanelObj, scorePanelObj, false);
        Helper.FadeObjectView(uiCanvasObj, lobbyPanelObj, true, callback: CommonLobbyAction);
    }

    public void OnRaceEnd()
    {
        AnimateScorePanel();
    }
}

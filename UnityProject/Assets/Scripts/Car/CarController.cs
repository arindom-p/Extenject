using UnityEngine;
using DG.Tweening;
using Zenject;
using System;

public class CarController : MonoBehaviour, ICarProperties
{
    [SerializeField] private UnityEngine.UI.Image carImage;
    /// <summary> Defines if the race started </summary>
    private bool isMoving = false,
        tweeningRotation = false;
    private float carPosLimitX;
    private RectTransform ownRt;
    private CarData[] cars;
    private Vector3 cachedCarPos; // just to make easy to set car position
    private SignalBus signalBus;

    #region From interface
    #region Helping veriables those should not be used from anywere except this implement
    private int _currentCarId = 0;
    private CarData _currentCarData;
    #endregion
    public int currentCarId
    {
        get => _currentCarId;
        private set
        {
            _currentCarId = value;
            _currentCarData = cars[_currentCarId];
        }
    }
    public CarData currentCarData
    {
        get => _currentCarData;
    }
    public float currentCarSpeed { get; private set; }
    #endregion

    [Inject]
    public void Construct(CarData[] cars, SignalBus signalBus)
    {
        this.cars = cars;
        this.signalBus = signalBus;

        currentCarId = 0; //just to initialize
    }

    void Start()
    {
        ownRt = GetComponent<RectTransform>();
        carPosLimitX = Helper.RoadWidth - ownRt.sizeDelta.x;
    }

    public void OnRaceStart(int carIndex)
    {
        currentCarId = carIndex;

        isMoving = true;
        currentCarSpeed = currentCarData.initialSpeed;
        carImage.sprite = currentCarData.sprite;
        cachedCarPos = ownRt.localPosition;
    }

    public void OnRaceEnd()
    {
        isMoving = false;
        signalBus.Fire(new GameEndedSignal());
    }

    private void Update()
    {
        if (isMoving)
        {
            currentCarSpeed += Time.deltaTime * currentCarData.acceleration;
            SetCurrentFrameProperty();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnRaceEnd();
    }

    private float GetInput()
    {
        float value;
#if UNITY_EDITOR
        value = Input.GetAxis("Horizontal");
#else
        // take touch input for mobile devices
#endif
        return value;
    }

    private void SetCurrentFrameProperty()
    {
        float dt = Time.deltaTime;
        float inputVal = GetInput();
        float currentRotZ = ownRt.eulerAngles.z;
        if (inputVal != 0)
        {
            { // handling rotation
                if (tweeningRotation)
                {
                    tweeningRotation = false;
                    DOTween.Kill(ownRt);
                }
                float res = currentRotZ - inputVal;
                if (res < 0) res += 360;
                if (res > 180) res = Mathf.Clamp(res, 360 - Helper.MaxCarRotateLimit, 360);
                else res = Mathf.Clamp(res, 0, Helper.MaxCarRotateLimit);
                ownRt.eulerAngles = res * Vector3.forward;
            }
            { // handling position
                float targetPosX = ownRt.localPosition.x + dt * 1f * inputVal * currentCarSpeed;
                targetPosX = Mathf.Clamp(targetPosX, -carPosLimitX, carPosLimitX);
                cachedCarPos.x = targetPosX;
                ownRt.localPosition = cachedCarPos;
            }
        }
        else if (currentRotZ != 0 && !tweeningRotation)
        {
            { // handling rotation to follow forward
                tweeningRotation = true;
                float targetRot = currentRotZ < 180 ? 0 : 360;
                float t = Math.Abs(targetRot - currentRotZ) / 180;
                ownRt.DORotate(targetRot * Vector3.forward, t).SetEase(Ease.Linear).OnComplete(() =>
                {
                    tweeningRotation = false;
                });
            }
        }
    }
}

using UnityEngine;
using DG.Tweening;
using Zenject;

public class CarController : MonoBehaviour, ICarProperties
{
    /// <summary> Defines if the race started </summary>
    private bool isMoving = false;
    private int _currentCarId;
    private RectTransform ownRt;
    private CarData[] cars;
    private Vector2 carSize;

    #region From interface
    int ICarProperties.currentCarId { get => _currentCarId; set { _currentCarId = value;} }
    CarData ICarProperties.currentCarData { get => cars[_currentCarId]; set { } }
    float ICarProperties.currentCarSpeed { get; set; }
    #endregion

    [Inject]
    public void Construct(CarData[] cars)
    {
        this.cars = cars;
    }

    void Start()
    {
        ownRt = GetComponent<RectTransform>();
        carSize = ownRt.sizeDelta;
        isMoving = true;
    }

    void Update()
    {
        if (isMoving)
        {
            SetCurrentFrameProperty();
        }
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
        float inputVal = GetInput();

    }
}

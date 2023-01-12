using UnityEngine;

public class MotionController : MonoBehaviour
{
    [SerializeField] private RectTransform[] roadRTs;

    private readonly int resetDistance = 10000;
    private int currentDistance,
        currentSpeed;
    private bool isMoving = false;

    void Start()
    {
        if (roadRTs.Length != 2) Debug.LogError("number of referenced road instances must be 2");
        isMoving = true;
    }

    private void Update()
    {
        if (isMoving)
        {

        }
    }

    public void StartingDisplay()
    {

    }


}

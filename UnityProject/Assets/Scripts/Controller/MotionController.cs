using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MotionController : MonoBehaviour
{
    [SerializeField] private RectTransform[] roadRTs;
    [SerializeField] private Transform obstacleParent;

    private RectTransform ownRt;
    private ICarProperties carProperties;
    private CarData currentCarData;
    private readonly int resetDistance = 10000;
    private int roadImageId;
    private float roadHeight,
        currentDistance;
    private bool isMoving = false;
    private Dictionary<int, Vector3> positionCacheDic = new Dictionary<int, Vector3>();

    [Inject]
    private void Construct(ICarProperties carProperties)
    {
        this.carProperties = carProperties;
    }

    void Start()
    {
        if (roadRTs.Length != 2) Debug.LogError("number of referenced road instances must be 2");
        roadHeight = roadRTs[0].sizeDelta.y;
        ownRt = GetComponent<RectTransform>();

        Invoke(nameof(StartMoving), 1);
    }

    private void Update()
    {
        if (isMoving)
        {
            EvaluateMotion();
        }
    }

    public void StartMoving()
    {
        isMoving = true;
        currentCarData = carProperties.currentCarData;
    }

    private void EvaluateMotion()
    {
        float dt = Time.deltaTime;
        currentDistance += dt * carProperties.currentCarSpeed;
        if (currentDistance > resetDistance)
        {
            PerformActionOnActiveObstacles(true);
            currentDistance %= roadHeight;
            PerformActionOnActiveObstacles(false);
        }
        ownRt.anchoredPosition = currentDistance * Vector3.down;
        if ((int)(currentDistance / roadHeight) != roadImageId) SyncRoadImageId();
    }

    private void SyncRoadImageId()
    {
        // assuming index'0' displaying first
        roadImageId = (int) (currentDistance / roadHeight);
        int imageInd = roadImageId % 2;
        roadRTs[imageInd].anchoredPosition = roadImageId * roadHeight * Vector3.up;
        roadRTs[(imageInd + 1) % 2].localPosition = roadRTs[imageInd].localPosition + roadHeight * Vector3.up;
    }

    /// <summary>
    /// This method will help to reposition the obstacles at the time of reseting parent's position
    /// </summary>
    private void PerformActionOnActiveObstacles(bool store)
    {
        int n = obstacleParent.childCount;
        Transform t;
        for (int i= 0; i < n; i++)
        {
            t = obstacleParent.GetChild(i);
            if (!t.gameObject.activeSelf)
            {
                if (store)
                {
                    positionCacheDic[t.GetInstanceID()] = t.position;
                }
                else
                {
                    t.position = positionCacheDic[t.GetInstanceID()];
                }
            }
        }
    }
}

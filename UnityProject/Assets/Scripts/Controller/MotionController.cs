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
    private SignalBus signalBus;

    [Inject]
    private void Construct(ICarProperties carProperties, SignalBus signalBus)
    {
        this.carProperties = carProperties;
        this.signalBus = signalBus;
    }

    private void Start()
    {
        if (roadRTs.Length != 2) Debug.LogError("number of referenced road instances must be 2");
        ownRt = GetComponent<RectTransform>();
        roadHeight = roadRTs[0].sizeDelta.y;
    }

    private void Update()
    {
        if (isMoving)
        {
            EvaluateMotion();
        }
    }

    public void OnRaceStart()
    {
        isMoving = true;
        currentCarData = carProperties.currentCarData;
    }

    public void OnRaceEnd()
    {
        isMoving = false;
    }

    private void EvaluateMotion()
    {
        float dt = Time.deltaTime;
        currentDistance += dt * carProperties.currentCarSpeed;
        //if (currentDistance and something)
        //{
        //    signalBus.Fire(new SpawnCollidedSignal());
        //}
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

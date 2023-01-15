using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private Transform obstacleSpawningPosTransform;
    private Transform ownTransform;
    private ObstacleData[] obstacles;
    private Stack[] idleObstacles = new Stack[3];
    private Dictionary<Transform, int> obstaclesTypeDictionary = new Dictionary<Transform, int>();

    [Inject]
    public void Construct(ObstacleData[] obstacles)
    {
        this.obstacles = obstacles;
    }

    private void Start()
    {
        ownTransform = transform;
        for (int i = 0; i < idleObstacles.Length; i++)
        {
            idleObstacles[i] = new Stack();
        }
    }

    public void OnRaceStart()
    {
        CollectAllObstacles();
    }

    public void SpawnObstacle()
    {
        int ind = Random.Range(0, obstacles.Length);
        ObstacleData obstacleData = obstacles[ind];
        float rotationZ = Random.Range(obstacleData.rotationRange.x, obstacleData.rotationRange.y);
        float dummy = (Helper.RoadWidth - obstacleData.size.x) / 2;
        float positionX = Random.Range(-dummy, dummy);
        Transform t = GetObstacleFromPool(ind);
        t.eulerAngles = rotationZ * Vector3.forward;
        t.localPosition = positionX * Vector3.right + ownTransform.InverseTransformPoint(obstacleSpawningPosTransform.position).y * Vector3.up;
    }

    private Transform GetObstacleFromPool(int ind)
    {
        Transform t;
        if (idleObstacles[ind].Count == 0)
        {
            t = Instantiate(obstacles[ind].prefab, ownTransform).transform;
            obstaclesTypeDictionary[t] = ind;
        }
        else
        {
            t = (Transform) idleObstacles[ind].Pop();
            t.gameObject.gameObject.SetActive(true);
        }
        return t;
    }

    private void CollectAllObstacles()
    {
        Transform t;
        int len = ownTransform.childCount;
        for(int i = 0; i < len; i++)
        {
            t = ownTransform.GetChild(i);
            if (t.gameObject.activeSelf)
            {
                MakeObstacleIdle(t);
            }
        }
    }

    public void MakeObstacleIdle(Transform t)
    {
        t.gameObject.SetActive(false);
        idleObstacles[obstaclesTypeDictionary[t]].Push(t);
    }
}

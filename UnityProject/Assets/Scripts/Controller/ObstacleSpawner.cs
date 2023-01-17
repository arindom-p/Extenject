using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private Transform obstacleSpawningPosTransform;
    private Transform ownTransform;
    private Obstacle.Factory obstacleFectory;
    private ObstacleData[] obstacleTypeData;
    private Stack[] idleObstacles = new Stack[3];
    private Dictionary<Obstacle, int> obstaclesTypeDictionary = new Dictionary<Obstacle, int>();

    [Inject]
    public void Construct(ObstacleData[] obstacleTypeData, Obstacle.Factory obstacleFactory)
    {
        this.obstacleTypeData = obstacleTypeData;
        this.obstacleFectory = obstacleFactory;
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
        int ind = Random.Range(0, obstacleTypeData.Length);
        ObstacleData obstacleData = obstacleTypeData[ind];
        float rotationZ = Random.Range(obstacleData.rotationRange.x, obstacleData.rotationRange.y);
        float dummy = (Helper.RoadWidth - obstacleData.size.x) / 2;
        float positionX = Random.Range(-dummy, dummy);
        Obstacle obstacle = GetObstacleFromPool(ind);
        Transform t = obstacle.transform;
        t.localPosition = positionX * Vector3.right + ownTransform.InverseTransformPoint(obstacleSpawningPosTransform.position).y * Vector3.up;
        t.eulerAngles = rotationZ * Vector3.forward;
    }

    private Obstacle GetObstacleFromPool(int ind)
    {
        Obstacle obstacle;
        if (idleObstacles[ind].Count == 0)
        {
            obstacle = obstacleFectory.Create(obstacleTypeData[ind].prefab);
            Transform t = obstacle.transform;
            t.parent = ownTransform;
            t.localScale = Vector3.one;
            obstaclesTypeDictionary[obstacle] = ind;
        }
        else
        {
            obstacle = (Obstacle) idleObstacles[ind].Pop();
            obstacle.gameObject.gameObject.SetActive(true);
        }
        return obstacle;
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
                MakeObstacleIdle(t.GetComponent<Obstacle>());
            }
        }
    }

    public void MakeObstacleIdle(Obstacle obstacle)
    {
        obstacle.gameObject.SetActive(false);
        idleObstacles[obstaclesTypeDictionary[obstacle]].Push(obstacle);
    }
}

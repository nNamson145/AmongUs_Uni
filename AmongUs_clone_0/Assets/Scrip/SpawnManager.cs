using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    public Vector2[] spawnPoints;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            return;
        }
    }

    public Vector2 spawnPosition()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }
}

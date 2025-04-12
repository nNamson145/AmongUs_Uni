using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;

    [SerializeField]
    private GameObject deadBodyPrefab;

    public Dictionary<ulong, PlayerController> playerMap = new();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    #region REGISTER PLAYER
    public void RegisterPlayer(PlayerController player)
    {
        if(!playerMap.ContainsKey(player.OwnerClientId))
        {
            playerMap.Add(player.OwnerClientId, player);
        }    

        foreach (var name in playerMap)
        {
            Debug.Log(name);
        }
    }
    public PlayerController GetPlayer(ulong playerId)
    {
        return playerMap.TryGetValue(playerId, out var player) ? player : null;
    }
    #endregion


    #region KILL PLAYER
    public void SpawnDeadBody(Vector2 spawnPos)
    {
        GameObject body = Instantiate(deadBodyPrefab, spawnPos, Quaternion.identity);
        body.GetComponent<NetworkObject>().Spawn();
    }

    #endregion
}

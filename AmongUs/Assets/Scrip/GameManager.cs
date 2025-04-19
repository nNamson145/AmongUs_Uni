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
        if (playerMap.ContainsKey(player.OwnerClientId))
        {
            //Debug.LogWarning($"Player {player.OwnerClientId} already registered.");
            return;
        }
        Debug.LogWarning($"Registering player {player.OwnerClientId}");
        playerMap[player.OwnerClientId] = player; 

        /*foreach (var name in playerMap)
        {
            Debug.LogWarning(player.OwnerClientId.ToString());
        }*/
    }
    public PlayerController GetPlayer(ulong playerId)
    {
        return playerMap.TryGetValue(playerId, out var player) ? player : null;
    }
    

    
#endregion

    public void AssignRole(PlayerController player)
    {
        var randomRole = Random.value < 0.5f ? PlayerController.RoleType.Impostor : PlayerController.RoleType.Crewmate;
        player.roleType.Value = randomRole;
        
        var impostor = player.GetComponent<ImpostorController>();
        var crewmate = player.GetComponent<CrewmateController>();

        if (impostor != null) impostor.enabled = (randomRole == PlayerController.RoleType.Impostor);
        if (crewmate != null) crewmate.enabled = (randomRole == PlayerController.RoleType.Crewmate);

        player.role = (IRole)(randomRole == PlayerController.RoleType.Impostor ? impostor : crewmate);
    }

#region KILL PLAYER
    public void SpawnDeadBody(Vector2 spawnPos)
    {
        GameObject body = Instantiate(deadBodyPrefab, spawnPos, Quaternion.identity);
        body.GetComponent<NetworkObject>().Spawn();
    }

#endregion


#region VENT IMPOSTOR

public void ImpostorJumpToVentLink(PlayerController player,Vent ventLink)
{
    //play anim
    var rb = player.GetComponent<Rigidbody2D>(); // hoặc Rigidbody nếu 3D
    if (rb != null)
    {
        rb.position = ventLink.transform.position;
        rb.linearVelocity = Vector2.zero; // optional: dừng lại
    }
    else
    {
        Debug.LogError("Missing Rigidbody on Player!");
    }        //play anim
}


#endregion
}

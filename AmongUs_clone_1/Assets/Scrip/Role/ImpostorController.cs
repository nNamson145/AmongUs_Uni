using Unity.Netcode;
using UnityEngine;

//[RequireComponent(typeof(NetworkObject))]
public class ImpostorController : NetworkBehaviour, IImpostor
{
    private PlayerController controller;

    void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

#region Kill
    public void Kill()
    {
        if (controller.hitCollisionObj == null)
        {
            Debug.Log("[CLIENT] hitCollisionObj is null");
            return;
        }

        if (controller.hitCollisionObj.CompareTag("Player"))
        {
            ulong victimId = controller.hitCollisionObj.GetComponent<NetworkObject>().OwnerClientId;
            Debug.Log("[CLIENT] Trying to kill player with ID: " + victimId);
            
            TryKillServerRpc(victimId);
        }
        else
        {
            ulong victimId = controller.hitCollisionObj.GetComponent<NetworkObject>().OwnerClientId;
            Debug.LogWarning("[CLIENT] Trying to kill player with ID: " + victimId);        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void TryKillServerRpc(ulong targetId)
    {
        Debug.Log("[SERVER] RPC Called - Target ID: " + targetId);

        var victim = GameManager.instance.GetPlayer(targetId);

        if (victim == null || victim.isDead.Value)
        {
            Debug.LogWarning("[SERVER] Victim is null or already dead");
            return;
        }

        victim.isDead.Value = true;
        victim.GetComponent<CapsuleCollider2D>().isTrigger = true;

        GameManager.instance.SpawnDeadBody(victim.transform.position);
    }
#endregion

    public void Sabotage()
    {

    }

    public bool CanUseVent()
    {
        return true;
    }


}

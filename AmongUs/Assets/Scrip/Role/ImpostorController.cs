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

    public void TickRole()
    {
    }

    
#region Kill
    public void Kill()
    {
        if (controller.currentObjHit == null)
        {
            Debug.Log("[CLIENT] hitCollisionObj is null");
            return;
        }

        if (controller.currentObjHit.CompareTag("Player"))
        {
            ulong victimId = controller.currentObjHit.GetComponent<NetworkObject>().OwnerClientId;
            Debug.Log("[CLIENT] Trying to kill player with ID: " + controller.currentObjHit.name);
            
            TryKillServerRpc(victimId);
        }
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

#region Sabotage
    public void Sabotage()
    {

    }
#endregion

#region Vent
    public bool CanUseVent()
    {
        if(controller.currentObjHit == null) return false;
        
        if(controller.currentObjHit.CompareTag("Vent"))
        {
            Debug.Log(" true " +controller.currentObjHit.name);
            return true;
        }
        Debug.Log(" false " + controller.currentObjHit.name);
        return false;
    }

    public void UseVent()
    {
        if (CanUseVent())
        {
            UseVentServerRpc();
        }
        else
        {
            Debug.LogError("bool can use vent is false");
        }
    }

    [ServerRpc(AllowTargetOverride = false)]
    public void UseVentServerRpc()
    {
        var netObj = GetComponent<NetworkObject>();
        if (netObj == null)
        {
            Debug.LogError("[SERVER RPC] Missing NetworkObject on ImpostorController!");
            return;
        }

        if (controller.currentObjHit == null)
        {
            Debug.LogWarning("[SERVER RPC] No currentObjHit found.");
            return;
        }

        var vent = controller.currentObjHit.GetComponent<Vent>();
        if (vent == null)
        {
            Debug.LogWarning("[SERVER RPC] Hit object is not a Vent.");
            return;
        }

        vent.TeleportToVent(controller);
        Debug.LogWarning($"[SERVER RPC] Client {netObj.OwnerClientId} teleported to vent.");
        
    }

#endregion
}

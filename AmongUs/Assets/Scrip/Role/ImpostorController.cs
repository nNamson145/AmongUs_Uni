using Unity.Netcode;
using UnityEngine;

//[RequireComponent(typeof(NetworkObject))]
public class ImpostorController : NetworkBehaviour, IImpostor
{
    private PlayerController localController;

    void Awake()
    {
        localController = GetComponent<PlayerController>();
    }

    public void TickRole()
    {
    }

    
#region Kill
    public void Kill()
    {
        if (localController.currentObjHit == null)
        {
            return;
        }

        if (localController.currentObjHit.CompareTag("Player"))
        {
            ulong victimId = localController.currentObjHit.GetComponent<NetworkObject>().OwnerClientId;
            //Debug.Log("[CLIENT] Trying to kill player with ID: " + victimId);
            TryKillServerRpc(victimId);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void TryKillServerRpc(ulong targetId)
    {
        var victim = GameManager.instance.GetPlayer(targetId);

        if (victim == null || victim.isDead.Value)
        {
            //Debug.LogWarning("[SERVER] Victim is null or already dead");
            return;
        }

        victim.isDead.Value = true;
        GameManager.instance.SpawnDeadBody(victim.transform.position);
        //Debug.LogWarning("[SERVER]"+ victim +"  is null or already dead");
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
        if(localController.currentObjHit == null) return false;
        
        if(localController.currentObjHit.CompareTag("Vent"))
        {
            Debug.LogError(" true " +localController.currentObjHit.name);
            return true;
        }
        Debug.Log(" false " + localController.currentObjHit.name);
        return false;
    }

    public void UseVent()
    {
        if (CanUseVent())
        {
            var ventNetObj = localController.currentObjHit.GetComponent<NetworkObject>();
            if (ventNetObj != null)
            {
                UseVentServerRpc(new NetworkObjectReference(ventNetObj));
            }
        }
        else
        {
            Debug.LogError("bool can use vent is false");
        }
    }

    [ServerRpc(AllowTargetOverride = false)]
    public void UseVentServerRpc(NetworkObjectReference  netObjref)
    {
        if (!netObjref.TryGet(out NetworkObject ventNetObj))
        {
            Debug.LogError("[SERVER RPC] Missing NetworkObject on ImpostorController!");
            return;
        }
        
        var vent = ventNetObj.GetComponent<Vent>();
        if (vent != null)
        {
            vent.TeleportToVent(localController);
            Debug.LogWarning($"[SERVER RPC] Client {ventNetObj.OwnerClientId} teleported to vent.");
        }
    }

#endregion
}

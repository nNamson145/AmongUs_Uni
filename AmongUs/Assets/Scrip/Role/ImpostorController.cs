using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

//[RequireComponent(typeof(NetworkObject))]
public class ImpostorController : NetworkBehaviour, IImpostor
{
    private PlayerController localController;

    void Awake()
    {
        localController = GetComponent<PlayerController>();
        Debug.Log(localController);
        //Debug.Log($"UseVent(): IsOwner={IsOwner}, IsSpawned={IsSpawned}, HasAuthority={NetworkObject.IsOwner}, NetworkObjectId={NetworkObject.NetworkObjectId}");
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
            //Debug.Log(" true " +localController.currentObjHit.name);
            return true;
        }
        //Debug.LogError(" false " + localController.currentObjHit.name);
        return false;
    }

    public void UseVent()
    {
        Debug.LogWarning($"[UseVent] Called on {gameObject.name}, IsOwner: {IsOwner}, IsClient: {IsClient}, IsServer: {IsServer}, IsSpawned: {NetworkObject.IsSpawned}");
        if (localController.currentObjHit == null)
        {
            return;
        }
        if (CanUseVent())
        {
            var ventNetObj = localController.currentObjHit.GetComponent<NetworkObject>();
            if (ventNetObj != null)
            {
                ulong ventId = ventNetObj.NetworkObjectId;
                Debug.LogWarning($"[CLIENT] Found ventNetObj: {ventNetObj.name}, ID: {ventId}");
                UseVentServerRpc(ventId);
                Debug.LogWarning("[CLIENT] Called UseVentServerRpc");
            }
        }
        else
        {
            Debug.LogWarning("bool can use vent is false");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UseVentServerRpc(ulong ventId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(ventId, out var ventNetObj))
        {
            var vent = ventNetObj.GetComponent<Vent>();
            if (vent != null)
            {
                Vector3 targetPosition = vent.ventLink.transform.position;
                TeleportClientRpc(localController.OwnerClientId, targetPosition);
                localController.transform.position = targetPosition;
            }
        }
    }

    [ClientRpc]
    private void TeleportClientRpc(ulong targetClientId, Vector3 position)
    {
        if (NetworkManager.Singleton.LocalClientId == targetClientId)
        {
            transform.position = position;
        }
    }

#endregion
}

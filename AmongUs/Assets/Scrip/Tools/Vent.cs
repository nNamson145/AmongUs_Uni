using Unity.Netcode;
using UnityEngine;

public class Vent : NetworkBehaviour
{
    [SerializeField]
    public Vent ventLink;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void TeleportToVent(PlayerController player)
    {
        GameManager.instance.ImpostorJumpToVentLink(player, ventLink);
    }
}

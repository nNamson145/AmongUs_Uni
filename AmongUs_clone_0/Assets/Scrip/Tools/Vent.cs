using Unity.Netcode;
using UnityEngine;

public class Vent : NetworkBehaviour
{
    [SerializeField]
    private Vent ventLink;
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
        if (!player.GetComponent<ImpostorController>().enabled) return;
        //play anim
        player.transform.position = ventLink.transform.position;
        //play anim
    }
}

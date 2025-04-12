using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using static UIManager;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private float movementSpeed = 2f;

    private Rigidbody2D rb;

    private Vector2 movementDirection;

    public Collider2D hitCollisionObj;

    // Sync animation state
    private NetworkVariable<bool> isMoving = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public IImpostor role;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            GameManager.instance.RegisterPlayer(this);
        }

        if (IsOwner)
        {
            UIManager.instance.RegisterLocalPlayer(this);
        }

        if (!IsOwner)
        {
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
        }

        //get location to span
        transform.position = SpawnManager.instance.spawnPosition();

        role = GetComponent<IImpostor>();

    }

    private PlayerController GetLocalPLayerController()
    {
        var playerObj = NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject();

        if (playerObj != null)
        {
            return playerObj.GetComponent<PlayerController>();
        }
        Debug.LogWarning("Local player object not found!");
        return null;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        movementDirection = new Vector2(horizontal,vertical);
        

        //flip sprite anim
        GetComponent<AnimationPlayer>().SetFlipByScale(horizontal);
        isMoving.Value = movementDirection != Vector2.zero;
    }

    void FixedUpdate()
    {
        if (!IsOwner) return;

        rb.linearVelocity = movementDirection * movementSpeed;
    }

    void LateUpdate()
    {
        if (isDead.Value)
        {
            GetComponent<AnimationPlayer>().SetPlayerDead(isDead.Value);
            return;
        }
                
        GetComponent<AnimationPlayer>().SetPlayerMoving(isMoving.Value);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!IsOwner) return;
        
        if (collision.CompareTag("Player"))
        {
            hitCollisionObj = collision;
        }
        
        /*if (collision.CompareTag("DeadBody"))
        {
            UIManager.instance.SetButtonInteractable(namebutton.ButtonReport, true);
        }*/
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            hitCollisionObj = null;
        }
        
        /*if (collision.CompareTag("DeadBody"))
        {
            UIManager.instance.SetButtonInteractable(namebutton.ButtonReport, false);
        }*/
    }

    /*// IMPOSTOR
    //Kill
    public void AttackOtherPlayer()
    {
        if (hitCollisionObj == null) return;

        if (hitCollisionObj.CompareTag("Player"))
        {
            ulong victimId = hitCollisionObj.GetComponent<NetworkObject>().OwnerClientId;
            TryKillOtherPlayerServerRpc(victimId);
            Debug.Log("Kill: " + victimId);
        }
    }
    [ServerRpc(RequireOwnership = false)]
    void TryKillOtherPlayerServerRpc(ulong targetId)
    {
        var victim = GameManager.instance.GetPlayer(targetId);

        if (victim == null || victim.isDead.Value) return;

        victim.isDead.Value = true;
        victim.GetComponent<CapsuleCollider2D>().isTrigger = true;

        GameManager.instance.SpawnDeadBody(victim.transform.position);
    }*/
}

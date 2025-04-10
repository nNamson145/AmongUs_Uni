using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;
using static UIManager;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private float movementSpeed = 2f;

    [SerializeField]
    private GameObject deadBodyPrefab;

    private Rigidbody2D rb;

    private Vector2 movementDirection;

    // Sync animation state
    private NetworkVariable<bool> isMoving = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private NetworkVariable<bool> isDead = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
        }

        //get location to span
        transform.position = SpawnManager.instance.spawnPosition();

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

    private void FixedUpdate()
    {
        if (!IsOwner) return;

        rb.linearVelocity = movementDirection * movementSpeed;
    }

    /*[ServerRpc]
    void SetMovingAnimServerRpc(bool isMoving, ServerRpcParams rpcParams = default)
    {
        if (rpcParams.Receive.SenderClientId != OwnerClientId) return;

        GetComponent<AnimationPlayer>().SetPlayerMoving(isMoving);
        
        SetMovingAnimClientRpc(isMoving);
    }

    [ClientRpc]
    void SetMovingAnimClientRpc(bool isMoving)
    {
        if (!IsOwner)
        {
            GetComponent<AnimationPlayer>().SetPlayerMoving(isMoving);
        }
    }*/

    private void LateUpdate()
    {
        if (isDead.Value) return;
                
        GetComponent<AnimationPlayer>().SetPlayerMoving(isMoving.Value);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision == null) return;


        if (collision.CompareTag("DeadBody"))
        {
            UIManager.instance.SetButtonInteractable(namebutton.ButtonReport, true);
        }
        else if (collision.CompareTag("Player"))
        {
            ImpostorAttackOtherPlayer(collision);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.CompareTag("DeadBody"))
        {
            UIManager.instance.SetButtonInteractable(namebutton.ButtonReport, false);
        }
    }


    public void ImpostorAttackOtherPlayer(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject playerIsKilled = collision.GetComponent<GameObject>();
            if (playerIsKilled == null) return;
            playerIsKilled.GetComponent<PlayerController>().isDead.Value = true;

            SpawnDeadBodyServerRpc(playerIsKilled.transform.position);
        }
    }

    [ServerRpc]
    public void SpawnDeadBodyServerRpc(Vector2 pos)
    {

        GameObject body = Instantiate(deadBodyPrefab, pos, Quaternion.identity);

        NetworkObject netObj = body.GetComponent<NetworkObject>();

        netObj.Spawn(true);

        //save id player to body
    }
}

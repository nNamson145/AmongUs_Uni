using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private float movementSpeed = 2f;

    private Rigidbody2D rb;

    private Vector2 movementDirection;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false;

            var audio = GetComponentInChildren<AudioListener>();
            if (audio) audio.enabled = false;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movementDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        rb.linearVelocity = movementDirection * movementSpeed;

        if (rb.linearVelocity == Vector2.zero)
        {
            this.GetComponent<AnimationPlayer>().SetPlayerMoving(false);
        }
        else
        {
            this.GetComponent<AnimationPlayer>().SetPlayerMoving(true);
        }

    }

   
}

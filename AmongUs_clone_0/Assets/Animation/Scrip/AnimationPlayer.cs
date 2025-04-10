using Unity.Netcode;
using UnityEngine;

public class AnimationPlayer : NetworkBehaviour
{

    const string STR_IS_MOVING = "isMoving";

    const string STR_IS_DEAD = "isDead";

    private SpriteRenderer playerSprite;

    public Animator animatorController;

    public NetworkVariable<bool> isfacingRight = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        playerSprite.flipX = isfacingRight.Value;
    }
    public void SetPlayerMoving(bool moving)
    {
        if(animatorController != null)
        {
            animatorController.SetBool(STR_IS_MOVING, moving);
        }
    }

    public void SetPlayerDead(bool dead)
    {
        if (animatorController != null)
        {
            animatorController.SetBool(STR_IS_DEAD, dead);
        }
    }
    public void SetFlipByScale(float horizontalInput)
    {
        if (horizontalInput < 0)
        {
            isfacingRight.Value = true;
        }
        else if (horizontalInput > 0)
        { 
            isfacingRight.Value = false; 
        }

        
    }
}

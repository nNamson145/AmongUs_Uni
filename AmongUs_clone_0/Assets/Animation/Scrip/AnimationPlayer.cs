using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    private float horizontalInput;
    private bool facingRight = true;

    const string STR_IS_MOVING = "isMoving";

    public Animator animatorController;

    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        SetupDirectionByScale();
    }

    public void SetPlayerMoving(bool moving)
    {
        animatorController.SetBool(STR_IS_MOVING, moving);
    }

    private void SetupDirectionByScale()
    {
        if (horizontalInput < 0 && facingRight || horizontalInput > 0 && !facingRight)
        {
            facingRight = !facingRight;
            Vector3 playerScale = transform.localScale;
            playerScale.x *= -1;
            transform.localScale = playerScale;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public Image icon;
    public float speed = 5f;
    private Animator animator; 

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 newPosition = transform.position + new Vector3(horizontalInput * speed * Time.deltaTime, 0f, 0f);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        transform.position = newPosition;
        UpdateAnimations(horizontalInput);
    }

    void UpdateAnimations(float horizontalInput)
    {
        if (horizontalInput != 0)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }
}

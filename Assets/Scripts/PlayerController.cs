using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float basicSpeed = 4f;
    float actualSpeed = 0;

    Rigidbody2D rb;

    bool isLookRight = true;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        bool isSwapRight = (horizontalInput > 0) && !isLookRight;
        bool isSwapLeft = (horizontalInput < 0) && isLookRight;

        if (isSwapRight || isSwapLeft)
        {
            isLookRight = isSwapRight ? true : false;
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }

        actualSpeed = horizontalInput * basicSpeed;
        transform.Translate(Vector2.right * actualSpeed * Time.deltaTime);
    }

    public float GetActualSpeed()
    {
        return Mathf.Abs(actualSpeed);
    }
}

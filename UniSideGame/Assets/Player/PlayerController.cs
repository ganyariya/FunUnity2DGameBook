using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float horizontalSpeed = 3f;

    private Rigidbody2D rBody;

    private float horizontalAxisInput;

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalAxisInput = Input.GetAxisRaw("Horizontal");
        if (horizontalAxisInput > 0) transform.localScale = new Vector2(1, 1);
        if (horizontalAxisInput < 0) transform.localScale = new Vector2(-1, 1);
    }

    void FixedUpdate()
    {
        rBody.velocity = new Vector2(horizontalAxisInput * horizontalSpeed, rBody.velocity.y);
    }
}

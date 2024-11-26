using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    public InventoryScript inventoryScript;
    public float moveSpeed = 10;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(inventoryScript.inventoryOpen) { return; }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        rb.AddForce(new Vector2(horizontalInput, verticalInput).normalized * Time.deltaTime * 1000 * moveSpeed);

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(rb.velocity.x * 0.5f, rb.velocity.y * 0.5f);

    }
}

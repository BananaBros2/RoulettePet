using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    public InventoryScript inventoryScript;
    public float moveSpeed = 10;
    public GameObject minimapArrow;
    public float mmArrowTurnSpeed = 10;

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

        if (horizontalInput != 0 || verticalInput != 0) 
        {
            Vector3 lookAt = new Vector2(rb.velocity.x + minimapArrow.transform.position.x, rb.velocity.y + minimapArrow.transform.position.y) - new Vector2(minimapArrow.transform.position.x, minimapArrow.transform.position.y);
            minimapArrow.transform.up = lookAt;
        }



    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(rb.velocity.x * 0.5f, rb.velocity.y * 0.5f);
    }
}

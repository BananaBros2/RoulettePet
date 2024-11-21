using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private TMP_Text damageTMPText;

    public float textSpeed = 10;
    public float timeUntilRemove = 1;

    // Start is called before the first frame update
    void Start()
    {
        damageTMPText = transform.GetChild(0).GetComponent<TMP_Text>();
        StartCoroutine(TextTiming());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x * Time.deltaTime * textSpeed, transform.position.y);

    }

    public IEnumerator TextTiming()
    {
        yield return new WaitForSeconds(timeUntilRemove);

        while (damageTMPText.color.a > 0)
        {
            damageTMPText.color = new Color(damageTMPText.color.r, damageTMPText.color.g, damageTMPText.color.b, damageTMPText.color.a - 0.05f);
            yield return new WaitForSeconds(0.1f);
        }

        Destroy(this.gameObject);

    }

    public void UpdateDamageText(string newDamage)
    {
        damageTMPText.text = newDamage;
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TMP_Text damageTMPText;

    public float textSpeed = 0.5f;
    public float timeUntilRemove = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        damageTMPText = transform.GetChild(0).GetComponent<TMP_Text>();
        StartCoroutine(TextTiming());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + Time.deltaTime * textSpeed);

    }

    public IEnumerator TextTiming()
    {
        yield return new WaitForSeconds(timeUntilRemove);

        while (damageTMPText.color.a > 0)
        {
            damageTMPText.color = new Color(damageTMPText.color.r, damageTMPText.color.g, damageTMPText.color.b, damageTMPText.color.a - 0.1f);
            yield return new WaitForSeconds(0.05f);
        }

        Destroy(this.gameObject);

    }

    public void UpdateDamageText(float damageValue, Color textColour)
    {
        damageTMPText.text = (Mathf.Round(damageValue*10)/10).ToString();
        damageTMPText.color = textColour;
    }

}

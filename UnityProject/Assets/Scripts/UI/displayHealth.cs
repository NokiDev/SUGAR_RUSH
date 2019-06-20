using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class displayHealth : MonoBehaviour
{
    public Image healthImage;
    public Damageable damageable;

    private GameObject player;
    private List<Image> healthList = new List<Image>();
    private uint healthDisplay = 0;

    private void Awake()
    {
        damageable.onHit += decreaseHealth;
        damageable.onDeath += () => { decreaseHealth(0, 0); };
        damageable.onHeal += increaseHealth;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        increaseHealth(player.GetComponent<Damageable>().healthMax);
    }

    void increaseHealth(uint health)
    {
        Debug.Log((int)(health - healthDisplay));
        for (int i = 0; i < (int) (health - healthDisplay); ++i)
        {
            int offsetX = 40 * healthList.Count;
            Image newHealth = Instantiate(healthImage, gameObject.transform);

            newHealth.GetComponent<RectTransform>().anchoredPosition = new Vector2(50 + offsetX, -50);
            healthList.Add(newHealth);
        }
        healthDisplay = health;
    }

    void decreaseHealth(float timer, uint health)
    {
        Debug.Log("health = " + health);
        Debug.Log("healthD = " + healthDisplay);
        for (int i = 0; i < (int) (healthDisplay - health); ++i)
        {
            int lastIndex = healthList.Count - 1;
            Destroy(healthList[lastIndex]);
            healthList.RemoveAt(lastIndex);
        }
        healthDisplay = health;
    }
}

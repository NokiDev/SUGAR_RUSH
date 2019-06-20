using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class displayHealth : MonoBehaviour
{
    public Image healthImage;
    public Image emptyImage;
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
        createHealth(player.GetComponent<Damageable>().healthMax);
    }

    void createHealth(uint health)
    {
        for (int i = 0; i < (int)(health - healthDisplay); ++i)
        {
            Image newHealth = Instantiate(healthImage, gameObject.transform);

            newHealth.GetComponent<RectTransform>().anchoredPosition = healthPos(healthList.Count);
            healthList.Add(newHealth);
        }
        healthDisplay = health;
    }
    void increaseHealth(uint health)
    {
        for (int i = 0; i < (int)(health - healthDisplay); ++i)
        {
            int healthIndex = (int)healthDisplay + i;
            imageSwitch(healthIndex, healthImage);
        }
        healthDisplay = health;
    }

    void decreaseHealth(float timer, uint health)
    {
        for (int i = 0; i < (int)(healthDisplay - health); ++i)
        {
            int healthIndex = (int)healthDisplay - i - 1;
            imageSwitch(healthIndex, emptyImage);
        }
        healthDisplay = health;
    }

    void imageSwitch(int index, Image image)
    {
        Destroy(healthList[index]);

        Image newHealth = Instantiate(image, gameObject.transform);

        newHealth.GetComponent<RectTransform>().anchoredPosition = healthPos(index);
        healthList[index] = newHealth;
    }

    Vector2 healthPos(int index)
    {
        int offsetX = 40 * (index % 10);
        int offsetY = -40 * (index / 10);

        return new Vector2(50 + offsetX, -50 + offsetY);
    }
}

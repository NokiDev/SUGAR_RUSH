using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class displayKeys : MonoBehaviour
{
    public Image keyImage;

    private GameObject player;
    private Pocket pocket;
    private List<Image> keysList = new List<Image>();

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pocket = player.GetComponent<Pocket>();

        pocket.keyAdded += addKey;
        pocket.keyUsed += removeKey;
    }

    void addKey(int keysAdded)
    {
        for (int i = 0; i < keysAdded; ++i)
        {
            int offsetX = -15 * keysList.Count;
            Image newKey = Instantiate(keyImage, gameObject.transform);

            newKey.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50 + offsetX, -50);
            keysList.Add(newKey);
        }
    }

    void removeKey(int keysUsed)
    {
        for (int i = 0; i < keysUsed; ++i)
        {
            int lastIndex = keysList.Count - 1;
            Destroy(keysList[lastIndex].gameObject);
            keysList.RemoveAt(lastIndex);
        }
    }
}

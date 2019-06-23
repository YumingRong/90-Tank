using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headquarter : MonoBehaviour
{
    public Sprite good, wreck;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
        sprite.sprite = good;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        sprite.sprite = wreck;
        StartCoroutine(BattleManager.GetInstance().GameOver());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invader : MonoBehaviour
{
    private SpriteRenderer spr;


    [Header ("Animation timeframe")]
    [SerializeField] private Sprite[] animationSprite;
    [SerializeField] private float animationTime = 1f;
    [SerializeField] private int animationFrame;

    public System.Action killed;

    //animation
    void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), this.animationTime, this.animationTime);
    }

    //just idle animation
    private void AnimateSprite()
    {
        animationFrame++;

        if (this.animationFrame >= this.animationSprite.Length)
        {
            this.animationFrame = 0;
        }

        spr.sprite = this.animationSprite[this.animationFrame];

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Laser"))
        {
            this.killed.Invoke();
            this.gameObject.SetActive(false);
        }
    }

}

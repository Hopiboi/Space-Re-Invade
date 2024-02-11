using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] private Vector3 direction;
    [SerializeField] private float speed;
    public System.Action destroyed;

    void Update()
    {
        this.transform.position += this.direction * this.speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        this.destroyed.Invoke();
        
        Destroy(this.gameObject);
    }
}

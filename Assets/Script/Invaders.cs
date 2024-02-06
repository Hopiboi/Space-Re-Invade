using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invaders : MonoBehaviour
{
    [SerializeField] private Invader[] prefabs;
    [SerializeField] private int rows = 5;
    [SerializeField] private int cols = 11;



    [SerializeField] private Vector3 direction = Vector2.right;
    [SerializeField] private float speed = .5f;



    void Awake()
    {
        for (int row = 0; row < this.rows; row++)
        {
            //Center Variables, setting up to the center
            float width = 1f * (this.cols - 1);
            float height = 1f * (this.rows - 1);
            Vector2 centering = new Vector2 (-width / 2 , -height / 2);
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * .6f), 0f); // spacing for the rows

            // columns
            for (int col = 0; col < this.cols; col++)
            {
                //instantiate in the rows
                Invader invader = Instantiate(this.prefabs[row], this.transform);

                // to create columns
                Vector3 position = rowPosition;
                position.x += col * .6f; // spacing columns
                invader.transform.localPosition = position; // local position in the transform
            }
        }
    }

    void Update()
    {
        this.transform.position += direction * this.speed * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (direction == Vector3.right && invader.position.x >= (rightEdge.x - .5f))
            {
                AdvanceRow();
                Debug.Log("going left");
            }

            else if (direction == Vector3.left && invader.position.x <= (leftEdge.x + .5f))
            {
                AdvanceRow();
                Debug.Log("going right");
            }
        }
    }

    //hitting edge, going down
    private void AdvanceRow()
    {
        direction.x *= -1f;

        Vector3 position = this.transform.position;
        position.y -= .10f;
        this.transform.position = position;

        Debug.Log("activated");
    }
}

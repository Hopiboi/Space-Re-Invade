using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Invaders : MonoBehaviour
{

    [Header("Grid")]
    [SerializeField] private Invader[] prefabs;
    [SerializeField] private int rows = 5;
    [SerializeField] private int cols = 11;


    [Header("Invaders movement")]
    [SerializeField] private Vector3 direction = Vector2.right;
    [SerializeField] private AnimationCurve speed;
    [SerializeField] private float missileAttackTime = 1f;
    [SerializeField] public Projectile missilePrefab;

    //kill invader
    public int amountKilled { get; private set; } //how many killed
    public int amountAlive => this.totalInvaders - this.amountKilled; // how many are still alive
    public int totalInvaders => this.rows * this.cols; // how many invader are
    public float percentKilled => (float)this.amountKilled / (float)this.totalInvaders; // percentage

    void Awake()
    {
        //creating rows
        for (int row = 0; row < this.rows; row++)
        {
            //Center Variables, setting up to the center
            float width = 1f * (this.cols - 1);
            float height = 1f * (this.rows - 1);
            Vector2 centering = new Vector2 (-width / 2 , -height / 2);
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * .6f), 0f); // spacing for the rows

            // creating columns inside in the rows
            for (int col = 0; col < this.cols; col++)
            {
                //instantiate in the columns
                Invader invader = Instantiate(this.prefabs[row], this.transform);

                invader.killed += InvaderKilled;

                // to create columns
                Vector3 position = rowPosition;
                position.x += col * .6f; // spacing columns
                invader.transform.localPosition = position; // local position in the transform
            }
        }
    }

    void Start()
    {
        InvokeRepeating(nameof(MissileAttack),this.missileAttackTime,this.missileAttackTime);
    }

    void Update()
    {
        this.transform.position += direction * this.speed.Evaluate(this.percentKilled) * Time.deltaTime;

        //edge of the camera and make them functional
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform invader in this.transform) // all childrens of this attached in this script will become invader name
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (direction == Vector3.right && invader.position.x >= (rightEdge.x - .5f)) // going left
            {
                AdvanceRow();
            }

            else if (direction == Vector3.left && invader.position.x <= (leftEdge.x + .5f)) // going right
            {
                AdvanceRow();
            }
        }
    }

    private void InvaderKilled()
    {
        this.amountKilled++;

        if (this.amountKilled >= this.totalInvaders)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    //hitting edge, going down
    private void AdvanceRow()
    {
        direction.x *= -1f;

        Vector3 position = this.transform.position;
        position.y -= .20f;
        this.transform.position = position;
    }

    private void MissileAttack()
    {
        foreach (Transform invader in this.transform)
        {
            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (Random.value < (1f / (float) this.amountAlive))
            {
                Instantiate(this.missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }
}

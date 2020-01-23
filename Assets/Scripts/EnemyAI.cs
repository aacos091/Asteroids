using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public Rigidbody2D rb;
    public Vector2 direction;
    public float speed;
    public GameObject bullet;
    public float shotDelay; //time between shots in seconds
    public float lastTimeShot;
    public float bulletSpeed;
    public Transform player;

    private GameController gameController;
    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the game controller object and the script
        GameObject gameControllerObject =
            GameObject.FindWithTag("GameController");

        gameController =
            gameControllerObject.GetComponent<GameController>();



        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
        lastTimeShot = 0;
        player = GameObject.FindWithTag("Ship").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Sets direction to player's position
        direction = (player.position - transform.position).normalized;

        if ( Time.time > lastTimeShot + shotDelay)
        {
            // shoot at player

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

            //Make a bullet

            GameObject newBullet = Instantiate(bullet, transform.position, q);

            newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(newBullet.transform.up * bulletSpeed);


            lastTimeShot = Time.time;
        }
    }
    void OnTriggerEnter2D(Collider2D c)
    {

        if (c.gameObject.tag.Equals("Bullet"))
        {

            // Destroy the bullet
            Destroy(c.gameObject);



            // Add to the score
            gameController.IncrementScoreEnemy();

            // Destroy the current asteroid
            Destroy(gameObject);

        }

    }


}

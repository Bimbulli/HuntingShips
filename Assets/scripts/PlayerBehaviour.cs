using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    ShooterBehaviour[] projectiles;
    HealthSystem healthSystem;

    public float moveSpeed = 10f;

    bool moveUp;
    bool moveDown;
    bool moveLeft;
    bool moveRight;

    bool shoot;

    GameObject shield;


    // Start is called before the first frame update
    void Start()
    {
        shield = transform.Find("Shield").gameObject;
        DeactivatShield();


        projectiles = transform.GetComponentsInChildren<ShooterBehaviour>();
        healthSystem = GetComponent<HealthSystem>();

        foreach (ShooterBehaviour shooter in projectiles)
        {
            shooter.isActive = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        moveUp = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
        moveDown = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
        moveLeft = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        moveRight = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);

        shoot = Input.GetKeyDown(KeyCode.L);
        if (shoot)
        {
            shoot = false;
            foreach (ShooterBehaviour projectile in projectiles)
            {
                projectile.Shoot();
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;

        float moveAmount = moveSpeed * Time.fixedDeltaTime;
        Vector2 move = Vector2.zero;

        if (moveUp)
        {
            move.y += moveAmount;
        }

        if (moveDown)
        {
            move.y -= moveAmount;
        }

        if (moveLeft)
        {
            move.x -= moveAmount;
        }

        if (moveRight)
        {
            move.x += moveAmount;
        }

        //Making sure diagonal movement is the same speed as going horizontal/vertical
        float moveMagnitude = Mathf.Sqrt(move.x * move.x + move.y * move.y);
        if (moveMagnitude > moveAmount)
        {
            float ratio = moveAmount / moveMagnitude;
            move *= ratio;
        }

        //Setting Boundaries
        pos += move;

        if (pos.x >= 8.5f)
        {
            pos.x = 8.5f;
        }

        if (pos.x <= -8.5f)
        {
            pos.x = -8.5f;
        }

        if (pos.y >= 4.5f)
        {
            pos.y = 4.5f;
        }

        if (pos.y <= -4.45f)
        {
            pos.y = -4.45f;
        }

        transform.position = pos;
    }

    void ActivatShield ()
    {
        shield.SetActive(true);
    }
    void DeactivatShield()
    {
        shield.SetActive(false);
    }

    bool HasShield()
    {
        return shield.activeSelf;
    }

    void IncreaseSpeed()
    {
        moveSpeed*=1.5f;
    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProjectileBehaviour projectile = collision.GetComponent<ProjectileBehaviour>();
        if (projectile != null)
        {
            if (projectile.isEnemy)
            {
                // Reduziere die Lebenspunkte des Spielers
                if (healthSystem != null)
                {
                    healthSystem.health--;
                }

                Destroy(projectile.gameObject);

                // Überprüfe, ob der Spieler noch Lebenspunkte hat
                if (healthSystem != null && healthSystem.health <= 0)
                {
                    // Der Spieler hat keine Lebenspunkte mehr, zerstöre das Spielerobjekt
                    Destroy(gameObject);
                }
            }
        }

        Destruction destructable = collision.GetComponent<Destruction>();
        if (destructable != null)
        {
            if(HasShield())
            {
                DeactivatShield();
            }
            else 
            { 
                // Reducing Health
                if (healthSystem != null)
                {
                    healthSystem.health--;
                }
            
                if (healthSystem != null && healthSystem.health <= 0)
                {
                    Destroy(gameObject);
                }

                Destroy(destructable.gameObject);
            }
        }

        PowerUp powerUp = collision.GetComponent<PowerUp>();
        if (powerUp)
        {
            if(powerUp.activateShield)
            {
                ActivatShield();
            }
            if(powerUp.increaseSpeed)
            {
                IncreaseSpeed();
            }
            Level.instance.AddScore(powerUp.pointValue);
            Destroy(powerUp.gameObject);
        }
    }
}
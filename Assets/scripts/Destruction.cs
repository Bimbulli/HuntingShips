using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruction : MonoBehaviour
{
    bool canBeDestroyed = false;
    public int scoreValue = 50;

    // Start is called before the first frame update
    void Start()
    {
        Level.instance.AddDestructable();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < 8f && !canBeDestroyed)
        {
            canBeDestroyed = true;
            ShooterBehaviour[] projectiles = transform.GetComponentsInChildren<ShooterBehaviour>();
            foreach (ShooterBehaviour projectile in projectiles)
            {
                projectile.isActive = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeDestroyed)
        {
            return;
        }
        ProjectileBehaviour projectile = collision.GetComponent<ProjectileBehaviour>();
        if (projectile != null)
        {
            if (!projectile.isEnemy)
            {
                Level.instance.AddScore(scoreValue);
              Destroy(gameObject);
              Destroy(projectile.gameObject);
            }
        }
    }
    private void OnDestroy()
    {
        Level.instance.RemoveDestructable();
    }
}

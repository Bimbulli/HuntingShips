using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterBehaviour : MonoBehaviour
{

    public ProjectileBehaviour projectile;
    Vector2 direction;

    public bool autoShoot = false;
    public float shootIntervalSeconds = 0.5f;
    public float shootDelaySeconds = 0f;
    float shootTimer = 0f;
    float delayTimer = 0f;

    public bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive)
        {
            return;
        }


        direction = (transform.localRotation*Vector2.right).normalized;
        if (autoShoot )
        {
            if (delayTimer >=shootDelaySeconds)
            {
                if(shootTimer>=shootIntervalSeconds)
                {
                    Shoot();
                    shootTimer = 0f;
                }
                else
                {
                    shootTimer += Time.deltaTime;
                }
            }
            else
            {
                delayTimer += Time.deltaTime;
            }
        }
    }

    public void Shoot () 
    {
        GameObject go = Instantiate(projectile.gameObject, transform.position, Quaternion.identity);
        ProjectileBehaviour goProjectile = go.GetComponent<ProjectileBehaviour>();
        goProjectile.direction = direction;
    }
}  



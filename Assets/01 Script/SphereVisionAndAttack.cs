using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class SphereVisionAndAttack : MonoBehaviour
{
    public float range = 0f;
    public LayerMask layerMask = 0;

    Transform target = null;
    public float spinSpeed = 0f;
    Animator anim;

    public float fireRate;
    float currentFireRate;

    public GameObject projectile;
    public Transform muzzlePoint;

    public MMFeedbacks ShootFeedback;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        currentFireRate = fireRate;
        InvokeRepeating("SetTarget", 0f, .5f);   
    }

    // Update is called once per frame
    void Update()
    {
        DetectAndShootEnemy();
    }

    void SetTarget()
    {

        Collider[] cols = Physics.OverlapSphere(transform.position, range, layerMask);
        Transform shortestTarget = null;
        if(cols.Length > 0)
        {
            float shortestDistance = Mathf.Infinity;
            foreach(Collider target in cols)
            {
                float distance = Vector3.SqrMagnitude(transform.position - target.transform.position);
                if(shortestDistance > distance) {

                    shortestDistance = distance;
                    shortestTarget = target.transform;
                }
            }
        }

        target = shortestTarget;

        /*
        if (target.GetComponent<Health>().isDead)
            target = null;
        */
    }

    void DetectAndShootEnemy()
    {
        //Health targetHealth = target.GetComponent<Health>();

        if (target != null)
        {
            anim.SetBool("isAiming", true);

            Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
            Vector3 euler = Quaternion.RotateTowards(transform.rotation, lookRotation, spinSpeed * Time.deltaTime).eulerAngles;
            transform.rotation = Quaternion.Euler(0, euler.y, 0);

            Quaternion fireRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
            if (Quaternion.Angle(transform.rotation, fireRotation) < 5f)
            {
                currentFireRate -= Time.deltaTime;
                if (currentFireRate <= 0)
                {
                    currentFireRate = fireRate;
                    Debug.Log("น฿ป็");
                    anim.SetTrigger("doShoot");
                    //anim.SetBool("isShooting", true);

                    ShootFeedback?.PlayFeedbacks();
                    Rigidbody rb = Instantiate(projectile, muzzlePoint.position, Quaternion.identity).GetComponent<Rigidbody>();

                    rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
                    rb.AddForce(transform.up * 8f, ForceMode.Impulse);
                }
            }
        }

        else
        {
            anim.SetBool("isAiming", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    }

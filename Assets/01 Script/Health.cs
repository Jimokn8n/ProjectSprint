using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int curHealth;
    public int maxHealth;

    bool isDamaged;
    public bool isDead;

    //Ragdoll Set
    public GameObject skeleton;

   
    void Start()
    {
        //Ragdoll Set
        setRigidbodyState(true);
        setColliderState(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    void setRigidbodyState(bool state)
    {
        // �� �ȿ� �ִ� ������ٵ� ���¸� �����Ѵ�.
        Rigidbody[] rigidbodies = skeleton.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = state;
        }
    }
    

    void setColliderState(bool state)
    {
        // �� �ȿ� �ִ� �ݶ��̴� ���¸� �����Ѵ�.
        Collider[] colliders = skeleton.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = state;
        }
    }

    IEnumerator OnDamage()
    {
        isDamaged = true;
        curHealth -= 10;

        if(curHealth <= 0 && !isDead)
        {
            Death();
        }
        
         //SoundManager.instance.SFXPlay("Dodge", clips[4]);

        yield return new WaitForSeconds(.5f);
        isDamaged = false;

    }

    public void Death()
    {
        Debug.Log("Im Dead");
        isDead = true;
        
        gameObject.GetComponent<Animator>().enabled = false;
        gameObject.layer = 9;

        setRigidbodyState(false);
        setColliderState(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile" && !isDamaged)
        {
                StartCoroutine(OnDamage());
        }
        
    }
}

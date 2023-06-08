using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAI : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;
    public Transform player;
    public Animator animator;
    public LayerMask whatIsGround, whatIsPlayer; 
    public EnemyType enemyType;
    public GameObject explosionEffect;
    public float blastRadius;
    public float explosionForce;
    public bool DestroyOnImpact;
    public bool hasProjectile = false;

    //Patroling
    public Vector3 walkPoint; 
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked; 
    public GameObject projectile;
    public float health;
    private bool dead = false;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private float timer;

    private void Awake()
    {
        player = GameObject.Find("Player").transform; //Todo Change to player name
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update(){
        if (!dead){
            //Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (!playerInSightRange && !playerInAttackRange) Patroling();
            if (playerInSightRange && !playerInAttackRange) ChasePlayer();
            if (playerInAttackRange && playerInSightRange) {
                AttackPlayer();
                alreadyAttacked = true;
                agent.SetDestination(transform.position);
            }else{
                animator.ResetTrigger("isAttack");
                animator.SetTrigger("isIdle");
            }
            
            timer += Time.deltaTime;
            // Check if the desired interval has passed
            if (timer >= timeBetweenAttacks)
            {
                alreadyAttacked = false;
                timer = 0f;
                if (!playerInAttackRange && playerInSightRange)
                {
                    animator.SetTrigger("isWalk");
                }
            }
        }else{
            agent.SetDestination(transform.position);
        }
    }

    void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.name == "BasicArrow(Clone)")
        {
            TakeDamage(5);
        }
    }

    private void Patroling()
    {
        //animator.SetBool("isWalking", true);
        if(!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false; 
        
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }
    private void ChasePlayer()
    {
        animator.SetTrigger("isWalk");
        if(alreadyAttacked) 
        {
            agent.SetDestination(transform.position);
            return;
        }
        //animator.SetBool("isAttack", false);
        //animator.SetBool("isWalking", true);
        agent.SetDestination(player.position);
    }

    public void AttackPlayer()
    {
        transform.LookAt(player);

        if (!alreadyAttacked){
            animator.ResetTrigger("isWalk");
            animator.ResetTrigger("isIdle");
            animator.SetTrigger("isAttack");
            alreadyAttacked = true;
            if (hasProjectile)
            {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 64f, ForceMode.Impulse);
            rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            }
            else{
                PlayerHealth.Instance.doDamege(5);
            }
            //todo add attack here
        }
        if (alreadyAttacked)
        {
            animator.ResetTrigger("notInAttack");
            animator.SetTrigger("isIdle");

        }
        /*
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        foreach(Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, blastRadius);
            }
        }
        if (DestroyOnImpact){
             DestroyEnemy();
        }
        */
        
    }
    private void ResetAttack()
    {
        alreadyAttacked = false; 
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), .5f);
    }
    private void DestroyEnemy()
    {

        dead = true;
        animator.SetTrigger("death");
        Debug.Log("Death");
        Invoke("DestroyObject", 1.5f);
    }
    private void DestroyObject()
{
    Destroy(gameObject);
}
    private void OnDrawGizmosSelected()
    {
        //todo add hit color

    }
}

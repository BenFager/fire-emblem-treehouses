using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUnit : MonoBehaviour
{
    Animator animator;
    public MovementType movementType;
    public int numOfMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void Move()
    {

    }
    void UseItem()
    {

    }
    void Attack()
    {

    }
    void EnableAction()
    {
        animator.SetBool("active", true);
    }
    void DisableAction()
    {
        animator.SetBool("active", false);
    }
}
public enum MovementType
{
    MOUNTED, GROUNDED, FLIER
}
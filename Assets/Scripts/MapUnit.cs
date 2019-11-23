using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUnit : MonoBehaviour
{
    Animator animator;
    public MovementType movementType;
    public int numOfMovement;
    public Vector2Int loc;
    WorldMap worldMap;
    public bool hasTurn = true;
    public List<Vector2Int> path;
    
    // Start is called before the first frame update
    public void Start()
    {
        animator = GetComponent<Animator>();
        worldMap = GameObject.FindGameObjectWithTag("WorldMap").GetComponent<WorldMap>();
        loc = worldMap.WorldToArrayPos(transform.position);
        transform.position = worldMap.arrayPosToWorld(loc) + new Vector2(0.5f, 0.5f);
    }

    // Update is called once per frame
    public void Update()
    {
        animator.SetBool("active", hasTurn);

        if(path != null)//or boolean like moving
        {
            //check if path.count > 1, 
            if(path.Count <= 1)
            {
                path = null;
            }
            else
            {
                Vector2 nextPoint = worldMap.arrayPosToWorld(path[1]) + new Vector2(0.5f, 0.5f);
                Vector2 currentPos = Vector2.MoveTowards(transform.position, nextPoint, 10 * Time.deltaTime /*mach 5*/);
                if(currentPos == nextPoint)
                {
                    path.RemoveAt(0);
                }
                transform.position = currentPos;
                if(path.Count <= 1)
                {
                    //Tell playercontroller that we're done moving
                    loc = path[0];
                }
            }
        }

    }
    public void Move(List<Vector2Int> path)
    {
        this.path = new List<Vector2Int>(path);
    }
    public void UseItem()
    {

    }
    public void Attack()
    {

    }
    public void EnableAction()
    {
        //animator.SetBool("active", true);
        hasTurn = true;
    }
    public void DisableAction()
    {
        //animator.SetBool("active", false);
        hasTurn = false;
    }
}
public enum MovementType
{
    MOUNTED, GROUNDED, FLIER
}

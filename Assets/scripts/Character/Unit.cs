using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract partial class Unit : MonoBehaviour
{
    public Transform unitManager;
    public GameObject targetObject { get; set; }
    public float speed { get; protected set; } = 3.0f;
    public bool isMoving { get; protected set; }


    private void Awake()
    {
        animator = GetComponent<Animator>();

    }
    private void Start()
    {
        unitManager = transform.parent;

    }

    private void Update()
    {
        HandleAnimation();
    }

    public abstract void SetTarget(GameObject target);    

    public virtual IEnumerator MoveToTarget(GameObject target)
    {
        /// we do it as a coroutine because we don't want the movement to be executed during update
        /// because it set isMoving to true every frame if we do so

        isMoving = true;
        //rotation of the movement
        Vector3 targetRotation = (target.transform.position - transform.position).normalized;
        Quaternion moveRot = Quaternion.LookRotation(targetRotation);

        //move to the target object if check....Target method return true
        while (!CheckDistanceToTarget(target))
        {
            //direction of the movement
            Vector3 moveDir = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

            transform.SetPositionAndRotation(moveDir, moveRot);
            //rotate to move direction if idle (not sure about the idle condition)            
            if (targetRotation != Vector3.zero)
            {
                transform.rotation = moveRot;
            }

            yield return null;
        }
    }

    public bool CheckDistanceToTarget(GameObject target)
    {
        //check if the object is reached
        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            OnObjectReached();
            return true;
        }
        else return false;
    }

    public virtual void OnObjectReached()
    {
        Debug.Log("object reached");
        //HandleLayerMaskWeight();
        DecideNextAction();
    }

    #region non utilisée pour l'instant mais peut etre utlie
    public virtual void Leave(Vector3 WorldPos)
    {        
        LeaveWithObject(WorldPos);
        Debug.Log("Leave method called");
    }

    public virtual void LeaveWithObject(Vector3 worldSpaceTarget)
    {
        transform.position = Vector3.MoveTowards(transform.position, worldSpaceTarget, speed * Time.deltaTime);
        CheckDistanceToTarget(worldSpaceTarget);
    }

    public bool CheckDistanceToTarget(Vector3 targetPos)
    {
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            OnObjectReached();
            return true;
        }
        else return false;
    }

    #endregion
}

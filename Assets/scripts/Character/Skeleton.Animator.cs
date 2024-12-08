using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public partial class Skeleton
{

    int _idleAnimation = Animator.StringToHash("Standard Idle");
    public override int _IdleAnimation 
    {
        get { return _idleAnimation; }        
    }
 
    int _runAnimation = Animator.StringToHash("Standard Run");
    public override int _RunAnimation 
    {
        get { return _runAnimation; }        
    }
       
    int _ReadytoLiftIdleAnimation = Animator.StringToHash("Ready To Lift Idle");
    int _LiftUpAnimation = Animator.StringToHash("Lift Up");
   //the booleans used in this script are in the Skeleton.Cs script        

    public override int GetState()
    {        
        if (ReadyToLift)
        {
            if (LiftAnObject)
            {
                //if(currentState != _LiftUp) Debug.Log("liftUp state");
                return _LiftUpAnimation;
            }
            else
            {
                return _ReadytoLiftIdleAnimation;
            }
        }

        if (isMoving)
        {
            return _RunAnimation;
        }

        return _IdleAnimation;

    }

    public override void HandleLayerMaskWeight()
    {
        if (LiftAnObject)
        {
            animator.SetLayerWeight(1, 1);
        }
        else
        {
            animator.SetLayerWeight(1, 0);
        }
    }
 
}

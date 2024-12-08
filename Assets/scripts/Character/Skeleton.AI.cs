using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Skeleton
{
    
    public override void DecideNextAction()
    {
        //mettre le squelette est a la bonne position et attend juste de soulever l'objet
        if (targetObject != null && !ReadyToLift)
        {
            GotoLiftPosition();
        }       
    }

    private void GotoLiftPosition()
    {
        //est appelé quand l'unit va vers une piece
        //pour quand le target est un objet a déplacer
        if (!ReadyToLift)
        {
            if (targetObject.GetComponent<ObjectToMove>() != null)
            {
                targetObject.GetComponent<ObjectToMove>().AddUnit(this);
                ReadyToLift = true;
            }
        }          
    }

    /// <summary>
    /// the trigger of the Animator to call when all the units needed are ready
    /// for the skeleton it's lift an object action
    /// </summary>
    /// <param name="triggerKey"></param>
    public override void ActionOnAllUnitsReady()
    {
        LiftAnObject = true;
    }
    /// <summary>
    /// set readyToLift boolean to value
    /// </summary>
    /// <param name="value"></param>
    public override void SetStateBool(bool value)
    {
        ReadyToLift = value;
    }
    /// <summary>
    /// set liftAnObject boolean to value
    /// </summary>
    /// <param name="value"></param>
    public override void SetStateBool1(bool value)
    {
        LiftAnObject = value;
    }
}

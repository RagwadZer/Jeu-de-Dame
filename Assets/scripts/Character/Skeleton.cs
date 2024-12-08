using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Skeleton : Unit
{    
    public bool ReadyToLift { get; set; } = false;
    public bool LiftAnObject { get; set; } = false;

    /// <summary>
    /// set a target for the unit.
    /// on appelle la fonction pour faire déplacer le personnage
    /// mais également pour le faire partir quand il faut emmener la piece
    /// </summary>
    /// <param name="target">target object of type GameObject </param>
    public override void SetTarget(GameObject target)
    {
        targetObject = target;
        ReadyToLift = false;
        LiftAnObject = false;
        HandleLayerMaskWeight();
        
     /* * we want MoveToObject methode to be called only when the unit is moving toward a piece
        * and when the unit has reached the piece we let the piece handle the movement
        * to do so : we want readyToLift = false because we dont want the units to move when it's waiting for all units to get ready
        * lift an object is true because the ObjectToMove.Cs script will handle the leaving movement 
        * */
        if (targetObject != null && (!ReadyToLift || !LiftAnObject))
        {
            StartCoroutine(MoveToTarget(target));
        }
    }    
}

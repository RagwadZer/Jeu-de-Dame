using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Unit
{
    /// <summary>
    /// for when an unit reach the target he was moving for
    /// </summary>
    public abstract void DecideNextAction();

    /// <summary>
    /// some action nne to wait that all units are ready
    /// </summary>
    public abstract void ActionOnAllUnitsReady(); 
    
    /// <summary>
    /// all unit have an isMoving bool 
    /// use this to set the value
    /// </summary>
    /// <param name="value"></param>
    public virtual void SetIsMovingBool(bool value)
    {
        isMoving = value;
    }

    /// <summary>
    /// to set an bool that are specific of each unit type
    /// </summary>
    /// <param name="value"></param>
    public abstract void SetStateBool(bool value);

    /// <summary>
    /// to set another bool that are specific of each unit type
    /// </summary>
    /// <param name="value"></param>
    public abstract void SetStateBool1(bool value);

}

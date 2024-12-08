using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Unit 
{
    public Animator animator { get; protected set; }
    public int currentState { get; protected set; }
    public abstract int _IdleAnimation { get;}
    public abstract int _RunAnimation { get;}

    public void HandleAnimation()
    {
        int state = GetState();

        if (state != currentState)
        {
            animator.CrossFade(state, 0, 0);
            currentState = state;
        }
    }

    public abstract int GetState();

    public virtual void HandleLayerMaskWeight() { }
}

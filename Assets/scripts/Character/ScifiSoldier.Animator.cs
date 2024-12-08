using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ScifiSoldier
{
    private int _idleAnimation = Animator.StringToHash("Idle");
    public override int _IdleAnimation
    {
        get => _idleAnimation;
    }
    private int _runAnimation = Animator.StringToHash("Run");
    public override int _RunAnimation
    {
        get => _runAnimation;
    }

    private int _AimingAnimation = Animator.StringToHash("Idle_AimGun");
    private int _ShootingAnimation = Animator.StringToHash("Idle_AimGun");
    private int shootingRepetition = 0;

    public override int GetState()
    {
        if (isMoving) return _RunAnimation;
        if(IsAiming) return _ShootingAnimation;
        if (IsShooting)
        {
            //the animation play 2 times. 
            if (shootingRepetition < 2)
            {
                shootingRepetition++;
                return _ShootingAnimation;
            }
            else
            {
                shootingRepetition = 0;
            }
        }
        return _IdleAnimation;

    }


}

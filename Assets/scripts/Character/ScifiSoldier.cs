using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class ScifiSoldier : Unit
{
    private bool IsAiming;
    private bool IsShooting;    
    
    public override void SetTarget(GameObject target)
    {
        print($"{this.name} � re�u le signal");
        StartCoroutine(MoveToTarget(target));
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

  


}

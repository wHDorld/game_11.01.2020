using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Angry : MonoBehaviour
{
    public AIController[] who_is_angry;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponentInParent<PlayerController>())
            return;
        foreach (var a in who_is_angry)
            a.iRule.self.isAngry = true;
        enabled = false;
    }
}

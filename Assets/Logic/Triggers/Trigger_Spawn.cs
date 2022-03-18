using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Spawn : MonoBehaviour
{
    public SpawnClass[] spawns;

    public bool isTriggered = false;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTriggered)
            return;
        if (!collision.gameObject.GetComponentInParent<PlayerController>())
            return;
        isTriggered = true;
        var player = GameObject.FindObjectOfType<PlayerController>();
        foreach (var a in spawns)
            player.StartCoroutine(_spawn(a));
        enabled = false;
    }

    IEnumerator _spawn(SpawnClass what)
    {
        yield return new WaitForSeconds(what.interval);
        foreach (var b in what.who)
        {
            var c = Instantiate(b) as GameObject;
            c.transform.position = what.where.position;
            c.transform.rotation = what.where.rotation;
            yield return null;
            if (c.GetComponent<AIController>())
                c.GetComponent<AIController>().iRule.self.isAngry = true;
        }
    }

    [System.Serializable]
    public class SpawnClass
    {
        public Transform where;
        public float interval;
        public Object[] who;
    }
}

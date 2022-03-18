using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailUPD : MonoBehaviour
{
    public LineRenderer tail_renderer;
    public Transform[] tails_element;

    public void Start()
    {
        for (int i = 1; i < tails_element.Length; i++)
            tails_element[i].SetParent(null);
    }

    void Update()
    {
        for (int i = 0; i < tails_element.Length; i++)
            tail_renderer.SetPosition(i, tails_element[i].position);
    }

    private void OnDestroy()
    {
        foreach (var a in tails_element)
            if (a != null)
                Destroy(a.gameObject);
    }
}

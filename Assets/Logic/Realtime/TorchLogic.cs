using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLogic : MonoBehaviour
{
    SFLight slight;
    float initiate_int;
    // Start is called before the first frame update
    void Start()
    {
        slight = GetComponent<SFLight>();
        initiate_int = slight.intensity;
    }

    float timer = 0;
    float intensity = 0;
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        timer = Random.Range(0, 0.2f);
        intensity = Random.Range(initiate_int - initiate_int / 6f, initiate_int + initiate_int / 6f);
        slight.intensity = Mathf.Lerp(slight.intensity, intensity, 8f * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StealthLogic_UI : MonoBehaviour
{
    public AIController ai;
    public RectTransform[] gauges;
    public Image[] gauges_img;
    public Image[] gauges2_img;
    
    Transform player;
    RectTransform rt;
    private void Start()
    {
        rt = GetComponent<RectTransform>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    void Update()
    {
        coef = ai.iRule.self.view_gauge;
        this_rotate();
        this_position();
        gauge_rotate();
        gauge_color();
        gauge_opacity();
    }

    void this_rotate()
    {
        Vector2 dir = -ai.transform.position + player.position;
        transform.rotation = 
            Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90);
    }
    void this_position()
    {
        float dis = (-ai.transform.position + player.position).magnitude;
        float a_dis = Mathf.Pow(dis, 0.4f) - 1.1f;
        for (int i = 0; i < gauges.Length; i++)
            gauges2_img[i].rectTransform.anchoredPosition = new Vector2(
                    0, a_dis
                );
    }

    void gauge_rotate()
    {
        for (int i = 0; i < gauges.Length; i++)
        {
            gauges[i].localEulerAngles = Vector3.Lerp(
                gauges[i].localEulerAngles,
                new Vector3(0, 0, (1f - coef) * 45f ),
                6f * Time.deltaTime);
        }
    }
    void gauge_color()
    {
        for (int i = 0; i < gauges_img.Length; i++)
        {
            if (coef < 0.5f)
                gauges_img[i].color = new Color(1, 1, 1, opacity);
            else if (coef >= 0.5f && coef < 0.99f)
                gauges_img[i].color = new Color(1, 0.5f, 0, opacity);
            else
                gauges_img[i].color = new Color(1, 0, 0, opacity);
        }
    }
    void gauge_opacity()
    {
        if (_opacity_time > 0)
        {
            _opacity_time -= Time.deltaTime;
            opacity = Mathf.Lerp(opacity, 0.8f, 15f * Time.deltaTime);

            for (int i = 0; i < gauges2_img.Length; i++)
                gauges2_img[i].color = Color.Lerp(
                gauges2_img[i].color,
                new Color(0.3f, 0.3f, 0.3f, 0.3f),
                15f * Time.deltaTime);

            return;
        }
        opacity = Mathf.Lerp(opacity, 0f, 15f * Time.deltaTime);

        for (int i = 0; i < gauges2_img.Length; i++)
            gauges2_img[i].color = Color.Lerp(
                gauges2_img[i].color,
                new Color(0.3f, 0.3f, 0.3f, 0.0f),
                15f * Time.deltaTime);
    }

    public float coef;
    public float opacity;

    float _opacity_time = 0;
    public void UPD()
    {
        if (coef < 0.5f)
            _opacity_time = ai.stealth.basic_time + 0.2f;
        else if (coef >= 0.5f && coef < 0.99f)
            _opacity_time = ai.stealth.suspicion_time + 0.2f;
        else
            _opacity_time = ai.stealth.angry_time + 0.2f;
    }
}

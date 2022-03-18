using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MainGameData;
using ArtifitialIntelligenceData;

public class EnemyInfoBar : MonoBehaviour
{
    public TextMeshProUGUI NAME;
    public RectTransform hp_bar;
    public RectTransform hp_flow;
    public Object damage_text;

    public void Start()
    {
        foreach (var a in gameObject.GetComponentsInChildren<Transform>())
        {
            if (a.name == "HP_FRONT") hp_bar = a.GetComponent<RectTransform>();
            if (a.name == "HP_FLOW") hp_flow = a.GetComponent<RectTransform>();
            if (a.name == "NAME") NAME = a.GetComponent<TextMeshProUGUI>();
        }
    }

    bool isOn;
    Image[] imgs;
    TextMeshProUGUI[] texts;
    Unit unit;
    AIRule iRule;
    Transform player;
    public void ON(Unit unit, AIRule iRule)
    {
        imgs = gameObject.GetComponentsInChildren<Image>();
        texts = gameObject.GetComponentsInChildren<TextMeshProUGUI>();

        player = GameObject.FindGameObjectWithTag("MainCamera").transform;
        isOn = true;
        this.unit = unit;
        this.iRule = iRule;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        unit.rule.self.OnHit += (args) =>
        {
            timer = 0.6f;
            var a = (UnitHitInfo)args[0];

            GameObject g = Instantiate(damage_text) as GameObject;
            g.transform.SetParent(transform, false);
            g.transform.localPosition = Vector3.zero + new Vector3(Random.Range(-0.6f, 0.6f), 0, 0);
            g.transform.localRotation = Quaternion.Euler(0, 0, 0);

            int dmg = Mathf.RoundToInt(a.CritDamage * 10f);
            g.GetComponentInChildren<TextMeshProUGUI>().text =
                "<" + GlobalData.damage_color_hex[a.DamageType] + ">" + dmg + 
                "</color><sprite=" + a.DamageType + ">";

            StartCoroutine(damage_text_move(g));
            Show_Bar();
            return true;
        };
    }

    public void Update()
    {
        if (!isOn) return;
        scale_update();
        //transform.rotation = Quaternion.LookRotation(-player.position + transform.position);
        NAME.text = iRule.data.ai.NAME;
        HP_Update();
        flow_update();
        pos_update();
        Opacity_Update();
    }
    void pos_update()
    {
        transform.position = unit.rule.data.transform.position + new Vector3(0, 1.2f, -1);
    }
    void scale_update()
    {
        float d = Vector3.Distance(player.position, transform.position) / 6f;
        if (d > 6)
            d = 0;

        transform.localScale = new Vector3(d, d, d);
    }
    void HP_Update()
    {
        hp_bar.sizeDelta = new Vector2(
            543.15f * (unit.rule.self.Health / unit.rule.data.initialAbilities.variableValue.MaxHealth),
            40
            );
    }

    float timer = 0;
    void flow_update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }
        hp_flow.sizeDelta = Vector2.Lerp(
            hp_flow.sizeDelta,
            new Vector2(
            543.15f * (unit.rule.self.Health / unit.rule.data.initialAbilities.variableValue.MaxHealth),
            40
            ),
            15f * Time.deltaTime);
    }

    float opacity = 0;
    void Opacity_Update()
    {
        for (int i = 0; i < imgs.Length; i++)
            imgs[i].color = Color.Lerp(
                imgs[i].color,
                new Color(
                    imgs[i].color.r,
                    imgs[i].color.g,
                    imgs[i].color.b,
                    opacity
                    ),
                6f * Time.deltaTime
                );
        for (int i = 0; i < texts.Length; i++)
            texts[i].color = Color.Lerp(
                texts[i].color,
                new Color(
                    texts[i].color.r,
                    texts[i].color.g,
                    texts[i].color.b,
                    opacity
                    ),
                6f * Time.deltaTime
                );
    }
    void Show_Bar()
    {
        if (current_show_bar != null)
            StopCoroutine(current_show_bar);
        current_show_bar = StartCoroutine(show_bar());
    }
    Coroutine current_show_bar;
    IEnumerator show_bar()
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 10;
            opacity = t;
            yield return null;
        }
        yield return new WaitForSeconds(1);
        while (t > 0)
        {
            t -= Time.deltaTime;
            opacity = t;
            yield return null;
        }
    }

    IEnumerator damage_text_move(GameObject dm_text)
    {
        float timer = 0;
        float random_speed = Random.Range(0.9f, 1.2f);
        Transform t = dm_text.transform;
        TextMeshProUGUI tm = dm_text.GetComponentInChildren<TextMeshProUGUI>();

        while (timer < 1.2f)
        {
            t.localPosition += new Vector3(0, 0.01f * random_speed, 0);
            timer += Time.deltaTime;
            if (timer > 1)
            {
                tm.color =
                    new Color(
                        tm.color.r,
                        tm.color.g,
                        tm.color.b,
                        (1.2f - timer) * 5f
                        );
            }

            yield return null;
        }

        Destroy(dm_text);
    }
}

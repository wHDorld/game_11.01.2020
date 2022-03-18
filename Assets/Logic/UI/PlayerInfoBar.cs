using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MainGameData;
using TMPro;

public class PlayerInfoBar : MonoBehaviour
{
    public RectTransform hp_pos;
    public RectTransform hp_front;
    public RectTransform hp_flow;
    public RectTransform stamina_front;
    public RectTransform stamina_flow;
    public Object damage_text;

    PlayerController player;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        player.rule.self.OnHit += (args) =>
        {
            timer = 1.4f;
            var a = (UnitHitInfo)args[0];

            GameObject g = Instantiate(damage_text) as GameObject;
            g.transform.SetParent(hp_pos, false);
            g.transform.localPosition = Vector3.zero + new Vector3(Random.Range(-0.6f, 0.6f), 0, 0);
            g.transform.localRotation = Quaternion.Euler(0, 0, 0);

            int dmg = Mathf.RoundToInt(a.Damage * 10f);
            g.GetComponentInChildren<TextMeshProUGUI>().text =
                "<" + GlobalData.damage_color_hex[a.DamageType] + ">" + dmg +
                "</color><sprite=" + a.DamageType + ">";

            StartCoroutine(damage_text_move(g));
            return true;
        };
        player.rule.self.OnStaminaOut += (args) =>
        {
            s_timer = 0.5f;
            return true;
        };
    }

    void Update()
    {
        hp_pos.rotation = Quaternion.Lerp(
            hp_pos.rotation,
            Quaternion.Euler(0, 0, 0),
            35f * Time.deltaTime);
        HP_Update();
        flow_update();
        STAMINA_Update();
        stamina_flow_update();
    }

    void HP_Update()
    {
        hp_front.sizeDelta = new Vector2(
            2f * (player.rule.self.Health / player.rule.data.initialAbilities.variableValue.MaxHealth),
            0.08f
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
            2f * (player.rule.self.Health / player.rule.data.initialAbilities.variableValue.MaxHealth),
            0.08f
            ),
            15f * Time.deltaTime);
    }

    void STAMINA_Update()
    {
        stamina_front.sizeDelta = new Vector2(
            1.5f * (player.rule.self.Stamina / player.rule.data.initialAbilities.variableValue.MaxStamina),
            0.08f
            );
    }
    float s_timer = 0;
    void stamina_flow_update()
    {
        if (s_timer > 0)
        {
            s_timer -= Time.deltaTime;
            return;
        }
        stamina_flow.sizeDelta = Vector2.Lerp(
            stamina_flow.sizeDelta,
            new Vector2(
            1.5f * (player.rule.self.Stamina / player.rule.data.initialAbilities.variableValue.MaxStamina),
            0.08f
            ),
            15f * Time.deltaTime);
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

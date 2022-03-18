using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGameData;
using Modules;

public class DoggoController : Unit
{
    public InitialAbilities initialAbilities;
    public WeaponData.InstanceWeapon instanceWeapon;
    public LineRenderer tail_renderer;
    public Transform[] tails_element;

    void Start()
    {
        tail_renderer.transform.SetParent(null);
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindObjectOfType<PlayerController>();
        Initiate(new UnitRule(gameObject, initialAbilities, "Player"), new WeaponRule(instanceWeapon, GetComponentInChildren<Animator>(), false));
        permanent_modules.Add(new C_LookAt(rule));

        StartCoroutine(Enemy_Update());
        StartCoroutine(Force_Update());
    }

    void Update()
    {
        Move();
        Look();
        AnimatorC();
        Tail_Update();
        TriggerUpdate();
    }

    public IEnumerator Enemy_Update() 
    {
        while (true)
        {
            List<AIController> a = new List<AIController>();
            AIController.CallEvent(AIController.AiEvents.OnAngry, ref a);
            if (a.Count != 0)
            enemy_now = a[Random.Range(0, a.Count)];
            yield return new WaitForSeconds(Random.Range(0, 4));
        }
    }
    public IEnumerator Force_Update()
    {
        while (true)
        {
            if (enemy_now != null && enemy_now.rule.self.Health > 0)
            {
                rigidbody2d.AddForce((move.normalized * rigidbody2d.mass) * -10f, ForceMode2D.Impulse);
                yield return new WaitForSeconds(0.6f);
                rigidbody2d.AddForce((move.normalized * rigidbody2d.mass) * 40f, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(Random.Range(2, 3));
        }
    }
    public void Look()
    {
        rule.preferableSystem.preferable_look_pos = look_at_pos;
    }
    public void Move()
    {
        rigidbody2d.AddForce((move * rigidbody2d.mass) / 16f , ForceMode2D.Impulse);
        if (enemy_now == null || enemy_now.rule.self.Health <= 0)
        {
            Stop();
            return;
        }
        move = enemy_now.transform.position - transform.position;
        look_at_pos = Vector3.Lerp(look_at_pos, enemy_now.transform.position, 2f * Time.deltaTime);
    }
    public void Stop()
    {
        if (Vector2.Distance(transform.position, player.transform.position) >= 4)
            move = player.transform.position - transform.position;
        else
            move = Vector3.zero;
        look_at_pos = player.transform.position;
    }
    public void AnimatorC()
    {
        Vector3 local_move = transform.InverseTransformDirection(move);
        animator.SetFloat("runX", move.x);
        animator.SetFloat("runY", move.y);
        animator.SetFloat("move_speed", local_move.magnitude);
    }
    public void Tail_Update()
    {
        for (int i = 0; i < tails_element.Length; i++)
            tail_renderer.SetPosition(i, tails_element[i].position);
    }
    public void TriggerUpdate()
    {
        if (Vector2.Distance(transform.position, player.transform.position) >= 15)
            collider2d.isTrigger = true;
        else
            collider2d.isTrigger = false;
    }

    AIController enemy_now;
    PlayerController player;

    Vector3 move;
    Vector3 look_at_pos;
    Rigidbody2D rigidbody2d;
    Collider2D collider2d;
    Animator animator;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.GetComponentInParent<AIController>())
            return;
        var a = collision.gameObject.GetComponentInParent<AIController>();
        a.rule.self.Hurt = new UnitHitInfo()
        {
            Damage = GlobalData.DAMAGE_NOW(collision.contacts[0].relativeVelocity.magnitude / 25f, player.rule.self.Level),
            OriginalDamage = collision.contacts[0].relativeVelocity.magnitude / 25f,
            from = rule,
            DamageType = 5,
            directional = collision.contacts[0].point - new Vector2(transform.position.x, transform.position.y),
            from_pos = transform.position,
            hit_pos = collision.contacts[0].point,
            same_tag = "Player",
            crit_name = collision.gameObject.name,
            hit_object = collision.gameObject
        };
    }
}

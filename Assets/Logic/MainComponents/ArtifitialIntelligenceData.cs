using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MainGameData;
using Modules;
using WeaponData;

namespace ArtifitialIntelligenceData
{
    public class AIRule
    {
        public Data data;
        public Container container;
        public Self self;

        public AIRule(AI_Object ai_Object)
        {
            data = new Data()
            {
                ai = ai_Object
            };
            container = new Container();
            self = new Self();
        }

        public class Data
        {
            public AI_Object ai;
        }
        public class Container
        {
            public Vector3 last_player_pos;
            public StealthLogic_UI stealth;
            public int ActualShootCount = 5;

            public int RefreshActualShoot
            {
                set
                {
                    ActualShootCount -= value;
                    if (ActualShootCount <= 0)
                        isOutOfActualShoot?.Invoke(new object[0]);
                }
            }

            public event main_delegate isOutOfActualShoot;

            Object view_gauge_ui;
            public GameObject get_viewGaugeUI
            {
                get
                {
                    view_gauge_ui = view_gauge_ui ?? Resources.Load("enemy_view_gauge");
                    return Object.Instantiate(view_gauge_ui) as GameObject;
                }
            }
            public void ApplyViewGauge(AIController ai)
            {
                GameObject infoCanvas = GameObject.FindGameObjectWithTag("InfoPlayerCanvas");
                var g = get_viewGaugeUI;
                g.transform.SetParent(infoCanvas.transform, false);
                stealth = g.GetComponent<StealthLogic_UI>();
                stealth.ai = ai;
            }
        }
        public class Self
        {
            public bool isAngry;
            public bool isSuspicion;
            public float view_gauge;
        }
    }


    [System.Serializable]
    public class InitiateStat
    {
        public ListOfControllModules[] initial_controll_modules;
        public InitialAbilities initialAbilities;
        public float random_aim_angle;
    }
    [System.Serializable]
    public class AI_Behaviour_Struct
    {
        public float[] actual_distance = new float[2];
        public float[] random_move_timing = new float[2];
        public int shoot_count_per_fire;
        public float fire_reload;
        [Range(0, 1)]
        public float actual_speed_use;
    }

    public enum MobType
    {
        UsualMob,
        MiniBoss,
        Boss
    }
    [System.Serializable]
    public class StealthSettings
    {
        [Header("Distance")]
        public float view_distance;

        [Header("View")]
        public float angle_of_view;
        public float reaction_speed;

        [Header("Suspicion")]
        public float suspicion_time;
        public float angry_time;
        public float basic_time;

        public float Cos_AngleOfView
        {
            get
            {
                if (baked_cos == -2)
                    baked_cos = Mathf.Cos(angle_of_view * Mathf.Deg2Rad);
                return baked_cos;
            }
        }
        float baked_cos = -2;
    }
}

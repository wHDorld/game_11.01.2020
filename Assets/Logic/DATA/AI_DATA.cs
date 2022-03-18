using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_DATA
{
    public class AI_Manager
    {
        public ai_controll controll;
        public ai_container container;

        public AI_Manager(UNIT[] owner_army, UNIT[] opposite_army, MapDATA.MAP_ELEMENT[,] owner_map, MapDATA.MAP_ELEMENT[,] opposite_map)
        {
            container = new ai_container(owner_army, opposite_army, owner_map, opposite_map);
            controll = new ai_controll(this);
        }
    }
    public class ai_container
    {
        public List<MOVE_ELEMENT> moves = new List<MOVE_ELEMENT>();
        public MapDATA.MAP_ELEMENT[,] owner_map;
        public MapDATA.MAP_ELEMENT[,] opposite_map;
        public UNIT[] owner_army;
        public UNIT[] opposite_army;

        public ai_container(UNIT[] owner_army, UNIT[] opposite_army, MapDATA.MAP_ELEMENT[,] owner_map, MapDATA.MAP_ELEMENT[,] opposite_map)
        {
            this.owner_army = owner_army;
            this.opposite_army = opposite_army;
            this.owner_map = owner_map;
            this.opposite_map = opposite_map;

            moves.Add(new MOVE_ELEMENT());
            moves[0].owner_team_units.AddRange(owner_army);
            moves[0].opposite_team_units.AddRange(opposite_army);
        }
    }
    public class ai_controll
    {
        AI_Manager link;
        public ai_controll(AI_Manager manager)
        {
            link = manager;
        }

        public bool MakeMove()
        {
            //if (link.container.moves.Count + 1 >= link.container.owner_map.GetLength(1) * 2)
                //return;
            if (link.container.moves[link.container.moves.Count - 1].owner_team_units.Count == 0
                ||
                link.container.moves[link.container.moves.Count - 1].opposite_team_units.Count == 0)
                return false;
            if (link.container.moves.Count % 2 == 0)
                isOwnerTurn();
            else
                isOppositeTurn();
            return true;
        }

        void isOwnerTurn()
        {
            MOVE_ELEMENT move = new MOVE_ELEMENT();
            for (int i = 0; i < link.container.moves[link.container.moves.Count - 1].owner_team_units.Count; i++)
                move.owner_team_units.Add(new UNIT()
                {
                    HP_NOW = link.container.moves[link.container.moves.Count - 1].owner_team_units[i].HP_NOW,
                    position = link.container.moves[link.container.moves.Count - 1].owner_team_units[i].position
                });
            for (int i = 0; i < link.container.moves[link.container.moves.Count - 1].opposite_team_units.Count; i++)
                move.opposite_team_units.Add(new UNIT()
                {
                    HP_NOW = link.container.moves[link.container.moves.Count - 1].opposite_team_units[i].HP_NOW,
                    position = link.container.moves[link.container.moves.Count - 1].opposite_team_units[i].position
                });

            for (int i = 0; i < move.owner_team_units.Count; i++)
            {
                if (move.owner_team_units[i].HP_NOW <= 0)
                {
                    link.container.moves[link.container.moves.Count - 1].owner_team_units.RemoveAt(i);
                    isOwnerTurn();
                    return;
                }

                int enemy = Random.Range(0, move.opposite_team_units.Count);

                float distance = Vector2.Distance(
                    new Vector2(move.opposite_team_units[enemy].position.x, (link.container.owner_map.GetLength(1) * 2f - 1) - move.opposite_team_units[enemy].position.y),
                    new Vector2(move.owner_team_units[i].position.x, (link.container.owner_map.GetLength(1) - move.owner_team_units[i].position.y))
                    );

                Fire(
                    move.owner_team_units[i],
                    move.opposite_team_units[enemy], 
                    distance,
                    Mathf.Sqrt(Mathf.Pow(link.container.owner_map.GetLength(0), 2f) + Mathf.Pow(link.container.owner_map.GetLength(1), 2f)) * 2f,
                    true
                    );
                if (move.owner_team_units[i].position.y < link.container.owner_map.GetLength(1) - 1)
                    move.owner_team_units[i].MoveForward();
            }
            link.container.moves.Add(move);
        }
        void isOppositeTurn()
        {
            MOVE_ELEMENT move = new MOVE_ELEMENT();
            for (int i = 0; i < link.container.moves[link.container.moves.Count - 1].owner_team_units.Count; i++)
                move.owner_team_units.Add(new UNIT()
                {
                    HP_NOW = link.container.moves[link.container.moves.Count - 1].owner_team_units[i].HP_NOW,
                    position = link.container.moves[link.container.moves.Count - 1].owner_team_units[i].position
                });
            for (int i = 0; i < link.container.moves[link.container.moves.Count - 1].opposite_team_units.Count; i++)
                move.opposite_team_units.Add(new UNIT()
                {
                    HP_NOW = link.container.moves[link.container.moves.Count - 1].opposite_team_units[i].HP_NOW,
                    position = link.container.moves[link.container.moves.Count - 1].opposite_team_units[i].position
                });

            for (int i = 0; i < move.opposite_team_units.Count; i++)
            {
                if (move.opposite_team_units[i].HP_NOW <= 0)
                {
                    link.container.moves[link.container.moves.Count - 1].opposite_team_units.RemoveAt(i);
                    isOppositeTurn();
                    return;
                }

                int enemy = Random.Range(0, move.owner_team_units.Count);

                float distance = Vector2.Distance(
                    new Vector2(move.opposite_team_units[i].position.x, (link.container.owner_map.GetLength(1) * 2f - 1) - move.opposite_team_units[i].position.y),
                    new Vector2(move.owner_team_units[enemy].position.x, (link.container.owner_map.GetLength(1) - move.owner_team_units[enemy].position.y))
                    );

                Fire(
                    move.opposite_team_units[i],
                    move.owner_team_units[enemy],
                    distance, 
                    Mathf.Sqrt(Mathf.Pow(link.container.owner_map.GetLength(0), 2f) + Mathf.Pow(link.container.owner_map.GetLength(1), 2f)) * 2f,
                    false
                    );
                if (move.opposite_team_units[i].position.y < link.container.owner_map.GetLength(1) - 1)
                    move.opposite_team_units[i].MoveForward();
            }
            link.container.moves.Add(move);
        }

        void Fire(UNIT from, UNIT to, float distance, float max_distance, bool isOwner)
        {
            float chance = (max_distance - distance) / max_distance;
            if (isOwner)
                chance *=
                    link.container.owner_map[to.position.x, to.position.y].coverType == MapDATA.CoverType.NoCover ? 1 :
                    (link.container.owner_map[to.position.x, to.position.y].coverType == MapDATA.CoverType.LightCover ? 0.8f : 0.5f);
            if (!isOwner)
                chance *=
                    link.container.opposite_map[to.position.x, to.position.y].coverType == MapDATA.CoverType.NoCover ? 1 :
                    (link.container.opposite_map[to.position.x, to.position.y].coverType == MapDATA.CoverType.LightCover ? 0.8f : 0.5f);

            if (Random.value >= chance)
                return;
            to.HP_NOW -= 1;
        }
    }
    #region DATA ELEMENTS
    public class MOVE_ELEMENT
    {
        public List<UNIT> owner_team_units = new List<UNIT>();
        public List<UNIT> opposite_team_units = new List<UNIT>();
    }
    public class UNIT
    {
        public int HP_NOW;
        public Vector2Int position;

        public void MoveForward()
        {
            position += new Vector2Int(0, 1);
        }
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using MapDATA;
using AI_DATA;

public class AI_Logic : MonoBehaviour
{
    public AI_Manager ai;
    public MapGenerator generator;
    public int width;
    public int heigth;
    public int health;

    void Start()
    {
        generator = new MapGenerator();
        ai = new AI_Manager(
            generator.Generate_Unit(health, width),
            generator.Generate_Unit(health, width),
            generator.Generate_Map(width, heigth),
            generator.Generate_Map(width, heigth)
            );
        UpdateMoves();
    }
    void UpdateMoves()
    {
        while (ai.controll.MakeMove())
        {
            continue;
        }
        var fs = File.Create("path_debug.txt");
        fs.Close();
        StreamWriter sw = new StreamWriter("path_debug.txt");
        for (int i = 0; i < ai.container.moves.Count; i++)
        {
            sw.WriteLine();
            sw.Write(string.Format("_> STATE : {0} / {1}", i + 1, ai.container.moves.Count));
            sw.WriteLine();

            for (int y = 0; y < ai.container.owner_map.GetLength(1) * 2; y++)
            {
                sw.WriteLine();
                sw.Write("\t");
                if (y < ai.container.owner_map.GetLength(1))
                {
                    for (int x = 0; x < ai.container.owner_map.GetLength(0); x++)
                    {
                        int points = -1;
                        for (int j = 0; j < ai.container.moves[i].opposite_team_units.Count; j++)
                            if (ai.container.moves[i].opposite_team_units[j].position == new Vector2Int(x, y))
                            {
                                points = ai.container.moves[i].opposite_team_units[j].HP_NOW;
                                break;
                            }
                        if (points != -1)
                        {
                            sw.Write(points);
                            continue;
                        }
                        sw.Write(
                            ai.container.opposite_map[x, y].coverType == CoverType.NoCover ? " " :
                            (ai.container.opposite_map[x, y].coverType == CoverType.LightCover ? "-" : "=")
                            );
                    }
                }
                else
                {
                    for (int x = 0; x < ai.container.owner_map.GetLength(0); x++)
                    {
                        int points = -1;
                        int _y = (ai.container.owner_map.GetLength(1) * 2 - 1) - y;
                        for (int j = 0; j < ai.container.moves[i].owner_team_units.Count; j++)
                            if (ai.container.moves[i].owner_team_units[j].position == new Vector2Int(x, _y))
                            {
                                points = ai.container.moves[i].owner_team_units[j].HP_NOW;
                                break;
                            }
                        if (points != -1)
                        {
                            sw.Write(points);
                            continue;
                        }
                        sw.Write(
                            ai.container.owner_map[x, _y].coverType == CoverType.NoCover ? " " :
                            (ai.container.owner_map[x, _y].coverType == CoverType.LightCover ? "-" : "=")
                            );
                    }
                }
            }
            sw.WriteLine();
        }
        sw.Close();
    }

    void Update()
    {
        
    }
}

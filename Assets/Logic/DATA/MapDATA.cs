using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapDATA
{
    public class MapGenerator
    {
        public MAP_ELEMENT[,] Generate_Map(int width, int heigth)
        {
            MAP_ELEMENT[,] ret = new MAP_ELEMENT[width, heigth];

            for (int y = 0; y < ret.GetLength(1); y++)
                for (int x = 0; x < ret.GetLength(0); x++)
                {
                    ret[x, y] = new MAP_ELEMENT()
                    {
                        position = new Vector2Int(x, y),
                        coverType = Random.value > 0.7f ? (Random.value > 0.6f ? CoverType.HeavyCover : CoverType.LightCover) : CoverType.NoCover
                    };
                }
            return ret;
        } 
        public AI_DATA.UNIT[] Generate_Unit(int health, int count)
        {
            List<AI_DATA.UNIT> ret = new List<AI_DATA.UNIT>();
            for (int i = 0; i < count; i++)
                ret.Add(new AI_DATA.UNIT() { HP_NOW = health, position = new Vector2Int(i, 0) });
            return ret.ToArray();
        }
    }

    #region DATA OBJECTS
    public class MAP_ELEMENT
    {
        public Vector2Int position;
        public CoverType coverType;
    }
    #endregion
    #region DATA ENUMS
    public enum CoverType
    {
        NoCover,
        LightCover,
        HeavyCover
    }
    #endregion
}

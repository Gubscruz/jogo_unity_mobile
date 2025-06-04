using System.Collections.Generic;
using UnityEngine;
public struct DicaInfo
{
    public int maxDicas;
    public int dicasVistas;

    public DicaInfo(int maxDicas, int dicasVistas)
    {
        this.maxDicas = maxDicas;
        this.dicasVistas = dicasVistas;
    }
}


public static class Dicas
{
    public static Dictionary<string, DicaInfo> dicasPorPuzzle = new Dictionary<string, DicaInfo>
    {
        { "puzzle1_sala1", new DicaInfo(3, 0) },
        { "puzzle1_sala2", new DicaInfo(4, 0) },
        { "puzzle2_sala2", new DicaInfo(4, 0) },
        { "puzzle1_sala3", new DicaInfo(5, 0) },
        { "puzzle2_sala3", new DicaInfo(5, 0) },
        { "puzzle2_sala4", new DicaInfo(5, 0) },
        { "puzzle3_sala4", new DicaInfo(5, 0) },
        { "puzzle1_sala5", new DicaInfo(5, 0) },
        { "puzzle1_salaSecreta", new DicaInfo(5, 0) }
    };
}

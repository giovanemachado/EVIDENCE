using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspect
{
    public enum Names
    {
        CALDEIRA,
        CASTRO,
        GREGOR,
        SALLES,
        BAUER,
        IGOR,
        JULIANO,
        THIAGO
    }

    public Names Name;
    public Local.Names[] seenIn;

    public Suspect(Names name, Local.Names[] seenIn)
    {
        this.Name = name;
        this.seenIn = seenIn;
    }

    public Sprite GetSprite()
    {
        switch (Name)
        {
            default:
                return Assets.Instance.DefaultSprite;
                case Names.CALDEIRA: return Assets.Instance.Caldeira;
                case Names.CASTRO: return Assets.Instance.Castro;
                case Names.GREGOR: return Assets.Instance.Gregor;
                case Names.SALLES: return Assets.Instance.Salles;
                case Names.BAUER: return Assets.Instance.Bauer;
                case Names.IGOR: return Assets.Instance.Igor;
                case Names.JULIANO: return Assets.Instance.Juliano;
                case Names.THIAGO: return Assets.Instance.Thiago;
        }
    }
}

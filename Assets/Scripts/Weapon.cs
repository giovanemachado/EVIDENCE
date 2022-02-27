using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public enum Types
    {
        DECORATION,
        CONCUSSION,
        SLASHING,
        NATURE,
        SUFFOCATING
    }

    public enum Names
    {
        KNIFE,
        SCISSORS,
        BROKEN_GLASS,
        PILLOW,
        BLANKET,
        TOWEL,
        FLOWER,
        FERTILIZER,
        TREE_BRANCH,
        BEDSIDE_LAMP,
        BOOK,
        FRAME,
        CAR,
        HAMMER,
        WRENCH,
        AXE
    }

    public Names Name;
    public Types Type;

    public Weapon(Names name, Types type)
    {
        this.Name = name;
        this.Type = type;
    }

    public Sprite GetSprite()
    {
        switch (Name)
        {
            default: return Assets.Instance.DefaultSprite;
            case Names.AXE: return Assets.Instance.Axe;
            case Names.KNIFE: return Assets.Instance.Knife;
            case Names.SCISSORS:  return Assets.Instance.Scissors;
            case Names.BROKEN_GLASS:  return Assets.Instance.BrokenGlass;
            case Names.PILLOW:  return Assets.Instance.Pillow;
            case Names.BLANKET:  return Assets.Instance.Blanket;
            case Names.TOWEL:  return Assets.Instance.Towel;
            case Names.FLOWER:  return Assets.Instance.Flower;
            case Names.FERTILIZER:  return Assets.Instance.Fertilizer;
            case Names.TREE_BRANCH:  return Assets.Instance.TreeBranch;
            case Names.BEDSIDE_LAMP:  return Assets.Instance.BedsideLamp;
            case Names.BOOK:  return Assets.Instance.Book;
            case Names.FRAME:  return Assets.Instance.Frame;
            case Names.CAR:  return Assets.Instance.Car;
            case Names.HAMMER:  return Assets.Instance.Hammer;
            case Names.WRENCH:  return Assets.Instance.Wrench;
        }
    }
}

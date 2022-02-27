using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Local
{
    public enum Names
    {
        BEDROOM,
        KITCHEN,
        GARDEN,
        REST_ROOM
    }

    public Names Name;
    public Weapon.Types WeaponTypeUsedHere;

    public Local(Names name, Weapon.Types weaponTypeUsedHere)
    {
        this.Name = name;
        this.WeaponTypeUsedHere = weaponTypeUsedHere;
    }
    public Sprite GetSprite()
    {
        switch (Name)
        {
            default:
                return Assets.Instance.DefaultSprite;
                case Names.BEDROOM: return Assets.Instance.Bedroom;
                case Names.GARDEN: return Assets.Instance.Garden;
                case Names.KITCHEN: return Assets.Instance.Kitchen;
                case Names.REST_ROOM: return Assets.Instance.RestRoom;
        }
    }
}

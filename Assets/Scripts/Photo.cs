using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photo 
{
    public enum Styles
    {
        LOCAL,
        WEAPON,
        SUSPECT,
        TIPS
    }

    public string Name;
    public string Description;
    public Styles Style;
    public Sprite Image;
        
    public Photo(string name, string description, Styles style)
    {
        this.Name = name;
        this.Description = description;
        this.Style = style;
    }
    public Photo(string name, string description, Styles style, Sprite image)
    {
        this.Name = name;
        this.Description = description;
        this.Style = style;
        this.Image = image;
    }
}

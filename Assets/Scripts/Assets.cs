using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Assets : MonoBehaviour
{
    public static Assets Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    [Header("Weapons")]
    public Sprite DefaultSprite;
    public Sprite Knife; 
    public Sprite Scissors;
    public Sprite BrokenGlass; 
    public Sprite Pillow;
    public Sprite Blanket;
    public Sprite Towel;
    public Sprite Flower;
    public Sprite Fertilizer;
    public Sprite TreeBranch;
    public Sprite BedsideLamp;
    public Sprite Book;
    public Sprite Frame;
    public Sprite Car;
    public Sprite Hammer;
    public Sprite Wrench;
    public Sprite Axe;

    [Header("Locals")]
    public Sprite Bedroom;
    public Sprite Garden;
    public Sprite Kitchen;
    public Sprite RestRoom;

    [Header("Suspects")]
    public Sprite Caldeira;
    public Sprite Castro;
    public Sprite Gregor;
    public Sprite Salles;
    public Sprite Bauer;
    public Sprite Igor;
    public Sprite Juliano;
    public Sprite Thiago;
}
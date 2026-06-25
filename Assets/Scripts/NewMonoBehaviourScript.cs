using UnityEngine;

public class SymbolDatabase : MonoBehaviour
{
    public Sprite seven;
    public Sprite cherry;
    public Sprite bell;
    public Sprite bar;

    public Sprite[] Symbols
    {
        get
        {
            return new Sprite[]
            {
                seven,
                cherry,
                bell,
                bar
            };
        }
    }
}
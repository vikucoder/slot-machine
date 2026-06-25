using System.Collections;
using UnityEngine;

public class ReelController : MonoBehaviour
{
    public RectTransform[] symbols;

    public float spinSpeed = 700f;
    public float symbolSpacing = 65f;

    private bool spinning = false;
    private int targetSymbol = 0;

    // Fixed symbol order:
    // 0 = 7
    // 1 = Cherry
    // 2 = Bell
    // 3 = BAR
    public int CurrentSymbol { get; private set; } = 0;

    void Update()
    {
        if (!spinning) return;

        foreach (RectTransform symbol in symbols)
        {
            symbol.anchoredPosition += Vector2.down * spinSpeed * Time.deltaTime;

            if (symbol.anchoredPosition.y < -symbolSpacing * 2)
            {
                float highest = GetHighestY();

                symbol.anchoredPosition = new Vector2(
                    symbol.anchoredPosition.x,
                    highest + symbolSpacing
                );
            }
        }
    }

    float GetHighestY()
    {
        float highest = symbols[0].anchoredPosition.y;

        foreach (RectTransform symbol in symbols)
        {
            if (symbol.anchoredPosition.y > highest)
                highest = symbol.anchoredPosition.y;
        }

        return highest;
    }
    public void SetTargetSymbol(int symbolIndex)
    {
    targetSymbol = symbolIndex;
    }

        public int GetTargetSymbol()
    {
        return targetSymbol;
    }   

    public void StartSpin()
    {
        spinning = true;
    }

    public void StopSpin()
{
    spinning = false;
    SnapToGrid();
}

   void SnapToGrid()
{
    System.Array.Sort(symbols, (a, b) =>
        b.anchoredPosition.y.CompareTo(a.anchoredPosition.y));

    float topY = 68f;   // Top visible position

    for (int i = 0; i < symbols.Length; i++)
    {
        symbols[i].anchoredPosition = new Vector2(
            symbols[i].anchoredPosition.x,
            topY - i * symbolSpacing
        );
    }
}
}
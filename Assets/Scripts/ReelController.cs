using UnityEngine;

public class ReelController : MonoBehaviour
{
    public RectTransform[] symbols;

    public float spinSpeed = 500f;
    public float symbolSpacing = 65f;

    public bool spinning = false;

    void Update()
    {
        if (!spinning) return;

        foreach (RectTransform symbol in symbols)
        {
            symbol.anchoredPosition += Vector2.down * spinSpeed * Time.deltaTime;

            if (symbol.anchoredPosition.y < -symbolSpacing * 2)
            {
                float highestY = GetHighestSymbolY();

                symbol.anchoredPosition = new Vector2(
                    symbol.anchoredPosition.x,
                    highestY + symbolSpacing
                );
            }
        }
    }

    float GetHighestSymbolY()
    {
        float highest = symbols[0].anchoredPosition.y;

        foreach (RectTransform symbol in symbols)
        {
            if (symbol.anchoredPosition.y > highest)
                highest = symbol.anchoredPosition.y;
        }

        return highest;
    }

    public void StartSpin()
    {
        spinning = true;
    }

    public void StopSpin()
    {
        spinning = false;
    }
}
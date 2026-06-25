using System.Collections;
using UnityEngine;

public class ReelController : MonoBehaviour
{
    public RectTransform[] symbols;

    public float spinSpeed = 700f;
    public float symbolSpacing = 65f;

    private bool spinning = false;

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
        foreach (RectTransform symbol in symbols)
        {
            float y = Mathf.Round(symbol.anchoredPosition.y / symbolSpacing) * symbolSpacing;

            symbol.anchoredPosition = new Vector2(
                symbol.anchoredPosition.x,
                y
            );
        }
    }
}
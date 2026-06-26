using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ReelController — RectTransform-as-truth design, fully corrected.
///
/// SINGLE SOURCE OF TRUTH:
///   ReelSlot.symbolIndex on each GameObject tells you what symbol that
///   physical slot is currently displaying. Position + identity always agree.
///
/// SPINNING (Update):
///   All slots move downward. When a slot wraps from bottom to top it must
///   display the symbol that logically comes next "off the top of the strip".
///   We track what the next incoming symbol is with topSymbolCursor, which
///   decrements each wrap (mod 4). The wrapping slot's ReelSlot.symbolIndex
///   is set to that cursor value — keeping visual and logical state identical.
///
/// STOPPING (SnapToTarget):
///   1. Sort symbols[] by Y → recovers top-to-bottom visual order.
///   2. Snap to grid.
///   3. Read middle slot's ReelSlot.symbolIndex.
///   4. If it doesn't equal targetSymbol, call CycleStripDown() which:
///        - physically moves the top RectTransform to the bottom (anchoredPosition)
///        - assigns it the next symbol in sequence
///        - re-snaps all positions
///      Repeat until middle matches. At most 4 iterations.
///   5. Apply sprites once. Set CurrentSymbol from physical middle slot.
///
/// Symbol indices:  0=Seven  1=Cherry  2=Bell  3=BAR
/// </summary>
public class ReelController : MonoBehaviour
{
    // ── Inspector ─────────────────────────────────────────────────────────────

    [Tooltip("Assign Symbol1..Symbol4 RectTransforms in top-to-bottom scene order.")]
    public RectTransform[] symbols;

    [Tooltip("Sprites 0=Seven, 1=Cherry, 2=Bell, 3=BAR.")]
    public Sprite[] symbolSprites;

    public float spinSpeed     = 700f;
    public float symbolSpacing = 65f;

    // ── Layout ────────────────────────────────────────────────────────────────

    private const float TOP_Y      =  70f;
    private const int   ROWS       =  4;
    private const int   MIDDLE_ROW =  1;   // payline: index 1 in sorted top→bottom array

    // ── State ─────────────────────────────────────────────────────────────────

    private bool spinning     = false;
    private int  targetSymbol = 0;

    // Tracks which symbol should appear next time a slot wraps to the top.
    // Starts at the symbol "above" slot 0 in the logical strip, i.e. ROWS-1,
    // and decrements (mod ROWS) each wrap.
    private int topSymbolCursor;

    public int CurrentSymbol { get; private set; } = 0;

    // ── Init ──────────────────────────────────────────────────────────────────

    void Awake()
    {
        // Attach ReelSlot to each symbol GameObject and assign initial indices.
        // symbols[] is provided top-to-bottom so slot i starts as logical symbol i.
        for (int i = 0; i < symbols.Length; i++)
        {
            ReelSlot slot = symbols[i].GetComponent<ReelSlot>();
            if (slot == null) slot = symbols[i].gameObject.AddComponent<ReelSlot>();
            slot.symbolIndex = i;
        }

        // The slot that will next wrap to the top should show the symbol
        // logically above symbol[0], which is ROWS-1 in a circular strip.
        topSymbolCursor = ROWS - 1;
    }

    // ── Update ────────────────────────────────────────────────────────────────

    void Update()
    {
        if (!spinning) return;

        float highestY = GetHighestY();

        foreach (RectTransform sym in symbols)
        {
            sym.anchoredPosition += Vector2.down * spinSpeed * Time.deltaTime;

            if (sym.anchoredPosition.y < -(symbolSpacing * 2f))
            {
                // Move this slot to the top of the strip.
                sym.anchoredPosition = new Vector2(sym.anchoredPosition.x,
                                                   highestY + symbolSpacing);

                // This slot now visually represents the next incoming symbol.
                // Update its ReelSlot so identity matches visual position.
                sym.GetComponent<ReelSlot>().symbolIndex = topSymbolCursor;

                // Advance cursor: next wrap will show the symbol above this one.
                topSymbolCursor = (topSymbolCursor - 1 + ROWS) % ROWS;
            }
        }
    }

    // ── Public API ────────────────────────────────────────────────────────────

    public void SetTargetSymbol(int index) => targetSymbol = index;
    public int  GetTargetSymbol()          => targetSymbol;
    public void StartSpin()                => spinning = true;

    public void StopSpin()
    {
        spinning = false;
        SnapToTarget();
    }

    // ── SnapToTarget ──────────────────────────────────────────────────────────

    void SnapToTarget()
    {
        // Step 1: sort by Y descending → symbols[0]=top … symbols[3]=bottom.
        System.Array.Sort(symbols, (a, b) =>
            b.anchoredPosition.y.CompareTo(a.anchoredPosition.y));

        // Step 2: snap to clean grid.
        SnapPositions();

        // Step 3: rotate visible strip until middle row shows targetSymbol.
        for (int attempt = 0; attempt < ROWS; attempt++)
        {
            if (symbols[MIDDLE_ROW].GetComponent<ReelSlot>().symbolIndex == targetSymbol)
                break;

            CycleStripDown();
        }

        // Step 4: apply sprites based on each slot's current symbolIndex.
        ApplySprites();

        // Step 5: CurrentSymbol comes from the physical middle slot — no assumption.
        CurrentSymbol = symbols[MIDDLE_ROW].GetComponent<ReelSlot>().symbolIndex;

        // Step 6: Recalculate topSymbolCursor from the actual top slot so the
        // next spin starts from the real visible strip state, not the old value.
        int topSymbol = symbols[0].GetComponent<ReelSlot>().symbolIndex;
        topSymbolCursor = (topSymbol - 1 + ROWS) % ROWS;

        Debug.Log($"[{gameObject.name}] Middle={CurrentSymbol}  target={targetSymbol}  nextCursor={topSymbolCursor}");
    }

    // ── CycleStripDown ────────────────────────────────────────────────────────

    /// <summary>
    /// Physically moves the TOP slot to the BOTTOM of the visible strip
    /// and assigns it the symbol that logically follows the current bottom symbol.
    ///
    /// Before:  [A][B][C][D]   (top→bottom, A is in middle candidate area)
    /// After:   [B][C][D][A']  (A moves to bottom, gets symbol after D)
    ///
    /// Because we physically move the RectTransform AND update its symbolIndex,
    /// what the player sees and what symbols[] reports are always identical.
    /// </summary>
    void CycleStripDown()
    {
        // The slot leaving the top
        RectTransform leaving = symbols[0];

        // What symbol comes after the current bottom in the logical strip?
        int bottomSymbol  = symbols[ROWS - 1].GetComponent<ReelSlot>().symbolIndex;
        int newSymbol     = (bottomSymbol + 1) % ROWS;

        // Shift the array: slots 1..3 move to positions 0..2
        for (int i = 0; i < ROWS - 1; i++)
            symbols[i] = symbols[i + 1];

        // Place leaving slot at position 3 (bottom) with its new symbol identity
        symbols[ROWS - 1] = leaving;
        leaving.GetComponent<ReelSlot>().symbolIndex = newSymbol;

        // Snap all positions to reflect the new array order
        SnapPositions();
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    void SnapPositions()
    {
        for (int i = 0; i < ROWS; i++)
            symbols[i].anchoredPosition = new Vector2(
                symbols[i].anchoredPosition.x,
                TOP_Y - i * symbolSpacing);
    }

    void ApplySprites()
    {
        if (symbolSprites == null) return;
        for (int i = 0; i < ROWS; i++)
        {
            int idx = symbols[i].GetComponent<ReelSlot>().symbolIndex;
            if (idx < symbolSprites.Length)
            {
                Image img = symbols[i].GetComponent<Image>();
                if (img != null) img.sprite = symbolSprites[idx];
            }
        }
    }

    float GetHighestY()
    {
        float highest = symbols[0].anchoredPosition.y;
        foreach (RectTransform sym in symbols)
            if (sym.anchoredPosition.y > highest)
                highest = sym.anchoredPosition.y;
        return highest;
    }
}

/// <summary>
/// Carries the logical symbol identity of its RectTransform.
/// This is the single source of truth — position and identity travel together.
/// </summary>
public class ReelSlot : MonoBehaviour
{
    public int symbolIndex;
}

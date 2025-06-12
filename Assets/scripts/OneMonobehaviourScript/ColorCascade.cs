using UnityEngine;

/// <summary>
/// Color Cascade - simpel puzzelspel waarin gekleurde blokken vallen en mengen.
/// Volledig geregeld met één MonoBehaviour script.
/// Martijn Laureijs 2025 HKU GDV
/// </summary>
public class ColorCascade : MonoBehaviour
{
    //aanmaak blokken
    private Texture2D blockTexture;

    // Speelveld afmetingen
    private const int width = 6;
    private const int height = 12;

    // Bitflag kleurdefinities
    private const int RED = 1;   // 001
    private const int GREEN = 2; // 010
    private const int BLUE = 4;  // 100
    private const int WHITE = 7; // 111 (R + G + B)

    // Speelveld (kleurwaarden per cel)
    private int[,] grid = new int[width, height];

    // Actief vallend blok
    private Vector2Int activeBlockPos;
    private int activeBlockColor;


    // Tijd voor automatisch vallen
    private float fallTimer = 0f;
    private float fallInterval = 1f; // begint langzaam

    void Start()
    {
        // Maak een witte 1x1 texture aan om blokken te tekenen
        blockTexture = new Texture2D(1, 1);
        blockTexture.SetPixel(0, 0, Color.white);
        blockTexture.Apply();

        SpawnNewBlock();
    }


    void Update()
    {
        HandleInput();
        HandleFalling();
    }

    bool IsGameOver()
    {
        // Spawnpositie bovenaan in het midden
        Vector2Int spawnPos = new Vector2Int(width / 2, height - 1);
        return !IsPositionValid(spawnPos);
    }


    /// <summary>
    /// Input voor het verplaatsen van het actieve blok.
    /// </summary>
    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int nextPos = activeBlockPos + Vector2Int.left;
            if (IsPositionValid(nextPos))
            {
                activeBlockPos = nextPos;
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector2Int nextPos = activeBlockPos + Vector2Int.right;
            if (IsPositionValid(nextPos))
            {
                activeBlockPos = nextPos;
            }
        }
    }

    /// <summary>
    /// Laat blok vallen, zet het vast als het iets raakt, en verwerkt menging.
    /// </summary>
    void HandleFalling()
    {
        fallTimer += Time.deltaTime;

        if (fallTimer >= fallInterval)
        {
            fallTimer = 0f;
            Vector2Int nextPos = activeBlockPos + Vector2Int.down;

            if (IsPositionValid(nextPos))
            {
                activeBlockPos = nextPos;
            }
            else
            {
                grid[activeBlockPos.x, activeBlockPos.y] = activeBlockColor;
                TryMergeAt(activeBlockPos);
                SpawnNewBlock();
            }
        }
    }

    /// <summary>
    /// Spawnt een nieuw blok in het midden bovenaan.
    /// </summary>
    void SpawnNewBlock()
    {
        Vector2Int spawnPos = new Vector2Int(width / 2, height - 1);

        if (!IsPositionValid(spawnPos))
        {
            Debug.Log("Game Over!");
            enabled = false;  // pauzeert dit script (geen update meer)
            return;
        }

        activeBlockColor = GetRandomPrimaryColor();
        activeBlockPos = spawnPos;
    }


    /// <summary>
    /// Genereert een willekeurige primaire kleur.
    /// </summary>
    int GetRandomPrimaryColor()
    {
        int[] colors = { RED, GREEN, BLUE };
        return colors[Random.Range(0, colors.Length)];
    }

    /// <summary>
    /// Controleert of een positie geldig is: binnen het speelveld én niet bezet.
    /// </summary>
    bool IsPositionValid(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= width || pos.y < 0 || pos.y >= height)
            return false;

        return grid[pos.x, pos.y] == 0;
    }

    /// <summary>
    /// Mengt het geplaatste blok met het blok eronder.
    /// </summary>
    void TryMergeAt(Vector2Int pos)
    {
        Vector2Int below = pos + Vector2Int.down;

        if (below.y >= 0 && grid[pos.x, below.y] != 0)
        {
            int merged = grid[pos.x, below.y] | grid[pos.x, pos.y];

            if (merged == WHITE)
            {
                grid[pos.x, below.y] = 0;
                grid[pos.x, pos.y] = 0;
            }
            else
            {
                grid[pos.x, below.y] = merged;
                grid[pos.x, pos.y] = 0;
            }
        }
    }

    /// <summary>
    /// Teken het speelveld en het actieve blok in het scherm.
    /// </summary>
    void OnGUI()
    {
        int blockSize = 30;
        Vector2 offset = new Vector2(10, 10);

        // Grid tekenen
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int color = grid[x, y];
                if (color != 0)
                {
                    DrawBlock(x, y, color, blockSize, offset);
                }
            }
        }

        // Game Over tekst weergeven als het spel is afgelopen
        if (!enabled)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 30;
            style.alignment = TextAnchor.MiddleCenter;
            style.normal.textColor = Color.white;

            Rect rect = new Rect(0, Screen.height / 2 - 15, Screen.width, 30);
            GUI.Label(rect, "GAME OVER", style);
        }

        // Actieve blok tekenen
        DrawBlock(activeBlockPos.x, activeBlockPos.y, activeBlockColor, blockSize, offset);
    }

    /// <summary>
    /// Teken een blok met een bepaalde kleur.
    /// </summary>
    void DrawBlock(int x, int y, int color, int size, Vector2 offset)
    {
        Color unityColor = GetUnityColorFromBitflag(color);
        GUI.color = unityColor;

        Rect rect = new Rect(offset.x + x * size, offset.y + (height - 1 - y) * size, size - 2, size - 2);

        GUI.DrawTexture(rect, blockTexture);

        GUI.color = Color.white; // reset kleur zodat volgende GUI elementen niet beïnvloed worden
    }


    /// <summary>
    /// Zet bitflag-kleur om naar een Unity-kleur.
    /// </summary>
    Color GetUnityColorFromBitflag(int color)
    {
        switch (color)
        {
            case RED: return Color.red;
            case GREEN: return Color.green;
            case BLUE: return Color.blue;
            case RED | GREEN: return Color.yellow;
            case RED | BLUE: return new Color(1f, 0f, 1f); // magenta
            case GREEN | BLUE: return Color.cyan;
            case WHITE: return Color.white;
            default: return Color.gray;
        }
    }

}

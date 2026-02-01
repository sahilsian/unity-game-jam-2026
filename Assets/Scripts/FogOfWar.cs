using UnityEngine;

public class FogOfWar : MonoBehaviour
{
    [SerializeField] private bool fogEnabled = true;
    [SerializeField] private float visionRadius = 5f;
    [SerializeField] private Color fogColor = new Color(0, 0, 0, 0.7f);
    [SerializeField] private KeyCode toggleKey = KeyCode.F;

    private Camera mainCamera;
    private Texture2D fogTexture;
    private Rect fogRect;

    private void Start()
    {
        mainCamera = Camera.main;
        CreateFogTexture();
    }

    private void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            fogEnabled = !fogEnabled;
            Debug.Log($"Fog of War: {(fogEnabled ? "ON" : "OFF")}");
        }

        if (fogEnabled)
        {
            UpdateFogOfWar();
        }
    }

    private void CreateFogTexture()
    {
        int width = (int)mainCamera.pixelWidth;
        int height = (int)mainCamera.pixelHeight;
        fogTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        fogRect = new Rect(0, 0, width, height);
    }

    private void UpdateFogOfWar()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null) return;

        Vector3 screenPos = mainCamera.WorldToScreenPoint(player.position);
        Color[] pixels = new Color[(int)fogRect.width * (int)fogRect.height];

        // Fill entire texture with fog
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = fogColor;
        }

        // Create circular reveal around player
        int centerX = (int)screenPos.x;
        int centerY = (int)fogRect.height - (int)screenPos.y; // Flip Y for texture coords
        int radiusPixels = (int)(visionRadius * mainCamera.orthographicSize / 5f);

        for (int y = 0; y < (int)fogRect.height; y++)
        {
            for (int x = 0; x < (int)fogRect.width; x++)
            {
                int distX = x - centerX;
                int distY = y - centerY;
                float distance = Mathf.Sqrt(distX * distX + distY * distY);

                if (distance < radiusPixels)
                {
                    // Fade out at edges for smooth transition
                    float alpha = 1f - (distance / radiusPixels);
                    pixels[y * (int)fogRect.width + x] = new Color(fogColor.r, fogColor.g, fogColor.b, Mathf.Lerp(fogColor.a, 0f, alpha));
                }
            }
        }

        fogTexture.SetPixels(pixels);
        fogTexture.Apply();
    }

    private void OnGUI()
    {
        if (fogEnabled && fogTexture != null)
        {
            GUI.DrawTexture(fogRect, fogTexture);
        }
    }

    public void SetVisionRadius(float radius)
    {
        visionRadius = radius;
    }

    public void SetFogColor(Color color)
    {
        fogColor = color;
    }
}


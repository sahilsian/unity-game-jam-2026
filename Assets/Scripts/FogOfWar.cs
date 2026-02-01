using UnityEngine;
using UnityEngine.UI;

public class FogOfWar : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool fogEnabled = true;
    [SerializeField] private Color fogColor = new Color(0, 0, 0, 0.7f);
    [SerializeField] private KeyCode toggleKey = KeyCode.F;
    
    [Header("UI References")]
    [SerializeField] private Image fogOverlay;
    
    private void Start()
    {
        // Create fog overlay if not assigned
        if (fogOverlay == null)
        {
            CreateFogOverlay();
        }
        
        UpdateFogVisibility();
    }
    
    private void Update()
    {
        // Toggle fog on/off
        if (Input.GetKeyDown(toggleKey))
        {
            fogEnabled = !fogEnabled;
            UpdateFogVisibility();
            Debug.Log($"Fog of War: {(fogEnabled ? "ON" : "OFF")}");
        }
    }
    
    private void CreateFogOverlay()
    {
        // Find or create canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObj = new GameObject("Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvasObj.AddComponent<UnityEngine.UI.GraphicRaycaster>();
        }
        
        // Create fog overlay image
        GameObject fogObj = new GameObject("FogOverlay");
        fogObj.transform.SetParent(canvas.transform, false);
        
        fogOverlay = fogObj.AddComponent<Image>();
        fogOverlay.color = fogColor;
        fogOverlay.raycastTarget = false; // Don't block clicks
        
        // Stretch to fill screen
        RectTransform rect = fogOverlay.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.sizeDelta = Vector2.zero;
        rect.anchoredPosition = Vector2.zero;
        
        // Move to top of canvas (renders last)
        fogObj.transform.SetAsLastSibling();
    }
    
    private void UpdateFogVisibility()
    {
        if (fogOverlay != null)
        {
            fogOverlay.enabled = fogEnabled;
        }
    }
    
    // Public methods to control fog
    public void SetFogEnabled(bool enabled)
    {
        fogEnabled = enabled;
        UpdateFogVisibility();
    }
    
    public void SetFogColor(Color color)
    {
        fogColor = color;
        if (fogOverlay != null)
        {
            fogOverlay.color = fogColor;
        }
    }
    
    public void SetFogAlpha(float alpha)
    {
        fogColor.a = alpha;
        if (fogOverlay != null)
        {
            fogOverlay.color = fogColor;
        }
    }
}
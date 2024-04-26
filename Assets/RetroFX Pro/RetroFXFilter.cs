using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Agava.WebUtility;
using Source.LevelLoader;

public enum RetroFXColoringMode {
    BitDepth, Palette, LuminosityBased, Steps
}

[ExecuteInEditMode]
[AddComponentMenu("Scripts/RetroFX Filter")]
public class RetroFXFilter : MonoBehaviour, IFader
{
    static float[][] BayerMatrices = new float[3][]
    {
        new float[64]
        {
            0.0000f, 0.6667f,
            1.0000f, 0.3333f,

            
            0.0000f, 0.0000f,
            0.0000f, 0.0000f,

            
            0.0000f, 0.0000f,
            0.0000f, 0.0000f,

            
            0.0000f, 0.0000f,
            0.0000f, 0.0000f,

            
            0.0000f, 0.0000f, 0.0000f, 0.0000f,
            0.0000f, 0.0000f, 0.0000f, 0.0000f,
            0.0000f, 0.0000f, 0.0000f, 0.0000f,
            0.0000f, 0.0000f, 0.0000f, 0.0000f,

            
            0.0000f, 0.0000f, 0.0000f, 0.0000f,
            0.0000f, 0.0000f, 0.0000f, 0.0000f,
            0.0000f, 0.0000f, 0.0000f, 0.0000f,
            0.0000f, 0.0000f, 0.0000f, 0.0000f,

            
            0.0000f, 0.0000f, 0.0000f, 0.0000f,
            0.0000f, 0.0000f, 0.0000f, 0.0000f,
            0.0000f, 0.0000f, 0.0000f, 0.0000f,
            0.0000f, 0.0000f, 0.0000f, 0.0000f
        },
        new float[64]
        {
            0.0000f, 0.5333f, 0.1333f, 0.6667f,
            0.8000f, 0.2667f, 0.9333f, 0.4000f,
            0.2000f, 0.7333f, 0.0667f, 0.6000f,
            1.0000f, 0.4667f, 0.8667f, 0.3333f,

            0.0000f, 0.0000f, 0.0000f, 0.0000f,
            0.0000f, 0.0000f, 0.0000f, 0.0000f,
            0.0000f, 0.0000f, 0.0000f, 0.0000f,
            0.0000f, 0.0000f, 0.0000f, 0.0000f,

            0.0000f, 0.0000f, 0.0000f, 0.0000f,
            0.0000f, 0.0000f, 0.0000f, 0.0000f,
            0.0000f, 0.0000f, 0.0000f, 0.0000f,
            0.0000f, 0.0000f, 0.0000f, 0.0000f,

            0.0000f, 0.0000f, 0.0000f, 0.0000f,
            0.0000f, 0.0000f, 0.0000f, 0.0000f,
            0.0000f, 0.0000f, 0.0000f, 0.0000f,
            0.0000f, 0.0000f, 0.0000f, 0.0000f
        },
        new float[64]
        {
            0.0000f, 0.5079f, 0.1270f, 0.6349f, 0.0317f, 0.5397f, 0.1587f, 0.6667f,
            0.7619f, 0.2540f, 0.8889f, 0.3810f, 0.7937f, 0.2857f, 0.9206f, 0.4127f,
            0.1905f, 0.6984f, 0.0635f, 0.5714f, 0.2222f, 0.7302f, 0.0952f, 0.6032f,
            0.9524f, 0.4444f, 0.8254f, 0.3175f, 0.9841f, 0.4762f, 0.8571f, 0.3492f,
            0.0476f, 0.5556f, 0.1746f, 0.6825f, 0.0159f, 0.5238f, 0.1429f, 0.6508f,
            0.8095f, 0.3016f, 0.9365f, 0.4286f, 0.7778f, 0.2698f, 0.9048f, 0.3968f,
            0.2381f, 0.7460f, 0.1111f, 0.6190f, 0.2063f, 0.7143f, 0.0794f, 0.5873f,
            1.0000f, 0.4921f, 0.8730f, 0.3651f, 0.9683f, 0.4603f, 0.8413f, 0.3333f
        }
    };

    static int[] BayerMatrixSideLength = new int[3]
    {
        2, 4, 8
    };

    // Display
    [SerializeField] float resolutionScale = 2;
    [SerializeField] float subtractiveFade = 0;

    // Dithering
    [SerializeField] bool enableDithering = false;
    [SerializeField] bool adjustDitheringByLuminosity = false;
    [SerializeField] float ditheringIntensity = 0;
    [SerializeField] int ditheringSize = 1;

    // Coloring
    [SerializeField] bool enableColoring = false;
    [SerializeField] Vector3Int colorBits = new Vector3Int(2, 2, 2);
    [SerializeField] Vector3Int colorSteps = new Vector3Int(4, 4, 4);
    [SerializeField] RetroFXColoringMode coloringMethod;
    [SerializeField] Color[] colorPalette = new Color[256]
    {
        Color.black, Color.white, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black,
        Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black, Color.black
    };

    [SerializeField] int colorPaletteLength = 2;

    public bool AdjustDitheringByLuminosity
    {
        get { return adjustDitheringByLuminosity; }
        set { adjustDitheringByLuminosity = value; }
    }

    public Vector3Int ColorBitDepth
    {
        get {
            return colorBits;
        }
        set
        {
            int r = Mathf.Clamp(value.x, 0, 8);
            int g = Mathf.Clamp(value.y, 0, 8);
            int b = Mathf.Clamp(value.z, 0, 8);

            colorBits = new Vector3Int(r, g, b);
        }
    }

    public RetroFXColoringMode ColoringMode
    {
        get
        { 
            return coloringMethod;
        }
        set
        { 
            coloringMethod = value;
        }
    }

    public Color[] ColorPalette
    {
        get
        {
            return colorPalette.Take(colorPaletteLength).ToArray();
        }
        set
        {
            if(value == null)
            {
                throw new System.Exception("Color palette cannot be null.");
            }

            Color[] colors = value;

            // Keep the array size under or equal to 64
            if(colors.Length > 256)
                colors = colors.Take(256).ToArray();

            // Store the length of the array
            colorPaletteLength = Mathf.Max(colors.Length, 2);

            // Populate the empty space with black
            colorPalette = PopulateArray(colors);

            blitMaterial.SetColorArray("_Palette", colorPalette);
            blitMaterial.SetInt("_PaletteLength", colorPaletteLength);
        }
    }

    public Vector3Int ColorSteps
    {
        get
        {
            return colorSteps;
        }
        set
        {
            int r = Mathf.Clamp(value.x, 0, 255);
            int g = Mathf.Clamp(value.y, 0, 255);
            int b = Mathf.Clamp(value.z, 0, 255);

            colorSteps = new Vector3Int(r, g, b);
        }
    }
    
    public bool EnableColoring
    {
        get
        {
            return enableColoring;
        }
        set
        { 
            enableColoring = value;
        }
    }
    
    public bool EnableDithering
    {
        get
        {
            return enableDithering;
        }
        set
        {
            enableDithering = value;
        }
    }

    public float DitheringIntensity
    {
        get
        {
            return ditheringIntensity;
        }
        set
        { 
            ditheringIntensity = Mathf.Clamp01(value);
        }
    }

    public int DitheringSize
    {
        get
        {
            return ditheringSize;
        }
        set
        {
            ditheringSize = Mathf.Clamp(value, 0, 2);
        }
    }
    
    public float Fade
    {
        get
        {
            return subtractiveFade;
        }
        set 
        {
            subtractiveFade = Mathf.Clamp01(value);
        }
    }

    public float Resolution
    {
        get 
        { 
            return resolutionScale;
        }
        set
        { 
            resolutionScale = Mathf.Clamp(value, 1, int.MaxValue);
        }
    }
    
    Color[] PopulateArray(Color[] colors)
    {
        Color[] newTable = new Color[256];

        for(int i = 0; i < 256; i++)
            newTable[i] = i < colors.Length ? colors[i] : Color.black;

        return newTable;
    }

    Material blitMaterial;

    private void Start()
    {
        resolutionScale = ((float)Screen.width / 1000f);
        blitMaterial = new Material(Shader.Find("Hidden/RetroFX Pro Filter"));
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        blitMaterial.SetFloat("_ResolutionScale", resolutionScale);
        blitMaterial.SetInt("_LuminosityAdjustment", adjustDitheringByLuminosity ? 1 : 0);
        blitMaterial.SetFloat("_DitherStrength", enableDithering ? ditheringIntensity : 0);
        blitMaterial.SetInt("_BayerLength", BayerMatrixSideLength[ditheringSize]);
        blitMaterial.SetFloatArray("_BayerMatrix", BayerMatrices[ditheringSize]);
        blitMaterial.SetFloat("_Fade", subtractiveFade);
        blitMaterial.SetColorArray("_Palette", colorPalette);
        blitMaterial.SetInt("_PaletteLength", colorPaletteLength);

        if(coloringMethod == RetroFXColoringMode.BitDepth)
            blitMaterial.SetVector("_BitDepth", new Vector4(colorBits.x, colorBits.y, colorBits.z, 0));
        else if(coloringMethod == RetroFXColoringMode.Steps)
            blitMaterial.SetVector("_BitDepth", new Vector4(colorSteps.x, colorSteps.y, colorSteps.z, 0));

        // Set coloring method
        if(enableColoring)
        {
            switch(coloringMethod)
            {
                case RetroFXColoringMode.BitDepth:
                    blitMaterial.SetFloat("_ColoringMode", 1);
                    break;
                case RetroFXColoringMode.Palette:
                    blitMaterial.SetFloat("_ColoringMode", 2);
                    break;
                case RetroFXColoringMode.LuminosityBased:
                    blitMaterial.SetFloat("_ColoringMode", 3);
                    break;
                case RetroFXColoringMode.Steps:
                    blitMaterial.SetFloat("_ColoringMode", 4);
                    break;
            }
        }
        else
        {
            blitMaterial.SetFloat("_ColoringMode", 0);
        }

        Graphics.Blit(source, destination, blitMaterial);
    }

    public void SetFade(float fade) => Fade = fade;
}

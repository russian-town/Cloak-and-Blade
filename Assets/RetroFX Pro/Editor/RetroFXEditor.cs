using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Globalization;
using System;
using System.IO;

[CustomEditor(typeof(RetroFXFilter))]
[CanEditMultipleObjects]
public class RetroFXEditor : Editor
    {
    #region "Preset Data"
    struct Preset {
        public string Name;
        public int ResolutionScale;
        public bool Dithering;
        public bool AdjustByLuminosity;
        public float DitheringIntensity;
        public int DitheringSize;
        public RetroFXColoringMode ColoringMode;
        public Vector3Int BitDepth;
        public Vector3Int Steps;
        public Color[] ColorPalette;
        public string PalettePath;
    }

    Preset[] presets = new Preset[] {
        // 16-bit
        new Preset() {
            Name = "16-bit",
            ResolutionScale = 1,
            Dithering = false,
            AdjustByLuminosity = false,
            DitheringIntensity = 0,
            DitheringSize = 1,
            ColoringMode = RetroFXColoringMode.BitDepth,
            BitDepth = new Vector3Int(5, 6, 5),
            Steps = new Vector3Int(3, 3, 3),
            ColorPalette = new Color[] {
                Color.black,
                Color.white
            }
        },
        // 8-bit
        new Preset() {
            Name = "8-bit",
            ResolutionScale = 2,
            Dithering = true,
            AdjustByLuminosity = false,
            DitheringIntensity = 0.1f,
            DitheringSize = 1,
            ColoringMode = RetroFXColoringMode.BitDepth,
            BitDepth = new Vector3Int(3, 2, 3),
            Steps = new Vector3Int(3, 3, 3),
            ColorPalette = new Color[] {
                Color.black,
                Color.white
            }
        },
        // Apple Macintosh 16-color
        new Preset() {
            Name = "Apple Macintosh 16-color",
            ResolutionScale = 2,
            Dithering = false,
            AdjustByLuminosity = false,
            DitheringIntensity = 0,
            DitheringSize = 1,
            ColoringMode = RetroFXColoringMode.Palette,
            BitDepth = new Vector3Int(5, 6, 5),
            ColorPalette = new Color[] {
                Color.white,
                new Color32(251, 243, 5, 255),
                new Color32(255, 100, 3, 255),
                new Color32(221, 9, 7, 255),
                new Color32(242, 8, 132, 255),
                new Color32(71, 0, 165, 255),
                new Color32(0, 0, 211, 255),
                new Color32(2, 171, 234, 255),
                new Color32(31, 183, 20, 255),
                new Color32(0, 100, 18, 255),
                new Color32(86, 44, 5, 255),
                new Color32(144, 113, 58, 255),
                new Color32(192, 192, 192, 255),
                new Color32(128, 128, 128, 255),
                new Color32(64, 64, 64, 255),
                Color.black
            }
        },
        // Apple Macintosh Default
        new Preset() {
            Name = "Apple Macintosh Default",
            ResolutionScale = 3,
            Dithering = false,
            AdjustByLuminosity = false,
            DitheringIntensity = 0,
            DitheringSize = 1,
            ColoringMode = RetroFXColoringMode.Steps,
            BitDepth = new Vector3Int(5, 6, 5),
            Steps = new Vector3Int(6, 6, 6),
            ColorPalette = new Color[] {
                Color.white,
                Color.black
            }
        },
        // Commodore 64
        new Preset() {
            Name = "Commodore 64",
            ResolutionScale = 4,
            Dithering = false,
            AdjustByLuminosity = false,
            DitheringIntensity = 0,
            DitheringSize = 1,
            ColoringMode = RetroFXColoringMode.Palette,
            BitDepth = new Vector3Int(5, 6, 5),
            Steps = new Vector3Int(3, 3, 3),
            ColorPalette = new Color[] {
                Color.black,
                Color.white,
                new Color32(161, 77, 67, 255),
                new Color32(106, 193, 200, 255),
                new Color32(162, 87, 165, 255),
                new Color32(92, 173, 95, 255),
                new Color32(79, 68, 156, 255),
                new Color32(203, 214, 137, 255),
                new Color32(163, 104, 58, 255),
                new Color32(110, 83, 11, 255),
                new Color32(204, 127, 118, 255),
                new Color32(99, 99, 99, 255),
                new Color32(139, 139, 139, 255),
                new Color32(155, 227, 157, 255),
                new Color32(138, 127, 205, 255),
                new Color32(175, 175, 175, 255)
            }
        },
        // Duke Nukem 3D
        new Preset() {
            Name = "Duke Nukem 3D",
            ResolutionScale = 1,
            Dithering = false,
            AdjustByLuminosity = false,
            DitheringIntensity = 0,
            DitheringSize = 1,
            ColoringMode = RetroFXColoringMode.Palette,
            BitDepth = new Vector3Int(5, 6, 5),
            Steps = new Vector3Int(3, 3, 3),
            ColorPalette = null,
            PalettePath = "PresetPalettes/dn3d"
        },
        // Doom
        new Preset() {
            Name = "Doom",
            ResolutionScale = 1,
            Dithering = false,
            AdjustByLuminosity = false,
            DitheringIntensity = 0,
            DitheringSize = 1,
            ColoringMode = RetroFXColoringMode.Palette,
            BitDepth = new Vector3Int(5, 6, 5),
            Steps = new Vector3Int(3, 3, 3),
            ColorPalette = null,
            PalettePath = "PresetPalettes/doom"
        },
        // Miami Sunsets
        new Preset() {
            Name = "Miami Sunsets",
            ResolutionScale = 1,
            Dithering = true,
            AdjustByLuminosity = false,
            DitheringIntensity = 1f,
            DitheringSize = 2,
            ColoringMode = RetroFXColoringMode.LuminosityBased,
            BitDepth = new Vector3Int(5, 6, 5),
            Steps = new Vector3Int(3, 3, 3),
            ColorPalette = new Color[] {
                new Color32(95, 0, 125, 255),
                new Color32(123, 21, 125, 255),
                new Color32(204, 96, 0, 255),
                new Color32(204, 96, 0, 255),
                new Color32(255, 253, 0, 255),
                Color.white
            }
        },
        // Microsoft Windows 9x Low Color
        new Preset() {
            Name = "Microsoft Windows 9x Low Color",
            ResolutionScale = 1,
            Dithering = true,
            AdjustByLuminosity = false,
            DitheringIntensity = 0.1f,
            DitheringSize = 1,
            ColoringMode = RetroFXColoringMode.Palette,
            BitDepth = new Vector3Int(5, 6, 5),
            Steps = new Vector3Int(3, 3, 3),
            ColorPalette = new Color[] {
                Color.black,
                new Color32(128, 0, 0, 255),
                new Color32(0, 128, 0, 255),
                new Color32(128, 128, 0, 255),
                new Color32(0, 0, 128, 255),
                new Color32(128, 0, 128, 255),
                new Color32(0, 128, 128, 255),
                new Color32(192, 192, 192, 255),
                new Color32(192, 220, 192, 255),
                new Color32(166, 202, 240, 255),
                new Color32(255, 251, 240, 255),
                new Color32(160, 160, 164, 255),
                new Color32(128, 128, 128, 255),
                new Color32(255, 0, 0, 255),
                new Color32(0, 255, 0, 255),
                new Color32(255, 255, 0, 255),
                new Color32(0, 0, 255, 255),
                new Color32(255, 0, 255, 255),
                new Color32(0, 255, 255, 255),
                Color.white
            }
        },
        // Monochrome
        new Preset() {
            Name = "Monochrome",
            ResolutionScale = 1,
            Dithering = true,
            AdjustByLuminosity = true,
            DitheringIntensity = 0.85f,
            DitheringSize = 2,
            ColoringMode = RetroFXColoringMode.Palette,
            BitDepth = new Vector3Int(0, 0, 0),
            Steps = new Vector3Int(1, 1, 1),
            ColorPalette = new Color[] {
                Color.black,
                Color.white
            }
        },
        // Nintendo Entertainment System
        new Preset() {
            Name = "Nintendo Entertainment System (NES)",
            ResolutionScale = 3,
            Dithering = false,
            AdjustByLuminosity = false,
            DitheringIntensity = 0,
            DitheringSize = 1,
            ColoringMode = RetroFXColoringMode.Palette,
            BitDepth = new Vector3Int(3, 2, 3),
            Steps = new Vector3Int(3, 3, 3),
            ColorPalette = new Color[] {
                Color.black,
                Color.white,
                new Color32(124, 124, 124, 255),
                new Color32(188, 188, 188, 255),
                new Color32(248, 248, 248, 255),
                new Color32(252, 252, 252, 255),
                new Color32(0, 0, 252, 255),
                new Color32(0, 120, 248, 255),
                new Color32(60, 188, 252, 255),
                new Color32(164, 228, 252, 255),
                new Color32(0, 0, 188, 255),
                new Color32(0, 88, 248, 255),
                new Color32(104, 136, 252, 255),
                new Color32(184, 184, 248, 255),
                new Color32(68, 40, 188, 255),
                new Color32(104, 68, 252, 255),
                new Color32(152, 120, 248, 255),
                new Color32(216, 184, 248, 255),
                new Color32(148, 0, 132, 255),
                new Color32(216, 0, 204, 255),
                new Color32(248, 120, 248, 255),
                new Color32(248, 184, 248, 255),
                new Color32(168, 0, 32, 255),
                new Color32(228, 0, 88, 255),
                new Color32(248, 88, 152, 255),
                new Color32(248, 164, 192, 255),
                new Color32(168, 16, 0, 255),
                new Color32(248, 56, 0, 255),
                new Color32(248, 120, 88, 255),
                new Color32(240, 208, 176, 255),
                new Color32(136, 20, 0, 255),
                new Color32(228, 92, 16, 255),
                new Color32(252, 160, 68, 255),
                new Color32(252, 224, 168, 255),
                new Color32(80, 48, 0, 255),
                new Color32(172, 124, 0, 255),
                new Color32(248, 184, 0, 255),
                new Color32(248, 216, 120, 255),
                new Color32(0, 120, 0, 255),
                new Color32(0, 184, 0, 255),
                new Color32(184, 248, 24, 255),
                new Color32(216, 248, 120, 255),
                new Color32(0, 104, 0, 255),
                new Color32(0, 168, 0, 255),
                new Color32(88, 216, 84, 255),
                new Color32(184, 248, 184, 255),
                new Color32(0, 88, 0, 255),
                new Color32(0, 168, 68, 255),
                new Color32(88, 248, 152, 255),
                new Color32(184, 248, 216, 255),
                new Color32(0, 64, 88, 255),
                new Color32(0, 136, 136, 255),
                new Color32(0, 232, 216, 255),
                new Color32(0, 252, 252, 255),
                new Color32(120, 120, 120, 255),
                new Color32(216, 216, 216, 255)
            }
        },
        // Nintendo Game Boy
        new Preset() {
            Name = "Nintendo Game Boy",
            ResolutionScale = 4,
            Dithering = true,
            AdjustByLuminosity = true,
            DitheringIntensity = 0.2f,
            DitheringSize = 0,
            ColoringMode = RetroFXColoringMode.LuminosityBased,
            BitDepth = new Vector3Int(3, 2, 3),
            Steps = new Vector3Int(0, 4, 0),
            ColorPalette = new Color[] {
                new Color32(15, 56, 15, 255),
                new Color32(48, 98, 48, 255),
                new Color32(139, 172, 15, 255),
                new Color32(155, 188, 15, 255)
            }
        },
        // Quake
        new Preset() {
            Name = "Quake",
            ResolutionScale = 1,
            Dithering = false,
            AdjustByLuminosity = false,
            DitheringIntensity = 0,
            DitheringSize = 1,
            ColoringMode = RetroFXColoringMode.Palette,
            BitDepth = new Vector3Int(5, 6, 5),
            Steps = new Vector3Int(3, 3, 3),
            ColorPalette = null,
            PalettePath = "PresetPalettes/quake"
        },
        // Vapor
        new Preset() {
            Name = "Vapor",
            ResolutionScale = 1,
            Dithering = true,
            AdjustByLuminosity = true,
            DitheringIntensity = 1,
            DitheringSize = 2,
            ColoringMode = RetroFXColoringMode.LuminosityBased,
            BitDepth = new Vector3Int(3, 2, 3),
            Steps = new Vector3Int(4, 0, 4),
            ColorPalette = new Color[] {
                new Color32(108, 0, 101, 255),
                new Color32(166, 29, 144, 255),
                new Color32(60, 59, 212, 255),
                new Color32(52, 167, 233, 255),
                new Color32(37, 251, 255, 255)
            }
        },
        // VGA/CGA
        new Preset() {
            Name = "VGA 16-color",
            ResolutionScale = 2,
            Dithering = true,
            AdjustByLuminosity = true,
            DitheringIntensity = 0.1f,
            DitheringSize = 1,
            ColoringMode = RetroFXColoringMode.Palette,
            BitDepth = new Vector3Int(3, 2, 3),
            Steps = new Vector3Int(1, 1, 1),
            ColorPalette = new Color[] {
                new Color32(0, 0, 0, 255),
                new Color32(0, 0, 170, 255),
                new Color32(0, 170, 0, 255),
                new Color32(0, 170, 170, 255),
                new Color32(170, 0, 0, 255),
                new Color32(170, 0, 170, 255),
                new Color32(170, 85, 0, 255),
                new Color32(170, 170, 170, 255),
                new Color32(85, 85, 85, 255),
                new Color32(85, 85, 255, 255),
                new Color32(85, 255, 85, 255),
                new Color32(85, 255, 255, 255),
                new Color32(255, 85, 85, 255),
                new Color32(255, 85, 255, 255),
                new Color32(255, 255, 85, 255),
                new Color32(255, 255, 255, 255)
            }
        },
        // Wolfenstein
        new Preset() {
            Name = "Wolfenstein 3D",
            ResolutionScale = 3,
            Dithering = false,
            AdjustByLuminosity = false,
            DitheringIntensity = 0,
            DitheringSize = 1,
            ColoringMode = RetroFXColoringMode.Palette,
            BitDepth = new Vector3Int(5, 6, 5),
            Steps = new Vector3Int(3, 3, 3),
            ColorPalette = null,
            PalettePath = "PresetPalettes/ws3d"
        }
    };

    #endregion

    string[] dropDownColoringMethods = new string[4]
    {
        "Bit Depth", "Color Palette", "Luminosity Based", "Steps"
    };

    public override void OnInspectorGUI()
    {
        RetroFXFilter t = target as RetroFXFilter;

        // Show presets
        if(GUILayout.Button("Presets", GUILayout.Width(300)))
            CreateMenuItems();

        // ==============================
        // Resolution
        // ==============================
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Display", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        t.Resolution = EditorGUILayout.Slider("Resolution scale", t.Resolution, 1, 100); // Set scale
        t.Fade = EditorGUILayout.Slider("Subtractive fade", t.Fade, 0, 1); // Subtractive fade
        EditorGUILayout.EndVertical();

        
        // ==============================
        // Dithering
        // ==============================
        EditorGUILayout.Space();

        // Label
        t.EnableDithering = EditorGUILayout.BeginToggleGroup("Dithering", t.EnableDithering);
        
        // Container
        EditorGUILayout.BeginVertical("box");
        t.AdjustDitheringByLuminosity = EditorGUILayout.ToggleLeft("Adjust intensity with luminescence", t.AdjustDitheringByLuminosity);
        t.DitheringIntensity = EditorGUILayout.Slider("Dithering intensity", t.DitheringIntensity*100, 0, 100) / 100f;
        t.DitheringSize = EditorGUILayout.Popup("Dithering size", t.DitheringSize, new string[]{ "2x2 Bayer", "4x4 Bayer", "8x8 Bayer" });
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndToggleGroup();

        // ==============================
        // Colors
        // ==============================
        EditorGUILayout.Space();
        t.EnableColoring = EditorGUILayout.BeginToggleGroup("Colors", t.EnableColoring);

        int coloringMethod = EditorGUILayout.Popup("Coloring method", (int)t.ColoringMode, dropDownColoringMethods);

        t.ColoringMode = (RetroFXColoringMode)coloringMethod;

        EditorGUILayout.BeginVertical("box");

        // Determine which layout we should draw
        switch(t.ColoringMode)
        {
            case RetroFXColoringMode.BitDepth:
                
                Vector3Int bits = Vector3Int.zero;
        
                bits.x = EditorGUILayout.IntSlider("Red bits", t.ColorBitDepth.x, 0, 8);
                bits.y = EditorGUILayout.IntSlider("Green bits", t.ColorBitDepth.y, 0, 8);
                bits.z = EditorGUILayout.IntSlider("Blue bits", t.ColorBitDepth.z, 0, 8);

                t.ColorBitDepth = bits;

                int possibleRedStates = (int)Mathf.Pow(2, bits.x);
                int possibleGreenStates = (int)Mathf.Pow(2, bits.y);
                int possibleBlueStates = (int)Mathf.Pow(2, bits.z);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField(string.Format("{0}-bit, {1} color(s)",
                    bits.x + bits.y + bits.z,
                    (possibleRedStates * possibleGreenStates * possibleBlueStates).ToString("n0")),
                    new GUIStyle() {
                        alignment = TextAnchor.MiddleRight
                        });

                break;
            case RetroFXColoringMode.Steps:
                Vector3Int steps = Vector3Int.zero;
        
                steps.x = EditorGUILayout.IntSlider("Red steps", t.ColorSteps.x, 0, 64);
                steps.y = EditorGUILayout.IntSlider("Green steps", t.ColorSteps.y, 0, 64);
                steps.z = EditorGUILayout.IntSlider("Blue steps", t.ColorSteps.z, 0, 64);

                t.ColorSteps = steps;

                int possibleRedSteps = Mathf.Max(1, steps.x);
                int possibleGreenSteps = Mathf.Max(1, steps.y);
                int possibleBlueSteps = Mathf.Max(1, steps.z);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField(string.Format("{0} color(s)",
                    (possibleRedSteps * possibleGreenSteps * possibleBlueSteps).ToString("n0")),
                    new GUIStyle() {
                        alignment = TextAnchor.MiddleRight
                        });

                break;
            case RetroFXColoringMode.LuminosityBased:
            case RetroFXColoringMode.Palette:
                // Draw optional load from image button
                if(GUILayout.Button("Create palette from file..."))
                    HandleOpenFileDialog(t);

                // Draw controls
                GUILayout.BeginHorizontal();
            
                // If this button is clicked: increase the size of the palette
                if(GUILayout.Button("Add") && t.ColorPalette.Length < 256) {
                    Color[] p = new Color[t.ColorPalette.Length + 1];

                    Array.Copy(t.ColorPalette, 0, p, 0, t.ColorPalette.Length);

                    p[p.Length-1] = Color.gray;

                    t.ColorPalette = p;
                }

                if(GUILayout.Button("Remove") && t.ColorPalette.Length > 2) {
                    Color[] p = new Color[t.ColorPalette.Length - 1];

                    Array.Copy(t.ColorPalette, 0, p, 0, t.ColorPalette.Length - 1);

                    t.ColorPalette = p;
                }
            
                GUILayout.Label(t.ColorPalette.Length.ToString() + " colors");
                GUILayout.EndHorizontal();

                // Colors
                EditorGUILayout.BeginVertical(GUILayout.MaxWidth(400));
            
                Color[] palette = t.ColorPalette;

                for(int i = 0; i < palette.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    
                    // Removal button
                    if(GUILayout.Button("X", GUILayout.Width(20)) && t.ColorPalette.Length > 2)
                    {
                        // Remove the color at the index
                        Color[] reducedPalette = new Color[t.ColorPalette.Length - 1];

                        for(int idx = 0; idx < reducedPalette.Length; idx++)
                            reducedPalette[idx] = t.ColorPalette[idx >= i ? idx + 1 : idx];

                        palette = reducedPalette;
                    }
                    else
                    {
                        // Color picker
                        palette[i] = EditorGUILayout.ColorField(GUIContent.none, palette[i], true, false, false);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();

                // Import and export buttons
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if(GUILayout.Button("Import ..."))
                {
                    string file = EditorUtility.OpenFilePanelWithFilters("Import RetroFX palette", string.Empty, new string[] { "RetroFX Palette File", "rfx"});

                    if(File.Exists(file))
                    {
                        if(EditorUtility.DisplayDialog("Overwrite current palette?", "Importing another palette will overwrite the current one. Are you sure you want to overwrite it?", "Yes", "Cancel"))
                            ImportPalette(file, ref palette);
                    }
                }
                
                if(GUILayout.Button("Export ..."))
                {
                    string file = EditorUtility.SaveFilePanel("Export RetroFX palette", string.Empty, UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "-palette", "rfx");

                    if(!string.IsNullOrEmpty(file))
                        ExportPalette(file, palette);
                }
                EditorGUILayout.EndHorizontal();

                // Return colors to the script
                t.ColorPalette = palette;

                break;
        }
        
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndToggleGroup();
    }

    void ImportPalette(string file, ref Color[] palette)
    {
        List<Color> importedPalette = new List<Color>();

        using(StreamReader sr = new StreamReader(new FileStream(file, FileMode.Open, FileAccess.Read)))
        {
            while(sr.EndOfStream == false)
            {
                string content = sr.ReadLine();

                if(content.StartsWith("//"))
                    continue;

                string[] splits = content.Split(new char[] { ',' });

                if(splits.Length != 3)
                    throw new Exception("Error when importing file. This file is not a valid RetroFX palette file");

                float r, g, b;

                float.TryParse(splits[0], out r);
                float.TryParse(splits[1], out g);
                float.TryParse(splits[2], out b);

                importedPalette.Add(new Color(r, g, b));
            }
        }

        palette = importedPalette.ToArray();
    }

    void ExportPalette(string file, Color[] palette)
    {
        using(StreamWriter sw = new StreamWriter(new FileStream(file, FileMode.Create, FileAccess.Write)))
        {
            sw.WriteLine("// RetroFX Palette File");
            foreach(Color c in palette)
            {
                sw.WriteLine($"{c.r},{c.g},{c.b}");
            }
        }
    }

    void LoadPaletteFromTexture(RetroFXFilter target, Texture2D texture, bool suppressWarning = false) {
        // Get all pixels in the texture
        Color[] allPixels = texture.GetPixels(0, 0, texture.width, texture.height);

        // Create the palette
        Dictionary<Color, int> dictPalette = new Dictionary<Color, int>();

        foreach(Color c in allPixels) {
            if(dictPalette.ContainsKey(c))
                dictPalette[c]++;
            else
                dictPalette.Add(c, 1);
        }

        List<KeyValuePair<Color, int>> orderedPalette = dictPalette.ToList().OrderBy(x => -x.Value).ToList();

        if(orderedPalette.Count > 256 && !suppressWarning) {
            orderedPalette = orderedPalette.Take(256).ToList();
            EditorUtility.DisplayDialog("Warning", "Image contained more then 256 colors. RetroFX will only pick the most common colors.", "OK");
        }

        // Set the palette
        Color[] palette = new Color[orderedPalette.Count];

        for(int i = 0; i < orderedPalette.Count; i++)
            palette[i] = orderedPalette[i].Key;

        target.ColorPalette = palette;
    }

    void HandleOpenFileDialog(RetroFXFilter target) {
        string file = EditorUtility.OpenFilePanelWithFilters("Open Image", string.Empty, new string[] { "Image files", "png,jpg,jpeg", "All files", "*" });

        // Dialog was cancelled
        if(file == string.Empty)
            return;

        Texture2D temporaryTexture = new Texture2D(128, 128);
        byte[] data = new byte[0];

        // Parse the data into a readable texture
        try {
            data = System.IO.File.ReadAllBytes(file);
            temporaryTexture.LoadImage(data);
        } catch(Exception ex) {
            EditorUtility.DisplayDialog("Error making palette", string.Format("Could not create palette from file: {0}", ex.Message), "OK");
            return;
        }

        LoadPaletteFromTexture(target, temporaryTexture);

        // Unload texture
        DestroyImmediate(temporaryTexture);
    }

    void CreateMenuItems() {
        GenericMenu menu = new GenericMenu();

        foreach(Preset p in presets)
            menu.AddItem(new GUIContent() { text = p.Name }, false, LoadPreset, p);

        menu.ShowAsContext();
    }

    void LoadPreset(object preset)
    {
        if(preset.GetType() != typeof(Preset))
            return;

        RetroFXFilter t = target as RetroFXFilter;
        Preset p = (Preset)preset;

        t.AdjustDitheringByLuminosity = p.AdjustByLuminosity;
        t.ColorBitDepth = p.BitDepth;
        t.ColoringMode = p.ColoringMode;
        t.ColorPalette = p.ColorPalette ?? new Color[2] { Color.black, Color.white };
        t.ColorSteps = p.Steps;
        t.DitheringIntensity = p.DitheringIntensity;
        t.DitheringSize = p.DitheringSize;
        t.EnableDithering = p.Dithering;
        t.Resolution = p.ResolutionScale;

        t.Fade = 0;
        t.EnableColoring = true;

        if(p.ColorPalette == null)
            LoadPaletteFromTexture(t, Resources.Load<Texture2D>(p.PalettePath), true);
    }
}

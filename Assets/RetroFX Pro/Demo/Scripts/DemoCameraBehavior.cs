using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemoCameraBehavior : MonoBehaviour {
    #region "Preset Data"
    struct Preset {
        public string Name;
        public int ResolutionScale;
        public bool Dithering;
        public bool AdjustByLuminosity;
        public bool EnableColoring;
        public float DitheringIntensity;
        public int DitheringSize;
        public RetroFXColoringMode ColoringMode;
        public Vector3Int BitDepth;
        public Vector3Int Steps;
        public Color[] ColorPalette;
        public string ImageBase64;
    }

    Preset[] presets = new Preset[] {
        // No effect
        new Preset() {
            Name = "No effect applied",
            ResolutionScale = 1,
            Dithering = false,
            AdjustByLuminosity = false,
            DitheringIntensity = 0,
            ColoringMode = RetroFXColoringMode.BitDepth,
            BitDepth = new Vector3Int(8, 8, 8),
            Steps = new Vector3Int(64, 64, 64),
            ColorPalette = new Color[] {
                Color.black,
                Color.white
            },
            EnableColoring = false
        },
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
            },
            EnableColoring = true
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
            },
            EnableColoring = true
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
            },
            EnableColoring = true
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
            },
            EnableColoring = true
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
            },
            EnableColoring = true
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
            ImageBase64 = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAIAAACQkWg2AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAFwSURBVDhPVVK7TsQwEBxLK2WlrHSWiEQKPoOCkiIlBcUVlBT8C8UVFBR8QEo+iJYPuJLipFAws/aBcDb27HjfSQFgZuHt4Yo4H/3MbRQeYozydngRdjeJnr8zSWe8xFrhLQMcTALeSdyiAzHCCiAZ3cphf/MvHGVQpEZ5DHJXDCnKcP/42hz0Dt2MxyCL801201ZvWhXlrUoQn0WlSiUxGaXuDiTl0Yxb/rN6rk424+jl8Lz3AEN0YbIEnEMOoc+EuO3KEBRD1W6VwFI12wn8Mg1YeVpmiyNnOgayKsSAUE5+LgGSph1Rhft3UBt8VadQFiUtycaKJihP+zse7DKnn2tQoyaiM2xexiGzcn2FGphccuGoCVgrd4vJY7bdJFBnj0uf5vJxO2dHcrfKyeWA6dfKb4X7DvpPKvMUr9c45SViAi19YnjzCT76d+Xd6SvqMexY7TPsG8uCdcW2YcGyYt2wPQDvwMZpLMD6D23YfgBamDT1JF8/1wAAAABJRU5ErkJggg==",
            EnableColoring = true
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
            ImageBase64 = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAIAAACQkWg2AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAF8SURBVDhPjVI9SwNBEJ3BhVvIEJew6OEdyZE7MJBAUggKKVKkFSzS2FrH0s7CH5DaX2BhaWORRrAMmD9gayVqoQdC0EJndmNILo1zb9+9mb2Z2Y/b6DWjRnWrVd95/no1NghjqtbD97eZ1rpcLlcqlTzPp9Np/vJkjf74/MarblcpxdMymJc0myIS5kHkGfsACkAzB4Ewa07itAJLnhg+XBx5t2CgDShmEiyEJmymm9JZKZLOTnjtCvuILMq5HMfRaMT1Fk28KLjLAtM0/f/XLLC0HWjuapQ2Ws9ZhGLmE3Ku6D/goNd2BVyNlQ4+4p4lw/3erp+TAyd37O49uR2zZp9h2BdjYfCnVoMkEcTxXDCy7KyFNgxsSDZhGJtYQRbLxdEazFrEg+MIMAAI1xBHkbT0Xb3wGm/6/C8A8VXqYH53rBUvXYFJwCYrbDNsRyW3S9nowfMjkU+G43sASAGyVTTwejicXEIHOh3YY4ZzgNNDGM/w5A6gVNwF0C/26EE4ZimhrgAAAABJRU5ErkJggg==",
            EnableColoring = true
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
            },
            EnableColoring = true
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
            },
            EnableColoring = true
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
            },
            EnableColoring = true
        },
        // Nintendo Entertainment System
        new Preset() {
            Name = "Nintendo Entertainment System (NES)",
            ResolutionScale = 3,
            Dithering = true,
            AdjustByLuminosity = false,
            DitheringIntensity = 0.2f,
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
            },
            EnableColoring = true
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
            },
            EnableColoring = true
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
            ImageBase64 = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAIAAACQkWg2AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAACxEAAAsRAX9kX5EAAAGVSURBVDhPlZHBitRAEIb/zhZJkTSTwulDWAcJKDLgnsTDguDJR/DoIwg+wIIXb3oRvHgUPCj4Sj7AHubQsHOIsey10okzC+rBysfXf6VJukIcgLZt5Xd575f0j3JtUzERM7IzBJ+dW7NtLdkzXNO0zN6wmkNG/rgzI9NIVQWa3nGgutnSfP4CuQrg/8Gt18vTNl+e3lwtmcg+xpwzTy2Ru7uu5g1Ztm382SweElQCpFPZqPSDbPVkTD8T/UDNygW4TGVpx6WasfK6WqW6G+s7Wtwfxofj/sl4+dR9fftpP1Uc8jLYktNgya7sadHcqBZyqbIbZKc+qt8rDxNewaqsIFUCjEOdvHrzotwUfFrSaVEGlLeKQlKxSskn1PYclDSVaUTSlMak7uXri+nYv5Xe8DTNlNVdX92OO93t1BwjYjwE43vc52CewwD709dNE5mPEEXgiGochiPuASBGhWBmowqTKXjynfed+I1wFygIh45DcJ8vHkuvskXf9x++fTnb4qzDO+fuAVtgNq7egx6Bzz8+f/YLk+rt6HtTtlAAAAAASUVORK5CYII=",
            EnableColoring = true
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
            },
            EnableColoring = true
        },
        // VGA/CGA
        new Preset() {
            Name = "VGA/CGA 16-color",
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
            },
            EnableColoring = true
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
            ImageBase64 = "iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAIAAACQkWg2AAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAG6SURBVDhPbVKxatxAEH1750ACgXClcZnmmnBlfiG1ERx2bEjhPlUg+QyXhhQxwfbVaS6BS/ILgUuhQDAS6OQVbGCFRtaCB5TZ1Vkc5obH4412NfNmJIS4wpXC7PJyMJjhYrY//PljMZ1Oo6iJmoOIb28PDzmquX7NTMoYY60loo570adlWVZVJdyligEL0DY8eF4HVhzHSBJkmedOpKkXnw9ANygT2KSoWk3QNXQjL3AMJEDmmQUiUtHFqtUGWkNbz0aEwED94iNCZpEYJFZDQHJc4N3xUg/yzdbCKVKFjSF2er8dtg2hYhYfvaGdtSGgLYpg4h7W+mZaq6MFZ4TEIfElgyePYvnhONcD7yN46oSsQ2HT00Ns8aTG4/Fy/lTVFo5AFuS+vW2q60c2eKqg/4U9VTAl/G7lO3C/1bUIQ6zaIqyxH8J0WrH+62sL2K2Fc2Bh6SZtS3J3zjEx144bZrV4/8rnjh37kAMR3Q0RZbjktVSTtHbqDU4+ncjkjPPvaJ9Nnme71y8ft0++YLW7pyeTF6PRKM9z+ReHw+HH+Vx91Tojio357dwfopY5FSZZQPAmkLjn07O9/1R4u05xtVgfAAAAAElFTkSuQmCC",
            EnableColoring = true
        }
    };

    #endregion

    float offsetX, offsetY, offsetZ = 0;

    // Movement
    float pitch = 0;

    // GUI stuff
    float tick = 0;
    int currentPresetIndex;
    int fps = 0;
    int lastFPS = 0;
    GUIStyle guiStyleMiniText;
    GUIStyle guiStyleCurrentPreset;

    int PresetIndex {
        get { return currentPresetIndex; }
        set {
            currentPresetIndex = value;

            if(currentPresetIndex < 0)
                currentPresetIndex = presets.Length-1;

            currentPresetIndex %= presets.Length;

            // Update
            RetroFXFilter f = GetComponent<RetroFXFilter>();
            Preset p = presets[currentPresetIndex];

            f.AdjustDitheringByLuminosity = p.AdjustByLuminosity;
            f.ColorBitDepth = p.BitDepth;
            f.ColoringMode = p.ColoringMode;
            f.ColorSteps = p.Steps;
            f.DitheringIntensity = p.DitheringIntensity;
            f.DitheringSize = p.DitheringSize;
            f.EnableColoring = p.EnableColoring;
            f.EnableDithering = p.Dithering;
            f.Resolution = p.ResolutionScale;

            if(!string.IsNullOrEmpty(p.ImageBase64))
            {
                byte[] imgBytes = Convert.FromBase64String(p.ImageBase64);
                Texture2D t2d = new Texture2D(2, 2);
                t2d.LoadImage(imgBytes);

                LoadPaletteFromTexture(f, t2d);
            }
            else
                f.ColorPalette = p.ColorPalette;
        }
    }

    void Start() {
        PresetIndex = 0;

        pitch = transform.eulerAngles.x;

        offsetX = UnityEngine.Random.value * 500;
        offsetY = UnityEngine.Random.value * 500;
        offsetZ = UnityEngine.Random.value * 500;
        
        guiStyleMiniText = new GUIStyle() {
            fontSize = 16
        };

        guiStyleCurrentPreset = new GUIStyle() {
            fontSize = 32,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.LowerCenter
        };

        // Confine the mouse
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        // FPS
        fps++;
        tick += Time.deltaTime;

        if(tick >= 1) {
            tick %= 1;

            lastFPS = fps;
            fps = 0;
        }

        // Mouse control
        if(Input.GetMouseButton(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if(Cursor.lockState != CursorLockMode.Locked)
            return;

        // Controls
        if(Input.GetKeyDown(KeyCode.LeftArrow))
            PresetIndex--;
        
        if(Input.GetKeyDown(KeyCode.RightArrow))
            PresetIndex++;

        float zMove = 0,
              xMove = 0,
              yMove = 0;

        if(Input.GetKey(KeyCode.W))
            zMove += 1;

        if(Input.GetKey(KeyCode.S))
            zMove -= 1;

        if(Input.GetKey(KeyCode.A))
            xMove -= 1;

        if(Input.GetKey(KeyCode.D))
            xMove += 1;

        if(Input.GetKey(KeyCode.Q))
            yMove -= 1;

        if(Input.GetKey(KeyCode.E))
            yMove += 1;

        Vector3 moveVector = transform.forward * zMove + transform.right * xMove + transform.up * yMove;
        moveVector.Normalize();

        transform.position += moveVector * 1.5f * Time.deltaTime;

        // Looking
        float yaw = transform.eulerAngles.y + Input.GetAxis("Mouse X");
        
        pitch -= Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -89.99f, 89.99f);

        transform.eulerAngles = new Vector3(pitch, yaw, 0);
    }

    void DrawText(Rect area, string text, GUIStyle style) {
        area.x += 2;
        area.y += 2;
        style.normal.textColor = Color.black;
        GUI.Label(area, text, style);

        area.x -= 2;
        area.y -= 2;
        style.normal.textColor = Color.white;
        GUI.Label(area, text, style);
    }

    void LoadPaletteFromTexture(RetroFXFilter target, Texture2D texture) {
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

        if(orderedPalette.Count > 256)
            orderedPalette = orderedPalette.Take(256).ToList();

        // Set the palette
        Color[] palette = new Color[orderedPalette.Count];

        for(int i = 0; i < orderedPalette.Count; i++)
            palette[i] = orderedPalette[i].Key;

        target.ColorPalette = palette;
    }

    private void OnGUI() {
        // FPS drawer
        DrawText(new Rect(4, 4, 300, 300), string.Format("Press ESC to release the mouse.\nMovement: W, A, S, D, Q, E", lastFPS), guiStyleMiniText);

        DrawText(new Rect(0, 0, Screen.width, Screen.height-12),
            string.Format("<< {0} >>{1}", presets[currentPresetIndex].Name, currentPresetIndex == 0 ? "\nUse the arrow keys to switch between presets" : string.Empty),
            guiStyleCurrentPreset);
    }
}

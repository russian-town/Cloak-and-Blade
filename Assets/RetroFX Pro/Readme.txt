=====================================
== Applying the RetroFX Pro Filter
=====================================

To apply the filter to your camera please do the following:

1) Go to the camera object that you wish to apply the filter to (Such as your player camera)
2) Click on "Add Component" at the bottom of the Inspector
3) Search "RetroFX Filter" or navigate to "Scripts" and select "RetroFX Filter"

=====================================
== Customizing the filter
=====================================

The RetroFX Filter is a very powerful and flexable filter that gives you great freedom on
how you wish your scene to look with its extensible filter settings.

Under the "Show Presets" button you will find various pre-made presets that demonstrate
many example filters that RetroFX can create.

I. Display category
  1) Resolution scale - Allows you to adjust the resolution of the filter and emulate old low resolution displays.
  2) Subtractive fade - An optional slider great for fading out scenes or for transitions.

II. Dithering category
  1) Adjust intensity with luminescence - When enabled will focus dithering mostly on midtones.
  2) Dithering intensity - Determines the intensity of the dithering effect. Dithering simulates more colors on the
	screen than it can display.
  3) Dithering size - Specifies the matrix and matrix size that will be used for dithering.

III. Colors category
  1) Coloring Methods
     a. Bit Depth - Determines the amount of colors for each channel by the amount of bits allocated to the channel.
           (Example: 4 Green bits = 16 different intensities for the green channel)
     b. Color Palette - When sampling, RetroFX will find the closest match to the color in the user provided color palette.
     c. Luminosity Based - When sampling, RetroFX will determine which color to use by calculating the sampled luminosity of the color.
           Lower on the palette = the brighter the color.
     d. Steps - Determines the amount of colors for each channel by the amount of steps allocated to the channel.
           (Example: 4 Red steps = 4 different intensities for the red channel)

=====================================
== Creating, Importing, and Exporting Palettes
=====================================

RetroFX allows you to automatically generate palettes as well as import and export any palettes
for later use.

These palettes may be used with the "Color Palette" and "Luminosity based" coloring modes.

I. Creating a palette from an image
    To create a palette from an image, simply click the button of the same name just below Coloring Method.
    This will prompt you to select an image from your file system to be used as a base for extracting colors.

    How it works is by making a list of all the colors in the image and determining which are the most prominant
    thus assuming these colors are more important. This is important to consider when selecting an image because
    an image with overwhelmingly dark or bright pixels may make RetroFX's palette full of redundant dark or light
    colors- thus making your game darker/lighter and the contrast between colors too low.

    RetroFX will only pick a maximum of 256 colors from the image. The rest of the colors will be discarded and
    not included in the palette.

    It is highly recommended to be very selective when chosing an image to generate a palette from. The best image
    that would make a good candidate for palette generation should include most if not all the colors you plan to use
    in your scene with balanced tones and contrasts.

    The suggested method to make a palette is to make a small PNG image at most 16x16 pixels and inputting all the colors
    you desire into this image. One pixel for each desired color. Examples of these images can be found in 
            RetroFX Pro / Editor / Resources / PresetPalettes

II. Exporting and importing a palette
    If you want to take your palette to another scene in your project or to another project entirely you will
    need to export your palette into a file so that it may be used across different scenes/projects. This will
    also be neccesary if you need to distribute a unified palette amongst your team of developers and artists.

    This can be done by clicking the 'Export...' button below a palette of colors in the RetroFX Filter component
    then saving the file to a destination on your file system.

    When a file is saved it is recommended to not edit a RetroFX Palette file directly.

    To import a palette, click the 'Import...' button next to the aforementioned Export button and select the file
    from your file system.
        WARNING: Doing this will overwrite the current palette you have in the RetroFX Filter Component. Be sure
        to either save your work or exporting your current palette if you do not wish to lose it.
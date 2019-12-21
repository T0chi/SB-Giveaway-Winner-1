using OpenTK;
using System;
using System.Linq;
using System.Drawing;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;
using System.Collections.Generic;

public class PointSystem
{
    private StoryboardObjectGenerator generator;

    public PointSystem(StoryboardObjectGenerator generator)
    {
        this.generator = generator;

        Generate();
    }

    public void Generate()
    {
        // Point System stuff
    }
}
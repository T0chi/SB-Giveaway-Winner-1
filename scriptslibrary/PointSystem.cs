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

    public PointSystem(StoryboardObjectGenerator generator, int[] totalPoints)
    {
        this.generator = generator;

        Generate(totalPoints);
    }

    public void Generate(int[] totalPoints)
    {
        List<int> TOTAL = new List<int>(totalPoints);

        generator.Log(TOTAL.ToString());
    }
}
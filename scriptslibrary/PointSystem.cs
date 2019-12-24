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

    public PointSystem(StoryboardObjectGenerator generator, int[] sectionPoints)
    {
        this.generator = generator;

        Generate(sectionPoints);
    }

    public void Generate(int[] sectionPoints)
    {

        foreach(var point in sectionPoints)
        {
            var thousand = sectionPoints[0] * 1000;
            var hundred = sectionPoints[1] * 100;
            var ten = sectionPoints[2] * 10;
            var one = sectionPoints[3];

            var TOTALPOINTS = (thousand + hundred + ten + one);

            generator.Log($"Section Points: {thousand + hundred + ten + one}pts || Total Points: {TOTALPOINTS}pts");
        }
    }
}
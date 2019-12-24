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

    public PointSystem(StoryboardObjectGenerator generator, int[] pointsPass, int[] pointsFail)
    {
        this.generator = generator;

        Generate(pointsPass, pointsFail);
    }

    public void Generate(int[] pointsPass, int[] pointsFail)
    {
        foreach(var point in pointsPass)
        {
            var thousand = pointsPass[0] * 1000;
            var hundred = pointsPass[1] * 100;
            var ten = pointsPass[2] * 10;
            var one = pointsPass[3];

            var SECTIONPOINTS = thousand + hundred + ten + one;
            var TOTALPOINTS = SECTIONPOINTS;

            generator.Log($"Pass Points: {SECTIONPOINTS}pts || Total Pass Points: {TOTALPOINTS}pts");
        }

        foreach(var point in pointsFail)
        {
            var hundred = pointsFail[0] * 100;
            var ten = pointsFail[1] * 10;
            var one = pointsPass[2];

            var SECTIONPOINTS = hundred + ten + one;
            var TOTALPOINTS = SECTIONPOINTS;

            generator.Log($"Fail Points: {SECTIONPOINTS}pts || Total Fail Points: {TOTALPOINTS}pts");
        }
    }
}
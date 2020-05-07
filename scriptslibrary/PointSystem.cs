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
    public int totalPass;
    // public int totalFail;
    public int pointsPass;
    // public int pointsFail;
        
    public PointSystem()
    {

    }

    public void AddPassPoints(int[] points)
    {
        int score = 0;
        for(int i = 0; i < points.Length; i++)
            score += points[i] * (1000 / (int)Math.Pow(10, i));

        totalPass += score;

        var thousands = points[0] * 1000;
        var hundreds = points[1] * 100;
        var tens = points[2] * 10;
        var ones = points[3] * 1;
        
        var sectionPoints = thousands + hundreds + tens + ones;
        pointsPass = sectionPoints;
    }

    // public void AddFailPoints(int[] points)
    // {
    //     int score = 0;
    //     for(int i = 0; i < points.Length; i++)
    //         score += points[i] * (1000 / (int)Math.Pow(10, i));

    //     totalFail += score;

    //     var hundreds = points[1] * 100;
    //     var tens = points[2] * 10;
    //     var ones = points[3] * 1;
        
    //     var sectionPoints = hundreds + tens + ones;
    //     pointsFail = sectionPoints;
    // }
}
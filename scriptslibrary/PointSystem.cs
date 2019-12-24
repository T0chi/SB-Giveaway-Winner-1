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
    public int totalPass;
    public int totalFail;

    public PointSystem(StoryboardObjectGenerator generator)
    {
        this.generator = generator;
    }
    
    public void AddPassPoints(int points) => totalPass += points;

    public void AddFailPoints(int points) => totalFail += points;
}
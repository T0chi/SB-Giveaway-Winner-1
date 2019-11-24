using OpenTK;
using System;
using System.Linq;
using System.Drawing;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;
using System.Collections.Generic;

public class Mission2
{
    //This is the parameters we pass in the class, these lines are used to make them available to use everywhere in the class.
    private StoryboardObjectGenerator generator;
    private int startTime;
    private int endTime;
    private int FadeTime = 5000;

    public Mission2(StoryboardObjectGenerator generator, int startTime, int endTime)
    {
        //And this pack of lines are just the way we set our local variable with the parameters values of the constructor.
        this.generator = generator;
        this.startTime = startTime;
        this.endTime = endTime;

        Something(startTime, endTime);
    }

    public void Something(int StartTime, int EndTime)
    {
    }
}
using OpenTK;
using System;
using System.Linq;
using System.Drawing;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;
using System.Collections.Generic;

public class Language
{
    // private StoryboardObjectGenerator generator;
    
    public bool setEnglish;

    public Language()
    {

    }

    public void chooseLanguage(bool Bool)
    {
        setEnglish = Bool;
    }
}
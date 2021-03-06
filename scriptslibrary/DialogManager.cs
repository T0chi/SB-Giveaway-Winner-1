using OpenTK;
using System;
using System.Linq;
using System.Drawing;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;
using System.Collections.Generic;

public class DialogTiming
{
    public int startTime;
    public int endTime;
}

public class Position
{
    public Vector2 position = new Vector2();
}

public class DialogManager
{
    public StoryboardObjectGenerator generator;
    public FontGenerator font;

    public int startTime;
    public int endTime;
    public int fontSize;
    // public string[] sentences;
    public float x;
    public float y;
    public string layerText;
    public string layerBox;
    public bool originCentre;
    public bool showBox;
    public float textFade;
    public Color4 textColor;
    public Color4 boxColor;
    public float boxFade;
    public int sampleDelay;
    public string sampleName;
    public DialogBoxes.Pointer pointer;
    public DialogBoxes.Push push;
    public int FadeIn;
    public int FadeOut;
    public OsbSprite sprite;
    public OsbSprite spriteBox;

    public DialogManager()
    {

    }

    public DialogManager(StoryboardObjectGenerator generator, FontGenerator font, int startTime, int endTime, string layerText, float x, float y, bool originCentre,
            int fontSize, float textFade, int FadeIn, int FadeOut, Color4 textColor, bool showBox, float boxFade, Color4 boxColor, string layerBox, int sampleDelay, string sampleName,
            DialogBoxes.Pointer pointer, DialogBoxes.Push push, string[] sentences)
    {
        Setup(generator,font, startTime, endTime, layerText, x, y, originCentre,
            fontSize, textFade, FadeIn, FadeOut, textColor, showBox, boxFade, boxColor, layerBox, sampleDelay, sampleName,
            pointer, push);

        Generate(sentences, FadeIn, FadeOut);
    }

    public void Setup(StoryboardObjectGenerator generator, FontGenerator font, int startTime, int endTime, string layerText, float x, float y, bool originCentre,
            int fontSize, float textFade, int FadeIn, int FadeOut, Color4 textColor, bool showBox, float boxFade, Color4 boxColor, string layerBox, int sampleDelay, string sampleName,
            DialogBoxes.Pointer pointer, DialogBoxes.Push push)
    {
        this.generator = generator;
        this.font = font;

        this.x = x;
        this.y = y;
        this.fontSize = fontSize;
        this.layerText = layerText;
        this.layerBox = layerBox;
        this.startTime = startTime;
        this.endTime = endTime;
        this.originCentre = originCentre;
        this.textColor = textColor;
        this.boxColor = boxColor;
        this.showBox = showBox;
        this.boxFade = boxFade;
        this.textFade = textFade;
        this.sampleDelay = sampleDelay;
        this.sampleName = sampleName;
        this.pointer = pointer;
        this.push = push;
        this.FadeIn = FadeIn;
        this.FadeOut = FadeOut;

        // Generate(sentences, FadeIn, FadeOut);
    }

    public void Generate(string[] text, int FadeIn, int FadeOut,
            bool startTriggerGroup = false, string triggerType = "", int startTrigger = 0, int endTrigger = 0, int triggerGroup = 0)
    {
        // convert text to string lines
        List<string> Sentences = new List<string>(text);
        // string[] text = sentences.Split('|');

        var position = new Position();
        var dialogTiming = new DialogTiming();
        dialogTiming.startTime = startTime;
        dialogTiming.endTime = endTime;
        position.position = new Vector2(x, y);
        var dialog = new DialogText(generator, layerText, font, sprite, textColor, position, dialogTiming, textFade, FadeIn, FadeOut, fontSize, originCentre);
        var dialogOne = new DialogBoxes(generator, layerBox, dialogTiming, position);

        // write sentences
        foreach (string line in Sentences)
        {
            dialog.lines.Add(line);
        }
        if (showBox)
        {
            dialog.calculateLineWidth();
            dialog.calculateLineHeight();

            sprite = dialog.Generate(sprite, startTriggerGroup, triggerType, startTrigger, endTrigger, triggerGroup);

            spriteBox = dialogOne.GenerateBoxes(generator, layerBox, sampleDelay, sampleName, boxColor, dialogTiming, (fontSize * 0.08f) - 1, boxFade,
            position, pointer, push, originCentre, dialog.GetLineWidth(), dialog.heightSpace(),
            spriteBox, startTriggerGroup, triggerType, startTrigger, endTrigger, triggerGroup);
        }

        else
        {
            dialog.calculateLineWidth();
            dialog.calculateLineHeight();
            
            sprite = dialog.Generate(sprite, startTriggerGroup, triggerType, startTrigger, endTrigger, triggerGroup);

            spriteBox = dialogOne.GenerateBoxes(generator, layerBox, sampleDelay, sampleName, boxColor, dialogTiming, (fontSize * 0.08f) - 1, 0f,
            position, pointer, push, originCentre, dialog.GetLineWidth(), dialog.heightSpace(),
            spriteBox, startTriggerGroup, triggerType, startTrigger, endTrigger, triggerGroup);
        }
    }
}

public class DialogText
{
    //This is the parameters we pass in the class, these lines are used to make them available to use everywhere in the class.
    private StoryboardObjectGenerator generator;
    private FontGenerator font;
    private float startTime;
    private int endTime;
    private float fade;
    private int fontSize;
    private float delay = 0.5f;
    private Position position;
    private Color4 Color;
    private float lineWidth;
    private float lineHeight;
    private float textScale = 0.5f;
    private bool centre = true;
    private string layerName;
    private int FadeIn;
    private int FadeOut;

    public List<String> lines = new List<String>();

    public DialogText(StoryboardObjectGenerator generator, string layerName, FontGenerator font, OsbSprite sprite, Color4 Color, Position position, DialogTiming timing, float fade, int FadeIn, int FadeOut, int fontSize, bool centre)
    {
        //And this pack of lines are just the way we set our local variable with the parameters values of the constructor.
        this.generator = generator;
        this.font = font;

        this.startTime = timing.startTime;
        this.endTime = timing.endTime;
        this.position = position;
        this.fade = fade;
        this.fontSize = fontSize;
        this.Color = Color;
        this.lineWidth = 0f;
        this.centre = centre;
        this.layerName = layerName;
        this.FadeIn = FadeIn;
        this.FadeOut = FadeOut;
    }

    public float lineHeightSpace()
    {
        var lineheightSpace = (this.fontSize * 0.5f) + 5;
        return lineheightSpace;
    }

    public float heightSpace()
    {
        var lineCount = lines.Count;
        var heightSpace = lineHeightSpace() * lineCount;
        return heightSpace;
    }

    public float GetLineWidth()
    {
        return this.lineWidth;
    }

    public void calculateLineWidth() {
        foreach (var line in lines)
        {
        float lineWidth = 0f;
        FontTexture[] textures = new FontTexture[line.Length];

        int t = 0;
        foreach (var letter in line)
        {
            var texture = font.GetTexture(letter.ToString());
                textures[t] = texture;
                lineWidth += texture.BaseWidth * this.textScale;
                t++;
        }

        this.lineWidth = Math.Max(this.lineWidth, lineWidth);
        }
    }

    public void calculateLineHeight() {
        foreach (var line in lines)
        {
        float lineHeight = 0f;
        FontTexture[] textures = new FontTexture[line.Length];

        int t = 0;
        foreach (var letter in line)
        {
            var texture = font.GetTexture(letter.ToString());
                textures[t] = texture;
                lineHeight += texture.BaseHeight * this.textScale;
                t++;
        }

        this.lineHeight = Math.Max(this.lineHeight, lineHeight);
        }
    }

    public OsbSprite Generate(OsbSprite sprite, bool startTriggerGroup = false, string triggerType = "", int startTrigger = 0, int endTrigger = 0, int triggerGroup = 0)
    {
        float letterY = position.position.Y - 4;

        foreach (var line in lines)
        {
            float i = 0;

            float letterX = position.position.X;
            if (this.centre)
            {
                letterX = position.position.X - this.lineWidth * 0.5f;
            }

            foreach (var letter in line)
            {
                var duration = endTime - startTime;
                var texture = font.GetTexture(letter.ToString());
                if (!texture.IsEmpty)
                {
                    var letterPos = new Vector2(letterX, letterY)
                        + texture.OffsetFor(OsbOrigin.Centre) * this.textScale;

                    var spriteText = generator.GetLayer(layerName).CreateSprite(texture.Path, OsbOrigin.Centre, letterPos);

                    if (startTriggerGroup)
                    {
                        spriteText.StartTriggerGroup(triggerType, startTrigger, endTrigger, triggerGroup);
                    }

                    spriteText.Fade(startTime + delay * i, startTime + FadeIn + delay * i, 0, fade);
                    spriteText.Fade(endTime, endTime + FadeOut, fade, 0);
                    spriteText.Scale(startTime, this.textScale);
                    spriteText.Color(startTime, Color);

                    if (startTriggerGroup)
                    {
                        spriteText.EndGroup();
                    }

                    sprite = spriteText;
                }
                letterX += texture.BaseWidth * this.textScale;
                startTime += delay * i;
                i++;
            }
            letterY += this.textScale * lineHeightSpace() / textScale;
        }
        return sprite;
    }
}

public class DialogBoxes
{
    private StoryboardObjectGenerator generator;
    private DialogTiming timing;
    private Position position;
    private string layerName;
    public DialogBoxes(StoryboardObjectGenerator generator, string layerName, DialogTiming timing, Position position)
    {
        this.generator = generator;
        this.timing = timing;
        this.position = position;
        this.layerName = layerName;
    }

    public enum Pointer { None, Up, Down, CentreLeft, CentreRight, TopLeft, TopRight, BottomLeft, BottomRight };
    public enum Push { None, Up, Down, Left, Right };
    public int sTime()
    {
        return this.timing.startTime;
    }
    public int eTime()
    {
        return this.timing.endTime;
    }

    public OsbSprite GenerateBoxes(StoryboardObjectGenerator generator, string layerName, int soundDelay, string soundEffect, Color4 Color, DialogTiming timing,
           float pointerScale, float Fade, Position position, Pointer pointer, Push push, bool centre, float lineWidth, float lineHeight,
           OsbSprite spriteBox, bool startTriggerGroup = false, string triggerType = "", int startTrigger = 0, int endTrigger = 0, int triggerGroup = 0)
    {
        var d = 300;
        var fadeTime = 500;
        var PushValue = 30;
        var biggerBox = 10;
        var boxPos = centre ? new Vector2(position.position.X - lineWidth * 0.5f - (biggerBox / 2) - 3, position.position.Y - (biggerBox / 2) - 4) :
                                          new Vector2(position.position.X - (biggerBox / 2) - 3, position.position.Y - (biggerBox / 2) - 4);

        var layer = generator.GetLayer(layerName);

        // var inputBox = layer.CreateSprite("sb/dialog/box/b.png", OsbOrigin.TopLeft, boxPos);
        var inputBox = layer.CreateAnimation("sb/dialog/box/b.png", 30, 60, OsbLoopType.LoopForever, OsbOrigin.TopLeft, boxPos);
        var bitmapInputBox = generator.GetMapsetBitmap("sb/pixel.png");
        var bitmapInputBox2 = generator.GetMapsetBitmap("sb/pixel.png");
        var bitmapInputBoxWidth = ((lineWidth / bitmapInputBox.Width) + biggerBox);
        var bitmapInputBoxHeight = (lineHeight / bitmapInputBox.Height) + biggerBox;

        var widthInputBox = bitmapInputBoxWidth / 900;
        var heightInputBox = bitmapInputBoxHeight / 400;
        var bWidth = widthInputBox * 900;
        var bHeight = heightInputBox * 900;
        // var bitmapPointer = generator.GetMapsetBitmap("sb/dialog/pointers/pointer.png");
        // var bitmapPointerCorner = generator.GetMapsetBitmap("sb/dialog/pointers/pointerCorner.png");

        // generator.Log($"width: {widthInputBox}     heigt: {heightInputBox}");

        if (startTriggerGroup)
        {
            inputBox.StartTriggerGroup(triggerType, startTrigger, endTrigger, triggerGroup);
        }

        // sfx - message-1
        var message1 = layer.CreateSample(soundEffect, timing.startTime - d + soundDelay, 40);
        //

        // start style
        if (pointer == Pointer.None)
        {
            inputBox.Color(timing.startTime - d, Color);
            inputBox.ScaleVec(timing.startTime - d, widthInputBox, heightInputBox);
            inputBox.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
            inputBox.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

            if (push == Push.None)

            if (push == Push.Up)
            {
                inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y + PushValue, boxPos.Y);
                inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y - PushValue);
            }

            if (push == Push.Down)
            {
                inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y - PushValue, boxPos.Y);
                inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y + PushValue);
            }

            if (push == Push.Left)
            {
                inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X + PushValue, boxPos.X);
                inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X - PushValue);
            }

            if (push == Push.Right)
            {
                inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X - PushValue, boxPos.X);
                inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X + PushValue);
            }
        }
        // end style

        else
        {
            inputBox.Color(timing.startTime - d, Color);
            inputBox.ScaleVec(timing.startTime - d, widthInputBox, heightInputBox);
            inputBox.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
            inputBox.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);
            
            if (push == Push.None)

            if (push == Push.Up)
            {
                inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y + PushValue, boxPos.Y);
                inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y - PushValue);
            }

            if (push == Push.Down)
            {
                inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y - PushValue, boxPos.Y);
                inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y + PushValue);
            }

            if (push == Push.Left)
            {
                inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X + PushValue, boxPos.X);
                inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X - PushValue);
            }

            if (push == Push.Right)
            {
                inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X - PushValue, boxPos.X);
                inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X + PushValue);
            }
        }

        // // start style
        // if (pointer == Pointer.Up)
        // {
        //     inputBox.Color(timing.startTime - d, Color);
        //     inputBox.ScaleVec(timing.startTime - d, widthInputBox, heightInputBox);
        //     inputBox.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
        //     inputBox.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

        //     var pointerPos = new Vector2(boxPos.X + (lineWidth / 2), boxPos.Y);
        //     var point = layer.CreateSprite("sb/dialog/pointers/pointer.png", OsbOrigin.BottomCentre, pointerPos);

        //     if (startTriggerGroup)
        //     {
        //         point.StartTriggerGroup(triggerType, startTrigger, endTrigger, triggerGroup);
        //     }

        //     point.Color(timing.startTime - d, Color);
        //     point.Scale(timing.startTime, pointerScale);
        //     point.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
        //     point.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

        //     if (push == Push.None)
        //     {
        //         point.MoveX(timing.startTime - d, pointerPos.X);
        //         point.MoveY(timing.endTime + d, pointerPos.Y);
        //     }

        //     if (push == Push.Up)
        //     {
        //         inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y + PushValue, boxPos.Y);
        //         inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y - PushValue);
        //         point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y + PushValue, pointerPos.Y);
        //         point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y - PushValue);
        //     }

        //     if (push == Push.Down)
        //     {
        //         inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y - PushValue, boxPos.Y);
        //         inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y + PushValue);
        //         point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y - PushValue, pointerPos.Y);
        //         point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y + PushValue);
        //     }

        //     if (push == Push.Left)
        //     {
        //         inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X + PushValue, boxPos.X);
        //         inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X - PushValue);
        //         point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X + PushValue, pointerPos.X);
        //         point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X - PushValue);
        //     }

        //     if (push == Push.Right)
        //     {
        //         inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X - PushValue, boxPos.X);
        //         inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X + PushValue);
        //         point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X - PushValue, pointerPos.X);
        //         point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X + PushValue);
        //     }

        //     if (startTriggerGroup)
        //     {
        //         point.EndGroup();
        //     }

        //     spriteBox = point;
        // }
        // // end style

        // // start style
        // if (pointer == Pointer.Down)
        // {
        //     inputBox.Color(timing.startTime - d, Color);
        //     inputBox.ScaleVec(timing.startTime - d, widthInputBox, heightInputBox);
        //     inputBox.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
        //     inputBox.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

        //     var pointerPos = new Vector2(boxPos.X + (lineWidth / 2), boxPos.Y + heightInputBox);
        //     var point = layer.CreateSprite("sb/dialog/pointers/pointer.png", OsbOrigin.BottomCentre, pointerPos);

        //     if (startTriggerGroup)
        //     {
        //         point.StartTriggerGroup(triggerType, startTrigger, endTrigger, triggerGroup);
        //     }

        //     point.Rotate(timing.startTime, MathHelper.DegreesToRadians(180));
        //     point.Scale(timing.startTime, pointerScale);
        //     point.Color(timing.startTime - d, Color);
        //     point.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
        //     point.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

        //     if (push == Push.None)
        //     {
        //         point.MoveX(timing.startTime - d, pointerPos.X);
        //         point.MoveY(timing.endTime + d, pointerPos.Y);
        //     }

        //     if (push == Push.Up)
        //     {
        //         inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y + PushValue, boxPos.Y);
        //         inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y - PushValue);
        //         point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y + PushValue, pointerPos.Y);
        //         point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y - PushValue);
        //     }

        //     if (push == Push.Down)
        //     {
        //         inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y - PushValue, boxPos.Y);
        //         inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y + PushValue);
        //         point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y - PushValue, pointerPos.Y);
        //         point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y + PushValue);
        //     }

        //     if (push == Push.Left)
        //     {
        //         inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X + PushValue, boxPos.X);
        //         inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X - PushValue);
        //         point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X + PushValue, pointerPos.X);
        //         point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X - PushValue);
        //     }

        //     if (push == Push.Right)
        //     {
        //         inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X - PushValue, boxPos.X);
        //         inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X + PushValue);
        //         point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X - PushValue, pointerPos.X);
        //         point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X + PushValue);
        //     }

        //     if (startTriggerGroup)
        //     {
        //         point.EndGroup();
        //     }

        //     spriteBox = point;
        // }
        // // end style

        // // start style
        // if (pointer == Pointer.CentreLeft)
        // {
        //     inputBox.Color(timing.startTime - d, Color);
        //     inputBox.ScaleVec(timing.startTime - d, widthInputBox, heightInputBox);
        //     inputBox.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
        //     inputBox.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

        //     var pointerPos = new Vector2(boxPos.X, boxPos.Y + (heightInputBox / 2));
        //     var point = layer.CreateSprite("sb/dialog/pointers/pointer.png", OsbOrigin.BottomCentre, pointerPos);

        //     if (startTriggerGroup)
        //     {
        //         point.StartTriggerGroup(triggerType, startTrigger, endTrigger, triggerGroup);
        //     }

        //     point.Rotate(timing.startTime, MathHelper.DegreesToRadians(-90));
        //     point.Scale(timing.startTime, pointerScale);
        //     point.Color(timing.startTime - d, Color);
        //     point.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
        //     point.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

        //     if (push == Push.None)
        //     {
        //         point.MoveX(timing.startTime - d, pointerPos.X);
        //         point.MoveY(timing.endTime + d, pointerPos.Y);
        //     }

        //     if (push == Push.Up)
        //     {
        //         inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y + PushValue, boxPos.Y);
        //         inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y - PushValue);
        //         point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y + PushValue, pointerPos.Y);
        //         point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y - PushValue);
        //     }

        //     if (push == Push.Down)
        //     {
        //         inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y - PushValue, boxPos.Y);
        //         inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y + PushValue);
        //         point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y - PushValue, pointerPos.Y);
        //         point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y + PushValue);
        //     }

        //     if (push == Push.Left)
        //     {
        //         inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X + PushValue, boxPos.X);
        //         inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X - PushValue);
        //         point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X + PushValue, pointerPos.X);
        //         point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X - PushValue);
        //     }

        //     if (push == Push.Right)
        //     {
        //         inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X - PushValue, boxPos.X);
        //         inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X + PushValue);
        //         point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X - PushValue, pointerPos.X);
        //         point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X + PushValue);
        //     }

        //     if (startTriggerGroup)
        //     {
        //         point.EndGroup();
        //     }

        //     spriteBox = point;
        // }
        // // end style

        // // start style
        // if (pointer == Pointer.CentreRight)
        // {
        //     inputBox.Color(timing.startTime - d, Color);
        //     inputBox.ScaleVec(timing.startTime - d, widthInputBox, heightInputBox);
        //     inputBox.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
        //     inputBox.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

        //     var pointerPos = new Vector2(boxPos.X + widthInputBox, boxPos.Y + (heightInputBox / 2));
        //     var point = layer.CreateSprite("sb/dialog/pointers/pointer.png", OsbOrigin.BottomCentre, pointerPos);

        //     if (startTriggerGroup)
        //     {
        //         point.StartTriggerGroup(triggerType, startTrigger, endTrigger, triggerGroup);
        //     }

        //     point.Rotate(timing.startTime, MathHelper.DegreesToRadians(90));
        //     point.Scale(timing.startTime, pointerScale);
        //     point.Color(timing.startTime - d, Color);
        //     point.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
        //     point.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

        //     if (push == Push.None)
        //     {
        //         point.MoveX(timing.startTime - d, pointerPos.X);
        //         point.MoveY(timing.endTime + d, pointerPos.Y);
        //     }

        //     if (push == Push.Up)
        //     {
        //         inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y + PushValue, boxPos.Y);
        //         inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y - PushValue);
        //         point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y + PushValue, pointerPos.Y);
        //         point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y - PushValue);
        //     }

        //     if (push == Push.Down)
        //     {
        //         inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y - PushValue, boxPos.Y);
        //         inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y + PushValue);
        //         point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y - PushValue, pointerPos.Y);
        //         point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y + PushValue);
        //     }

        //     if (push == Push.Left)
        //     {
        //         inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X + PushValue, boxPos.X);
        //         inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X - PushValue);
        //         point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X + PushValue, pointerPos.X);
        //         point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X - PushValue);
        //     }

        //     if (push == Push.Right)
        //     {
        //         inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X - PushValue, boxPos.X);
        //         inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X + PushValue);
        //         point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X - PushValue, pointerPos.X);
        //         point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X + PushValue);
        //     }

        //     if (startTriggerGroup)
        //     {
        //         point.EndGroup();
        //     }

        //     spriteBox = point;
        // }
        // // end style

        // // start style
        // if (pointer == Pointer.TopLeft)
        // {
        //     inputBox.Color(timing.startTime - d, Color);
        //     inputBox.ScaleVec(timing.startTime - d, widthInputBox, heightInputBox);
        //     inputBox.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
        //     inputBox.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

        //     var pointerPos = new Vector2(boxPos.X + (bitmapPointerCorner.Height / 4) + 0.5f, boxPos.Y + (bitmapPointerCorner.Height / 4) + 0.5f);
        //     var point = layer.CreateSprite("sb/dialog/pointers/pointerCorner.png", OsbOrigin.BottomCentre, pointerPos);

        //     if (startTriggerGroup)
        //     {
        //         point.StartTriggerGroup(triggerType, startTrigger, endTrigger, triggerGroup);
        //     }

        //     point.Rotate(timing.startTime, MathHelper.DegreesToRadians(-45));
        //     point.Scale(timing.startTime, pointerScale);
        //     point.Color(timing.startTime - d, Color);
        //     point.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
        //     point.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

        //     if (push == Push.None)
        //     {
        //         point.MoveX(timing.startTime - d, pointerPos.X);
        //         point.MoveY(timing.endTime + d, pointerPos.Y);
        //     }

        //     if (push == Push.Up)
        //     {
        //         inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y + PushValue, boxPos.Y);
        //         inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y - PushValue);
        //         point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y + PushValue, pointerPos.Y);
        //         point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y - PushValue);
        //     }

        //     if (push == Push.Down)
        //     {
        //         inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y - PushValue, boxPos.Y);
        //         inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y + PushValue);
        //         point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y - PushValue, pointerPos.Y);
        //         point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y + PushValue);
        //     }

        //     if (push == Push.Left)
        //     {
        //         inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X + PushValue, boxPos.X);
        //         inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X - PushValue);
        //         point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X + PushValue, pointerPos.X);
        //         point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X - PushValue);
        //     }

        //     if (push == Push.Right)
        //     {
        //         inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X - PushValue, boxPos.X);
        //         inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X + PushValue);
        //         point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X - PushValue, pointerPos.X);
        //         point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X + PushValue);
        //     }

        //     if (startTriggerGroup)
        //     {
        //         point.EndGroup();
        //     }

        //     spriteBox = point;
        // }
        // // end style

        // // start style
        // if (pointer == Pointer.TopRight)
        // {
        //     inputBox.Color(timing.startTime - d, Color);
        //     inputBox.ScaleVec(timing.startTime - d, widthInputBox, heightInputBox);
        //     inputBox.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
        //     inputBox.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

        //     var pointerPos = new Vector2(boxPos.X + widthInputBox - (bitmapPointerCorner.Height / 4) - 0.5f, boxPos.Y + (bitmapPointerCorner.Height / 4) + 0.5f);
        //     var point = layer.CreateSprite("sb/dialog/pointers/pointerCorner.png", OsbOrigin.BottomCentre, pointerPos);

        //     if (startTriggerGroup)
        //     {
        //         point.StartTriggerGroup(triggerType, startTrigger, endTrigger, triggerGroup);
        //     }

        //     point.Rotate(timing.startTime, MathHelper.DegreesToRadians(45));
        //     point.Scale(timing.startTime, pointerScale);
        //     point.Color(timing.startTime - d, Color);
        //     point.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
        //     point.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

        //     if (push == Push.None)
        //     {
        //         point.MoveX(timing.startTime - d, pointerPos.X);
        //         point.MoveY(timing.endTime + d, pointerPos.Y);
        //     }

        //     if (push == Push.Up)
        //     {
        //         inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y + PushValue, boxPos.Y);
        //         inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y - PushValue);
        //         point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y + PushValue, pointerPos.Y);
        //         point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y - PushValue);
        //     }

        //     if (push == Push.Down)
        //     {
        //         inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y - PushValue, boxPos.Y);
        //         inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y + PushValue);
        //         point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y - PushValue, pointerPos.Y);
        //         point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y + PushValue);
        //     }

        //     if (push == Push.Left)
        //     {
        //         inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X + PushValue, boxPos.X);
        //         inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X - PushValue);
        //         point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X + PushValue, pointerPos.X);
        //         point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X - PushValue);
        //     }

        //     if (push == Push.Right)
        //     {
        //         inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X - PushValue, boxPos.X);
        //         inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X + PushValue);
        //         point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X - PushValue, pointerPos.X);
        //         point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X + PushValue);
        //     }

        //     if (startTriggerGroup)
        //     {
        //         point.EndGroup();
        //     }

        //     spriteBox = point;
        // }
        // // end style

        // // start style
        // if (pointer == Pointer.BottomLeft)
        // {
        //     inputBox.Color(timing.startTime - d, Color);
        //     inputBox.ScaleVec(timing.startTime - d, widthInputBox, heightInputBox);
        //     inputBox.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
        //     inputBox.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

        //     var pointerPos = new Vector2(boxPos.X + (bitmapPointerCorner.Height / 4) + 0.5f, boxPos.Y + heightInputBox - (bitmapPointerCorner.Height / 4) - 0.5f);
        //     var point = layer.CreateSprite("sb/dialog/pointers/pointerCorner.png", OsbOrigin.BottomCentre, pointerPos);

        //     if (startTriggerGroup)
        //     {
        //         point.StartTriggerGroup(triggerType, startTrigger, endTrigger, triggerGroup);
        //     }

        //     point.Rotate(timing.startTime, MathHelper.DegreesToRadians(-135));
        //     point.Scale(timing.startTime, pointerScale);
        //     point.Color(timing.startTime - d, Color);
        //     point.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
        //     point.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

        //     if (push == Push.None)
        //     {
        //         point.MoveX(timing.startTime - d, pointerPos.X);
        //         point.MoveY(timing.endTime + d, pointerPos.Y);
        //     }

        //     if (push == Push.Up)
        //     {
        //         inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y + PushValue, boxPos.Y);
        //         inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y - PushValue);
        //         point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y + PushValue, pointerPos.Y);
        //         point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y - PushValue);
        //     }

        //     if (push == Push.Down)
        //     {
        //         inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y - PushValue, boxPos.Y);
        //         inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y + PushValue);
        //         point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y - PushValue, pointerPos.Y);
        //         point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y + PushValue);
        //     }

        //     if (push == Push.Left)
        //     {
        //         inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X + PushValue, boxPos.X);
        //         inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X - PushValue);
        //         point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X + PushValue, pointerPos.X);
        //         point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X - PushValue);
        //     }

        //     if (push == Push.Right)
        //     {
        //         inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X - PushValue, boxPos.X);
        //         inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X + PushValue);
        //         point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X - PushValue, pointerPos.X);
        //         point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X + PushValue);
        //     }

        //     if (startTriggerGroup)
        //     {
        //         point.EndGroup();
        //     }

        //     spriteBox = point;
        // }
        // // end style

        // // start style
        // if (pointer == Pointer.BottomRight)
        // {
        //     inputBox.Color(timing.startTime - d, Color);
        //     inputBox.ScaleVec(timing.startTime - d, widthInputBox, heightInputBox);
        //     inputBox.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
        //     inputBox.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

        //     var pointerPos = new Vector2(boxPos.X + widthInputBox - (bitmapPointerCorner.Height / 4) - 0.5f, boxPos.Y + heightInputBox - (bitmapPointerCorner.Height / 4) - 0.5f);
        //     var point = layer.CreateSprite("sb/dialog/pointers/pointerCorner.png", OsbOrigin.BottomCentre, pointerPos);

        //     if (startTriggerGroup)
        //     {
        //         point.StartTriggerGroup(triggerType, startTrigger, endTrigger, triggerGroup);
        //     }

        //     point.Rotate(timing.startTime, MathHelper.DegreesToRadians(135));
        //     point.Scale(timing.startTime, pointerScale);
        //     point.Color(timing.startTime - d, Color);
        //     point.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
        //     point.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

        //     if (push == Push.None)
        //     {
        //         point.MoveX(timing.startTime - d, pointerPos.X);
        //         point.MoveY(timing.endTime + d, pointerPos.Y);
        //     }

        //     if (push == Push.Up)
        //     {
        //         inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y + PushValue, boxPos.Y);
        //         inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y - PushValue);
        //         point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y + PushValue, pointerPos.Y);
        //         point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y - PushValue);
        //     }

        //     if (push == Push.Down)
        //     {
        //         inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y - PushValue, boxPos.Y);
        //         inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y + PushValue);
        //         point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y - PushValue, pointerPos.Y);
        //         point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y + PushValue);
        //     }

        //     if (push == Push.Left)
        //     {
        //         inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X + PushValue, boxPos.X);
        //         inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X - PushValue);
        //         point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X + PushValue, pointerPos.X);
        //         point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X - PushValue);
        //     }

        //     if (push == Push.Right)
        //     {
        //         inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X - PushValue, boxPos.X);
        //         inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X + PushValue);
        //         point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X - PushValue, pointerPos.X);
        //         point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X + PushValue);
        //     }

        //     if (startTriggerGroup)
        //     {
        //         point.EndGroup();
        //     }

        //     spriteBox = point;
        // }
        // // end style

        if (startTriggerGroup)
        {
            inputBox.EndGroup();
        }

        spriteBox = inputBox;

        return spriteBox;
    }
}
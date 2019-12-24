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
    private StoryboardObjectGenerator generator;
    private FontGenerator font;

    private int startTime;
    private int endTime;
    private int fontSize;
    public string[] sentences;
    private float x;
    private float y;
    private string layerText;
    private string layerBox;
    private bool originCentre;
    private bool showBox;
    private float textFade;
    private int startFadeTime;
    private int endFadeTime;
    private Color4 textColor;
    private Color4 boxColor;
    private float boxFade;
    private int sampleDelay;
    private string sampleName;
    private DialogBoxes.Pointer pointer;
    private DialogBoxes.Push push;

    public DialogManager(StoryboardObjectGenerator generator, FontGenerator font, int startTime, int endTime, string layerText, float x, float y, bool originCentre,
            int fontSize, float textFade, int startFadeTime, int endFadeTime, Color4 textColor, bool showBox, float boxFade, Color4 boxColor, string layerBox, int sampleDelay, string sampleName,
            DialogBoxes.Pointer pointer, DialogBoxes.Push push, string[] sentences)
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
        this.startFadeTime = startFadeTime;
        this.endFadeTime = endFadeTime;
        this.sampleDelay = sampleDelay;
        this.sampleName = sampleName;
        this.pointer = pointer;
        this.push = push;

        Generate(sentences);
    }

    public void Generate(string[] text)
    {
        // convert text to string lines
        List<string> Sentences = new List<string>(text);
        // string[] text = sentences.Split('|');

        var position = new Position();
        var dialogTiming = new DialogTiming();
        dialogTiming.startTime = startTime;
        dialogTiming.endTime = endTime;
        position.position = new Vector2(x, y);
        var dialog = new DialogText(generator, layerText, font, textColor, position, dialogTiming, textFade, startFadeTime, endFadeTime, fontSize, originCentre);

        // write sentences
        foreach (string line in Sentences)
        {
            dialog.lines.Add(line);
        }
        if (showBox)
        {
            dialog.calculateLineWidth();
            dialog.calculateLineHeight();
            dialog.Generate();
            var dialogOne = new DialogBoxes(generator, layerBox, sampleDelay, sampleName, boxColor, dialogTiming, (fontSize * 0.08f) - 1, boxFade,
            position, pointer, push, originCentre,
            dialog.GetLineWidth(), dialog.heightSpace());
        }

        else
        {
            dialog.calculateLineWidth();
            dialog.calculateLineHeight();
            dialog.Generate();
            var dialogOne = new DialogBoxes(generator, layerBox, sampleDelay, sampleName, boxColor, dialogTiming, (fontSize * 0.08f) - 1, 0f,
            position, pointer, push, originCentre,
            dialog.GetLineWidth(), dialog.heightSpace());
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
    private int startFadeTime;
    private int endFadeTime;
    private int fontSize;
    private float delay = 0.5f;
    private Position position;
    private Color4 Color;
    private float lineWidth;
    private float lineHeight;
    private float textScale = 0.5f;
    private bool centre = true;
    private string layerName;

    public List<String> lines = new List<String>();

    public DialogText(StoryboardObjectGenerator generator, string layerName, FontGenerator font, Color4 Color, Position position, DialogTiming timing, float fade, int startFadeTime, int endFadeTime, int fontSize, bool centre)
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

    public void Generate()
    {
        // float newLineSpacing = 0;
        float letterY = position.position.Y - 4;

        foreach (var line in lines)
        {
            // float lineWidth = 0;
            // float lineHeight = 0;
            // float letterSpacing = 4f * this.textScale;
            float i = 0;
            var FadeTime = startFadeTime;

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

                    var sprite = generator.GetLayer(layerName).CreateSprite(texture.Path, OsbOrigin.Centre, letterPos);
                    sprite.Fade(startTime + delay * i, startTime + (FadeTime * 1.5) + delay * i, 0, fade);
                    sprite.Fade(endTime - endFadeTime, endTime, fade, 0);
                    sprite.Scale(startTime, this.textScale);
                    sprite.Color(startTime, Color);
                }
                letterX += texture.BaseWidth * this.textScale;
                startTime += delay * i;
                i++;
            }
            // newLineSpacing += lineHeight + 2;
            letterY += this.textScale * lineHeightSpace() / textScale;
        }
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

    public DialogBoxes(StoryboardObjectGenerator generator, string layerName, int soundDelay, string soundEffect, Color4 Color, DialogTiming timing,
           float pointerScale, float Fade, Position position, Pointer pointer, Push push, bool centre, float lineWidth, float lineHeight)
    {
        var d = 300;
        var fadeTime = 500;
        var PushValue = 30;
        var biggerBox = 10;
        var boxPos = centre ? new Vector2(position.position.X - lineWidth * 0.5f - (biggerBox / 2), position.position.Y - (biggerBox / 2)) :
                                          new Vector2(position.position.X - (biggerBox / 2), position.position.Y - (biggerBox / 2));

        var layer = generator.GetLayer(layerName);

        var inputBox = layer.CreateSprite("sb/pixel.png", OsbOrigin.TopLeft, boxPos);
        var bitmapInputBox = generator.GetMapsetBitmap("sb/pixel.png");
        var widthInputBox = (lineWidth / bitmapInputBox.Width) + biggerBox;
        var heightInputBox = (lineHeight / bitmapInputBox.Height) + biggerBox;
        var bitmapPointer = generator.GetMapsetBitmap("sb/dialog/pointers/pointer.png");
        var bitmapPointerCorner = generator.GetMapsetBitmap("sb/dialog/pointers/pointerCorner.png");

        // sfx - message-1
        var message1 = layer.CreateSample(soundEffect, timing.startTime - d + soundDelay, 60);
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

        // start style
        if (pointer == Pointer.Up)
        {
            inputBox.Color(timing.startTime - d, Color);
            inputBox.ScaleVec(timing.startTime - d, widthInputBox, heightInputBox);
            inputBox.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
            inputBox.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

            var pointerPos = new Vector2(boxPos.X + (lineWidth / 2), boxPos.Y);
            var point = layer.CreateSprite("sb/dialog/pointers/pointer.png", OsbOrigin.BottomCentre, pointerPos);

            point.Color(timing.startTime - d, Color);
            point.Scale(timing.startTime, pointerScale);
            point.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
            point.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

            if (push == Push.None)
            {
                point.MoveX(timing.startTime - d, pointerPos.X);
                point.MoveY(timing.endTime + d, pointerPos.Y);
            }

            if (push == Push.Up)
            {
                inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y + PushValue, boxPos.Y);
                inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y - PushValue);
                point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y + PushValue, pointerPos.Y);
                point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y - PushValue);
            }

            if (push == Push.Down)
            {
                inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y - PushValue, boxPos.Y);
                inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y + PushValue);
                point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y - PushValue, pointerPos.Y);
                point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y + PushValue);
            }

            if (push == Push.Left)
            {
                inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X + PushValue, boxPos.X);
                inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X - PushValue);
                point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X + PushValue, pointerPos.X);
                point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X - PushValue);
            }

            if (push == Push.Right)
            {
                inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X - PushValue, boxPos.X);
                inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X + PushValue);
                point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X - PushValue, pointerPos.X);
                point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X + PushValue);
            }
        }
        // end style

        // start style
        if (pointer == Pointer.Down)
        {
            inputBox.Color(timing.startTime - d, Color);
            inputBox.ScaleVec(timing.startTime - d, widthInputBox, heightInputBox);
            inputBox.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
            inputBox.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

            var pointerPos = new Vector2(boxPos.X + (lineWidth / 2), boxPos.Y + heightInputBox);
            var point = layer.CreateSprite("sb/dialog/pointers/pointer.png", OsbOrigin.BottomCentre, pointerPos);

            point.Rotate(timing.startTime, MathHelper.DegreesToRadians(180));
            point.Scale(timing.startTime, pointerScale);
            point.Color(timing.startTime - d, Color);
            point.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
            point.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

            if (push == Push.None)
            {
                point.MoveX(timing.startTime - d, pointerPos.X);
                point.MoveY(timing.endTime + d, pointerPos.Y);
            }

            if (push == Push.Up)
            {
                inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y + PushValue, boxPos.Y);
                inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y - PushValue);
                point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y + PushValue, pointerPos.Y);
                point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y - PushValue);
            }

            if (push == Push.Down)
            {
                inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y - PushValue, boxPos.Y);
                inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y + PushValue);
                point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y - PushValue, pointerPos.Y);
                point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y + PushValue);
            }

            if (push == Push.Left)
            {
                inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X + PushValue, boxPos.X);
                inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X - PushValue);
                point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X + PushValue, pointerPos.X);
                point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X - PushValue);
            }

            if (push == Push.Right)
            {
                inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X - PushValue, boxPos.X);
                inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X + PushValue);
                point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X - PushValue, pointerPos.X);
                point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X + PushValue);
            }
        }
        // end style

        // start style
        if (pointer == Pointer.CentreLeft)
        {
            inputBox.Color(timing.startTime - d, Color);
            inputBox.ScaleVec(timing.startTime - d, widthInputBox, heightInputBox);
            inputBox.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
            inputBox.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

            var pointerPos = new Vector2(boxPos.X, boxPos.Y + (heightInputBox / 2));
            var point = layer.CreateSprite("sb/dialog/pointers/pointer.png", OsbOrigin.BottomCentre, pointerPos);

            point.Rotate(timing.startTime, MathHelper.DegreesToRadians(-90));
            point.Scale(timing.startTime, pointerScale);
            point.Color(timing.startTime - d, Color);
            point.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
            point.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

            if (push == Push.None)
            {
                point.MoveX(timing.startTime - d, pointerPos.X);
                point.MoveY(timing.endTime + d, pointerPos.Y);
            }

            if (push == Push.Up)
            {
                inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y + PushValue, boxPos.Y);
                inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y - PushValue);
                point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y + PushValue, pointerPos.Y);
                point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y - PushValue);
            }

            if (push == Push.Down)
            {
                inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y - PushValue, boxPos.Y);
                inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y + PushValue);
                point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y - PushValue, pointerPos.Y);
                point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y + PushValue);
            }

            if (push == Push.Left)
            {
                inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X + PushValue, boxPos.X);
                inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X - PushValue);
                point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X + PushValue, pointerPos.X);
                point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X - PushValue);
            }

            if (push == Push.Right)
            {
                inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X - PushValue, boxPos.X);
                inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X + PushValue);
                point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X - PushValue, pointerPos.X);
                point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X + PushValue);
            }
        }
        // end style

        // start style
        if (pointer == Pointer.CentreRight)
        {
            inputBox.Color(timing.startTime - d, Color);
            inputBox.ScaleVec(timing.startTime - d, widthInputBox, heightInputBox);
            inputBox.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
            inputBox.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

            var pointerPos = new Vector2(boxPos.X + widthInputBox, boxPos.Y + (heightInputBox / 2));
            var point = layer.CreateSprite("sb/dialog/pointers/pointer.png", OsbOrigin.BottomCentre, pointerPos);

            point.Rotate(timing.startTime, MathHelper.DegreesToRadians(90));
            point.Scale(timing.startTime, pointerScale);
            point.Color(timing.startTime - d, Color);
            point.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
            point.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

            if (push == Push.None)
            {
                point.MoveX(timing.startTime - d, pointerPos.X);
                point.MoveY(timing.endTime + d, pointerPos.Y);
            }

            if (push == Push.Up)
            {
                inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y + PushValue, boxPos.Y);
                inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y - PushValue);
                point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y + PushValue, pointerPos.Y);
                point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y - PushValue);
            }

            if (push == Push.Down)
            {
                inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y - PushValue, boxPos.Y);
                inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y + PushValue);
                point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y - PushValue, pointerPos.Y);
                point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y + PushValue);
            }

            if (push == Push.Left)
            {
                inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X + PushValue, boxPos.X);
                inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X - PushValue);
                point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X + PushValue, pointerPos.X);
                point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X - PushValue);
            }

            if (push == Push.Right)
            {
                inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X - PushValue, boxPos.X);
                inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X + PushValue);
                point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X - PushValue, pointerPos.X);
                point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X + PushValue);
            }
        }
        // end style

        // start style
        if (pointer == Pointer.TopLeft)
        {
            inputBox.Color(timing.startTime - d, Color);
            inputBox.ScaleVec(timing.startTime - d, widthInputBox, heightInputBox);
            inputBox.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
            inputBox.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

            var pointerPos = new Vector2(boxPos.X + (bitmapPointerCorner.Height / 4) + 0.5f, boxPos.Y + (bitmapPointerCorner.Height / 4) + 0.5f);
            var point = layer.CreateSprite("sb/dialog/pointers/pointerCorner.png", OsbOrigin.BottomCentre, pointerPos);

            point.Rotate(timing.startTime, MathHelper.DegreesToRadians(-45));
            point.Scale(timing.startTime, pointerScale);
            point.Color(timing.startTime - d, Color);
            point.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
            point.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

            if (push == Push.None)
            {
                point.MoveX(timing.startTime - d, pointerPos.X);
                point.MoveY(timing.endTime + d, pointerPos.Y);
            }

            if (push == Push.Up)
            {
                inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y + PushValue, boxPos.Y);
                inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y - PushValue);
                point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y + PushValue, pointerPos.Y);
                point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y - PushValue);
            }

            if (push == Push.Down)
            {
                inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y - PushValue, boxPos.Y);
                inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y + PushValue);
                point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y - PushValue, pointerPos.Y);
                point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y + PushValue);
            }

            if (push == Push.Left)
            {
                inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X + PushValue, boxPos.X);
                inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X - PushValue);
                point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X + PushValue, pointerPos.X);
                point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X - PushValue);
            }

            if (push == Push.Right)
            {
                inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X - PushValue, boxPos.X);
                inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X + PushValue);
                point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X - PushValue, pointerPos.X);
                point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X + PushValue);
            }
        }
        // end style

        // start style
        if (pointer == Pointer.TopRight)
        {
            inputBox.Color(timing.startTime - d, Color);
            inputBox.ScaleVec(timing.startTime - d, widthInputBox, heightInputBox);
            inputBox.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
            inputBox.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

            var pointerPos = new Vector2(boxPos.X + widthInputBox - (bitmapPointerCorner.Height / 4) - 0.5f, boxPos.Y + (bitmapPointerCorner.Height / 4) + 0.5f);
            var point = layer.CreateSprite("sb/dialog/pointers/pointerCorner.png", OsbOrigin.BottomCentre, pointerPos);

            point.Rotate(timing.startTime, MathHelper.DegreesToRadians(45));
            point.Scale(timing.startTime, pointerScale);
            point.Color(timing.startTime - d, Color);
            point.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
            point.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

            if (push == Push.None)
            {
                point.MoveX(timing.startTime - d, pointerPos.X);
                point.MoveY(timing.endTime + d, pointerPos.Y);
            }

            if (push == Push.Up)
            {
                inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y + PushValue, boxPos.Y);
                inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y - PushValue);
                point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y + PushValue, pointerPos.Y);
                point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y - PushValue);
            }

            if (push == Push.Down)
            {
                inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y - PushValue, boxPos.Y);
                inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y + PushValue);
                point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y - PushValue, pointerPos.Y);
                point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y + PushValue);
            }

            if (push == Push.Left)
            {
                inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X + PushValue, boxPos.X);
                inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X - PushValue);
                point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X + PushValue, pointerPos.X);
                point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X - PushValue);
            }

            if (push == Push.Right)
            {
                inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X - PushValue, boxPos.X);
                inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X + PushValue);
                point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X - PushValue, pointerPos.X);
                point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X + PushValue);
            }
        }
        // end style

        // start style
        if (pointer == Pointer.BottomLeft)
        {
            inputBox.Color(timing.startTime - d, Color);
            inputBox.ScaleVec(timing.startTime - d, widthInputBox, heightInputBox);
            inputBox.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
            inputBox.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

            var pointerPos = new Vector2(boxPos.X + (bitmapPointerCorner.Height / 4) + 0.5f, boxPos.Y + heightInputBox - (bitmapPointerCorner.Height / 4) - 0.5f);
            var point = layer.CreateSprite("sb/dialog/pointers/pointerCorner.png", OsbOrigin.BottomCentre, pointerPos);

            point.Rotate(timing.startTime, MathHelper.DegreesToRadians(-135));
            point.Scale(timing.startTime, pointerScale);
            point.Color(timing.startTime - d, Color);
            point.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
            point.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

            if (push == Push.None)
            {
                point.MoveX(timing.startTime - d, pointerPos.X);
                point.MoveY(timing.endTime + d, pointerPos.Y);
            }

            if (push == Push.Up)
            {
                inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y + PushValue, boxPos.Y);
                inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y - PushValue);
                point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y + PushValue, pointerPos.Y);
                point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y - PushValue);
            }

            if (push == Push.Down)
            {
                inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y - PushValue, boxPos.Y);
                inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y + PushValue);
                point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y - PushValue, pointerPos.Y);
                point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y + PushValue);
            }

            if (push == Push.Left)
            {
                inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X + PushValue, boxPos.X);
                inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X - PushValue);
                point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X + PushValue, pointerPos.X);
                point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X - PushValue);
            }

            if (push == Push.Right)
            {
                inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X - PushValue, boxPos.X);
                inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X + PushValue);
                point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X - PushValue, pointerPos.X);
                point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X + PushValue);
            }
        }
        // end style

        // start style
        if (pointer == Pointer.BottomRight)
        {
            inputBox.Color(timing.startTime - d, Color);
            inputBox.ScaleVec(timing.startTime - d, widthInputBox, heightInputBox);
            inputBox.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
            inputBox.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

            var pointerPos = new Vector2(boxPos.X + widthInputBox - (bitmapPointerCorner.Height / 4) - 0.5f, boxPos.Y + heightInputBox - (bitmapPointerCorner.Height / 4) - 0.5f);
            var point = layer.CreateSprite("sb/dialog/pointers/pointerCorner.png", OsbOrigin.BottomCentre, pointerPos);

            point.Rotate(timing.startTime, MathHelper.DegreesToRadians(135));
            point.Scale(timing.startTime, pointerScale);
            point.Color(timing.startTime - d, Color);
            point.Fade(timing.startTime - d, timing.startTime - d + 200, 0, Fade);
            point.Fade(timing.endTime + d - fadeTime, timing.endTime + d, Fade, 0);

            if (push == Push.None)
            {
                point.MoveX(timing.startTime - d, pointerPos.X);
                point.MoveY(timing.endTime + d, pointerPos.Y);
            }

            if (push == Push.Up)
            {
                inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y + PushValue, boxPos.Y);
                inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y - PushValue);
                point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y + PushValue, pointerPos.Y);
                point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y - PushValue);
            }

            if (push == Push.Down)
            {
                inputBox.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.Y - PushValue, boxPos.Y);
                inputBox.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.Y, boxPos.Y + PushValue);
                point.MoveY(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.Y - PushValue, pointerPos.Y);
                point.MoveY(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.Y, pointerPos.Y + PushValue);
            }

            if (push == Push.Left)
            {
                inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X + PushValue, boxPos.X);
                inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X - PushValue);
                point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X + PushValue, pointerPos.X);
                point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X - PushValue);
            }

            if (push == Push.Right)
            {
                inputBox.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, boxPos.X - PushValue, boxPos.X);
                inputBox.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), boxPos.X, boxPos.X + PushValue);
                point.MoveX(OsbEasing.OutBack, timing.startTime - d, timing.startTime - d + 400, pointerPos.X - PushValue, pointerPos.X);
                point.MoveX(OsbEasing.OutSine, timing.endTime + (d * 2) - 400, timing.endTime + (d * 2), pointerPos.X, pointerPos.X + PushValue);
            }
        }
        // end style
    }
}
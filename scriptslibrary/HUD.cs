using OpenTK;
using System;
using System.Linq;
using System.Drawing;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;
using System.Collections.Generic;

public class HUD
{
    //This is the parameters we pass in the class, these lines are used to make them available to use everywhere in the class.
    private StoryboardObjectGenerator generator;
    private int startTime;
    private int endTime;
    private int loadingTextEndtime;
    private int FadeTime = 5000;
    private string mission;
    private string songName;
    private string nameTag;
    private int progressBarDelay;
    private string avatar;
    private OsbSprite sprite;

    public HUD(StoryboardObjectGenerator generator, int startTime, int endTime, int loadingTextEndtime, string mission, string songName, string nameTag, int progressBarDelay, string avatar)
    {
        //And this pack of lines are just the way we set our local variable with the parameters values of the constructor.
        this.generator = generator;
        this.startTime = startTime;
        this.endTime = endTime;
        this.loadingTextEndtime = loadingTextEndtime;
        this.mission = mission;
        this.songName = songName;
        this.nameTag = nameTag;
        this.progressBarDelay = progressBarDelay;
        this.avatar = avatar;

        Overlay();
        Loading();
        Playfield();
        Performance();
        LoadingText(startTime, endTime, loadingTextEndtime);
        DialogHUD(startTime, endTime, mission, songName);
        ProgressBar(nameTag, avatar, startTime + progressBarDelay, endTime);
    }

    public void Overlay()
    {
        var bitmap = generator.GetMapsetBitmap("sb/HUD/overlay.png");
        var sprite = generator.GetLayer("HUD").CreateSprite("sb/HUD/overlay.png", OsbOrigin.Centre);

        sprite.Scale(OsbEasing.OutSine, startTime - FadeTime, startTime, 520.0f / bitmap.Height, 480.0f / bitmap.Height);
        sprite.Fade(startTime - FadeTime, startTime, 0, 0.2f);
        sprite.Fade(endTime, endTime + FadeTime, 0.2f, 0);
        sprite.Additive(startTime, endTime);

        var duration = 15000;
        var loopCount = (endTime - startTime) / duration;
        for (var i = startTime; i < startTime + duration; i += 500)
        {
            var particleLeft = generator.GetLayer("HUD").CreateSprite("sb/pixel.png", OsbOrigin.Centre);
            var particleRight = generator.GetLayer("HUD").CreateSprite("sb/pixel.png", OsbOrigin.Centre);

            var pad = 10;
            var startX = new Vector2(-98, 12);
            var startY = new Vector2(0, 240);
            var endX = new Vector2(-98, 26);
            var endY = new Vector2(240, 436);
            var rotation = MathHelper.DegreesToRadians(90);

            particleLeft.StartLoopGroup(i, loopCount);
            particleLeft.ScaleVec(OsbEasing.Out, 0, duration / 2, 1, 0, 1, 7);
            particleLeft.ScaleVec(OsbEasing.In, duration / 2, duration, 1, 7, 1, 0);
            // first half
            particleLeft.MoveX(OsbEasing.Out, 0, duration / 2, startX.Y + pad, startX.X + pad);
            particleLeft.MoveY(0, duration / 2, startY.X, startY.Y);
            // second half
            particleLeft.MoveX(OsbEasing.InQuad, duration / 2, duration, endX.X + pad, endX.Y + pad);
            particleLeft.MoveY(duration / 2, duration, endY.X, endY.Y);
            particleLeft.EndGroup();
            particleLeft.Fade(i, 0.2f);
            particleLeft.Rotate(i, rotation);
            particleLeft.Additive(i, i + duration);

            var startX2 = new Vector2(738, 628);
            var startY2 = new Vector2(0, 240);
            var endX2 = new Vector2(738, 614);
            var endY2 = new Vector2(240, 436);
            particleRight.StartLoopGroup(i, loopCount);
            particleRight.ScaleVec(OsbEasing.Out, 0, duration / 2, 1, 0, 1, 7);
            particleRight.ScaleVec(OsbEasing.In, duration / 2, duration, 1, 7, 1, 0);
            // first half
            particleRight.MoveX(OsbEasing.Out, 0, duration / 2, startX2.Y - pad, startX2.X - pad);
            particleRight.MoveY(0, duration / 2, startY2.X, startY2.Y);
            // second half
            particleRight.MoveX(OsbEasing.InQuad, duration / 2, duration, endX2.X - pad, endX2.Y - pad);
            particleRight.MoveY(duration / 2, duration, endY2.X, endY2.Y);
            particleRight.EndGroup();
            particleRight.Fade(i, 0.2f);
            particleRight.Rotate(i, rotation);
            particleRight.Additive(i, i + duration);
        }

        var duration2 = 6500;
        var loopCount2 = (endTime - startTime) / duration2;
        for (var i = startTime; i < startTime + duration2; i += 1000)
        {
            var moveBy = 61;
            var posLeft = new Vector2(-25, 240);
            var posRight = new Vector2(665, 240);
            var bitmapLines = generator.GetMapsetBitmap("sb/HUD/line.png");
            var sideLinesLeft = generator.GetLayer("HUD").CreateSprite("sb/HUD/line.png", OsbOrigin.CentreLeft);
            var sideLinesRight = generator.GetLayer("HUD").CreateSprite("sb/HUD/line.png", OsbOrigin.CentreRight);

            sideLinesLeft.StartLoopGroup(i, loopCount2);
            sideLinesLeft.MoveY(0, duration2, posLeft.Y, posLeft.Y);
            sideLinesLeft.MoveX(OsbEasing.OutCirc, 0, duration2, posLeft.X, posLeft.X - moveBy);
            sideLinesLeft.Scale(OsbEasing.OutCirc, 0, duration2, 370.0f / bitmapLines.Height, 458.0f / bitmapLines.Height);
            sideLinesLeft.Fade(0, duration2 / 4, 0, 0.15f);
            sideLinesLeft.Fade(duration2 - duration2 / 2, duration2, 0.15f, 0);
            sideLinesLeft.Additive(0, duration2);
            sideLinesLeft.EndGroup();

            sideLinesRight.StartLoopGroup(i, loopCount2);
            sideLinesRight.FlipH(0, duration2);
            sideLinesRight.MoveY(0, duration2, posRight.Y, posRight.Y);
            sideLinesRight.MoveX(OsbEasing.OutCirc, 0, duration2, posRight.X, posRight.X + moveBy);
            sideLinesRight.Scale(OsbEasing.OutCirc, 0, duration2, 370.0f / bitmapLines.Height, 458.0f / bitmapLines.Height);
            sideLinesRight.Fade(0, duration2 / 4, 0, 0.15f);
            sideLinesRight.Fade(duration2 - duration2 / 2, duration2, 0.15f, 0);
            sideLinesRight.Additive(0, duration2);
            sideLinesRight.EndGroup();
        }
    }

    public void Loading()
    {
        // Spiral
        int i = 0;
        int b = 0;
        int duration;
        int Amount = 40; // less = more
        int Length = 20;
        float Size = 3;
        Vector2 Position = new Vector2(320, 240);

        double[] x = new double[1000];
        double[] y = new double[1000];
        for (int t = Length * 360; t > 0; t -= Amount)
        {
            var angle = t * Math.PI / 450;
            x[i] = (Length / 2) * Size * Math.Cos(angle) + Position.X;
            y[i] = (Length / 2) * Size * Math.Sin(angle) + Position.Y;
            b++;
            i++;
        }

        int FadeTime2 = 700;
        int EndTime = startTime + 10000;
        duration = (EndTime - startTime) / b;
        for (i = 0; i < b; i++)
        {
            var sprite = generator.GetLayer("HUD").CreateSprite("sb/pixel.png", OsbOrigin.Centre);

            sprite.ScaleVec(startTime, 7, 1);
            sprite.Fade(startTime - (FadeTime2 / 10), startTime, 0, 0.2f);
            sprite.Fade(startTime, startTime + FadeTime2, 0.5f, 0);

            var rotation = Math.Atan2((Position.Y - y[i]), (Position.X - x[i])) - Math.PI / 2f;
            sprite.Move(startTime, x[i], y[i]);
            sprite.Rotate(startTime, rotation);
            // sprite.Additive(startTime, EndTime);

            startTime += duration;
        }


        // Circle
        var StartTime = startTime - 9900;
        var circleBitmap = generator.GetMapsetBitmap("sb/HUD/circle.png");
        var circle = generator.GetLayer("HUD").CreateSprite("sb/HUD/circle.png", OsbOrigin.Centre);

        var circleFade = 0.1f;

        circle.Color(StartTime, Color4.Black);
        circle.Scale(OsbEasing.OutElasticQuarter, StartTime, StartTime + 500, (53.0f / circleBitmap.Width) / 4, 53.0f / circleBitmap.Width);
        circle.Scale(OsbEasing.OutElasticQuarter, EndTime - 200 - 500, EndTime, 53.0f / circleBitmap.Width, (53.0f / circleBitmap.Width) / 4);

        circle.StartLoopGroup(StartTime, 4);
        circle.Fade(0, 500 / 4, 0, circleFade);
        circle.EndGroup();
        circle.StartLoopGroup(EndTime - 200, 6);
        circle.Fade(0, FadeTime2 / 6, 0, circleFade);
        circle.EndGroup();

        circle.Fade(StartTime + 500, EndTime - 200, circleFade, circleFade);

        // generator.Log((StartTime).ToString());

        // Timer
        var Scale = 0.01f;
        var FadeDelay = 0;
        var Number = 100;
        var Reverse = false;
        var countDelay = 104;
        var LetterSpacing = 5;

        string numbString = Number.ToString();
        int[] intArray = new int[numbString.Length];
        for (int t = 0; t < numbString.Length; t++)
            intArray[t] = int.Parse(numbString[t].ToString());

        var frameDelay = Math.Pow(countDelay, intArray.Length - 1);

        for (int t = 0; t < intArray.Length; t++)
        {
            var position = new Vector2((Position.X - ((LetterSpacing * 3) - LetterSpacing) + (LetterSpacing * t)), Position.Y);
            var timer = generator.GetLayer("HUD").CreateAnimation(Reverse ? "sb/HUD/timer/reverse/counter.png" :
                                "sb/HUD/timer/counter.png", 10, frameDelay, OsbLoopType.LoopForever, OsbOrigin.Centre,
                                position + new Vector2(LetterSpacing * t, 0));

            timer.Fade(StartTime, StartTime + FadeDelay, 0, 0.5f);
            timer.Fade(EndTime - FadeDelay, EndTime, 0.5f, 0);
            timer.Scale(Reverse ? StartTime : StartTime - (Math.Pow(10, intArray.Length) - Number), Scale);

            // end animation
            timer.StartLoopGroup(EndTime, 3);
            timer.Fade(0, ((countDelay * intArray[t]) * 4), 0.5f, 0);
            timer.EndGroup();

            frameDelay /= 10;
        }
        // end animation
        var endTimer0 = generator.GetLayer("HUD").CreateSprite("sb/HUD/timer/counter0.png", OsbOrigin.Centre);
        var endTimer1 = generator.GetLayer("HUD").CreateSprite("sb/HUD/timer/counter0.png", OsbOrigin.Centre);

        endTimer0.StartLoopGroup(EndTime, 3);
        endTimer0.Fade(0, ((countDelay * intArray[0]) * 4), 0.5f, 0);
        endTimer0.EndGroup();
        endTimer0.Scale(EndTime, Scale);
        endTimer0.Move(EndTime, EndTime + 200, 320, 240, 320, 240);

        endTimer1.StartLoopGroup(EndTime, 3);
        endTimer1.Fade(0, ((countDelay * intArray[0]) * 4), 0.5f, 0);
        endTimer1.EndGroup();
        endTimer1.Scale(EndTime, Scale);
        endTimer1.Move(EndTime, EndTime + 200, 320 + 10, 240, 320 + 10, 240);

        // lines
        var line = generator.GetLayer("HUD").CreateSprite("sb/pixel.png", OsbOrigin.CentreLeft);
        var line2 = generator.GetLayer("HUD").CreateSprite("sb/pixel.png", OsbOrigin.CentreRight);

        line.ScaleVec(OsbEasing.Out, EndTime - 2500, EndTime, 0, 1, 310, 1);
        line.Move(EndTime, -23, 240);
        line.Fade(EndTime - 2500, EndTime, 0.2f, 0.2f);
        line.StartLoopGroup(EndTime, 8);
        line.Fade(0, 100, 0.2f, 0);
        line.EndGroup();

        line2.ScaleVec(OsbEasing.Out, EndTime - 2500, EndTime, 0, 1, 310, 1);
        line2.Move(EndTime, 663, 240);
        line2.Fade(EndTime - 2500, EndTime, 0.2f, 0.2f);
        line2.StartLoopGroup(EndTime, 8);
        line2.Fade(0, 100, 0.2f, 0);
        line2.EndGroup();
    }

    public void Performance()
    {
        var text = generator.GetLayer("Performance").CreateSprite("sb/HUD/txt/badPerformance.png", OsbOrigin.CentreRight);

        var sectionDuration = endTime - startTime;

        text.Scale(startTime, 0.2f);
        text.Move(startTime, 620, 328);

        text.StartLoopGroup(startTime, sectionDuration / 1000);
        text.Fade(OsbEasing.Out, 0, 500, 0, 0.3);
        text.Fade(OsbEasing.In, 500, 1000, 0.3, 0);
        text.EndGroup();


        var bitmap = generator.GetMapsetBitmap("sb/HUD/stripe.jpg");
        var stripe = generator.GetLayer("Performance").CreateSprite("sb/HUD/stripe.jpg", OsbOrigin.Centre, new Vector2(320, 240));

        stripe.Color(startTime, Color4.Red);
        stripe.Scale(startTime, 480.0f / bitmap.Height);
        stripe.Additive(startTime, endTime);
        
        var fadeTime = 1000;

        stripe.StartLoopGroup(startTime, sectionDuration / (fadeTime * 3));
        stripe.Fade(0, fadeTime, 0.2f, 0.5f);
        stripe.Fade(fadeTime * 2, fadeTime * 3, 0.5f, 0.2f);
        stripe.EndGroup();
    }

    public void Playfield()
    {
        var pos = new Vector2(320, 240);
        var bitmap = generator.GetMapsetBitmap("sb/HUD/playfield.png");
        var sprite = generator.GetLayer("HUD").CreateSprite("sb/HUD/playfield.png", OsbOrigin.Centre, pos);

        sprite.Scale(startTime, 854.0f / bitmap.Width);
        sprite.Fade(startTime, startTime + 1000, 0, 0.05f);
        sprite.Fade(endTime - 1000, endTime, 0.05f, 0);
        // sprite.Color(startTime, Color4.Black);
        // sprite.Additive(startTime, endTime);
    }

    public void ProgressBar(string NameTag, string Avatar, int StartTime, int EndTime)
    {
        var pb = generator.GetLayer("Progress Bar").CreateSprite("sb/pixel.png", OsbOrigin.TopLeft);
        var pbOverLay = generator.GetLayer("Progress Bar").CreateSprite("sb/HUD/progressBar.png", OsbOrigin.BottomCentre);
        var pbProfile = generator.GetLayer("Progress Bar").CreateSprite("sb/HUD/progressBar_profile.png", OsbOrigin.TopCentre);
        var profile = generator.GetLayer("Progress Bar").CreateSprite(Avatar, OsbOrigin.BottomCentre);
        var nameTag = generator.GetLayer("Progress Bar").CreateSprite(NameTag, OsbOrigin.TopLeft);
        var circleRight = generator.GetLayer("Progress Bar").CreateSprite("sb/particle2.png", OsbOrigin.Centre);
        var circleLeft = generator.GetLayer("Progress Bar").CreateSprite("sb/particle2.png", OsbOrigin.Centre);

        var duration = EndTime - StartTime;
        pb.MoveX(StartTime, 200);
        pb.Fade(StartTime, 0.1f);
        pb.Color(StartTime, Color4.White);
        pb.Additive(StartTime, EndTime + FadeTime);
        pb.MoveY(OsbEasing.Out, StartTime, StartTime + 1500, -26, 24);
        pb.ScaleVec(OsbEasing.InOutSine, loadingTextEndtime, loadingTextEndtime + duration / 2, 0, 22.05f, 286.5 / 2, 22.05f);
        pb.ScaleVec(OsbEasing.InOutSine, loadingTextEndtime + duration / 2, EndTime - 2000, 286.5 / 2, 22.05f, 286.5, 22.05f);
        pb.StartLoopGroup(EndTime - 2000, 5);
        pb.Fade(OsbEasing.Out, 0, 2000 / 5, 0.15f, 0.1f);
        pb.EndGroup();
        pb.StartLoopGroup(EndTime - 2000, 4);
        pb.Color(0, 2000 / 5, Color4.IndianRed, Color4.White);
        pb.EndGroup();
        pb.Fade(EndTime, EndTime + FadeTime, 0.1f, 0);
        pb.Color(EndTime - (2000 / 5), Color4.IndianRed);

        pbOverLay.MoveX(StartTime, 320);
        pbOverLay.Scale(StartTime, 0.45f);
        pbOverLay.Additive(StartTime, EndTime + FadeTime);
        pbOverLay.MoveY(OsbEasing.Out, StartTime, StartTime + 1500, -10, 50);
        pbOverLay.Fade(StartTime, StartTime + 1000, 0, 0.2f);
        pbOverLay.Fade(EndTime, EndTime + FadeTime, 0.2f, 0);

        pbProfile.MoveX(StartTime, 320);
        pbProfile.Scale(StartTime, 0.45f);
        pbProfile.Additive(StartTime, EndTime + FadeTime);
        pbProfile.MoveY(OsbEasing.Out, StartTime, StartTime + 1500, -10, 50);
        pbProfile.Fade(StartTime, StartTime + 1000, 0, 0.2f);
        pbProfile.Fade(EndTime, EndTime + FadeTime, 0.2f, 0);

        profile.MoveX(StartTime + 1500, 320);
        profile.MoveY(StartTime + 1500, 120);
        profile.Scale(StartTime + 1500, 0.25f);
        profile.StartLoopGroup(StartTime + 1500, 4);
        profile.Fade(0, 100, 0, 0.4f);
        profile.EndGroup();
        profile.Fade(StartTime + 1500 + (100 * 4), EndTime, 0.4f, 0.4f);
        profile.Fade(EndTime, EndTime + FadeTime, 0.4f, 0);

        nameTag.MoveX(StartTime + 2500, 320);
        nameTag.MoveY(StartTime + 2500, 50);
        nameTag.ScaleVec(OsbEasing.OutSine, StartTime + 2500, StartTime + 5000, 0.45f, 0f, 0.45f, 0.45f);
        nameTag.Fade(OsbEasing.In, StartTime + 2500, StartTime + 3500, 0, 0.4f);
        nameTag.Fade(EndTime, EndTime + FadeTime, 0.4f, 0);


        // small circles following line
        var speed = 4;
        var delay = 2500 / speed;
        var Duration = (loadingTextEndtime + 6700 / (speed / 2) + speed + delay) - (loadingTextEndtime + 2500 / speed + delay);
        var loopCount = (EndTime - loadingTextEndtime) / Duration;

        circleRight.StartLoopGroup(loadingTextEndtime, loopCount);
        // right
        circleRight.Fade(2500 / speed + delay, 0.5);
        circleRight.Scale(OsbEasing.In, 2500 / speed + delay, 3000 / speed + delay, 0, 0.03f);
        // x
        circleRight.MoveX(OsbEasing.None, 2500 / speed + delay, 3000 / speed + delay, 523, 490); // line 1 short
        circleRight.MoveX(OsbEasing.None, 3000 / speed + delay, 5500 / speed + delay, 490, 320); // line 2 long
        circleRight.MoveX(OsbEasing.None, 5700 / speed + delay, 6700 / speed + delay, 320, 361); // line 4 curve
        // y
        circleRight.MoveY(OsbEasing.None, 2500 / speed + delay, 3000 / speed + delay, 25, 49); // line 1 short
        circleRight.MoveY(OsbEasing.None, 5500 / speed + delay, 5650 / speed + delay, 49, 57); // line 3 shortest
        circleRight.MoveY(OsbEasing.InCubic, 5650 / speed + delay, 6700 / speed + delay, 57, 90); // line 4 curve
        //
        circleRight.Scale(OsbEasing.InCubic, 5650 / speed + delay, 6700 / speed + delay, 0.03f, 0.01f);
        circleRight.Fade(6700 / speed + delay, 6700 / speed + delay + 1, 0.5, 0);
        circleRight.Fade(6700 / speed + delay + 1, 6700 / (speed / 2) + speed + delay, 0, 0);
        //
        circleRight.EndGroup();

        circleLeft.StartLoopGroup(loadingTextEndtime, loopCount);
        // left
        circleLeft.Fade(2500 / speed + delay, 0.5);
        circleLeft.Scale(OsbEasing.In, 2500 / speed + delay, 3000 / speed + delay, 0, 0.03f);
        // x
        circleLeft.MoveX(OsbEasing.None, 2500 / speed + delay, 3000 / speed + delay, 117, 150); // line 1 short
        circleLeft.MoveX(OsbEasing.None, 3000 / speed + delay, 5500 / speed + delay, 150, 320); // line 2 long
        circleLeft.MoveX(OsbEasing.None, 5700 / speed + delay, 6700 / speed + delay, 320, 279); // line 4 curve
        // y
        circleLeft.MoveY(OsbEasing.None, 2500 / speed + delay, 3000 / speed + delay, 25, 49); // line 1 short
        circleLeft.MoveY(OsbEasing.None, 5500 / speed + delay, 5650 / speed + delay, 49, 57); // line 3 shortest
        circleLeft.MoveY(OsbEasing.InCubic, 5650 / speed + delay, 6700 / speed + delay, 57, 90); // line 4 curve
        //
        circleLeft.Scale(OsbEasing.InCubic, 5650 / speed + delay, 6700 / speed + delay, 0.03f, 0.01f);
        circleLeft.Fade(6700 / speed + delay, 6700 / speed + delay + 1, 0.5, 0);
        circleLeft.Fade(6700 / speed + delay + 1, 6700 / (speed / 2) + speed + delay, 0, 0);
        //
        circleLeft.EndGroup();

        // after transition
        // circle.StartLoopGroup(startTime + 1500, 4);
        // circle.MoveX();
        // circle.MoveY();
        // circle.EndGroup();
    }

    public void DialogHUD(int startTime, int endTime, string Mission, string SongName)
    {
        // DIALOG BOXES STARTS HERE
        var fontSize = 20;
        var GlowRadius = 0;
        var ShadowThickness = 0;
        var OutlineThickness = 0;
        var font = generator.LoadFont("sb/HUD/txt/system/" + Mission, new FontDescription()
        {
            FontPath = "Impact",
            FontSize = fontSize,
            Color = Color4.White,
            Padding = Vector2.Zero,
            FontStyle = FontStyle.Italic & FontStyle.Bold,
            TrimTransparency = true,
            EffectsOnly = false,
            Debug = false,
        },
        new FontGlow()
        {
            Radius = true ? 0 : GlowRadius,
            Color = Color4.Black,
        },
        new FontOutline()
        {
            Thickness = OutlineThickness,
            Color = Color4.Black,
        },
        new FontShadow()
        {
            Thickness = ShadowThickness,
            Color = Color4.Black,
        });

        // MISSION + SONG NAME -----------------------------------------
        string[] sentence = { Mission };
        var dialog = new DialogManager(generator, font, startTime + 924, startTime + 8077, "HUD", 320, 290, true,
            fontSize, 0.2f, 50, 500, Color4.White, false, 0.7f, Color4.Black, "HUD", 0, "sb/sfx/message-2.ogg",
            DialogBoxes.Pointer.TopRight, DialogBoxes.Push.Right, sentence);

        string[] sentence2 = { SongName };
        var dialog2 = new DialogManager(generator, font, startTime + 924, startTime + 8077, "HUD", 320, 425, true,
            fontSize, 0.2f, 50, 500, Color4.White, false, 0.7f, Color4.Black, "HUD", 0, "sb/sfx/message-2.ogg",
            DialogBoxes.Pointer.None, DialogBoxes.Push.Right, sentence2);

        // READY -----------------------------------------
        string[] sentence3 = { "Ready" };
        var dialog3 = new DialogManager(generator, font, startTime + 10154, startTime + 11378, "HUD", 320, 290, true,
            fontSize, 0.5f, 50, 500, Color4.White, false, 0.7f, Color4.Black, "HUD", 300, "sb/sfx/start-2.ogg",
            DialogBoxes.Pointer.TopRight, DialogBoxes.Push.Right, sentence3);
    }

    public void LoadingText(int startTime, int endTime, int loadingTextEndtime)
    {
        // "LOADING"
        var timeStep = 400;
        var duration = 6000;
        var StartTime = startTime - timeStep;
        var EndTime = loadingTextEndtime;
        var loopCount = (EndTime - StartTime) / duration;
        for (var i = StartTime; i < EndTime; i += timeStep)
        {
            var sprite = generator.GetLayer("HUD").CreateSprite("sb/HUD/txt/loadingText.png", OsbOrigin.Centre);
            var rotation = MathHelper.DegreesToRadians(27);

            sprite.StartLoopGroup(i, loopCount);
            // first half
            sprite.Fade(0, duration / 2, 0, 0.3f);
            sprite.MoveX(0, duration / 2, -40, 320);
            sprite.MoveY(OsbEasing.OutSine, 0, duration / 2, 464, 407);
            sprite.Rotate(OsbEasing.OutSine, 0, duration / 2, -rotation, 0);
            // second half
            sprite.Fade(duration / 2, duration, 0.3f, 0);
            sprite.MoveX(duration / 2, duration, 320, 680);
            sprite.MoveY(OsbEasing.InSine, duration / 2, duration, 407, 464);
            sprite.Rotate(OsbEasing.InSine, duration / 2, duration, 0, rotation);
            // end
            sprite.Scale(0, 0.25f);
            sprite.Additive(0, duration);
            sprite.EndGroup();
        }
    }
}
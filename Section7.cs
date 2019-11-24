using OpenTK;
using System.Drawing;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class Section7 : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            Dialog();
            Mission();
            SnowChaos(290896, 301425, 1000);
            SnowChaos(303310, 311006, 500);
            Blank(303310, 307237);
            Tochi(255870, 265294);
            HUD(283671, 329854, 290901, "Mission #6", "Strahv", "sb/HUD/txt/nameTag/Heilia.png", 4500, "sb/avatars/HeiliaProfile.png");
        }

        public void Blank(int startTime, int endTime)
        {
            // Data for the backgrounds are included in the OsbImport.cs

            var bitmap = GetMapsetBitmap("sb/pixel.png");
            var sprite = GetLayer("Blank").CreateSprite("sb/bgs/7/bg.jpg", OsbOrigin.Centre, new Vector2(320, 240));

            sprite.Scale(startTime, 480.0f / bitmap.Height);
            sprite.Fade(startTime, endTime, 0.2, 0);
            sprite.Additive(startTime, endTime);
        }

        public void HUD(int startTime, int endTime, int loadingTextEndtime, string mission, string songName, string nameTag, int progressBarDelay, string avatar)
        {
            var hud = new HUD(this, startTime, endTime, loadingTextEndtime, mission, songName, nameTag, progressBarDelay, avatar);
        }

        public void Mission()
        {
        }

        public void SnowChaos(int startTime, int endTime, int FadeTime)
        {
            // var mission = new Mission3(this, startTime, endTime);

            // Snow chaos
            var interval = 40;
            var MinTravelTime = 2000;
            var MaxTravelTime = 4000;

            for (int i = startTime; i < endTime - MinTravelTime; i += interval)
            {
                var sprite = GetLayer("SnowChaos").CreateSprite("sb/missions/7/snow" + Random(1, 3) + ".png", OsbOrigin.Centre);
                var duration = Random(MinTravelTime, MaxTravelTime);
                var Fade = Random(0.05, 0.2);
                var Rotation = Random(-10, 10);
                var Scale = Random(0.01, 0.3);
                var RandomScale = Random(Scale / 2, Scale);
                
                // sprite.Fade(i, i + 1000, 0, Fade);
                // sprite.Fade(i + duration - 1000, i + duration, Fade, 0);
                sprite.ScaleVec(i, i + duration, RandomScale, RandomScale, Random(0, Scale / 6), Random(0, Scale / 6));
                // sprite.Additive(i, i + duration);

                if (i < endTime - (FadeTime + FadeTime))
                    {
                        sprite.Fade(i, i + FadeTime, 0, Fade);
                        if (i < endTime - duration)
                        {
                            sprite.Fade(i + duration - FadeTime, i + duration, Fade, 0);
                        }
                        else
                        {
                            sprite.Fade(endTime - 10, endTime, Fade, 0);
                        }
                    }
                    else
                    {
                        sprite.Fade(i, 0);
                    }

                var speed = 1000;
                for (int r = i; r < endTime - MinTravelTime; r += speed)
                {
                    sprite.Rotate(r, r + (speed / 2), Rotation / 2, Rotation);
                    sprite.Rotate(r + (speed / 2), r + speed, Rotation, Rotation);
                }

                var lastX = Random(-107, 747) - 150;
                var lastY = Random(0, 480) - 150;
                var speedMin = 100;
                var speedMax = 200;

                var rVec = MathHelper.DegreesToRadians(Random(360));
                var sVec = Random(speedMin, speedMax);
                var vX = Math.Cos(rVec) * sVec;
                var vY = Math.Sin(rVec) * sVec;

                var UpdateRate = 1000;
                for (var t = i; t < i + duration; t += UpdateRate)
                {
                    var nextX = lastX + vX;
                    var nextY = lastY + vY;
                    Log(vX.ToString());

                    sprite.Move(t, t + UpdateRate, lastX, lastY, nextX, nextY);

                    vX += Random(UpdateRate / 10) * UpdateRate / 1000;
                    vY += Random(UpdateRate / 10) * UpdateRate / 1000;

                    lastX = (int)nextX;
                    lastY = (int)nextY;
                }
            }
        }

        public void Dialog()
        {
            // DIALOG BOXES STARTS HERE
            var fontSize = 15;
            var GlowRadius = 0;
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            var font = LoadFont("sb/dialog/txt/2", new FontDescription()
            {
                FontPath = "Microsoft Yi Baiti",
                FontSize = fontSize,
                Color = Color4.White,
                Padding = Vector2.Zero,
                FontStyle = FontStyle.Bold,
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


            // DIALOG 1 -----------------------------------------
            string[] sentence = { "That must've been fun.",
                                  "The next quest is much easier though." };
            var dialog = new DialogManager(this, font, 255870, 260268, "-Tochi", 105, 326, false,
                fontSize, 1, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.wav",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "Just collect some of the artifacts from the ground.",
                                   "Should be easy!" };
            var dialog2 = new DialogManager(this, font, 260268, 265294, "-Tochi", 105, 326, false,
                fontSize, 1, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.wav",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence2);
        }

        public void Tochi(int startTime, int endTime)
        {
            var Hoveduration = 5000;
            var loopCount = (endTime - startTime) / Hoveduration;
            var pos = new Vector2(320, 240);
            var avatar = GetLayer("-Tochi").CreateSprite("sb/avatars/-TochiProfile.png", OsbOrigin.Centre);
            var ring = GetLayer("-Tochi").CreateSprite("sb/ring2.png", OsbOrigin.Centre);

            avatar.MoveX(startTime, 64);
            avatar.Scale(startTime, 0.3);
            avatar.Fade(startTime, startTime + 500, 0, 1);
            avatar.Fade(endTime - 500, endTime, 1, 0);

            avatar.StartLoopGroup(startTime, loopCount + 1);
            avatar.MoveY(OsbEasing.InOutSine, 0, Hoveduration / 2, 335, 345);
            avatar.MoveY(OsbEasing.InOutSine, Hoveduration / 2, Hoveduration, 345, 335);
            avatar.EndGroup();

            ring.MoveX(startTime, 64);
            ring.Scale(startTime, 0.3);
            ring.Fade(startTime, startTime + 500, 0, 1);
            ring.Fade(endTime - 500, endTime, 1, 0);
            var rotation = MathHelper.DegreesToRadians(180);
            ring.Rotate(startTime, endTime, -rotation, rotation);

            ring.StartLoopGroup(startTime, loopCount + 1);
            ring.MoveY(OsbEasing.InOutSine, 0, Hoveduration / 2, 335, 345);
            ring.MoveY(OsbEasing.InOutSine, Hoveduration / 2, Hoveduration, 345, 335);
            ring.EndGroup();
        }
    }
}

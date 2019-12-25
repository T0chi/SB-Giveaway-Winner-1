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
    public class Section2 : StoryboardObjectGenerator
    {
        [Configurable]
        public Color4 BoxColor = new Color4(255, 255, 255, 255);
        
        [Configurable]
        public Color4 BoxColorText = new Color4(255, 255, 255, 255);

        [Configurable]
        public Color4 BoxColorNarrator = new Color4(255, 255, 255, 255);

        [Configurable]
        public Color4 BoxColorNarratorText = new Color4(255, 255, 255, 255);

        [Configurable]
        public Color4 GlowColor = new Color4(255, 255, 255, 255);

        [Configurable]
        public Color4 OutlineColor = new Color4(50, 50, 50, 200);

        [Configurable]
        public Color4 ShadowColor = new Color4(0, 0, 0, 200);

        public override void Generate()
        {
            Background();
            HUD(61860, 115238, 72572, "Mission #1", "Ein", "sb/HUD/txt/nameTag/Pino.png", 0, "sb/avatars/PinoProfile.png");
            Dialog();

            // Mission is to destroy all the enemies (aircrafts)
            Mission();
            Tochi(66476, 84571);
        }

        public void Background()
        {  
            var bitmap = GetMapsetBitmap("sb/bgs/2/bg.jpg");
            var bg = GetLayer("Background").CreateSprite("sb/bgs/2/bg.jpg", OsbOrigin.Centre, new Vector2(320, 240));

            bg.Scale(57245, 480.0f / bitmap.Height);
            bg.Fade(57245, 72572, 0, 0.5);
            bg.Fade(115238, 128263, 0.5, 0);
        }

        public void HUD(int startTime, int endTime, int loadingTextEndtime, string mission, string songName, string nameTag, int progressBarDelay, string avatar)
        {
            var hud = new HUD(this, startTime, endTime, loadingTextEndtime, mission, songName, nameTag, progressBarDelay, avatar);
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
                Color = GlowColor,
            },
            new FontOutline()
            {
                Thickness = OutlineThickness,
                Color = OutlineColor,
            },
            new FontShadow()
            {
                Thickness = ShadowThickness,
                Color = ShadowColor,
            });

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "Ready for your first quest?" };
            var dialog = new DialogManager(this, font, 66937, 70600, "-Tochi", 105, 326, false,
                fontSize, 1, 50, 500, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.ogg",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "Shoot the enemy and try not to miss. If you miss too many times",
                                   "you will end up with less points so you must do your best.",
                                   "Good luck fellow player!" };
            var dialog2 = new DialogManager(this, font, 70630, 84571, "-Tochi", 105, 326, false,
                fontSize, 1, 50, 500, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.ogg",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence2);
        }

        public void Mission()
        {
            var mission = new Mission1(this, 79238, 112571);

            // INTRODUCTION TO THE MISSION
            Aircraft();
            Clouds();
        }

        public void Aircraft()
        {
            var startTime = 72571;
            var endTime = 79238;
            var duration3 = 1500;
            var tiltAt = startTime + (duration3 * 3);

            var loopCount3 = (tiltAt - startTime) / (duration3 * 2);
            var jetSound = GetLayer("Aircraft 3 Front").CreateSample("sb/sfx/jet-1.ogg", startTime, 70);
            var jetSound2 = GetLayer("Aircraft 3 Front").CreateSample("sb/sfx/jet-3.ogg", startTime, 70);
            var jetSound3 = GetLayer("Aircraft 3 Front").CreateSample("sb/sfx/jet_passing-1.ogg", tiltAt - 700, 100);
            var aircraft3 = GetLayer("Aircraft 3 Front").CreateSprite("sb/missions/1/aircrafts/3_front.png", OsbOrigin.Centre);
        
            aircraft3.Scale(OsbEasing.Out, startTime, startTime + 4000, 0.05, 0.3);
            aircraft3.Fade(OsbEasing.Out, startTime, startTime + 2000, 0, 1);
            aircraft3.Fade(endTime - 500, endTime, 1, 0);

            aircraft3.StartLoopGroup(startTime, loopCount3);
            // x
            aircraft3.MoveX(OsbEasing.InOutSine, 0, duration3 / 2, 310, 330);
            aircraft3.MoveX(OsbEasing.InOutSine, duration3 / 2, duration3, 330, 300);
            aircraft3.MoveX(OsbEasing.InOutSine, duration3, duration3 * 2, 300, 310);
            aircraft3.MoveX(OsbEasing.InOutSine, duration3 * 2, duration3 * 3, 310, 330);
            // aircraft3.MoveX(OsbEasing.InOutSine, duration3 * 3, duration3 * 4, 330, 320);
            // aircraft3.MoveX(OsbEasing.InOutSine, duration3 * 4, duration3 * 5, 320, 310);
            // y
            aircraft3.MoveY(OsbEasing.InOutSine, 0, duration3 / 2, 270, 240);
            aircraft3.MoveY(OsbEasing.InOutSine, duration3 / 2, duration3, 240, 230);
            aircraft3.MoveY(OsbEasing.InOutSine, duration3, duration3 * 2, 230, 250);
            aircraft3.MoveY(OsbEasing.InOutSine, duration3 * 2, duration3 * 3, 250, 210);
            // aircraft3.MoveY(OsbEasing.InOutSine, duration3 * 3, duration3 * 4, 210, 230);
            // aircraft3.MoveY(OsbEasing.InOutSine, duration3 * 4, duration3 * 5, 230, 270);
            //
            aircraft3.EndGroup();

            // tilt
            aircraft3.MoveX(OsbEasing.InSine, tiltAt, endTime, 330, -180);
            aircraft3.MoveY(OsbEasing.InSine, tiltAt, endTime, 210, 330);
            var rotation5 = MathHelper.DegreesToRadians(5);
            var rotation = MathHelper.DegreesToRadians(-20);
            aircraft3.Rotate(OsbEasing.InOutSine, startTime, tiltAt - 3000, rotation5, -rotation5);
            aircraft3.Rotate(OsbEasing.InOutSine, tiltAt - 3000, tiltAt, -rotation5, rotation5 / 2);
            aircraft3.Rotate(OsbEasing.InSine, tiltAt, endTime, rotation5 / 2, rotation);

            // SMOKE
            float X = 0;
            float scale = 0.15f;
            float fade = 0;
            var smokeAmount = 40;
            var smokeDuration = 200;
            for (int i = startTime; i <= endTime; i += smokeAmount)
            {
                var smokeLeft = GetLayer("Aircraft 3 Front - Smoke").CreateAnimation("sb/missions/1/aircrafts/smoke/smoke.png",
                                                                4, 100, OsbLoopType.LoopForever, OsbOrigin.Centre);
                var smokeRight = GetLayer("Aircraft 3 Front - Smoke").CreateAnimation("sb/missions/1/aircrafts/smoke/smoke.png",
                                                                4, 100, OsbLoopType.LoopForever, OsbOrigin.Centre);
                int x = 42;
                int y = 15;
                int delay = 4;
                int fadeDelay = 10;

                if (i > startTime + 4000)
                {
                    smokeLeft.Fade(i, 0.2);
                    smokeLeft.Additive(i, i + smokeDuration);
                    smokeLeft.Scale(OsbEasing.In, i, i + (smokeDuration * fadeDelay), 0.3, 0.3 / 4);
                    smokeLeft.Fade(i + smokeDuration, i + (smokeDuration * fadeDelay), 0.2f, 0);
                    smokeLeft.Move(OsbEasing.Out, i, i + smokeDuration, aircraft3.PositionAt(i).X - x, aircraft3.PositionAt(i).Y - y,
                            aircraft3.PositionAt(i - (smokeDuration * 2)).X - x, aircraft3.PositionAt(i - (smokeDuration * delay)).Y - y);
                            
                    smokeRight.Fade(i, 0.2);
                    smokeRight.Additive(i, i + smokeDuration);
                    smokeRight.Scale(OsbEasing.In, i, i + (smokeDuration * fadeDelay), 0.3, 0.3 / 4);
                    smokeRight.Fade(i + smokeDuration, i + (smokeDuration * fadeDelay), 0.2f, 0);
                    smokeRight.Move(OsbEasing.Out, i, i + smokeDuration, aircraft3.PositionAt(i).X + x, aircraft3.PositionAt(i).Y - y,
                            aircraft3.PositionAt(i - (smokeDuration * 2)).X + x, aircraft3.PositionAt(i - (smokeDuration * delay)).Y - y);
                }

                else
                {
                    X += 0.4f;
                    scale += 0.0015f;
                    fade += 0.002f;

                    smokeLeft.Fade(i, fade);
                    smokeLeft.Additive(i, i + smokeDuration);
                    smokeLeft.Scale(OsbEasing.In, i, i + (smokeDuration * fadeDelay), scale, scale / 4);
                    smokeLeft.Fade(i + smokeDuration, i + (smokeDuration * fadeDelay), fade, 0);
                    smokeLeft.Move(OsbEasing.Out, i, i + smokeDuration, aircraft3.PositionAt(i).X - X, aircraft3.PositionAt(i).Y - y,
                            aircraft3.PositionAt(i - (smokeDuration * 2)).X - X, aircraft3.PositionAt(i - (smokeDuration * delay)).Y - y);

                    smokeRight.Fade(i, fade);
                    smokeRight.Additive(i, i + smokeDuration);
                    smokeRight.Scale(OsbEasing.In, i, i + (smokeDuration * fadeDelay), scale, scale / 4);
                    smokeRight.Fade(i + smokeDuration, i + (smokeDuration * fadeDelay), fade, 0);
                    smokeRight.Move(OsbEasing.Out, i, i + smokeDuration, aircraft3.PositionAt(i).X + X, aircraft3.PositionAt(i).Y - y,
                            aircraft3.PositionAt(i - (smokeDuration * 2)).X + X, aircraft3.PositionAt(i - (smokeDuration * delay)).Y - y);
                }
            }
        }

        public void Clouds()
        {
            
            var startTime = 72571;
            var endTime = 79238;
            float cloudVelocity = 3;
            var d = (endTime - startTime) / 4;

            for (float i = startTime; i < startTime + d; i += cloudVelocity)
            {
                var duration = Random(400, 800);
                var RandomX = Random(-107, 747);
                var RandomFade = Random(0.005, 0.1);
                var RandomScale = Random(0.4, 2);
                var startPos  = new Vector2(RandomX, Random(355, 360));
                var endPos  = new Vector2(RandomX, Random(340, 345));

                var sprite = GetLayer("Clouds").CreateSprite("sb/missions/1/cloud/cloud" + Random(1, 11) + ".png", OsbOrigin.TopCentre);

                var loopCount = (endTime - startTime) / duration;
                sprite.StartLoopGroup(i, loopCount);
                sprite.Additive(0, duration);
                sprite.Fade(0, duration / 4, 0, RandomFade);
                sprite.Fade(duration - duration / 4, duration, RandomFade, 0);
                sprite.MoveX(0, duration, startPos.X, endPos.X);
                sprite.MoveY(0, duration, startPos.Y, endPos.Y);
                sprite.ScaleVec(OsbEasing.OutSine, 0, duration, RandomScale, RandomScale, RandomScale / 4, RandomScale / 4);
                sprite.EndGroup();
            }
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

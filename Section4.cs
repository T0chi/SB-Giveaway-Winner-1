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
    public class Section4 : StoryboardObjectGenerator
    {
        [Configurable]
        public Color4 wingsColor = Color4.White;

        [Configurable]
        public bool NewColorEvery = false;

        [Configurable]
        public Color4 WallColorMin = Color4.White;

        [Configurable]
        public Color4 WallColorMax = Color4.Cyan;

        [Configurable]
        public bool NewColorEvery2 = false;

        [Configurable]
        public Color4 WallColorMin2 = Color4.White;

        [Configurable]
        public Color4 WallColorMax2 = Color4.Black;

        private DialogManager dialog;

        private DialogManager dialog2;

        public override void Generate()
        {
            Dialog();
            Mission(172011, 211525);
            Tochi(163962, 175868);
            Background(160662, 211525, 167640);
            HUD(160662, 211525, 167640, "Mission #3", "Central Nucleus", "sb/HUD/txt/nameTag/ScubDomino.png", 2000, "sb/avatars/ScubDominoProfile.png");
        }

        // SHAKE EFFECT
        public Vector2 CirclePos(double angle, int radius)
        {
            double posX = 320 + (radius * Math.Cos(angle));
            double posY = 240 + ((radius * 5) * Math.Sin(angle));

            return new Vector2((float)posX, (float)posY);
        }

        public void Background(int startTime, int endTime, int loadingTextEndtime)
        {
            var bitmap = GetMapsetBitmap("sb/bgs/4/bg.jpg");
            var bg = GetLayer("Background").CreateSprite("sb/bgs/4/bg.jpg", OsbOrigin.Centre);

            bg.Scale(startTime, 854.0f / bitmap.Width);
            bg.Fade(startTime, loadingTextEndtime, 0, 0.3);
            bg.Fade(endTime, 220447, 0.3, 0);
            bg.Move(OsbEasing.InOutBack, startTime, loadingTextEndtime, new Vector2(320, 250), new Vector2(320, 240));


            // SHAKE EFFECT
            var bgRed = GetLayer("Background").CreateSprite("sb/bgs/4/bg.jpg", OsbOrigin.Centre);

            var Radius = 7;
            var ShakeAmount = 100;
            var ShakeEasing = OsbEasing.InOutSine;

            bgRed.Color(189582, Color4.IndianRed);
            bgRed.Scale(189582, 854.0f / bitmap.Width);
            bgRed.Fade(189582, 189582 + 150, 0, 0.2);
            bgRed.Fade(endTime, endTime + 1000, 0.2, 0);
            bgRed.Move(189582, new Vector2(320, 240));

            var Additive = true;
            if (Additive)
            {
                bgRed.Additive(189582, endTime + 100);
            }

            var angleCurrent = 0d;
            var radiusCurrent = 0;
            // ShakeAmount -> smaller number = more shaking!
            for (int i = 189582; i < endTime + 100 - ShakeAmount; i += ShakeAmount)
            {
                var angle = Random(angleCurrent - Math.PI / 4, angleCurrent + Math.PI / 4);
                var radius = Math.Abs(Random(radiusCurrent - Radius / 4, radiusCurrent + Radius / 4));

                while (radius > Radius)
                {
                    radius = Math.Abs(Random(radiusCurrent - Radius / 4, radiusCurrent + Radius / 4));
                }

                var start = bgRed.PositionAt(i);
                var end = CirclePos(angle, radius);

                if (i + ShakeAmount >= endTime + 100)
                {
                    bgRed.Move(ShakeEasing, i, endTime + 100, start, end);
                }
                else
                {
                    bgRed.Move(ShakeEasing, i, i + ShakeAmount, start, end);
                }

                angleCurrent = angle;
                radiusCurrent = radius;
            }
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
            string[] sentence = { "Hope you enjoyed yourself while collecting items!",
                                  "Now it's time for something more fun...",
                                  "Slashing it is! I hope your stamina is good heheh~." };
            this.dialog = new DialogManager(this, font, 163962, 170382, "-Tochi", 105, 326, false,
                fontSize, 1, 50, 500, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.ogg",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "Slash the black virus boxes to complete the quest.",
                                   "Watch out at the end though!" };
            this.dialog2 = new DialogManager(this, font, 170382, 175868, "-Tochi", 105, 326, false,
                fontSize, 1, 50, 500, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.ogg",
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

        public void Mission(int startTime, int endTime)
        {
            // var mission = new Mission3(this, startTime, endTime);

            var sTime = 173125;
            var eTime = 189582;
            var speed = Random(10, 15);
            var Beat = Beatmap.GetTimingPointAt(sTime).BeatDuration;
            var interval = Random(Beat * 2, Beat * 4);
            var spawnDelay = Random(Beat, Beat * 6);

            // SMALL WALLS FROM LEFT AND RIGHT ///////////////////////////////////////
            // left and right
            for (double i = sTime; i < eTime - interval; i += interval)
            {
                var wallLeftL = GetLayer("Walls").CreateSprite("sb/missions/3/wallRightL.png", OsbOrigin.CentreRight);
                var wallLeftR = GetLayer("Walls").CreateSprite("sb/missions/3/wallRightR.png", OsbOrigin.CentreLeft);
                var wallRightL = GetLayer("Walls").CreateSprite("sb/missions/3/wallLeftR.png", OsbOrigin.CentreLeft);
                var wallRightR = GetLayer("Walls").CreateSprite("sb/missions/3/wallLeftL.png", OsbOrigin.CentreRight);

                var Duration = interval / speed;

                var L_StartPosX = 0;
                var L_StartPosY = 400;
                var L_EndPosX = Random(180, 250);
                var L_EndPosY = 330;

                var R_StartPosX = 640;
                var R_StartPosY = 400;
                var R_EndPosX = Random(390, 460);
                var R_EndPosY = 330;

                var ScaleXL = Random(0.2f, 0.5f);
                var ScaleYL = Random(0.2f, 0.5f);

                var ScaleXR = Random(0.2f, 0.5f);
                var ScaleYR = Random(0.2f, 0.5f);

                var RandomScatterScale = Random(0.5f, 0.95f);

                var randomY = new Vector2((float)Random(70, 100), (float)Random(70, 100));
                var randomY2 = new Vector2((float)Random(20, 50), (float)Random(20, 50));

                // calculate y offset
                var offsetY = (((ScaleYL * 4) / 60) * 1000);
                var offsetY2 = (((ScaleYR * 4) / 60) * 1000);
                // end calculation

                // calculate sprite half height
                var bitmap = GetMapsetBitmap("sb/missions/3/wallRightL.png");
                var heightHalfL = 0; // (ScaleYL / 2) * bitmap.Height
                var heightHalfR = 0; // (ScaleYR / 2) * bitmap.Height
                // end calculation

                var FadeInDelay = 1000;
                var FadeOutDelay = (Random(700, 1000)) * 1.3;
                var Easing = OsbEasing.None;
                var ScatteringSpeed = FadeOutDelay;
                var slashYL = Random(150, 380);
                var slashYR = Random(150, 380);

                // left /////////////////////////////////////////////////////////////

                // (left)
                // wallLeftL.FlipH(i - FadeInDelay, i + Duration + FadeOutDelay);
                wallLeftL.Fade(i - FadeInDelay, i + Duration / 4, 0, 1);
                wallLeftL.Fade(i + Duration, i + Duration + FadeOutDelay, 1, 0);
                wallLeftL.ScaleVec(i - FadeInDelay, ScaleXL, ScaleYL);
                if (ScaleYL >= 0.35f)
                    wallLeftL.Move(Easing, i - FadeInDelay, i + Duration, L_StartPosX, L_StartPosY + heightHalfL - randomY.X, L_EndPosX, L_EndPosY + heightHalfL - randomY.Y);
                else if (ScaleYL <= 0.25f)
                    wallLeftL.Move(Easing, i - FadeInDelay, i + Duration, L_StartPosX, L_StartPosY + heightHalfL + randomY2.X, L_EndPosX, L_EndPosY + heightHalfL + randomY2.Y);
                else
                    wallLeftL.Move(Easing, i - FadeInDelay, i + Duration, L_StartPosX, L_StartPosY + heightHalfL, L_EndPosX, L_EndPosY + heightHalfL);

                // (right)
                // wallLeftR.FlipH(i - FadeInDelay, i + Duration + FadeOutDelay);
                wallLeftR.Fade(i - FadeInDelay, i + Duration / 4, 0, 1);
                wallLeftR.Fade(i + Duration, i + Duration + FadeOutDelay, 1, 0);
                wallLeftR.ScaleVec(i - FadeInDelay, ScaleXL, ScaleYL);
                if (ScaleYL >= 0.35f)
                    wallLeftR.Move(Easing, i - FadeInDelay, i + Duration, L_StartPosX, L_StartPosY + heightHalfL + (offsetY - 2.5) - randomY.X, L_EndPosX, L_EndPosY + heightHalfL + (offsetY - 2.5) - randomY.Y);
                else if (ScaleYL <= 0.25f)
                    wallLeftR.Move(Easing, i - FadeInDelay, i + Duration, L_StartPosX, L_StartPosY + heightHalfL + (offsetY - 1.5) + randomY2.X, L_EndPosX, L_EndPosY + heightHalfL + (offsetY - 1.5) + randomY2.Y);
                else
                    wallLeftR.Move(Easing, i - FadeInDelay, i + Duration, L_StartPosX, L_StartPosY + heightHalfL + (offsetY - 2), L_EndPosX, L_EndPosY + heightHalfL + (offsetY - 2));

                // (left) + (right) slashing to pieces
                var ScatteringEndPosL = wallLeftL.PositionAt(i + Duration);
                var ScatteringEndPosR = wallLeftR.PositionAt(i + Duration);

                wallLeftL.Move(i + Duration, i + Duration + ScatteringSpeed, ScatteringEndPosL.X, ScatteringEndPosL.Y,
                            ScatteringEndPosL.X + Random(-200, -100), ScatteringEndPosL.Y + Random(-200, 100));
                wallLeftR.Move(i + Duration, i + Duration + ScatteringSpeed, ScatteringEndPosR.X, ScatteringEndPosR.Y,
                            ScatteringEndPosR.X + Random(100, 200), ScatteringEndPosR.Y + Random(-200, 100));

                // rotate from top
                if (wallLeftL.PositionAt(i + Duration).Y > slashYL)
                {
                    wallLeftL.Rotate(i + Duration, i + Duration + ScatteringSpeed, 0, Random(1, 10));
                    wallLeftR.Rotate(i + Duration, i + Duration + ScatteringSpeed, 0, Random(-10, -1));
                }
                // rotate from bottom
                else
                {
                    wallLeftL.Rotate(i + Duration, i + Duration + ScatteringSpeed, 0, Random(-10, -1));
                    wallLeftR.Rotate(i + Duration, i + Duration + ScatteringSpeed, 0, Random(1, 10));
                }

                wallLeftL.ScaleVec(i + Duration, i + Duration + (ScatteringSpeed / 2), ScaleXL, ScaleYL, ScaleXL * -RandomScatterScale, ScaleYL * RandomScatterScale);
                wallLeftL.ScaleVec(i + Duration + (ScatteringSpeed / 2), i + Duration + ScatteringSpeed, ScaleXL * -RandomScatterScale, ScaleYL * RandomScatterScale, ScaleXL * RandomScatterScale, ScaleYL * RandomScatterScale);
                wallLeftR.ScaleVec(i + Duration, i + Duration + (ScatteringSpeed / 2), ScaleXL, ScaleYL, ScaleXL * -RandomScatterScale, ScaleYL * RandomScatterScale);
                wallLeftR.ScaleVec(i + Duration + (ScatteringSpeed / 2), i + Duration + ScatteringSpeed, ScaleXL * -RandomScatterScale, ScaleYL * RandomScatterScale, ScaleXL * RandomScatterScale, ScaleYL * RandomScatterScale);


                // right /////////////////////////////////////////////////////////////

                // (left)
                wallRightL.Fade(i - FadeInDelay + spawnDelay, i + spawnDelay + Duration / 4, 0, 1);
                wallRightL.Fade(i + spawnDelay + Duration, i + spawnDelay + Duration + FadeOutDelay, 1, 0);
                wallRightL.ScaleVec(i - FadeInDelay + spawnDelay, ScaleXR, ScaleYR);
                if (ScaleYR >= 0.35f)
                    wallRightL.Move(Easing, i - FadeInDelay + spawnDelay, i + spawnDelay + Duration, R_StartPosX, R_StartPosY + heightHalfR - randomY.X, R_EndPosX, R_EndPosY + heightHalfR - randomY.Y);
                else if (ScaleYR <= 0.25f)
                    wallRightL.Move(Easing, i - FadeInDelay + spawnDelay, i + spawnDelay + Duration, R_StartPosX, R_StartPosY + heightHalfR + randomY2.X, R_EndPosX, R_EndPosY + heightHalfR + randomY2.Y);
                else
                    wallRightL.Move(Easing, i - FadeInDelay + spawnDelay, i + spawnDelay + Duration, R_StartPosX, R_StartPosY + heightHalfR, R_EndPosX, R_EndPosY + heightHalfR);

                // (right)
                wallRightR.Fade(i - FadeInDelay + spawnDelay, i + spawnDelay + Duration / 4, 0, 1);
                wallRightR.Fade(i + spawnDelay + Duration, i + spawnDelay + Duration + FadeOutDelay, 1, 0);
                wallRightR.ScaleVec(i - FadeInDelay + spawnDelay, ScaleXR, ScaleYR);
                if (ScaleYR >= 0.35f)
                    wallRightR.Move(Easing, i - FadeInDelay + spawnDelay, i + spawnDelay + Duration, R_StartPosX, R_StartPosY + heightHalfR + (offsetY2 - 2.5) - randomY.X, R_EndPosX, R_EndPosY + heightHalfR + (offsetY2 - 2.5) - randomY.Y);
                else if (ScaleYR <= 0.25f)
                    wallRightR.Move(Easing, i - FadeInDelay + spawnDelay, i + spawnDelay + Duration, R_StartPosX, R_StartPosY + heightHalfR + (offsetY2 - 1.5) + randomY2.X, R_EndPosX, R_EndPosY + heightHalfR + (offsetY2 - 1.5) + randomY2.Y);
                else
                    wallRightR.Move(Easing, i - FadeInDelay + spawnDelay, i + spawnDelay + Duration, R_StartPosX, R_StartPosY + heightHalfR + (offsetY2 - 2), R_EndPosX, R_EndPosY + heightHalfR + (offsetY2 - 2));

                // slashing to pieces (scattering)
                var ScatteringEndPosL2 = wallRightL.PositionAt(i + spawnDelay + Duration);
                var ScatteringEndPosR2 = wallRightR.PositionAt(i + spawnDelay + Duration);

                wallRightL.Move(i + spawnDelay + Duration, i + spawnDelay + Duration + ScatteringSpeed, ScatteringEndPosL2.X, ScatteringEndPosL2.Y,
                            ScatteringEndPosL2.X + Random(100, 200), ScatteringEndPosL2.Y + Random(-200, 100));
                wallRightR.Move(i + spawnDelay + Duration, i + spawnDelay + Duration + ScatteringSpeed, ScatteringEndPosR2.X, ScatteringEndPosR2.Y,
                            ScatteringEndPosR2.X + Random(-200, -100), ScatteringEndPosR2.Y + Random(-200, 100));

                // rotate from top
                if (wallRightL.PositionAt(i + spawnDelay + Duration).Y > slashYR)
                {
                    wallRightL.Rotate(i + spawnDelay + Duration, i + spawnDelay + Duration + ScatteringSpeed, 0, Random(-10, -1));
                    wallRightR.Rotate(i + spawnDelay + Duration, i + spawnDelay + Duration + ScatteringSpeed, 0, Random(1, 10));
                }
                // rotate from bottom
                else
                {
                    wallRightL.Rotate(i + spawnDelay + Duration, i + spawnDelay + Duration + ScatteringSpeed, 0, Random(1, 10));
                    wallRightR.Rotate(i + spawnDelay + Duration, i + spawnDelay + Duration + ScatteringSpeed, 0, Random(-10, -1));
                }

                wallRightL.ScaleVec(i + spawnDelay + Duration, i + spawnDelay + Duration + (ScatteringSpeed / 2), ScaleXR, ScaleYR, ScaleXR * -RandomScatterScale, ScaleYR * RandomScatterScale);
                wallRightL.ScaleVec(i + spawnDelay + Duration + (ScatteringSpeed / 2), i + spawnDelay + Duration + ScatteringSpeed, ScaleXR * -RandomScatterScale, ScaleYR * RandomScatterScale, ScaleXR * RandomScatterScale, ScaleYR * RandomScatterScale);
                wallRightR.ScaleVec(i + spawnDelay + Duration, i + spawnDelay + Duration + (ScatteringSpeed / 2), ScaleXR, ScaleYR, ScaleXR * -RandomScatterScale, ScaleYR * RandomScatterScale);
                wallRightR.ScaleVec(i + spawnDelay + Duration + (ScatteringSpeed / 2), i + spawnDelay + Duration + ScatteringSpeed, ScaleXR * -RandomScatterScale, ScaleYR * RandomScatterScale, ScaleXR * RandomScatterScale, ScaleYR * RandomScatterScale);

                // left + right //////////////////////////////////////////////////////
                // color
                var nColorEvery = 2; // change the color of the sprite every chosen value
                if (NewColorEvery)
                {
                    if (i % nColorEvery == 1)
                    {
                        wallLeftL.Color(i - FadeInDelay, WallColorMin);
                        wallLeftR.Color(i - FadeInDelay, WallColorMin);
                        wallRightL.Color(i - FadeInDelay, WallColorMin);
                        wallRightR.Color(i - FadeInDelay, WallColorMin);
                    }
                    else
                    {
                        wallLeftL.Color(i - FadeInDelay, WallColorMax);
                        wallLeftR.Color(i - FadeInDelay, WallColorMax);
                        wallRightL.Color(i - FadeInDelay, WallColorMax);
                        wallRightR.Color(i - FadeInDelay, WallColorMax);
                    }
                }
                wallLeftL.Color(i - FadeInDelay, i + Duration, WallColorMin, WallColorMax);
                wallLeftR.Color(i - FadeInDelay, i + Duration, WallColorMin, WallColorMax);
                wallRightL.Color(i - FadeInDelay, i + spawnDelay + Duration, WallColorMin, WallColorMax);
                wallRightR.Color(i - FadeInDelay, i + spawnDelay + Duration, WallColorMin, WallColorMax);

                // additive
                // wallLeftL.Additive(i - FadeInDelay, i + Duration + ScatteringSpeed);
                // wallLeftR.Additive(i - FadeInDelay, i + Duration + ScatteringSpeed);
                // wallRightL.Additive(i - FadeInDelay, i + spawnDelay + Duration + ScatteringSpeed);
                // wallRightR.Additive(i - FadeInDelay, i + spawnDelay + Duration + ScatteringSpeed);

                // SLASHING EFFECT ///////////////////////////////////////////////////////////////////////////////////////////////
                var slashLeft = GetLayer("Slashing").CreateAnimation("sb/missions/3/slashAnimation/slash" + Random(1, 3) + "_.jpg", 8, 40, OsbLoopType.LoopOnce, OsbOrigin.Centre);
                var slashRight = GetLayer("Slashing").CreateAnimation("sb/missions/3/slashAnimation/slash" + Random(1, 3) + "_.jpg", 8, 40, OsbLoopType.LoopOnce, OsbOrigin.Centre);

                var slashDelay = -50;
                var featherAmount = Random(2, 5);
                var featherScaleAverage = Random(1, 3);
                var featherScaleEnd = Random(0.7, 0.95);
                var featherScaleX = Random(0.05, 0.2);
                var featherScaleY = Random(0.01, 0.07);
                var featherOutDelay = Random(ScatteringSpeed / 8, ScatteringSpeed / 2);
                // left
                for (int f = 0; f < featherAmount; f++)
                {
                    var featherLeft = GetLayer("Wings").CreateSprite("sb/missions/3/feather.png", OsbOrigin.Centre);

                    featherLeft.Color(i + Duration, wingsColor);
                    featherLeft.Fade(i + Duration, i + Duration + FadeOutDelay, Random(0.3, 1), 0);
                    featherLeft.Move(i + Duration, i + Duration + ScatteringSpeed, ScatteringEndPosL.X, ScatteringEndPosL.Y,
                            ScatteringEndPosL.X + Random(-200, -25), ScatteringEndPosL.Y + Random(-100, 100));

                    // rotate from top
                    if (featherLeft.PositionAt(i + Duration).Y > slashYL)
                    {
                        featherLeft.Rotate(i + Duration, i + Duration + ScatteringSpeed, 0, Random(1, 10));
                    }
                    // rotate from bottom
                    else
                    {
                        featherLeft.Rotate(i + Duration, i + Duration + ScatteringSpeed, 0, Random(-10, -1));
                    }

                    featherLeft.ScaleVec(i + Duration, i + Duration + (ScatteringSpeed / 2), (featherScaleX * featherScaleAverage), (featherScaleY * featherScaleAverage), (featherScaleX * featherScaleAverage) * -featherScaleEnd, (featherScaleY * featherScaleAverage) * featherScaleEnd);
                    featherLeft.ScaleVec(i + Duration + (ScatteringSpeed / 2), i + Duration + ScatteringSpeed, (featherScaleX * featherScaleAverage) * -featherScaleEnd, (featherScaleY * featherScaleAverage) * featherScaleEnd, (featherScaleX * featherScaleAverage) * featherScaleEnd, (featherScaleY * featherScaleAverage) * featherScaleEnd);
                }
                // right
                for (int f = 0; f < featherAmount; f++)
                {
                    var featherRight = GetLayer("Wings").CreateSprite("sb/missions/3/feather.png", OsbOrigin.Centre);

                    featherRight.Color(i + spawnDelay + Duration, wingsColor);
                    featherRight.Fade(i + spawnDelay + Duration, i + spawnDelay + Duration + FadeOutDelay, Random(0.3, 1), 0);
                    featherRight.Move(i + spawnDelay + Duration, i + spawnDelay + Duration + ScatteringSpeed, ScatteringEndPosL2.X, ScatteringEndPosL2.Y,
                            ScatteringEndPosL2.X + Random(25, 50), ScatteringEndPosL2.Y + Random(-100, 100));

                    // rotate from top
                    if (featherRight.PositionAt(i + spawnDelay + Duration).Y > slashYL)
                    {
                        featherRight.Rotate(i + spawnDelay + Duration, i + spawnDelay + Duration + ScatteringSpeed, 0, Random(-10, -1));
                    }
                    // rotate from bottom
                    else
                    {
                        featherRight.Rotate(i + spawnDelay + Duration, i + spawnDelay + Duration + ScatteringSpeed, 0, Random(1, 10));
                    }

                    featherRight.ScaleVec(i + spawnDelay + Duration, i + spawnDelay + Duration + (ScatteringSpeed / 2), (featherScaleX * featherScaleAverage), (featherScaleY * featherScaleAverage), (featherScaleX * featherScaleAverage) * -featherScaleEnd, (featherScaleY * featherScaleAverage) * featherScaleEnd);
                    featherRight.ScaleVec(i + spawnDelay + Duration + (ScatteringSpeed / 2), i + spawnDelay + Duration + ScatteringSpeed, (featherScaleX * featherScaleAverage) * -featherScaleEnd, (featherScaleY * featherScaleAverage) * featherScaleEnd, (featherScaleX * featherScaleAverage) * featherScaleEnd, (featherScaleY * featherScaleAverage) * featherScaleEnd);
                }

                slashLeft.Move(i + slashDelay + Duration, wallLeftL.PositionAt(i + slashDelay + Duration));
                slashRight.Move(i + slashDelay + spawnDelay + Duration, wallRightL.PositionAt(i + slashDelay + spawnDelay + Duration));
                slashLeft.ScaleVec(i + slashDelay + Duration, i + slashDelay + Duration + ScatteringSpeed, Random(0.2, 0.4), Random(0.4, 0.6), Random(0.2, 0.4), Random(0.4, 0.6));
                slashRight.ScaleVec(i + slashDelay + spawnDelay + Duration, i + slashDelay + spawnDelay + Duration + ScatteringSpeed, Random(0.2, 0.4), Random(0.4, 0.6), Random(0.2, 0.4), Random(0.4, 0.6));
                slashLeft.Additive(i + slashDelay + Duration, i + slashDelay + Duration + ScatteringSpeed);
                slashRight.Additive(i + slashDelay + spawnDelay + Duration, i + slashDelay + spawnDelay + Duration + ScatteringSpeed);
                // flip horizontally if it's on the right side
                if (wallRightL.PositionAt(i + slashDelay + spawnDelay + Duration).X > 320)
                {
                    slashRight.FlipH(i + slashDelay + spawnDelay + Duration, i + slashDelay + spawnDelay + Duration + ScatteringSpeed);
                }
                // flip vertically if position is > Random(280, 380)
                if (wallLeftL.PositionAt(i + slashDelay + Duration).Y > slashYL)
                {
                    slashLeft.FlipV(i + slashDelay + Duration, i + slashDelay + Duration + ScatteringSpeed);
                }
                // flip vertically if position is > Random(280, 380)
                if (wallRightL.PositionAt(i + slashDelay + spawnDelay + Duration).Y > slashYR)
                {
                    slashRight.FlipV(i + slashDelay + spawnDelay + Duration, i + slashDelay + spawnDelay + Duration + ScatteringSpeed);
                }
                // sound effects
                var slashLeftSFX = GetLayer("Slashing").CreateSample("sb/sfx/swoosh-" + Random(1, 4) + ".ogg", i + slashDelay + Duration, 100);
                var slashRightSFX = GetLayer("Slashing").CreateSample("sb/sfx/swoosh-" + Random(1, 4) + ".ogg", i + slashDelay + spawnDelay + Duration, 100);
            }




            // INTENSE WALLS
            // SMALL WALLS FROM LEFT AND RIGHT ///////////////////////////////////////
            var sTime2 = 189582;
            var eTime2 = 211525;
            var speed2 = Random(1, 5);
            var Beat2 = Beatmap.GetTimingPointAt(sTime2).BeatDuration;
            var interval2 = Random(Beat2 / 3, Beat2 * 2);
            var spawnDelay2 = Random(Beat2, Beat2 * 2);

            // left and right
            for (double i = sTime2; i < eTime2 - interval2; i += interval2)
            {
                var wallLeftL = GetLayer("Walls").CreateSprite("sb/missions/3/wallRightL.png", OsbOrigin.CentreRight);
                var wallLeftR = GetLayer("Walls").CreateSprite("sb/missions/3/wallRightR.png", OsbOrigin.CentreLeft);
                var wallRightL = GetLayer("Walls").CreateSprite("sb/missions/3/wallLeftR.png", OsbOrigin.CentreLeft);
                var wallRightR = GetLayer("Walls").CreateSprite("sb/missions/3/wallLeftL.png", OsbOrigin.CentreRight);

                var Duration = interval2 / speed2;

                var L_StartPosX = 0;
                var L_StartPosY = 400;
                var L_EndPosX = Random(180, 250);
                var L_EndPosY = 330;

                var R_StartPosX = 640;
                var R_StartPosY = 400;
                var R_EndPosX = Random(390, 460);
                var R_EndPosY = 330;

                var ScaleXL = Random(0.2f, 0.5f);
                var ScaleYL = Random(0.2f, 0.5f);

                var ScaleXR = Random(0.2f, 0.5f);
                var ScaleYR = Random(0.2f, 0.5f);

                var RandomScatterScale = Random(0.5f, 0.95f);

                var randomY = new Vector2((float)Random(70, 100), (float)Random(70, 100));
                var randomY2 = new Vector2((float)Random(20, 50), (float)Random(20, 50));

                // calculate y offset
                var offsetY = (((ScaleYL * 4) / 60) * 1000);
                var offsetY2 = (((ScaleYR * 4) / 60) * 1000);
                // end calculation

                // calculate sprite half height
                var bitmap = GetMapsetBitmap("sb/missions/3/wallRightL.png");
                var heightHalfL = 0; // (ScaleYL / 2) * bitmap.Height
                var heightHalfR = 0; // (ScaleYR / 2) * bitmap.Height
                // end calculation

                var FadeInDelay = 1000;
                var FadeOutDelay = (Random(700, 1000)) * 1.3;
                var Easing = OsbEasing.None;
                var ScatteringSpeed = FadeOutDelay;
                var slashYL = Random(150, 380);
                var slashYR = Random(150, 380);

                // left /////////////////////////////////////////////////////////////

                // (left)
                // wallLeftL.FlipH(i - FadeInDelay, i + Duration + FadeOutDelay);
                wallLeftL.Fade(i - FadeInDelay, i + Duration / 4, 0, 1);
                wallLeftL.Fade(i + Duration, i + Duration + FadeOutDelay, 1, 0);
                wallLeftL.ScaleVec(i - FadeInDelay, ScaleXL, ScaleYL);
                if (ScaleYL >= 0.35f)
                    wallLeftL.Move(Easing, i - FadeInDelay, i + Duration, L_StartPosX, L_StartPosY + heightHalfL - randomY.X, L_EndPosX, L_EndPosY + heightHalfL - randomY.Y);
                else if (ScaleYL <= 0.25f)
                    wallLeftL.Move(Easing, i - FadeInDelay, i + Duration, L_StartPosX, L_StartPosY + heightHalfL + randomY2.X, L_EndPosX, L_EndPosY + heightHalfL + randomY2.Y);
                else
                    wallLeftL.Move(Easing, i - FadeInDelay, i + Duration, L_StartPosX, L_StartPosY + heightHalfL, L_EndPosX, L_EndPosY + heightHalfL);

                // (right)
                // wallLeftR.FlipH(i - FadeInDelay, i + Duration + FadeOutDelay);
                wallLeftR.Fade(i - FadeInDelay, i + Duration / 4, 0, 1);
                wallLeftR.Fade(i + Duration, i + Duration + FadeOutDelay, 1, 0);
                wallLeftR.ScaleVec(i - FadeInDelay, ScaleXL, ScaleYL);
                if (ScaleYL >= 0.35f)
                    wallLeftR.Move(Easing, i - FadeInDelay, i + Duration, L_StartPosX, L_StartPosY + heightHalfL + (offsetY - 2.5) - randomY.X, L_EndPosX, L_EndPosY + heightHalfL + (offsetY - 2.5) - randomY.Y);
                else if (ScaleYL <= 0.25f)
                    wallLeftR.Move(Easing, i - FadeInDelay, i + Duration, L_StartPosX, L_StartPosY + heightHalfL + (offsetY - 1.5) + randomY2.X, L_EndPosX, L_EndPosY + heightHalfL + (offsetY - 1.5) + randomY2.Y);
                else
                    wallLeftR.Move(Easing, i - FadeInDelay, i + Duration, L_StartPosX, L_StartPosY + heightHalfL + (offsetY - 2), L_EndPosX, L_EndPosY + heightHalfL + (offsetY - 2));

                // (left) + (right) slashing to pieces
                var ScatteringEndPosL = wallLeftL.PositionAt(i + Duration);
                var ScatteringEndPosR = wallLeftR.PositionAt(i + Duration);

                wallLeftL.Move(i + Duration, i + Duration + ScatteringSpeed, ScatteringEndPosL.X, ScatteringEndPosL.Y,
                            ScatteringEndPosL.X + Random(-200, -100), ScatteringEndPosL.Y + Random(-200, 100));
                wallLeftR.Move(i + Duration, i + Duration + ScatteringSpeed, ScatteringEndPosR.X, ScatteringEndPosR.Y,
                            ScatteringEndPosR.X + Random(100, 200), ScatteringEndPosR.Y + Random(-200, 100));

                // rotate from top
                if (wallLeftL.PositionAt(i + Duration).Y > slashYL)
                {
                    wallLeftL.Rotate(i + Duration, i + Duration + ScatteringSpeed, 0, Random(1, 10));
                    wallLeftR.Rotate(i + Duration, i + Duration + ScatteringSpeed, 0, Random(-10, -1));
                }
                // rotate from bottom
                else
                {
                    wallLeftL.Rotate(i + Duration, i + Duration + ScatteringSpeed, 0, Random(-10, -1));
                    wallLeftR.Rotate(i + Duration, i + Duration + ScatteringSpeed, 0, Random(1, 10));
                }

                wallLeftL.ScaleVec(i + Duration, i + Duration + (ScatteringSpeed / 2), ScaleXL, ScaleYL, ScaleXL * -RandomScatterScale, ScaleYL * RandomScatterScale);
                wallLeftL.ScaleVec(i + Duration + (ScatteringSpeed / 2), i + Duration + ScatteringSpeed, ScaleXL * -RandomScatterScale, ScaleYL * RandomScatterScale, ScaleXL * RandomScatterScale, ScaleYL * RandomScatterScale);
                wallLeftR.ScaleVec(i + Duration, i + Duration + (ScatteringSpeed / 2), ScaleXL, ScaleYL, ScaleXL * -RandomScatterScale, ScaleYL * RandomScatterScale);
                wallLeftR.ScaleVec(i + Duration + (ScatteringSpeed / 2), i + Duration + ScatteringSpeed, ScaleXL * -RandomScatterScale, ScaleYL * RandomScatterScale, ScaleXL * RandomScatterScale, ScaleYL * RandomScatterScale);


                // right /////////////////////////////////////////////////////////////

                // (left)
                wallRightL.Fade(i - FadeInDelay + spawnDelay2, i + spawnDelay2 + Duration / 4, 0, 1);
                wallRightL.Fade(i + spawnDelay2 + Duration, i + spawnDelay2 + Duration + FadeOutDelay, 1, 0);
                wallRightL.ScaleVec(i - FadeInDelay + spawnDelay2, ScaleXR, ScaleYR);
                if (ScaleYR >= 0.35f)
                    wallRightL.Move(Easing, i - FadeInDelay + spawnDelay2, i + spawnDelay2 + Duration, R_StartPosX, R_StartPosY + heightHalfR - randomY.X, R_EndPosX, R_EndPosY + heightHalfR - randomY.Y);
                else if (ScaleYR <= 0.25f)
                    wallRightL.Move(Easing, i - FadeInDelay + spawnDelay2, i + spawnDelay2 + Duration, R_StartPosX, R_StartPosY + heightHalfR + randomY2.X, R_EndPosX, R_EndPosY + heightHalfR + randomY2.Y);
                else
                    wallRightL.Move(Easing, i - FadeInDelay + spawnDelay2, i + spawnDelay2 + Duration, R_StartPosX, R_StartPosY + heightHalfR, R_EndPosX, R_EndPosY + heightHalfR);

                // (right)
                wallRightR.Fade(i - FadeInDelay + spawnDelay2, i + spawnDelay2 + Duration / 4, 0, 1);
                wallRightR.Fade(i + spawnDelay2 + Duration, i + spawnDelay2 + Duration + FadeOutDelay, 1, 0);
                wallRightR.ScaleVec(i - FadeInDelay + spawnDelay2, ScaleXR, ScaleYR);
                if (ScaleYR >= 0.35f)
                    wallRightR.Move(Easing, i - FadeInDelay + spawnDelay2, i + spawnDelay2 + Duration, R_StartPosX, R_StartPosY + heightHalfR + (offsetY2 - 2.5) - randomY.X, R_EndPosX, R_EndPosY + heightHalfR + (offsetY2 - 2.5) - randomY.Y);
                else if (ScaleYR <= 0.25f)
                    wallRightR.Move(Easing, i - FadeInDelay + spawnDelay2, i + spawnDelay2 + Duration, R_StartPosX, R_StartPosY + heightHalfR + (offsetY2 - 1.5) + randomY2.X, R_EndPosX, R_EndPosY + heightHalfR + (offsetY2 - 1.5) + randomY2.Y);
                else
                    wallRightR.Move(Easing, i - FadeInDelay + spawnDelay2, i + spawnDelay2 + Duration, R_StartPosX, R_StartPosY + heightHalfR + (offsetY2 - 2), R_EndPosX, R_EndPosY + heightHalfR + (offsetY2 - 2));

                // slashing to pieces (scattering)
                var ScatteringEndPosL2 = wallRightL.PositionAt(i + spawnDelay2 + Duration);
                var ScatteringEndPosR2 = wallRightR.PositionAt(i + spawnDelay2 + Duration);

                wallRightL.Move(i + spawnDelay2 + Duration, i + spawnDelay2 + Duration + ScatteringSpeed, ScatteringEndPosL2.X, ScatteringEndPosL2.Y,
                            ScatteringEndPosL2.X + Random(100, 200), ScatteringEndPosL2.Y + Random(-200, 100));
                wallRightR.Move(i + spawnDelay2 + Duration, i + spawnDelay2 + Duration + ScatteringSpeed, ScatteringEndPosR2.X, ScatteringEndPosR2.Y,
                            ScatteringEndPosR2.X + Random(-200, -100), ScatteringEndPosR2.Y + Random(-200, 100));

                // rotate from top
                if (wallRightL.PositionAt(i + spawnDelay2 + Duration).Y > slashYR)
                {
                    wallRightL.Rotate(i + spawnDelay2 + Duration, i + spawnDelay2 + Duration + ScatteringSpeed, 0, Random(-10, -1));
                    wallRightR.Rotate(i + spawnDelay2 + Duration, i + spawnDelay2 + Duration + ScatteringSpeed, 0, Random(1, 10));
                }
                // rotate from bottom
                else
                {
                    wallRightL.Rotate(i + spawnDelay2 + Duration, i + spawnDelay2 + Duration + ScatteringSpeed, 0, Random(1, 10));
                    wallRightR.Rotate(i + spawnDelay2 + Duration, i + spawnDelay2 + Duration + ScatteringSpeed, 0, Random(-10, -1));
                }

                wallRightL.ScaleVec(i + spawnDelay2 + Duration, i + spawnDelay2 + Duration + (ScatteringSpeed / 2), ScaleXR, ScaleYR, ScaleXR * -RandomScatterScale, ScaleYR * RandomScatterScale);
                wallRightL.ScaleVec(i + spawnDelay2 + Duration + (ScatteringSpeed / 2), i + spawnDelay2 + Duration + ScatteringSpeed, ScaleXR * -RandomScatterScale, ScaleYR * RandomScatterScale, ScaleXR * RandomScatterScale, ScaleYR * RandomScatterScale);
                wallRightR.ScaleVec(i + spawnDelay2 + Duration, i + spawnDelay2 + Duration + (ScatteringSpeed / 2), ScaleXR, ScaleYR, ScaleXR * -RandomScatterScale, ScaleYR * RandomScatterScale);
                wallRightR.ScaleVec(i + spawnDelay2 + Duration + (ScatteringSpeed / 2), i + spawnDelay2 + Duration + ScatteringSpeed, ScaleXR * -RandomScatterScale, ScaleYR * RandomScatterScale, ScaleXR * RandomScatterScale, ScaleYR * RandomScatterScale);

                // left + right //////////////////////////////////////////////////////
                // color
                var nColorEvery = 2; // change the color of the sprite every chosen value
                if (NewColorEvery)
                {
                    if (i % nColorEvery == 1)
                    {
                        wallLeftL.Color(i - FadeInDelay, WallColorMin2);
                        wallLeftR.Color(i - FadeInDelay, WallColorMin2);
                        wallRightL.Color(i - FadeInDelay, WallColorMin2);
                        wallRightR.Color(i - FadeInDelay, WallColorMin2);
                    }
                    else
                    {
                        wallLeftL.Color(i - FadeInDelay, WallColorMax2);
                        wallLeftR.Color(i - FadeInDelay, WallColorMax2);
                        wallRightL.Color(i - FadeInDelay, WallColorMax2);
                        wallRightR.Color(i - FadeInDelay, WallColorMax2);
                    }
                }
                wallLeftL.Color(i - FadeInDelay, i + Duration, WallColorMin2, WallColorMax2);
                wallLeftR.Color(i - FadeInDelay, i + Duration, WallColorMin2, WallColorMax2);
                wallRightL.Color(i - FadeInDelay, i + spawnDelay2 + Duration, WallColorMin2, WallColorMax2);
                wallRightR.Color(i - FadeInDelay, i + spawnDelay2 + Duration, WallColorMin2, WallColorMax2);

                // additive
                // wallLeftL.Additive(i - FadeInDelay, i + Duration + ScatteringSpeed);
                // wallLeftR.Additive(i - FadeInDelay, i + Duration + ScatteringSpeed);
                // wallRightL.Additive(i - FadeInDelay, i + spawnDelay2 + Duration + ScatteringSpeed);
                // wallRightR.Additive(i - FadeInDelay, i + spawnDelay2 + Duration + ScatteringSpeed);

                // SLASHING EFFECT ///////////////////////////////////////////////////////////////////////////////////////////////
                var slashLeft = GetLayer("Slashing").CreateAnimation("sb/missions/3/slashAnimation/slash" + Random(1, 3) + "_.jpg", 8, 40, OsbLoopType.LoopOnce, OsbOrigin.Centre);
                var slashRight = GetLayer("Slashing").CreateAnimation("sb/missions/3/slashAnimation/slash" + Random(1, 3) + "_.jpg", 8, 40, OsbLoopType.LoopOnce, OsbOrigin.Centre);

                var slashDelay = -50;
                var featherAmount = Random(1, 2);
                var featherScaleAverage = Random(1, 3);
                var featherScaleEnd = Random(0.7, 0.95);
                var featherScaleX = Random(0.05, 0.2);
                var featherScaleY = Random(0.01, 0.07);
                var featherOutDelay = Random(ScatteringSpeed / 8, ScatteringSpeed / 2);
                // left
                for (int f = 0; f < featherAmount; f++)
                {
                    var featherLeft = GetLayer("Wings").CreateSprite("sb/missions/3/feather.png", OsbOrigin.Centre);

                    featherLeft.Color(i + Duration, wingsColor);
                    featherLeft.Fade(i + Duration, i + Duration + FadeOutDelay, Random(0.3, 1), 0);
                    featherLeft.Move(i + Duration, i + Duration + ScatteringSpeed, ScatteringEndPosL.X, ScatteringEndPosL.Y,
                            ScatteringEndPosL.X + Random(-200, -25), ScatteringEndPosL.Y + Random(-100, 100));

                    // rotate from top
                    if (featherLeft.PositionAt(i + Duration).Y > slashYL)
                    {
                        featherLeft.Rotate(i + Duration, i + Duration + ScatteringSpeed, 0, Random(1, 10));
                    }
                    // rotate from bottom
                    else
                    {
                        featherLeft.Rotate(i + Duration, i + Duration + ScatteringSpeed, 0, Random(-10, -1));
                    }

                    featherLeft.ScaleVec(i + Duration, i + Duration + (ScatteringSpeed / 2), (featherScaleX * featherScaleAverage), (featherScaleY * featherScaleAverage), (featherScaleX * featherScaleAverage) * -featherScaleEnd, (featherScaleY * featherScaleAverage) * featherScaleEnd);
                    featherLeft.ScaleVec(i + Duration + (ScatteringSpeed / 2), i + Duration + ScatteringSpeed, (featherScaleX * featherScaleAverage) * -featherScaleEnd, (featherScaleY * featherScaleAverage) * featherScaleEnd, (featherScaleX * featherScaleAverage) * featherScaleEnd, (featherScaleY * featherScaleAverage) * featherScaleEnd);
                }
                // right
                for (int f = 0; f < featherAmount; f++)
                {
                    var featherRight = GetLayer("Wings").CreateSprite("sb/missions/3/feather.png", OsbOrigin.Centre);

                    featherRight.Color(i + spawnDelay + Duration, wingsColor);
                    featherRight.Fade(i + spawnDelay + Duration, i + spawnDelay + Duration + FadeOutDelay, Random(0.3, 1), 0);
                    featherRight.Move(i + spawnDelay + Duration, i + spawnDelay + Duration + ScatteringSpeed, ScatteringEndPosL2.X, ScatteringEndPosL2.Y,
                            ScatteringEndPosL2.X + Random(25, 50), ScatteringEndPosL2.Y + Random(-100, 100));

                    // rotate from top
                    if (featherRight.PositionAt(i + spawnDelay + Duration).Y > slashYL)
                    {
                        featherRight.Rotate(i + spawnDelay + Duration, i + spawnDelay + Duration + ScatteringSpeed, 0, Random(-10, -1));
                    }
                    // rotate from bottom
                    else
                    {
                        featherRight.Rotate(i + spawnDelay + Duration, i + spawnDelay + Duration + ScatteringSpeed, 0, Random(1, 10));
                    }

                    featherRight.ScaleVec(i + spawnDelay + Duration, i + spawnDelay + Duration + (ScatteringSpeed / 2), (featherScaleX * featherScaleAverage), (featherScaleY * featherScaleAverage), (featherScaleX * featherScaleAverage) * -featherScaleEnd, (featherScaleY * featherScaleAverage) * featherScaleEnd);
                    featherRight.ScaleVec(i + spawnDelay + Duration + (ScatteringSpeed / 2), i + spawnDelay + Duration + ScatteringSpeed, (featherScaleX * featherScaleAverage) * -featherScaleEnd, (featherScaleY * featherScaleAverage) * featherScaleEnd, (featherScaleX * featherScaleAverage) * featherScaleEnd, (featherScaleY * featherScaleAverage) * featherScaleEnd);
                }

                slashLeft.Move(i + slashDelay + Duration, wallLeftL.PositionAt(i + slashDelay + Duration));
                slashRight.Move(i + slashDelay + spawnDelay2 + Duration, wallRightL.PositionAt(i + slashDelay + spawnDelay2 + Duration));
                slashLeft.ScaleVec(i + slashDelay + Duration, i + slashDelay + Duration + ScatteringSpeed, Random(0.2, 0.4), Random(0.4, 0.6), Random(0.2, 0.4), Random(0.4, 0.6));
                slashRight.ScaleVec(i + slashDelay + spawnDelay2 + Duration, i + slashDelay + spawnDelay2 + Duration + ScatteringSpeed, Random(0.2, 0.4), Random(0.4, 0.6), Random(0.2, 0.4), Random(0.4, 0.6));
                slashLeft.Additive(i + slashDelay + Duration, i + slashDelay + Duration + ScatteringSpeed);
                slashRight.Additive(i + slashDelay + spawnDelay2 + Duration, i + slashDelay + spawnDelay2 + Duration + ScatteringSpeed);
                // flip horizontally if it's on the right side
                if (wallRightL.PositionAt(i + slashDelay + spawnDelay2 + Duration).X > 320)
                {
                    slashRight.FlipH(i + slashDelay + spawnDelay2 + Duration, i + slashDelay + spawnDelay2 + Duration + ScatteringSpeed);
                }
                // flip vertically if position is > Random(280, 380)
                if (wallLeftL.PositionAt(i + slashDelay + Duration).Y > slashYL)
                {
                    slashLeft.FlipV(i + slashDelay + Duration, i + slashDelay + Duration + ScatteringSpeed);
                }
                // flip vertically if position is > Random(280, 380)
                if (wallRightL.PositionAt(i + slashDelay + spawnDelay2 + Duration).Y > slashYR)
                {
                    slashRight.FlipV(i + slashDelay + spawnDelay2 + Duration, i + slashDelay + spawnDelay2 + Duration + ScatteringSpeed);
                }
                // sound effects
                var slashLeftSFX = GetLayer("Slashing").CreateSample("sb/sfx/swoosh-" + Random(1, 4) + ".ogg", i + slashDelay + Duration, 100);
                var slashRightSFX = GetLayer("Slashing").CreateSample("sb/sfx/swoosh-" + Random(1, 4) + ".ogg", i + slashDelay + spawnDelay2 + Duration, 100);
            }

            // AVATAR //////////////////////////////////////////////////////////////////////////////////////
            var avatar = GetLayer("Avatar").CreateSprite("sb/avatars/ScubDomino.png", OsbOrigin.Centre);
            var wingRight = GetLayer("Wings").CreateSprite("sb/missions/3/wing.png", OsbOrigin.CentreLeft);
            var wingLeft = GetLayer("Wings").CreateSprite("sb/missions/3/wing.png", OsbOrigin.CentreLeft);

            var delay = 500;

            avatar.MoveX(startTime, 320);
            avatar.Scale(startTime, 0.16f);
            avatar.Fade(startTime, startTime + 800, 0, 1);
            avatar.Fade(endTime + delay, endTime + delay + 1000, 1, 0);

            wingRight.Color(startTime, wingsColor);
            wingRight.MoveX(startTime, 320 - 2);
            wingRight.Fade(startTime + 300, startTime + 300 + 800, 0, 1);
            wingRight.Fade(endTime + delay, endTime + delay + 1000, 1, 0);

            wingLeft.MoveX(startTime, 320 - 2);
            wingLeft.Color(startTime, wingsColor);
            wingLeft.FlipV(startTime, endTime + delay + 1000);
            wingLeft.Fade(startTime + 300, startTime + 300 + 800, 0, 1);
            wingLeft.Fade(endTime + delay, endTime + delay + 1000, 1, 0);
            wingLeft.Rotate(startTime, MathHelper.DegreesToRadians(180));

            // hover avatar + wings
            var posY = 265;
            var wingsPosY = 220;
            var height = 10;
            var loopSpeed = 3500;
            var duration = (189582) - startTime;

            avatar.StartLoopGroup(startTime, (duration / loopSpeed));
            avatar.MoveY(OsbEasing.InOutQuad, 0, loopSpeed / 2, posY + height, posY - (height + 15));
            avatar.MoveY(OsbEasing.InOutSine, loopSpeed / 2, loopSpeed, posY - (height + 15), posY + height);
            avatar.EndGroup();

            wingRight.StartLoopGroup(startTime, (duration / loopSpeed));
            wingRight.MoveY(OsbEasing.InOutQuad, 0, loopSpeed / 2, wingsPosY + height, wingsPosY - (height + 15));
            wingRight.MoveY(OsbEasing.InOutSine, loopSpeed / 2, loopSpeed, wingsPosY - (height + 15), wingsPosY + height);
            wingRight.Rotate(OsbEasing.InOutQuad, 0, loopSpeed / 2, -0.5, 0.7);
            wingRight.Rotate(OsbEasing.InOutSine, loopSpeed / 2, loopSpeed, 0.7, -0.5);
            wingRight.ScaleVec(OsbEasing.InOutQuad, 0, loopSpeed / 2, 0.27f, 0.2f, 0.25f, 0.05f);
            wingRight.ScaleVec(OsbEasing.InOutSine, loopSpeed / 2, loopSpeed, 0.25f, 0.05f, 0.27f, 0.2f);
            wingRight.EndGroup();

            wingLeft.StartLoopGroup(startTime, (duration / loopSpeed));
            wingLeft.MoveY(OsbEasing.InOutQuad, 0, loopSpeed / 2, wingsPosY + height, wingsPosY - (height + 15));
            wingLeft.MoveY(OsbEasing.InOutSine, loopSpeed / 2, loopSpeed, wingsPosY - (height + 15), wingsPosY + height);
            wingLeft.Rotate(OsbEasing.InOutQuad, 0, loopSpeed / 2, wingLeft.RotationAt(startTime) + 0.5, wingLeft.RotationAt(startTime) + -0.7);
            wingLeft.Rotate(OsbEasing.InOutSine, loopSpeed / 2, loopSpeed, wingLeft.RotationAt(startTime) + -0.7, wingLeft.RotationAt(startTime) + 0.5);
            wingLeft.ScaleVec(OsbEasing.InOutQuad, 0, loopSpeed / 2, 0.27f, 0.2f, 0.25f, 0.05f);
            wingLeft.ScaleVec(OsbEasing.InOutSine, loopSpeed / 2, loopSpeed, 0.25f, 0.05f, 0.27f, 0.2f);
            wingLeft.EndGroup();

            // intense part
            var posY2 = 265;
            var wingsPosY2 = 220;
            var height2 = 8;
            var loopSpeed2 = 1000;
            var startTime2 = 189582;
            var duration2 = (endTime + delay) - startTime2;

            avatar.StartLoopGroup(startTime2, (duration2 / loopSpeed2) + 2);
            avatar.MoveY(OsbEasing.InOutQuad, 0, loopSpeed2 / 2, posY2 + height2, posY2 - (height2 + 15));
            avatar.MoveY(OsbEasing.InOutSine, loopSpeed2 / 2, loopSpeed2, posY2 - (height2 + 15), posY2 + height2);
            avatar.EndGroup();

            wingRight.StartLoopGroup(startTime2, (duration2 / loopSpeed2) + 2);
            wingRight.MoveY(OsbEasing.InOutQuad, 0, loopSpeed2 / 2, wingsPosY2 + height2, wingsPosY2 - (height2 + 15));
            wingRight.MoveY(OsbEasing.InOutSine, loopSpeed2 / 2, loopSpeed2, wingsPosY2 - (height2 + 15), wingsPosY2 + height2);
            wingRight.Rotate(OsbEasing.InOutQuad, 0, loopSpeed2 / 2, -0.5, 0.7);
            wingRight.Rotate(OsbEasing.InOutSine, loopSpeed2 / 2, loopSpeed2, 0.7, -0.5);
            wingRight.ScaleVec(OsbEasing.InOutQuad, 0, loopSpeed2 / 2, 0.27f, 0.2f, 0.25f, 0.05f);
            wingRight.ScaleVec(OsbEasing.InOutSine, loopSpeed2 / 2, loopSpeed2, 0.25f, 0.05f, 0.27f, 0.2f);
            wingRight.EndGroup();

            wingLeft.StartLoopGroup(startTime2, (duration2 / loopSpeed2) + 2);
            wingLeft.MoveY(OsbEasing.InOutQuad, 0, loopSpeed2 / 2, wingsPosY2 + height2, wingsPosY2 - (height2 + 15));
            wingLeft.MoveY(OsbEasing.InOutSine, loopSpeed2 / 2, loopSpeed2, wingsPosY2 - (height2 + 15), wingsPosY2 + height2);
            wingLeft.Rotate(OsbEasing.InOutQuad, 0, loopSpeed2 / 2, wingLeft.RotationAt(startTime) + 0.5, wingLeft.RotationAt(startTime) + -0.7);
            wingLeft.Rotate(OsbEasing.InOutSine, loopSpeed2 / 2, loopSpeed2, wingLeft.RotationAt(startTime) + -0.7, wingLeft.RotationAt(startTime) + 0.5);
            wingLeft.ScaleVec(OsbEasing.InOutQuad, 0, loopSpeed2 / 2, 0.27f, 0.2f, 0.25f, 0.05f);
            wingLeft.ScaleVec(OsbEasing.InOutSine, loopSpeed2 / 2, loopSpeed2, 0.25f, 0.05f, 0.27f, 0.2f);
            wingLeft.EndGroup();
        }
    }
}

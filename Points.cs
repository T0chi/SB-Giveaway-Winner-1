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
    public class Points : StoryboardObjectGenerator
    {
        [Configurable]
        public Color4 ColorFail = Color.White;

        [Configurable]
        public Color4 ColorPass = Color.White;

        private PointSystem pointSystem;

        public override void Generate()
        {
            pointSystem = new PointSystem();

            points(1, "sb/points/1", 61399, 72571, 2); // break 1
            var pointsBreak1 = new Vector2(pointSystem.pointsPass, pointSystem.pointsFail);
            currentPoints("sb/points/1", 61399, 72571, pointSystem.pointsPass, pointSystem.pointsFail);

            points(2, "sb/points/2", 115238, 128262, 2); // break 2
            var pointsBreak2 = new Vector2(pointSystem.pointsPass, pointSystem.pointsFail);
            currentPoints("sb/points/2", 115238, 128262, pointSystem.pointsPass + (int)pointsBreak1.X,
                                                         pointSystem.pointsFail + (int)pointsBreak1.Y);

            points(3, "sb/points/3", 157062, 167640, 2); // break 3
            var pointsBreak3 = new Vector2(pointSystem.pointsPass + pointsBreak2.X,
                                           pointSystem.pointsFail + pointsBreak2.Y);
            currentPoints("sb/points/3", 157062, 167640, pointSystem.pointsPass + (int)pointsBreak3.X,
                                                         pointSystem.pointsFail + (int)pointsBreak3.Y);

            points(4, "sb/points/4", 213925, 221716, 2); // break 4
            var pointsBreak4 = new Vector2(pointSystem.pointsPass + pointsBreak2.X + pointsBreak3.X,
                                           pointSystem.pointsFail + pointsBreak2.Y + pointsBreak3.Y);
            currentPoints("sb/points/4", 213925, 221716, pointSystem.pointsPass + (int)pointsBreak4.X,
                                                         pointSystem.pointsFail + (int)pointsBreak4.Y);

            points(5, "sb/points/5", 247113, 254732, 2); // break 5
            var pointsBreak5 = new Vector2(pointSystem.pointsPass + pointsBreak2.X + pointsBreak3.X + pointsBreak4.X,
                                           pointSystem.pointsFail + pointsBreak2.Y + pointsBreak3.Y + pointsBreak4.Y);
            currentPoints("sb/points/5", 247113, 254732, pointSystem.pointsPass + (int)pointsBreak5.X,
                                                         pointSystem.pointsFail + (int)pointsBreak5.Y);

            points(6, "sb/points/6", 283514, 290901, 2); // break 6
            var pointsBreak6 = new Vector2(pointSystem.pointsPass + pointsBreak2.X + pointsBreak3.X +
                                           pointsBreak4.X + pointsBreak5.X,
                                           pointSystem.pointsFail + pointsBreak2.Y + pointsBreak3.Y +
                                           pointsBreak4.Y + pointsBreak5.Y);
            currentPoints("sb/points/6", 283514, 290901, pointSystem.pointsPass + (int)pointsBreak6.X,
                                                         pointSystem.pointsFail + (int)pointsBreak6.Y);

            points(7, "sb/points/7", 331111, 339380, 2); // break 7
            var pointsBreak7 = new Vector2(pointSystem.pointsPass + pointsBreak2.X + pointsBreak3.X +
                                           pointsBreak4.X + pointsBreak5.X + pointsBreak6.X,
                                           pointSystem.pointsFail + pointsBreak2.Y + pointsBreak3.Y +
                                           pointsBreak4.Y + pointsBreak5.Y + pointsBreak6.Y);
            currentPoints("sb/points/7", 331111, 339380, pointSystem.pointsPass + (int)pointsBreak7.X,
                                                         pointSystem.pointsFail + (int)pointsBreak7.Y);


            Log($"TOTALPOINTS:                {pointSystem.totalPass} ; {pointSystem.totalFail}");
        }

        public void currentPoints(string fontPath, int startTime, int endTime, int pointsPass, int pointsFail)
        {
            var fontSize = 14;
            var GlowRadius = 0;
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            var font = LoadFont(fontPath + "currentPoints", new FontDescription()
            {
                FontPath = "Verdana",
                FontSize = fontSize,
                Color = Color4.White,
                Padding = Vector2.Zero,
                FontStyle = FontStyle.Regular,
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
            
            var pos = new Vector2(320, 173);

            string[] resultPass = { $"Overall: {pointsPass}pts" };
                var pointPass = new DialogManager(this, font, startTime + ((endTime - startTime) / 2), endTime, "Points Pass", pos.X, pos.Y, true,
                    fontSize, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/blank.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, resultPass);

            string[] resultFail = { $"Overall: {pointsFail}pts" };
                var pointFail = new DialogManager(this, font, startTime + ((endTime - startTime) / 2), endTime, "Points Fail", pos.X, pos.Y, true,
                    fontSize, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/blank.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, resultFail);
        }

        public void points(int breakNumber, string fontPath, int startTime, int breakEnd, int speed)
        {
            // for the "you gained..." etc font
            var fontSize = 18;
            var GlowRadius = 0;
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            var font = LoadFont(fontPath, new FontDescription()
            {
                FontPath = "Verdana",
                FontSize = fontSize,
                Color = Color4.White,
                Padding = Vector2.Zero,
                FontStyle = FontStyle.Regular,
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
            
            // for the "pts" font
            var fontSize2 = 20;
            var GlowRadius2 = 0;
            var ShadowThickness2 = 0;
            var OutlineThickness2 = 0;
            var font2 = LoadFont(fontPath + "pts", new FontDescription()
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
                Radius = true ? 0 : GlowRadius2,
                Color = Color4.Black,
            },
            new FontOutline()
            {
                Thickness = OutlineThickness2,
                Color = Color4.Black,
            },
            new FontShadow()
            {
                Thickness = ShadowThickness2,
                Color = Color4.Black,
            });
















            
            var thresholdDelay = 5000; // when the fail/pass should be taking effect

            // Pass Points ------------------------------------------------------------------------------------------------
            
            var delay = (breakEnd - startTime) / 2 - 800;
            var endTime = startTime + delay;
            var randomOnePass = Random(0, 10);
            var randomTenPass = Random(0, 10);
            var randomHundredPass = Random(0, 10);
            var randomThousandPass = Random(0, 10);

            var fade = 1;

            // numbers
            
            var frameDelayOne = 100 / speed;
            var frameDelayTen = 400 / speed;
            var frameDelayHundred = 600 / speed;
            var frameDelayThousand = 800 / speed;
            var dot = GetLayer("Points Pass").CreateSprite("sb/points/dot.png", OsbOrigin.Centre);
            var one = GetLayer("Points Pass").CreateAnimation("sb/points/n.png", randomOnePass + 1, frameDelayOne, OsbLoopType.LoopOnce, OsbOrigin.CentreLeft);
            var ten = GetLayer("Points Pass").CreateAnimation("sb/points/n.png", randomTenPass + 1, frameDelayTen, OsbLoopType.LoopOnce, OsbOrigin.CentreLeft);
            var hundred = GetLayer("Points Pass").CreateAnimation("sb/points/n.png", randomHundredPass + 1, frameDelayHundred, OsbLoopType.LoopOnce, OsbOrigin.CentreRight);
            var thousand = GetLayer("Points Pass").CreateAnimation("sb/points/n.png", randomThousandPass + 1, frameDelayThousand, OsbLoopType.LoopOnce, OsbOrigin.CentreRight);


            // scale stuff

            var scale = new Vector2(0.5f, 0.5f);
            dot.ScaleVec(startTime, scale.X, scale.Y);
            one.ScaleVec(startTime, scale.X, scale.Y);
            ten.ScaleVec(startTime, scale.X, scale.Y);
            hundred.ScaleVec(startTime, scale.X, scale.Y);
            thousand.ScaleVec(startTime, scale.X, scale.Y);


            // positions

            var bitmap = GetMapsetBitmap("sb/points/n0.png");
            float numberWidth = bitmap.Width * scale.X;
            float numberHeight = bitmap.Height * scale.Y;
            var bitmapDot = GetMapsetBitmap("sb/points/dot.png");
            float dotWidth = bitmap.Width * scale.X;

            var pos = new Vector2(320, 160);
            var posDot = new Vector2(pos.X - numberWidth - 1, pos.Y);
            var posOne = new Vector2(pos.X + numberWidth, pos.Y);
            var posTen = new Vector2(pos.X, pos.Y);
            var posHundred = new Vector2(pos.X, pos.Y);
            var posThousand = new Vector2(pos.X - numberWidth - 2, pos.Y);

            dot.Move(startTime, posDot.X - (numberWidth * 1.5f), posDot.Y);
            dot.Move(startTime + frameDelayTen, posDot.X - numberWidth, posDot.Y);
            dot.Move(startTime + frameDelayHundred, posDot.X - (numberWidth / 2), posDot.Y);

            one.Move(startTime, posOne.X - (numberWidth * 1.5f), posOne.Y);
            one.Move(startTime + frameDelayTen, posOne.X - numberWidth, posOne.Y);

            ten.Move(startTime, posTen.X - (numberWidth * 1.5f), posTen.Y);
            ten.Move(startTime + frameDelayTen, posTen.X - numberWidth, posTen.Y);

            hundred.Move(startTime, posHundred.X - (numberWidth * 1.5f), posHundred.Y);
            hundred.Move(startTime + frameDelayTen, posHundred.X - numberWidth, posHundred.Y);

            thousand.Move(startTime, posThousand.X - (numberWidth * 1.5f), posThousand.Y);
            thousand.Move(startTime + frameDelayTen, posThousand.X - numberWidth, posThousand.Y);

            // opacity

            dot.Fade(startTime - thresholdDelay, startTime + frameDelayThousand, 0, 0);

            one.Fade(startTime - thresholdDelay, startTime, 0, 0);
            one.Fade(startTime, endTime, fade, fade);

            ten.Fade(startTime - thresholdDelay, startTime + frameDelayTen, 0, 0);
            ten.Fade(startTime + frameDelayTen, endTime, fade, fade);

            hundred.Fade(startTime - thresholdDelay, startTime + frameDelayHundred, 0, 0);
            hundred.Fade(startTime + frameDelayHundred, endTime, fade, fade);

            thousand.Fade(startTime - thresholdDelay, startTime + frameDelayThousand, 0, 0);
    
            
            if (randomThousandPass > 0)
            {
                string[] result = { $"You gained: {randomThousandPass}.{randomHundredPass}{randomTenPass}{randomOnePass} points" };
                var pointPass = new DialogManager(this, font, endTime, endTime + delay, "Points Pass", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 2000, ColorPass, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, result);

                int[] sectionPoints = new int[] { randomThousandPass, randomHundredPass, randomTenPass, randomOnePass };
                pointSystem.AddPassPoints(sectionPoints);
            }
            else if (randomTenPass == 0 && randomOnePass == 0)
            {
                string[] result = { $"You gained: {randomHundredPass}00pts" };
                var pointPass = new DialogManager(this, font, endTime, endTime + delay, "Points Pass", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 2000, ColorPass, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, result);

                int[] sectionPoints = new int[] { randomThousandPass, randomHundredPass, 0, 0 };
                pointSystem.AddPassPoints(sectionPoints);
            }
            else if (randomTenPass == 0)
            {
                string[] result = { $"You gained: {randomThousandPass}{randomHundredPass}0{randomOnePass} points" };
                var pointPass = new DialogManager(this, font, endTime, endTime + delay, "Points Pass", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 2000, ColorPass, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, result);

                int[] sectionPoints = new int[] { randomThousandPass, randomHundredPass, 0, randomOnePass };
                pointSystem.AddPassPoints(sectionPoints);
            }
            else if (randomOnePass == 0)
            {
                string[] result = { $"You gained: {randomThousandPass}{randomHundredPass}{randomTenPass}0pts" };
                var pointPass = new DialogManager(this, font, endTime, endTime + delay, "Points Pass", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 2000, ColorPass, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, result);

                int[] sectionPoints = new int[] { randomThousandPass, randomHundredPass, randomTenPass, 0 };
                pointSystem.AddPassPoints(sectionPoints);
            }
            else if (randomThousandPass == 0)
            {
                string[] result = { $"You gained: {randomHundredPass}{randomTenPass}{randomOnePass} points" };
                var pointPass = new DialogManager(this, font, endTime, endTime + delay, "Points Pass", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 2000, ColorPass, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, result);

                int[] sectionPoints = new int[] { 0, randomHundredPass, randomTenPass, randomOnePass };
                pointSystem.AddPassPoints(sectionPoints);
            }

            if (randomThousandPass == 0)
            {
                dot.Move(startTime + frameDelayHundred, posDot.X - (numberWidth / 2), posDot.Y);
                one.Move(startTime + frameDelayHundred, posOne.X - (numberWidth / 2), posOne.Y);
                ten.Move(startTime + frameDelayHundred, posTen.X - (numberWidth / 2), posTen.Y);
                thousand.Move(startTime + frameDelayHundred, posThousand.X - (numberWidth / 2), posThousand.Y);

                string[] pts = { "pts" };   
                var text = new DialogManager(this, font2, startTime, startTime + frameDelayTen, "Points Pass", pos.X - (numberWidth * 1.5f) + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pts);
                var text2 = new DialogManager(this, font2, startTime + frameDelayTen, startTime + frameDelayHundred, "Points Pass", pos.X - numberWidth + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pts);
                var text3 = new DialogManager(this, font2, startTime + frameDelayHundred, endTime, "Points Pass", pos.X - (numberWidth / 2) + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pts);

                thousand.Fade(startTime + frameDelayThousand, endTime, 0, 0);
                dot.Fade(startTime + frameDelayThousand, endTime, 0, 0);
            }
            else
            {
                dot.Move(startTime + frameDelayThousand, posDot);
                one.Move(startTime + frameDelayThousand, posOne);
                ten.Move(startTime + frameDelayThousand, posTen);
                hundred.Move(startTime + frameDelayThousand, posHundred);
                thousand.Move(startTime + frameDelayThousand, posThousand);

                string[] pts = { "pts" };   
                var text = new DialogManager(this, font2, startTime, startTime + frameDelayTen, "Points Pass", pos.X - (numberWidth * 1.5f) + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pts);
                var text2 = new DialogManager(this, font2, startTime + frameDelayTen, startTime + frameDelayHundred, "Points Pass", pos.X - numberWidth + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pts);
                var text3 = new DialogManager(this, font2, startTime + frameDelayHundred, startTime + frameDelayThousand, "Points Pass", pos.X - (numberWidth / 2) + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pts);
                var text4 = new DialogManager(this, font2, startTime + frameDelayThousand, endTime, "Points Pass", pos.X + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pts);
    
                thousand.Fade(startTime + frameDelayThousand, endTime, fade, fade);
                dot.Fade(startTime + frameDelayThousand, endTime, fade, fade);
            }



















            // Fail Points ------------------------------------------------------------------------------------------------

            var randomOneFail = Random(0, 10);
            var randomTenFail = Random(0, 10);
            var randomHundredFail = Random(0, 6);


            // numbers

            var oneFail = GetLayer("Points Fail").CreateAnimation("sb/points/n.png", randomOneFail + 1, frameDelayOne, OsbLoopType.LoopOnce, OsbOrigin.CentreLeft);
            var tenFail = GetLayer("Points Fail").CreateAnimation("sb/points/n.png", randomTenFail + 1, frameDelayTen, OsbLoopType.LoopOnce, OsbOrigin.CentreLeft);
            var hundredFail = GetLayer("Points Fail").CreateAnimation("sb/points/n.png", randomHundredFail + 1, frameDelayHundred, OsbLoopType.LoopOnce, OsbOrigin.CentreRight);


            // scale stuff

            oneFail.ScaleVec(startTime, scale.X, scale.Y);
            tenFail.ScaleVec(startTime, scale.X, scale.Y);
            hundredFail.ScaleVec(startTime, scale.X, scale.Y);


            // positions

            oneFail.Move(startTime, posOne.X - (numberWidth * 1.5f), posOne.Y);
            oneFail.Move(startTime + frameDelayTen, posOne.X - numberWidth, posOne.Y);
            oneFail.Move(startTime + frameDelayHundred, posOne.X - (numberWidth / 2), posOne.Y);

            tenFail.Move(startTime, posTen.X - (numberWidth * 1.5f), posTen.Y);
            tenFail.Move(startTime + frameDelayTen, posTen.X - numberWidth, posTen.Y);
            tenFail.Move(startTime + frameDelayHundred, posTen.X - (numberWidth / 2), posTen.Y);

            hundredFail.Move(startTime, posHundred.X - (numberWidth * 1.5f), posHundred.Y);
            hundredFail.Move(startTime + frameDelayTen, posHundred.X - numberWidth, posHundred.Y);
            hundredFail.Move(startTime + frameDelayHundred, posHundred.X - (numberWidth / 2), posHundred.Y);


            // opacity

            oneFail.Fade(startTime - thresholdDelay, startTime, 0, 0);
            oneFail.Fade(startTime, endTime, fade, fade);

            tenFail.Fade(startTime - thresholdDelay, startTime + frameDelayTen, 0, 0);
            tenFail.Fade(startTime + frameDelayTen, endTime, fade, fade);

            hundredFail.Fade(startTime - thresholdDelay, startTime + frameDelayHundred, 0, 0);
            hundredFail.Fade(startTime + frameDelayHundred, endTime, fade, fade);


            if (randomHundredFail > 0)
            {
                string[] result = { $"You gained: {randomHundredFail}{randomTenFail}{randomOneFail} points" };
                var pointFail = new DialogManager(this, font, endTime, endTime + delay, "Points Fail", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 2000, ColorFail, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, result);

                int[] sectionPoints = new int[] { 0, randomHundredFail, randomTenFail, randomOneFail };
                pointSystem.AddFailPoints(sectionPoints);
            }
            else if (randomTenFail == 0 && randomOneFail == 0)
            {
                string[] result = { $"You gained: {randomHundredFail}00pts" };
                var pointFail = new DialogManager(this, font, endTime, endTime + delay, "Points Fail", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 2000, ColorFail, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, result);

                int[] sectionPoints = new int[] { 0, randomHundredFail, 0, 0 };
                pointSystem.AddFailPoints(sectionPoints);
            }
            else if (randomTenFail == 0)
            {
                string[] result = { $"You gained: {randomHundredFail}0{randomOneFail} points" };
                var pointFail = new DialogManager(this, font, endTime, endTime + delay, "Points Fail", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 2000, ColorFail, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, result);

                int[] sectionPoints = new int[] { 0, randomHundredFail, 0, randomOneFail };
                pointSystem.AddFailPoints(sectionPoints);
            }
            else if (randomOneFail == 0)
            {
                string[] result = { $"You gained: {randomHundredFail}{randomTenFail}0pts" };
                var pointFail = new DialogManager(this, font, endTime, endTime + delay, "Points Fail", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 2000, ColorFail, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, result);

                int[] sectionPoints = new int[] { 0, randomHundredFail, randomTenFail, 0 };
                pointSystem.AddFailPoints(sectionPoints);
            }

            string[] pts2 = { "pts" };   
            var text5 = new DialogManager(this, font2, startTime, startTime + frameDelayTen, "Points Fail", pos.X - (numberWidth * 1.5f) + (numberWidth * 2.7f), pos.Y - 2, true,
                fontSize2, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/points-result.ogg",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pts2);
            var text6 = new DialogManager(this, font2, startTime + frameDelayTen, startTime + frameDelayHundred, "Points Fail", pos.X - numberWidth + (numberWidth * 2.7f), pos.Y - 2, true,
                fontSize2, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/points-result.ogg",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pts2);
            var text7 = new DialogManager(this, font2, startTime + frameDelayHundred, endTime, "Points Fail", pos.X - (numberWidth / 2) + (numberWidth * 2.7f), pos.Y - 2, true,
                fontSize2, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/points-result.ogg",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pts2);

            Log($"Pass/Fail {breakNumber}:                       {pointSystem.pointsPass} ; {pointSystem.pointsFail}");
        }
    }
}

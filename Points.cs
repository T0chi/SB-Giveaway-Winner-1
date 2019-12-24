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

        public override void Generate()
        {
            points("sb/points/1", 61399, 72571, 2); // break 1
            points("sb/points/2", 115238, 128262, 2); // break 2
            points("sb/points/3", 157062, 167640, 2); // break 3
            points("sb/points/4", 213925, 221716, 2); // break 4
            points("sb/points/5", 247113, 254732, 2); // break 5
            points("sb/points/6", 283514, 290901, 2); // break 6
            points("sb/points/7", 331111, 339380, 2); // break 7
        }

        public void points(string path, int startTime, int breakEnd, int speed)
        {
            // for the "you gained..." etc font
            var fontSize = 18;
            var GlowRadius = 0;
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            var font = LoadFont(path, new FontDescription()
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
            var font2 = LoadFont(path + "pts", new FontDescription()
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


            // Pass Points ------------------------------------------------------------------------------------------------
            
            var delay = (breakEnd - startTime) / 2 - 800;
            var endTime = startTime + delay;
            var randomOnePass = Random(1, 11);
            var randomTenPass = Random(1, 11);
            var randomHundredPass = Random(1, 11);
            var randomThousandPass = Random(1, 11);

            var fadeTime = 0;
            var fade = 1;

            // numbers
            
            var frameDelayOne = 100 / speed;
            var frameDelayTen = 400 / speed;
            var frameDelayHundred = 600 / speed;
            var frameDelayThousand = 800 / speed;
            var dot = GetLayer("Points Pass").CreateSprite("sb/points/dot.png", OsbOrigin.Centre);
            var one = GetLayer("Points Pass").CreateAnimation("sb/points/n.png", randomOnePass, frameDelayOne, OsbLoopType.LoopOnce, OsbOrigin.CentreLeft);
            var ten = GetLayer("Points Pass").CreateAnimation("sb/points/n.png", randomTenPass, frameDelayTen, OsbLoopType.LoopOnce, OsbOrigin.CentreLeft);
            var hundred = GetLayer("Points Pass").CreateAnimation("sb/points/n.png", randomHundredPass, frameDelayHundred, OsbLoopType.LoopOnce, OsbOrigin.CentreRight);
            var thousand = GetLayer("Points Pass").CreateAnimation("sb/points/n.png", randomThousandPass, frameDelayThousand, OsbLoopType.LoopOnce, OsbOrigin.CentreRight);

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

            var pos = new Vector2(320, 170);
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

            dot.Fade(startTime, startTime + frameDelayThousand, 0, 0);
            dot.Fade(endTime - fadeTime, endTime, fade, 0);

            one.Fade(startTime, startTime + fadeTime, 0, fade);
            one.Fade(endTime - fadeTime, endTime, fade, 0);

            ten.Fade(startTime, startTime + frameDelayTen, 0, 0);
            ten.Fade(startTime + frameDelayTen, endTime, fade, fade);
            ten.Fade(endTime - fadeTime, endTime, fade, 0);

            hundred.Fade(startTime, startTime + frameDelayHundred, 0, 0);
            hundred.Fade(startTime + frameDelayHundred, endTime, fade, fade);
            hundred.Fade(endTime - fadeTime, endTime, fade, 0);

            thousand.Fade(startTime, startTime + frameDelayThousand, 0, 0);
            thousand.Fade(endTime - fadeTime, endTime, fade, 0);

            if (randomThousandPass > 1)
            {
                string[] result = { $"You gained: {randomThousandPass - 1}.{randomHundredPass - 1}{randomTenPass - 1}{randomOnePass - 1} points" };
                var pointFail2 = new DialogManager(this, font, endTime, endTime + delay, "Points Pass", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 2000, ColorPass, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/points-result.wav",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, result);

                int[] empty = new int[] { -1, -1, -1 };
                int[] sectionPoints = new int[] { randomThousandPass - 1, randomHundredPass - 1, randomTenPass - 1, randomOnePass - 1 };
                var sum = new PointSystem(this, sectionPoints, empty);
            }
            else if (randomTenPass == 1 && randomOnePass == 1)
            {
                string[] result = { $"You gained: {randomHundredPass - 1}00pts" };
                var pointFail2 = new DialogManager(this, font, endTime, endTime + delay, "Points Pass", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 2000, ColorPass, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/point-result.wav",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, result);

                int[] empty = new int[] { -1, -1, -1 };
                int[] sectionPoints = new int[] { 0, randomHundredPass - 1, 0, 0 };
                var sum = new PointSystem(this, sectionPoints, empty);
            }
            else if (randomTenPass == 1)
            {
                string[] result = { $"You gained: {randomHundredPass - 1}0{randomOnePass - 1} points" };
                var pointFail2 = new DialogManager(this, font, endTime, endTime + delay, "Points Pass", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 2000, ColorPass, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/point-result.wav",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, result);

                int[] empty = new int[] { -1, -1, -1 };
                int[] sectionPoints = new int[] { 0, randomHundredPass - 1, 0, randomOnePass - 1 };
                var sum = new PointSystem(this, sectionPoints, empty);
            }
            else if (randomOnePass == 1)
            {
                string[] result = { $"You gained: {randomHundredPass - 1}{randomTenPass - 1}0pts" };
                var pointFail2 = new DialogManager(this, font, endTime, endTime + delay, "Points Pass", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 2000, ColorPass, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/point-result.wav",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, result);

                int[] empty = new int[] { -1, -1, -1 };
                int[] sectionPoints = new int[] { 0, randomHundredPass - 1, randomTenPass - 1, 0 };
                var sum = new PointSystem(this, sectionPoints, empty);
            }
            else if (randomThousandPass == 1)
            {
                string[] result = { $"You gained: {randomHundredPass - 1}{randomTenPass - 1}{randomOnePass - 1} points" };
                var pointFail2 = new DialogManager(this, font, endTime, endTime + delay, "Points Pass", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 2000, ColorPass, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/point-result.wav",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, result);

                int[] empty = new int[] { -1, -1, -1 };
                int[] sectionPoints = new int[] { 0, randomHundredPass - 1, randomTenPass - 1, randomOnePass - 1 };
                var sum = new PointSystem(this, sectionPoints, empty);
            }

            if (randomThousandPass == 1)
            {
                dot.Move(startTime + frameDelayHundred, posDot.X - (numberWidth / 2), posDot.Y);
                one.Move(startTime + frameDelayHundred, posOne.X - (numberWidth / 2), posOne.Y);
                ten.Move(startTime + frameDelayHundred, posTen.X - (numberWidth / 2), posTen.Y);
                thousand.Move(startTime + frameDelayHundred, posThousand.X - (numberWidth / 2), posThousand.Y);

                string[] pts = { "pts" };   
                var text = new DialogManager(this, font2, startTime, startTime + frameDelayTen, "Points Pass", pos.X - (numberWidth * 1.5f) + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/point-result.wav",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pts);
                var text2 = new DialogManager(this, font2, startTime + frameDelayTen, startTime + frameDelayHundred, "Points Pass", pos.X - numberWidth + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/point-result.wav",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pts);
                var text3 = new DialogManager(this, font2, startTime + frameDelayHundred, endTime, "Points Pass", pos.X - (numberWidth / 2) + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/point-result.wav",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pts);

                thousand.Fade(startTime, endTime, 0, 0);
                dot.Fade(startTime, endTime, 0, 0);
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
                    fontSize2, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/point-result.wav",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pts);
                var text2 = new DialogManager(this, font2, startTime + frameDelayTen, startTime + frameDelayHundred, "Points Pass", pos.X - numberWidth + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/point-result.wav",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pts);
                var text3 = new DialogManager(this, font2, startTime + frameDelayHundred, startTime + frameDelayThousand, "Points Pass", pos.X - (numberWidth / 2) + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/point-result.wav",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pts);
                var text4 = new DialogManager(this, font2, startTime + frameDelayThousand, endTime, "Points Pass", pos.X + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/point-result.wav",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pts);

                thousand.Fade(startTime + frameDelayThousand, endTime - fadeTime, fade, fade);
                dot.Fade(startTime + frameDelayThousand, endTime - fadeTime, fade, fade);
            }






            // Fail Points ------------------------------------------------------------------------------------------------

            var randomOneFail = Random(1, 11);
            var randomTenFail = Random(1, 11);
            var randomHundredFail = Random(1, 11);


            // numbers

            var oneFail = GetLayer("Points Fail").CreateAnimation("sb/points/n.png", randomOneFail, frameDelayOne, OsbLoopType.LoopOnce, OsbOrigin.CentreLeft);
            var tenFail = GetLayer("Points Fail").CreateAnimation("sb/points/n.png", randomTenFail, frameDelayTen, OsbLoopType.LoopOnce, OsbOrigin.CentreLeft);
            var hundredFail = GetLayer("Points Fail").CreateAnimation("sb/points/n.png", randomHundredFail, frameDelayHundred, OsbLoopType.LoopOnce, OsbOrigin.CentreRight);


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

            oneFail.Fade(startTime, startTime + fadeTime, 0, fade);
            oneFail.Fade(endTime - fadeTime, endTime, fade, 0);

            tenFail.Fade(startTime, startTime + frameDelayTen, 0, 0);
            tenFail.Fade(startTime + frameDelayTen, endTime, fade, fade);
            tenFail.Fade(endTime - fadeTime, endTime, fade, 0);

            hundredFail.Fade(startTime, startTime + frameDelayHundred, 0, 0);
            hundredFail.Fade(startTime + frameDelayHundred, endTime, fade, fade);
            hundredFail.Fade(endTime - fadeTime, endTime, fade, 0);

            if (randomHundredFail > 1)
            {
                string[] result = { $"You gained: {randomHundredFail - 1}{randomTenFail - 1}{randomOneFail - 1} points" };
                var pointFail2 = new DialogManager(this, font, endTime, endTime + delay, "Points Fail", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 2000, ColorFail, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/points-result.wav",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, result);

                int[] empty = new int[] { -1, -1, -1, -1 };
                int[] sectionPoints = new int[] { randomHundredFail - 1, randomTenFail - 1, randomOneFail - 1 };
                var sum = new PointSystem(this, empty, sectionPoints);
            }
            else if (randomTenFail == 1 && randomOneFail == 1)
            {
                string[] result = { $"You gained: {randomHundredFail - 1}00pts" };
                var pointFail2 = new DialogManager(this, font, endTime, endTime + delay, "Points Fail", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 2000, ColorFail, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/point-result.wav",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, result);

                int[] empty = new int[] { -1, -1, -1, -1 };
                int[] sectionPoints = new int[] { 0, randomHundredFail - 1, 0, 0 };
                var sum = new PointSystem(this, empty, sectionPoints);
            }
            else if (randomTenFail == 1)
            {
                string[] result = { $"You gained: {randomHundredFail - 1}0{randomOneFail - 1} points" };
                var pointFail2 = new DialogManager(this, font, endTime, endTime + delay, "Points Fail", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 2000, ColorFail, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/point-result.wav",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, result);

                int[] empty = new int[] { -1, -1, -1, -1 };
                int[] sectionPoints = new int[] { 0, randomHundredFail - 1, 0, randomOneFail - 1 };
                var sum = new PointSystem(this, empty, sectionPoints);
            }
            else if (randomOneFail == 1)
            {
                string[] result = { $"You gained: {randomHundredFail - 1}{randomTenFail - 1}0pts" };
                var pointFail2 = new DialogManager(this, font, endTime, endTime + delay, "Points Fail", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 2000, ColorFail, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/point-result.wav",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, result);

                int[] empty = new int[] { -1, -1, -1, -1 };
                int[] sectionPoints = new int[] { 0, randomHundredFail - 1, randomTenFail - 1, 0 };
                var sum = new PointSystem(this, empty, sectionPoints);
            }

                string[] pts2 = { "pts" };   
                var text5 = new DialogManager(this, font2, startTime, startTime + frameDelayTen, "Points Fail", pos.X - (numberWidth * 1.5f) + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/point-result.wav",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pts2);
                var text6 = new DialogManager(this, font2, startTime + frameDelayTen, startTime + frameDelayHundred, "Points Fail", pos.X - numberWidth + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/point-result.wav",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pts2);
                var text7 = new DialogManager(this, font2, startTime + frameDelayHundred, endTime, "Points Fail", pos.X - (numberWidth / 2) + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 50, 2000, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/point-result.wav",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pts2);
        }
    }
}

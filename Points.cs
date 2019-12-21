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
            SectionPoints();
        }

        public void SectionPoints()
        {
            var fontSize = 30;
            var GlowRadius = 0;
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            var font = LoadFont("sb/points", new FontDescription()
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
        









            var s = 0;
            var startTime2 = 115238;
            var endTime2 = 117905;

            var randomPointFail2 = Random(10, 100);
            var randomPointPass2 = Random(100, 1000);
            var intervalFail2 = (endTime2 - startTime2) / randomPointFail2;
            var intervalPass2 = (endTime2 - startTime2) / (randomPointPass2 / 10);

            // Section 2 Fail Points -----------------------------------------
            string[] pointResultFail2 = { $"{randomPointFail2}pts" };
            var pointFail2 = new DialogManager(this, font, endTime2, endTime2 + 6000, "Points Fail", 320, 150, true,
                fontSize, 0.7f, 50, 2000, ColorFail, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pointResultFail2);

            for (var i = startTime2; i < endTime2; i += intervalFail2)
            {
                s += 1;

                string[] randomNumber = { $"{s}pts" };
                var number = new DialogManager(this, font, i, i + intervalFail2, "Points Fail", 320, 150, true,
                fontSize, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, randomNumber);
            }
            
            // Section 2 Pass Points -----------------------------------------
            string[] pointResultPass2 = { $"{randomPointPass2}pts" };
            var pointPass2 = new DialogManager(this, font, endTime2, endTime2 + 6000, "Points Pass", 320, 150, true,
                fontSize, 0.7f, 50, 2000, ColorPass, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pointResultPass2);

            for (var i = startTime2; i < endTime2; i += intervalPass2)
            {
                s += 10;

                string[] randomNumber = { $"{s}pts" };
                var number = new DialogManager(this, font, i, i + intervalPass2, "Points Pass", 320, 150, true,
                fontSize, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, randomNumber);
            }









            var s2 = 0;
            var startTime3 = 160662;
            var endTime3 = 163662;

            var randomPointFail3 = Random(10, 100);
            var randomPointPass3 = Random(100, 1000);
            var intervalFail3 = (endTime3 - startTime3) / randomPointFail3;
            var intervalPass3 = (endTime3 - startTime3) / (randomPointPass3 / 10);

            // Section 3 Fail Points -----------------------------------------
            string[] pointResultFail3 = { $"{randomPointFail3}pts" };
            var pointFail3 = new DialogManager(this, font, endTime3, endTime3 + 6000, "Points Fail", 320, 150, true,
                fontSize, 0.7f, 50, 2000, ColorFail, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pointResultFail3);

            for (var i = startTime3; i < endTime3; i += intervalFail3)
            {
                s2 += 1;

                string[] randomNumber = { $"{s2}pts" };
                var number = new DialogManager(this, font, i, i + intervalFail3, "Points Fail", 320, 150, true,
                fontSize, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, randomNumber);
            }

            // Section 3 Pass Points -----------------------------------------
            string[] pointResultPass3 = { $"{randomPointPass3}pts" };
            var pointPass3 = new DialogManager(this, font, endTime3, endTime3 + 6000, "Points Pass", 320, 150, true,
                fontSize, 0.7f, 50, 2000, ColorPass, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pointResultPass3);

            for (var i = startTime3; i < endTime3; i += intervalPass3)
            {
                s2 += 10;

                string[] randomNumber = { $"{s2}pts" };
                var number = new DialogManager(this, font, i, i + intervalPass3, "Points Pass", 320, 150, true,
                fontSize, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, randomNumber);
            }

















            var s3 = 0;
            var startTime4 = 214268;
            var endTime4 = 217182;

            var randomPointFail4 = Random(10, 100);
            var randomPointPass4 = Random(100, 1000);
            var intervalFail4 = (endTime4 - startTime4) / randomPointFail4;
            var intervalPass4 = (endTime4 - startTime4) / (randomPointPass4 / 10);

            // Section 4 Fail Points -----------------------------------------
            string[] pointResultFail4 = { $"{randomPointFail4}pts" };
            var pointFail4 = new DialogManager(this, font, endTime4, endTime4 + 6000, "Points Fail", 320, 150, true,
                fontSize, 0.7f, 50, 2000, ColorFail, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pointResultFail4);

            for (var i = startTime4; i < endTime4; i += intervalFail4)
            {
                s3 += 1;

                string[] randomNumber = { $"{s3}pts" };
                var number = new DialogManager(this, font, i, i + intervalFail3, "Points Fail", 320, 150, true,
                fontSize, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, randomNumber);
            }
            
            // Section 4 Pass Points -----------------------------------------
            string[] pointResultPass4 = { $"{randomPointPass4}pts" };
            var pointPass4 = new DialogManager(this, font, endTime4, endTime4 + 6000, "Points Pass", 320, 150, true,
                fontSize, 0.7f, 50, 2000, ColorPass, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pointResultPass4);

            for (var i = startTime4; i < endTime4; i += intervalPass4)
            {
                s3 += 10;

                string[] randomNumber = { $"{s3}pts" };
                var number = new DialogManager(this, font, i, i + intervalPass4, "Points Pass", 320, 150, true,
                fontSize, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, randomNumber);
            }















            var s4 = 0;
            var startTime5 = 253462;
            var endTime5 = 256420;

            var randomPointFail5 = Random(10, 100);
            var randomPointPass5 = Random(100, 1000);
            var intervalFail5 = (endTime5 - startTime5) / randomPointFail5;
            var intervalPass5 = (endTime5 - startTime5) / (randomPointPass5 / 10);

            // Section 5 Fail Points -----------------------------------------
            string[] pointResultFail5 = { $"{randomPointFail5}pts" };
            var pointFail5 = new DialogManager(this, font, endTime5, endTime5 + 6000, "Points Fail", 320, 150, true,
                fontSize, 0.7f, 50, 2000, ColorFail, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pointResultFail5);

            for (var i = startTime5; i < endTime5; i += intervalFail5)
            {
                s4 += 1;

                string[] randomNumber = { $"{s4}pts" };
                var number = new DialogManager(this, font, i, i + intervalFail3, "Points Fail", 320, 150, true,
                fontSize, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, randomNumber);
            }
            
            // Section 5 Pass Points -----------------------------------------
            string[] pointResultPass5 = { $"{randomPointPass5}pts" };
            var pointPass5 = new DialogManager(this, font, endTime5, endTime5 + 6000, "Points Pass", 320, 150, true,
                fontSize, 0.7f, 50, 2000, ColorPass, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pointResultPass5);

            for (var i = startTime5; i < endTime5; i += intervalPass5)
            {
                s4 += 10;

                string[] randomNumber = { $"{s4}pts" };
                var number = new DialogManager(this, font, i, i + intervalPass5, "Points Pass", 320, 150, true,
                fontSize, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, randomNumber);
            }















            var s5 = 0;
            var startTime6 = 283514;
            var endTime6 = 286577;

            var randomPointFail6 = Random(10, 100);
            var randomPointPass6 = Random(100, 1000);
            var intervalFail6 = (endTime6 - startTime6) / randomPointFail6;
            var intervalPass6 = (endTime6 - startTime6) / (randomPointPass6 / 10);

            // Section 5 Fail Points -----------------------------------------
            string[] pointResultFail6 = { $"{randomPointFail6}pts" };
            var pointFail6 = new DialogManager(this, font, endTime6, endTime6 + 6000, "Points Fail", 320, 150, true,
                fontSize, 0.7f, 50, 2000, ColorFail, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pointResultFail6);

            for (var i = startTime6; i < endTime6; i += intervalFail6)
            {
                s5 += 1;

                string[] randomNumber = { $"{s5}pts" };
                var number = new DialogManager(this, font, i, i + intervalFail3, "Points Fail", 320, 150, true,
                fontSize, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, randomNumber);
            }
            
            // Section 5 Pass Points -----------------------------------------
            string[] pointResultPass6 = { $"{randomPointPass6}pts" };
            var pointPass6 = new DialogManager(this, font, endTime6, endTime6 + 6000, "Points Pass", 320, 150, true,
                fontSize, 0.7f, 50, 2000, ColorPass, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, pointResultPass6);

            for (var i = startTime6; i < endTime6; i += intervalPass6)
            {
                s5 += 10;

                string[] randomNumber = { $"{s5}pts" };
                var number = new DialogManager(this, font, i, i + intervalPass6, "Points Pass", 320, 150, true,
                fontSize, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, randomNumber);
            }
        }
    }
}

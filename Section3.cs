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
    public class Section3 : StoryboardObjectGenerator
    {
        [Configurable]
        public Color4 ThemeColor = Color4.White;

        [Configurable]
        public Color4 PetalColorMin = Color4.White;

        [Configurable]
        public Color4 PetalColorMax = Color4.Cyan;

        public override void Generate()
        {
            Dialog();
            Background();
            ItemCollect();
            Tochi(119905, 137262);
            Petals(128262, 157062);
            HUD(115238, 157062, 128262, "Mission #2", "Adaptation Window", "sb/HUD/txt/nameTag/Acyl.png", 4500, "sb/avatars/AcylProfile.png");
        }

        public void Background()
        {
            var bitmap = GetMapsetBitmap("sb/bgs/3/bg.jpg");
            var bg = GetLayer("Background").CreateSprite("sb/bgs/3/bg.jpg", OsbOrigin.Centre, new Vector2(320, 240));

            bg.Scale(115238, 480.0f / bitmap.Height);
            bg.Fade(115238, 128262, 0, 0.3);
            bg.Fade(157062, 167640, 0.3, 0);
        }

        public void HUD(int startTime, int endTime, int loadingTextEndtime, string mission, string songName, string nameTag, int progressBarDelay, string avatar)
        {
            var hud = new HUD(this, startTime, endTime, loadingTextEndtime, mission, songName, nameTag, progressBarDelay, avatar);
        }

        public void ItemCollect()
        {
            // Item Collect
            var itemcollect = new ItemCollect(this, "sb/avatars/Acyl.png", 0.25f, 4000, 620, 470,
                                                    "items", 1, 380, 450, false,
                                                    128262, 157062, ThemeColor, 2000, 6000);
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
            string[] sentence = { "What a relief! Did you enjoy your first quest?",
                                  "Well, no worry. You will have to do something easier for this section.",
                                  "Which is... to obtain a few items from the ground." };
            var dialog = new DialogManager(this, font, 119905, 131862, "-Tochi", 105, 326, false,
                fontSize, 1, 50, 500, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.wav",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "Easy right?",
                                   "Good luck, fellow player!" };
            var dialog2 = new DialogManager(this, font, 133662, 137262, "-Tochi", 105, 326, false,
                fontSize, 1, 50, 500, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.wav",
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

        public void Petals(int startTime, int endTime)
        {
            var interval = 200;
            for (var i = startTime; i < endTime; i += interval)
            {
                var randomEasingX = true;
                var randomEasingY = true;
                var easingX = randomEasingX ? OsbEasing.OutQuad : OsbEasing.None;
                var easingY = randomEasingY ? OsbEasing.None : OsbEasing.Out;

                var duration = Random(6000, 20000);
                var sPosFront = new Vector2(Random(-127, -117), Random(-20, -10));
                var ePosFront = new Vector2(Random(-90, 640), Random(370, 470));
                var sPosBack = new Vector2(Random(-128, -118), Random(-21, -11));
                var ePosBack = new Vector2(Random(-91, 641), Random(371, 471));
                var Rotation = Random(MathHelper.DegreesToRadians(-180), MathHelper.DegreesToRadians(180));

                var petalFront = GetLayer("PetalsFront").CreateSprite("sb/p.png", OsbOrigin.Centre); // foreground
                var petalBack = GetLayer("PetalsBack").CreateSprite("sb/p.png", OsbOrigin.Centre); // background
                
                petalFront.MoveX(easingX, i, i + duration, sPosFront.X, ePosFront.X);
                petalFront.MoveY(easingY, i, i + duration, sPosFront.Y, ePosFront.Y);
                petalBack.MoveX(easingX, i, i + duration, sPosBack.X, ePosBack.X);
                petalBack.MoveY(easingY, i, i + duration, sPosBack.Y, ePosBack.Y);
                
                var Scale = Random(0.1, 0.05);
                petalFront.ScaleVec(i, i + (duration / 4), Scale, Scale, -Scale, Scale);
                petalFront.ScaleVec(i + (duration / 4), i + (duration / 3), -Scale, Scale, Scale, Scale);
                petalFront.ScaleVec(i + (duration / 3), i + (duration / 2), Scale, Scale, -Scale, Scale);
                petalFront.ScaleVec(i + (duration / 2), i + (duration / 1), -Scale, Scale, Scale, Scale);
                petalBack.ScaleVec(i, i + (duration / 4), Scale, Scale, -Scale, Scale);
                petalBack.ScaleVec(i + (duration / 4), i + (duration / 3), -Scale, Scale, Scale, Scale);
                petalBack.ScaleVec(i + (duration / 3), i + (duration / 2), Scale, Scale, -Scale, Scale);
                petalBack.ScaleVec(i + (duration / 2), i + (duration / 1), -Scale, Scale, Scale, Scale);

                // Fade stuff
                var Fade = Random(0.4, 0.08);
                petalFront.Fade(OsbEasing.InExpo, i, i + Random(2000, 4000), 0, Fade);
                petalBack.Fade(OsbEasing.InExpo, i, i + Random(2000, 4000), 0, Fade);
                if (i + duration - 2000 <= endTime)
                {
                    petalFront.Fade(i + duration - Random(500, 2000), i + duration, Fade, 0);
                    petalBack.Fade(i + duration - Random(500, 2000), i + duration, Fade, 0);
                }

                else
                {
                    petalFront.Fade(endTime, endTime + 1000, Fade, 0);
                    petalBack.Fade(endTime, endTime + 1000, Fade, 0);
                }

                petalFront.Additive(i, i + duration);
                petalFront.Rotate(i, i + duration, Rotation, Rotation + Random(-50, 50));
                petalBack.Additive(i, i + duration);
                petalBack.Rotate(i, i + duration, Rotation, Rotation + Random(-50, 50));
                
                var RandomColor = true;
                var Color = RandomColor ? new Color4((float)Random(PetalColorMin.R, PetalColorMax.R),
                                                        (float)Random(PetalColorMin.G, PetalColorMax.G),
                                                        (float)Random(PetalColorMin.B, PetalColorMax.B),
                                                        255
                                                        ) : PetalColorMin;
                petalFront.Color(i, Color);
                petalBack.Color(i, Color);
            }
        }
    }
}

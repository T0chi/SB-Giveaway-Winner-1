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
    public class Section6 : StoryboardObjectGenerator
    {
        [Configurable]
        public Color4 ThemeColor = Color4.White;

        private DialogManager dialog;

        private DialogManager dialog2;

        public override void Generate()
        {
            Dialog();
            Background();
            Mission();
            Tochi(255870, 265294);
            HUD(253462, 283671, 262781, "Mission #5", "Dstorv", "sb/HUD/txt/nameTag/Moecho.png", 4500, "sb/avatars/MoechoProfile.png");
        }

        public void Background()
        {
            var bitmap = GetMapsetBitmap("sb/bgs/6/bg.jpg");
            var bg = GetLayer("Background").CreateSprite("sb/bgs/6/bg.jpg", OsbOrigin.Centre, new Vector2(320, 240));

            bg.Scale(247113, 854.0f / bitmap.Width);
            bg.Fade(247113, 262781, 0, 0.5);
            bg.Fade(283671, 290901, 0.5, 0);
        }

        public void HUD(int startTime, int endTime, int loadingTextEndtime, string mission, string songName, string nameTag, int progressBarDelay, string avatar)
        {
            var hud = new HUD(this, startTime, endTime, loadingTextEndtime, mission, songName, nameTag, progressBarDelay, avatar);
        }

        public void Mission()
        {
            // Item Collect
            var itemcollect = new ItemCollect(this, "sb/avatars/Moecho.png", 0.2f, 2000, 100, 450,
                                                    "items/artifacts", 0.8f, 410, 460, true,
                                                    262781, 283671, ThemeColor, 3000, 6500);
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
            this.dialog = new DialogManager(this, font, 255870, 260268, "-Tochi", 105, 326, false,
                fontSize, 1, 50, 500, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.ogg",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "Just collect a few artifacts from the ground.",
                                   "Should be easy!" };
            this.dialog2 = new DialogManager(this, font, 260268, 265294, "-Tochi", 105, 326, false,
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
    }
}

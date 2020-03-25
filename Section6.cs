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
            Tochi(252193, 265294);
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
            // var fontSize = 13; //  japanese
            var fontSize = 15; // english
            var GlowRadius = 15;
            var GlowColor = new Color4(150, 150, 150, 255);
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            // var font = LoadFont("sb/dialog/txt/jp/2", new FontDescription() // japanese
            var font = LoadFont("sb/dialog/txt/6", new FontDescription() // english
            {
                // FontPath = "font/jp/KozGoPro-Light.otf", // japanese
                FontPath = "Microsoft Yi Baiti", // english
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
                Radius = false ? 0 : GlowRadius,
                Color = GlowColor,
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
            string[] sentence = { "Well executed!",
                                  "However, the next mission involves slightly more technical knowledge." };
            this.dialog = new DialogManager(this, font, 252193, 257955, "-Tochi", 105, 326, false,
                fontSize, 1, 50, 50, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.ogg",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "Retrieve broken machine parts for research from Ice Biome 3.5.",
                                   "Finger control is of utmost necessity, stay safe." };
            this.dialog2 = new DialogManager(this, font, 257955, 268278, "-Tochi", 105, 326, false,
                fontSize, 1, 50, 250, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.ogg",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence2);

            // // DIALOG 1 -----------------------------------------
            // string[] sentence = { "うまくやりましたね！",
            //                       "ただし、次のミッションは専門的な知識が必要になります。" };
            // this.dialog = new DialogManager(this, font, 252193, 257755, "-Tochi", 105, 326, false,
            //     fontSize, 1, 50, 50, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.ogg",
            //     DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence);

            // // DIALOG 2 -----------------------------------------
            // string[] sentence2 = { "研究のために氷のバイオーム3.5から壊れた機械の部品を回収してください。",
            //                        "指のコントロールが最も必要になります、気を付けて。" };
            // this.dialog2 = new DialogManager(this, font, 257755, 265294, "-Tochi", 105, 326, false,
            //     fontSize, 1, 50, 250, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.ogg",
            //     DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence2);
        }

        public void Tochi(int startTime, int endTime)
        {
            var Hoveduration = 5000;
            var loopCount = (endTime - startTime) / Hoveduration;
            var pos = new Vector2(320, 240);
            // var avatar = GetLayer("-Tochi").CreateSprite("sb/avatars/-TochiProfile.png", OsbOrigin.Centre);
            var avatar = GetLayer("-Tochi").CreateAnimation("sb/avatars/hologram/2/-TochiProfile.png", 31, 50, OsbLoopType.LoopForever, OsbOrigin.Centre);
            var ring = GetLayer("-Tochi").CreateSprite("sb/ring2.png", OsbOrigin.Centre);

            avatar.MoveX(startTime, 64);
            avatar.Scale(startTime, 0.6);
            avatar.Fade(startTime, startTime + 500, 0, 1);
            avatar.Fade(endTime, endTime + 500, 1, 0);

            avatar.StartLoopGroup(startTime, loopCount + 1);
            avatar.MoveY(OsbEasing.InOutSine, 0, Hoveduration / 2, 335, 345);
            avatar.MoveY(OsbEasing.InOutSine, Hoveduration / 2, Hoveduration, 345, 335);
            avatar.EndGroup();

            ring.MoveX(startTime, 64);
            ring.Scale(startTime, 0.3);
            ring.Fade(startTime, startTime + 500, 0, 1);
            ring.Fade(endTime, endTime + 500, 1, 0);
            var rotation = MathHelper.DegreesToRadians(180);
            ring.Rotate(startTime, endTime, -rotation, rotation);

            ring.StartLoopGroup(startTime, loopCount + 1);
            ring.MoveY(OsbEasing.InOutSine, 0, Hoveduration / 2, 335, 345);
            ring.MoveY(OsbEasing.InOutSine, Hoveduration / 2, Hoveduration, 345, 335);
            ring.EndGroup();
        }
    }
}

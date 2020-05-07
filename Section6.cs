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
            Background();
            Mission();
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
    }
}

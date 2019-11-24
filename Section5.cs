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
    public class Section5 : StoryboardObjectGenerator
    {
        [Configurable]
        public Color4 balloonColorMin = Color4.White;

        [Configurable]
        public Color4 balloonColorMax = Color4.IndianRed;

        [Configurable]
        public Color4 AvatarColor = Color4.White;

        [Configurable]
        public Color4 FlowerColor = Color4.White;

        public override void Generate()
        {
            Dialog();
            Flowers(220447, 247113);
            Background(211525, 247113, 221081);
            Mission(224256, 247113);
            Tochi(214268, 225526);
            //  sTime,  eTime,  realSTime
            HUD(214268, 247113, 220447, "Mission #4", "Quon", "sb/HUD/txt/nameTag/Dailycare.png", 1000, "sb/avatars/DailycareProfile.png");
        }

        public void HUD(int startTime, int endTime, int loadingTextEndtime, string mission, string songName, string nameTag, int progressBarDelay, string avatar)
        {
            var hud = new HUD(this, startTime, endTime, loadingTextEndtime, mission, songName, nameTag, progressBarDelay, avatar);
        }

        public void Background(int startTime, int endTime, int loadingTextEndtime)
        {
            var bitmap = GetMapsetBitmap("sb/bgs/5/bg.jpg");
            var bg = GetLayer("Background").CreateSprite("sb/bgs/5/bg.jpg", OsbOrigin.Centre);

            bg.Scale(startTime, 854.0f / bitmap.Width);
            bg.Fade(startTime, loadingTextEndtime, 0, 0.4);
            bg.Fade(endTime, 255241, 0.4, 0);
            bg.Move(OsbEasing.InOutBack, startTime, loadingTextEndtime, new Vector2(320, 250), new Vector2(320, 240));
        }

        public void Flowers(int startTime, int endTime)
        {
            // back
            for (int i = 0; i < 150; i++)
            {
                var delay = Random(0, 2000);
                var Scale = Random(0.2, 0.6);
                var Fade = 1;
                var sprite = GetLayer("FlowerBack").CreateSprite("sb/missions/4/flower" + Random(1, 4) + ".png", OsbOrigin.BottomCentre);

                sprite.Color(startTime + delay, FlowerColor);
                sprite.Move(startTime + delay, Random(-107, 747), Random(400, 450));
                sprite.ScaleVec(startTime + delay, Random(-Scale, Scale), Random(Scale * 0.5, Scale));
                sprite.Fade(startTime + delay, startTime + delay + Random(1000, 4000), 0, Fade);
                sprite.Fade(endTime + delay, endTime + delay + Random(1000, 4000), Fade, 0);

                var rotationSpeed = Random(5000, 10000);
                var rotationStart = MathHelper.DegreesToRadians(Random(-20, 20));
                var rotationEnd = MathHelper.DegreesToRadians(-rotationStart + 50);
                sprite.StartLoopGroup(startTime + delay, (endTime - startTime) / rotationSpeed);
                sprite.Rotate(OsbEasing.InOutSine, 0, rotationSpeed / 2, rotationStart, rotationEnd);
                sprite.Rotate(OsbEasing.InOutSine, rotationSpeed / 2, rotationSpeed, rotationEnd, rotationStart);
                sprite.EndGroup();
            }

            // front
            for (int i = 0; i < 150; i++)
            {
                var delay = Random(0, 2000);
                var Scale = Random(0.2, 0.6);
                var Fade = Random(0.1, 0.4);
                var sprite = GetLayer("FlowerFront").CreateSprite("sb/missions/4/flower" + Random(1, 4) + ".png", OsbOrigin.BottomCentre);
                
                sprite.Color(startTime + delay, FlowerColor);
                sprite.Move(startTime + delay, Random(-107, 747), Random(400, 450));
                sprite.ScaleVec(startTime + delay, Random(-Scale, Scale), Random(Scale * 0.5, Scale));
                sprite.Fade(startTime + delay, startTime + delay + Random(1000, 4000), 0, Fade);
                sprite.Fade(endTime + delay, endTime + delay + Random(1000, 4000), Fade, 0);

                var rotationSpeed = Random(5000, 10000);
                var rotationStart = MathHelper.DegreesToRadians(Random(-20, 20));
                var rotationEnd = MathHelper.DegreesToRadians(-rotationStart + 50);
                sprite.StartLoopGroup(startTime + delay, (endTime - startTime) / rotationSpeed);
                sprite.Rotate(OsbEasing.InOutSine, 0, rotationSpeed / 2, rotationStart, rotationEnd);
                sprite.Rotate(OsbEasing.InOutSine, rotationSpeed / 2, rotationSpeed, rotationEnd, rotationStart);
                sprite.EndGroup();
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
            string[] sentence = { "Whew... That was intense!",
                                  "It's time for some aim based challenge now.",
                                  "Hopefully your aim is good enough!" };
            var dialog = new DialogManager(this, font, 214268, 219754, "-Tochi", 105, 326, false,
                fontSize, 1, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.wav",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "Popping balloons is fun, right?",
                                  "Well, now it's your chance to pop as many as you can!" };
            var dialog2 = new DialogManager(this, font, 219754, 225526, "-Tochi", 105, 326, false,
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

        public void Mission(int startTime, int endTime)
        {
            var interval = Random(1500, 3000);
            var balloon = GetLayer("Avatar").CreateSprite("sb/missions/4/balloon.png", OsbOrigin.BottomCentre);

            // avatar
            var prevPosition = 200;
            var hoverDuration = 2000;
            // var timeStepArrow = Random(2000, 5000);
            var timeStepAvatar = Random(3500, 6000);
            // var timeStepAvatar = interval;
            var loopAmount = (endTime - startTime) / (hoverDuration * 2);
            var positionY = 450;
            var standbyTime = Random(timeStepAvatar / 6, timeStepAvatar / 3);
            var avatar = GetLayer("Avatar").CreateSprite("sb/avatars/Dailycare.png", OsbOrigin.BottomCentre);

            avatar.Scale(startTime, 0.2 * 0.7);
            avatar.Color(startTime, AvatarColor);
            avatar.Fade(startTime, startTime + 1000, 0, 1);
            avatar.Fade(endTime - 1000 + 7000, endTime + 7000, 1, 0);
            // avatar hovering
            avatar.StartLoopGroup(startTime, loopAmount);
            avatar.MoveY(OsbEasing.InOutSine, 0, hoverDuration, positionY, positionY + 15);
            avatar.MoveY(OsbEasing.InOutSine, hoverDuration, hoverDuration * 2, positionY + 15, positionY);
            avatar.EndGroup();

            for (int i = startTime; i < endTime + 7000; i += timeStepAvatar)
            {
                var randomX = Random(50, 320);

                // avatar vertical movement
                if (balloon.PositionAt(i + (timeStepAvatar - standbyTime)).X >= 320) // right side
                {
                    var nextPosX = balloon.PositionAt(i + (timeStepAvatar - standbyTime)).X - randomX;
                    avatar.MoveX(OsbEasing.InOutSine, i, i + (timeStepAvatar - standbyTime), prevPosition, nextPosX);

                    if (prevPosition > nextPosX)
                    {
                        avatar.FlipH(i, i + (timeStepAvatar - standbyTime));
                    }
                    prevPosition = (int)nextPosX;
                }
                else if (balloon.PositionAt(i + (timeStepAvatar - standbyTime)).X <= 320) // left side
                {
                    var nextPosX = balloon.PositionAt(i + (timeStepAvatar - standbyTime)).X + randomX;
                    avatar.MoveX(OsbEasing.InOutSine, i, i + (timeStepAvatar - standbyTime), prevPosition, nextPosX);

                    if (prevPosition > nextPosX)
                    {
                        avatar.FlipH(i, i + (timeStepAvatar - standbyTime));
                    }
                    prevPosition = (int)nextPosX;
                }
            }
            var mission = new Mission4(this, balloon, avatar, startTime, endTime, interval, balloonColorMin, balloonColorMax);
        }
    }
}

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
    public class Section9 : StoryboardObjectGenerator
    {
        public override void Generate()
        {
		    Dialog();
            Mission();
            Background();
            Tochi(333624, 344407);
            HUD(379590, 488992, 387313, "Mission #8", "Ordirehv", "sb/HUD/txt/nameTag/Necho&Otosaka.png", 4500, "sb/avatars/Necho&OtosakaProfile.png");
        }

        public void Background()
        {
            var bitmap = GetMapsetBitmap("sb/bgs/9/bg.jpg");
            var bg = GetLayer("Background").CreateSprite("sb/bgs/9/bg.jpg", OsbOrigin.Centre, new Vector2(320, 240));

            bg.Scale(379590, 854.0f / bitmap.Width);
            bg.Fade(379590, 387313, 0, 0.5);
            bg.Fade(488992, 488992 + 40000, 0.5, 0);
        }

        public void HUD(int startTime, int endTime, int loadingTextEndtime, string mission, string songName, string nameTag, int progressBarDelay, string avatar)
        {
            var hud = new HUD(this, startTime, endTime, loadingTextEndtime, mission, songName, nameTag, progressBarDelay, avatar);
        }

        public void Mission()
        {
            // INTRO ////////////////////////////////////////////////////////////////////////

            var appearStartTime = 389827;
            var disappearStartTime = 398622;
            var endTime = 488992;

            var nechoScale = 0.2;
            var otosakaScale = 0.2;

            var nechoPosX = 107;
            var otosakaPosX = 555;
            
            var necho = GetLayer("Necho - Intro").CreateSprite("sb/avatars/Necho.png", OsbOrigin.Centre);
            var otosaka = GetLayer("Otosaka-Yu - Intro").CreateSprite("sb/avatars/Otosaka-Yu.png", OsbOrigin.Centre);
            
            int speed = 0;
            for (int i = appearStartTime; i < appearStartTime + 4000; i += speed)
            {
                speed += 200;

                necho.ScaleVec(OsbEasing.InOutSine, i, i + (speed / 2), nechoScale, nechoScale, -nechoScale, nechoScale);
                necho.ScaleVec(OsbEasing.InOutSine, i + (speed / 2), i + speed, -nechoScale, nechoScale, nechoScale, nechoScale);
                otosaka.ScaleVec(OsbEasing.InOutSine, i, i + (speed / 2), otosakaScale, otosakaScale, -otosakaScale, otosakaScale);
                otosaka.ScaleVec(OsbEasing.InOutSine, i + (speed / 2), i + speed, -nechoScale, nechoScale, nechoScale, nechoScale);
            }

            necho.Fade(appearStartTime, appearStartTime + 1500, 0, 1);
            necho.Fade(endTime, endTime + 1500, 1, 0);
            necho.Move(OsbEasing.OutSine, appearStartTime, appearStartTime + 4000, -107, 280, nechoPosX, 280);
            necho.Move(OsbEasing.InOutExpo, disappearStartTime, disappearStartTime + 1500, nechoPosX, 280, 127, 240);

            otosaka.FlipH(appearStartTime, endTime + 1500);
            otosaka.Fade(appearStartTime, appearStartTime + 1500, 0, 1);
            otosaka.Fade(endTime, endTime + 1500, 1, 0);
            otosaka.Move(OsbEasing.OutSine, appearStartTime, appearStartTime + 4000, 747, 200, otosakaPosX, 200);
            otosaka.Move(OsbEasing.InOutExpo, disappearStartTime, disappearStartTime + 1500, otosakaPosX, 200, 535, 240);

            var nechoTag = GetLayer("VS + Tag").CreateSprite("sb/missions/9/necho.png", OsbOrigin.Centre);
            var otosakaTag = GetLayer("VS + Tag").CreateSprite("sb/missions/9/otosaka-yu.png", OsbOrigin.Centre);

            var tagStartTime = appearStartTime + 4000;
            var tagEndTime = disappearStartTime;

            nechoTag.Move(OsbEasing.Out, tagStartTime, tagStartTime + 500, -107 - 20, 400, nechoPosX - 20, 400);
            nechoTag.Move(tagEndTime, tagEndTime + 500, nechoPosX - 20, 400, -107 - 20, 400);
            nechoTag.Fade(tagStartTime, tagStartTime + 500, 0, 1);
            nechoTag.Fade(tagEndTime, tagEndTime + 500, 1, 0);
            nechoTag.Scale(tagStartTime, nechoScale * 1.5);

            otosakaTag.Move(OsbEasing.Out, tagStartTime, tagStartTime + 500, 747, 280, otosakaPosX, 280);
            otosakaTag.Move(tagEndTime, tagEndTime + 500, otosakaPosX, 280, 747, 280);
            otosakaTag.Fade(tagStartTime, tagStartTime + 500, 0, 1);
            otosakaTag.Fade(tagEndTime, tagEndTime + 500, 1, 0);
            otosakaTag.Scale(tagStartTime, otosakaScale * 1.5);

            var vs = GetLayer("VS + Tag").CreateSprite("sb/missions/9/vs.png", OsbOrigin.Centre, new Vector2(320, 240));
            var vs2 = GetLayer("VS + Tag").CreateSprite("sb/missions/9/vs.png", OsbOrigin.Centre, new Vector2(320, 240));

            vs.Fade(appearStartTime, appearStartTime + 5000, 0, 1);
            vs.Fade(disappearStartTime + 500, disappearStartTime + 1500, 1, 0);
            vs.Scale(OsbEasing.InBack, appearStartTime, appearStartTime + 5000, 1, 0.4);

            vs2.Additive(appearStartTime + 5000, appearStartTime + 7000);
            vs2.Fade(OsbEasing.OutSine, appearStartTime + 5000, appearStartTime + 7000, 0.5, 0);
            vs2.Scale(OsbEasing.OutSine, appearStartTime + 5000, appearStartTime + 7000, 0.4, 0.7);

            var nechoH1 = GetLayer("HUD - Intro").CreateSprite("sb/missions/9/h1.png", OsbOrigin.Centre);
            var nechoH2 = GetLayer("HUD - Intro").CreateSprite("sb/missions/9/h2.png", OsbOrigin.Centre);
            var nechoH3 = GetLayer("HUD - Intro").CreateSprite("sb/missions/9/h3.png", OsbOrigin.Centre);
            var otosakaH1 = GetLayer("HUD - Intro").CreateSprite("sb/missions/9/h1.png", OsbOrigin.Centre);
            var otosakaH2 = GetLayer("HUD - Intro").CreateSprite("sb/missions/9/h2.png", OsbOrigin.Centre);
            var otosakaH3 = GetLayer("HUD - Intro").CreateSprite("sb/missions/9/h3.png", OsbOrigin.Centre);
            
            var hudStartTime = appearStartTime + 4000;
            var hudEndTime = disappearStartTime - 300;

            nechoH1.Scale(hudStartTime, 0.35);
            nechoH1.Move(hudStartTime, nechoPosX - 20, 280);
            nechoH1.Additive(hudStartTime, hudEndTime + 500);
            nechoH1.Rotate(hudStartTime, hudEndTime + 500, Random(-1, 1), Random(1, 2));
            nechoH1.StartLoopGroup(hudStartTime, 5);
            nechoH1.Fade(0, 100, 0.5, 0);
            nechoH1.EndGroup();
            nechoH1.Fade(hudStartTime + 500, hudEndTime, 0.5, 0.5);
            nechoH1.StartLoopGroup(hudEndTime, 3);
            nechoH1.Fade(0, 100, 0.5, 0);
            nechoH1.EndGroup();

            nechoH2.Scale(hudStartTime, 0.35);
            nechoH2.Move(hudStartTime, nechoPosX - 20, 280);
            nechoH2.Additive(hudStartTime, hudEndTime + 500);
            nechoH2.Rotate(hudStartTime, hudEndTime + 500, Random(1, 2), Random(-1.5, -1));
            nechoH2.StartLoopGroup(hudStartTime, 5);
            nechoH2.Fade(0, 100, 0.5, 0);
            nechoH2.EndGroup();
            nechoH2.Fade(hudStartTime + 500, hudEndTime, 0.5, 0.5);
            nechoH2.StartLoopGroup(hudEndTime, 3);
            nechoH2.Fade(0, 100, 0.5, 0);
            nechoH2.EndGroup();

            nechoH3.Scale(hudStartTime, 0.35);
            nechoH3.Move(hudStartTime, nechoPosX - 20, 280);
            nechoH3.Additive(hudStartTime, hudEndTime + 500);
            nechoH3.Rotate(hudStartTime, hudEndTime + 500, Random(-1, 1), Random(1, 2));
            nechoH3.StartLoopGroup(hudStartTime, 5);
            nechoH3.Fade(0, 100, 0.5, 0);
            nechoH3.EndGroup();
            nechoH3.Fade(hudStartTime + 500, hudEndTime, 0.5, 0.5);
            nechoH3.StartLoopGroup(hudEndTime, 3);
            nechoH3.Fade(0, 100, 0.5, 0);
            nechoH3.EndGroup();

            otosakaH1.Scale(hudStartTime, 0.35);
            otosakaH1.Move(hudStartTime, otosakaPosX, 200);
            otosakaH1.Additive(hudStartTime, hudEndTime + 500);
            otosakaH1.Rotate(hudStartTime, hudEndTime + 500, Random(-1, 1), Random(1, 2));
            otosakaH1.StartLoopGroup(hudStartTime, 5);
            otosakaH1.Fade(0, 100, 0.5, 0);
            otosakaH1.EndGroup();
            otosakaH1.Fade(hudStartTime + 500, hudEndTime, 0.5, 0.5);
            otosakaH1.StartLoopGroup(hudEndTime, 3);
            otosakaH1.Fade(0, 100, 0.5, 0);
            otosakaH1.EndGroup();

            otosakaH2.Scale(hudStartTime, 0.35);
            otosakaH2.Move(hudStartTime, otosakaPosX, 200);
            otosakaH2.Additive(hudStartTime, hudEndTime + 500);
            otosakaH2.Rotate(hudStartTime, hudEndTime + 500, Random(1, 2), Random(-1.5, -1));
            otosakaH2.StartLoopGroup(hudStartTime, 5);
            otosakaH2.Fade(0, 100, 0.5, 0);
            otosakaH2.EndGroup();
            otosakaH2.Fade(hudStartTime + 500, hudEndTime, 0.5, 0.5);
            otosakaH2.StartLoopGroup(hudEndTime, 3);
            otosakaH2.Fade(0, 100, 0.5, 0);
            otosakaH2.EndGroup();

            otosakaH3.Scale(hudStartTime, 0.35);
            otosakaH3.Move(hudStartTime, otosakaPosX, 200);
            otosakaH3.Additive(hudStartTime, hudEndTime + 500);
            otosakaH3.Rotate(hudStartTime, hudEndTime + 500, Random(-1, 1), Random(1, 2));
            otosakaH3.StartLoopGroup(hudStartTime, 5);
            otosakaH3.Fade(0, 100, 0.5, 0);
            otosakaH3.EndGroup();
            otosakaH3.Fade(hudStartTime + 500, hudEndTime, 0.5, 0.5);
            otosakaH3.StartLoopGroup(hudEndTime, 3);
            otosakaH3.Fade(0, 100, 0.5, 0);
            otosakaH3.EndGroup();

            // FIGHT ////////////////////////////////////////////////////////////////////////
            var sTime = disappearStartTime + 2200;
            var eTime = endTime;
            var interval = 1500;
            var rotate = MathHelper.DegreesToRadians(90);
            for (double i = sTime; i < eTime - interval; i += interval)
            {
                var Beat = Beatmap.GetTimingPointAt((int)i).BeatDuration;

                var slashLeft = GetLayer("Slashing").CreateAnimation("sb/missions/9/slashAnimation/slash" + Random(1, 3) + "_.jpg", 8, 50, OsbLoopType.LoopOnce, OsbOrigin.Centre);
                var slashRight = GetLayer("Slashing").CreateAnimation("sb/missions/9/slashAnimation/slash" + Random(1, 3) + "_.jpg", 8, 50, OsbLoopType.LoopOnce, OsbOrigin.Centre);

                var slashDelay = -50;
                var Speed = Random(10, 15);
                var Duration = interval / Speed;
                var spawnDelay = Random(Beat, Beat * 6);
                var effectDelay = Random(500, 1000);
                
                var slashYL = Random(necho.PositionAt(i + slashDelay + Duration).Y - 100, necho.PositionAt(i + slashDelay + Duration).Y + 100);
                var slashYR = Random(otosaka.PositionAt(i + slashDelay + Duration).Y - 35, otosaka.PositionAt(i + slashDelay + Duration).Y + 60);

                slashLeft.Rotate(i + slashDelay + Duration, rotate);
                slashRight.Rotate(i + slashDelay + spawnDelay + Duration, rotate);
                slashLeft.Move(i + slashDelay + Duration, necho.PositionAt(i + slashDelay + Duration).X - 20, necho.PositionAt(i + slashDelay + Duration).Y);
                slashRight.Move(i + slashDelay + spawnDelay + Duration, otosaka.PositionAt(i + slashDelay + spawnDelay + Duration));
                slashLeft.ScaleVec(i + slashDelay + Duration, i + slashDelay + Duration, Random(0.2, 0.4), Random(0.4, 0.6), Random(0.2, 0.4), Random(0.4, 0.6));
                slashRight.ScaleVec(i + slashDelay + spawnDelay + Duration, i + slashDelay + spawnDelay + Duration + effectDelay, Random(0.2, 0.4), Random(0.4, 0.6), Random(0.2, 0.4), Random(0.4, 0.6));
                slashLeft.Additive(i + slashDelay + Duration, i + slashDelay + Duration + effectDelay);
                slashRight.Additive(i + slashDelay + spawnDelay + Duration, i + slashDelay + spawnDelay + Duration + effectDelay);
                // flip vertically if it's on the right side
                if (otosaka.PositionAt(i + slashDelay + spawnDelay + Duration).X > 320)
                {
                    slashRight.FlipV(i + slashDelay + spawnDelay + Duration, i + slashDelay + spawnDelay + Duration);
                }
                // flip horizontally sometimes
                if (necho.PositionAt(i + slashDelay + Duration).Y > slashYL)
                {
                    slashLeft.FlipH(i + slashDelay + Duration, i + slashDelay + Duration);
                }
                // flip horizontally sometimes
                if (otosaka.PositionAt(i + slashDelay + spawnDelay + Duration).Y > slashYR)
                {
                    slashRight.FlipH(i + slashDelay + spawnDelay + Duration, i + slashDelay + spawnDelay + Duration);
                }
                // sound effects
                var slashLeftSFX = GetLayer("Slashing").CreateSample("sb/sfx/swoosh-" + Random(1, 4) + ".wav", i + slashDelay + Duration, 100);
                var slashRightSFX = GetLayer("Slashing").CreateSample("sb/sfx/swoosh-" + Random(1, 4) + ".wav", i + slashDelay + spawnDelay + Duration, 100);

                interval = Random(1000, 3000);
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
            string[] sentence = { "Alright.",
                                  "Look, we are almost finished!" };
            var dialog = new DialogManager(this, font, 332996, 336137, "-Tochi", 105, 326, false,
                fontSize, 1, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.wav",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "Next quest is even easier than the previous one.",
                                   "Just keep an eye on the aircrafts evading from behind the building." };
            var dialog2 = new DialogManager(this, font, 336137, 344407, "-Tochi", 105, 326, false,
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
    }
}

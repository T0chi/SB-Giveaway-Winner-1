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

        private DialogManager pointsPass;

        private DialogManager pointsFail;

        [Configurable]
        public Color4 TochiTagColor = Color4.Cyan;

        [Configurable]
        public Color4 TextColor = new Color4(255, 255, 255, 255);

        [Configurable]
        public Color4 BoxColor = new Color4(255, 255, 255, 255);

        [Configurable]
        public Color4 GlowColor = new Color4(255, 255, 255, 255);

        [Configurable]
        public Color4 OutlineColor = new Color4(50, 50, 50, 200);

        [Configurable]
        public Color4 ShadowColor = new Color4(0, 0, 0, 200);

        [Configurable]
        public int StartTime = 490992;
        // public int StartTime = 490992;

        [Configurable]
        public int EndTime = 558992;
        // public int EndTime = 558992;

        public override void Generate()
        {
            pointSystem = new PointSystem();
            pointsPass = new DialogManager();
            pointsFail = new DialogManager();

            points(1, "sb/points/", 48476, 48476, 61399, 72571, 2, true); // break 1
            currentPointsPass("Overall", 1, "sb/points/", 48476, 48476, 61399, 72571, pointSystem.totalPass, true);
            currentPointsFail("Overall", 1, "sb/points/", 48476, 48476, 61399, 72571, pointSystem.totalFail, true);

            points(3, "sb/points/", 106905, 107572, 115238, 128262, 2, true); // break 2
            currentPointsPass("Overall", 3, "sb/points/", 106905, 107572, 115238, 128262, pointSystem.totalPass, true);
            currentPointsFail("Overall", 3, "sb/points/", 106905, 107572, 115238, 128262, pointSystem.totalFail, true);
            points(9, "sb/points/", 146863, 147063, 157062, 167640, 2, true); // break 3
            currentPointsPass("Overall", 9, "sb/points/", 146863, 147063, 157062, 167640, pointSystem.totalPass, true);
            currentPointsFail("Overall", 9, "sb/points/", 146863, 147063, 157062, 167640, pointSystem.totalFail, true);

            points(15, "sb/points/", 197982, 198240, 213925, 221716, 2, true); // break 4
            currentPointsPass("Overall", 15, "sb/points/", 197982, 198240, 213925, 221716, pointSystem.totalPass, true);
            currentPointsFail("Overall", 15, "sb/points/", 197982, 198240, 213925, 221716, pointSystem.totalFail, true);

            points(21, "sb/points/", 237589, 237589, 247113, 254732, 2, true); // break 5
            currentPointsPass("Overall", 21, "sb/points/", 237589, 237589, 247113, 254732, pointSystem.totalPass, true);
            currentPointsFail("Overall", 21, "sb/points/", 237589, 237589, 247113, 254732, pointSystem.totalFail, true);

            points(27, "sb/points/", 274561, 274639, 283671, 290901, 2, true); // break 6
            currentPointsPass("Overall", 27, "sb/points/", 274561, 274639, 283671, 290901, pointSystem.totalPass, true);
            currentPointsFail("Overall", 27, "sb/points/", 274561, 274639, 283671, 290901, pointSystem.totalFail, true);

            points(33, "sb/points/", 320430, 321059, 331111, 339380, 2, true); // break 7
            currentPointsPass("Overall", 33, "sb/points/", 320430, 321059, 331111, 339380, pointSystem.totalPass, true);
            currentPointsFail("Overall", 33, "sb/points/", 320430, 321059, 331111, 339380, pointSystem.totalFail, true);

            points(39, "sb/points/", 368909, 369223, 379590, 388570, 2, true); // break 8
            currentPointsPass("Overall", 39, "sb/points/", 368909, 369223, 379590, 388570, pointSystem.totalPass, true);
            currentPointsFail("Overall", 39, "sb/points/", 368909, 369223, 379590, 388570, pointSystem.totalFail, true);

            var pointsFight = new Vector2(pointSystem.totalPass, pointSystem.totalFail); // middle of the fight
            currentPointsPass("Overall after lost points", 100, "sb/points/", 408675, 409303, 426266, 432549, (int)pointsFight.X - 10000, true);
            currentPointsFail("Overall after lost points", 100, "sb/points/", 408675, 409303, 426266, 432549, (int)pointsFight.Y - 10000, true);

            points(45, "sb/points/", 484268, 484343, 488992, 488992 + 8000, 2, true); // break 9
            currentPointsPass("Overall", 45, "sb/points/", 484268, 484343, 488992, 488992 + 8000, pointSystem.totalPass - 10000, true);
            currentPointsFail("Overall", 45, "sb/points/", 484268, 484343, 488992, 488992 + 8000, pointSystem.totalFail - 10000, true);

            fightSound(398622, 488992);


            Log($"TOTALPOINTS:                {pointSystem.totalPass} ; {pointSystem.totalFail}");


            Avatars();
            Background();
            ResultsPass(408675, 409303, StartTime + 5000, StartTime + 10000, 2, true);
            ResultsPass2(408675, 409303, StartTime + 12000, StartTime + 18000, 2, true);
            ResultsFail(408675, 409303, StartTime + 5000, StartTime + 10000, 2, true);
            ResultsFail2(408675, 409303, StartTime + 12000, StartTime + 20000, 2, true);
            CyanRain(0.3f, "RainFront", Random(5000, 10000), 0.15f, (float)Random(0.2f, 0.5f));
            CyanRain(0.2f, "RainBack", Random(500, 1000), 0.1f, (float)Random(0.1f, 0.5f));

            // outro song
            var song = GetLayer("").CreateSample("sb/sfx/outro-song.ogg", StartTime - 2000, 100);

            // cool visuals
            Information(StartTime + 17000, EndTime - 2000);
        }

        public void Information(int startTime, int endTime)
        {
            var bgBitmap = GetMapsetBitmap("sb/pixel.png");
            var bg = GetLayer("Information").CreateSprite("sb/pixel.png", OsbOrigin.Centre, new Vector2(320, 240));
            var github = GetLayer("Information").CreateSprite("sb/outro/github.png", OsbOrigin.Centre);
            var githubText = GetLayer("Information").CreateSprite("sb/outro/githubText.png", OsbOrigin.Centre);
            var tyMessage = GetLayer("Information").CreateSprite("sb/outro/ty.png", OsbOrigin.Centre);
            var heart = GetLayer("Information").CreateAnimation("sb/outro/ani/heart.jpg", 16, 50, OsbLoopType.LoopForever, OsbOrigin.Centre);
            var skip = GetLayer("Information").CreateSprite("sb/outro/skip.png", OsbOrigin.Centre, new Vector2(320, 240));
            var skipNot = GetLayer("Information").CreateSprite("sb/outro/skipNot.png", OsbOrigin.Centre, new Vector2(320, 200));

            var scrollDuration = 20000;
            var scrollStart = startTime;
            var scrollStartPos = new Vector2(320, 480);
            var scrollEndPos = new Vector2(320, 0);

            bg.ScaleVec(startTime, 854.0f / bgBitmap.Width, 480.0f / bgBitmap.Height);
            bg.Fade(startTime, startTime + 2000, 0, 0.9f);
            bg.Fade(endTime, endTime + 2000, 0.9f, 0);
            bg.Color(startTime, Color4.Black);
            
            // github logo + text
            github.Scale(scrollStart, 0.35f);
            github.Fade(scrollStart, scrollStart + 1000, 0, 1);
            github.Move(scrollStart, scrollStart + scrollDuration, scrollStartPos.X, scrollStartPos.Y + 150,
                                                                   scrollEndPos.X, scrollEndPos.Y - 150);
            githubText.Scale(scrollStart + 2000, 0.3f);
            githubText.Fade(scrollStart + 2000, scrollStart + 2000 + 1000, 0, 1);
            githubText.Move(scrollStart + 2000, scrollStart + 2000 + scrollDuration, scrollStartPos.X, scrollStartPos.Y + 150,
                                                                       scrollEndPos.X, scrollEndPos.Y - 150);

            // ty message + heart
            tyMessage.Scale(scrollStart + 10000, 0.3f);
            tyMessage.Fade(scrollStart + 10000, scrollStart + 10000 + 1000, 0, 1);
            tyMessage.Move(scrollStart + 10000, scrollStart + 10000 + scrollDuration, scrollStartPos.X, scrollStartPos.Y + 150,
                                                                       scrollEndPos.X, scrollEndPos.Y - 150);
            heart.Scale(scrollStart + 13000, 0.3f);
            heart.Additive(scrollStart + 13000, scrollStart + 13000 + scrollDuration);
            heart.Fade(scrollStart + 13000, scrollStart + 13000 + 1000, 0, 1);
            heart.Move(scrollStart + 13000, scrollStart + 13000 + scrollDuration, scrollStartPos.X, scrollStartPos.Y + 150,
                                                                       scrollEndPos.X, scrollEndPos.Y - 150);

            // skip + skipNot
            var loopDuration = 6000;
            // var loopCount = ((endTime + 2000) - (scrollStart + 14000)) / loopDuration;
            // var loopCount2 = ((startTime + 10000) - startTime) / loopDuration;
            var startRotation = MathHelper.DegreesToRadians(-360);
            var endRotation = MathHelper.DegreesToRadians(0);
            
            skip.Fade(scrollStart + 26000, scrollStart + 26000 + 1000, 0, 1);
            skip.Fade(endTime, endTime + 2000, 1, 0);
            skip.StartLoopGroup(scrollStart + 26000, 1);
                skip.ScaleVec(0, loopDuration, -0.6f, 0.6f, 0.6f, 0.6f);
                skip.Rotate(OsbEasing.InOutSine, 0, loopDuration, startRotation, endRotation);
            skip.EndGroup();
            skipNot.Fade(startTime, startTime + 2500, 0, 1);
            skipNot.Fade(startTime + 9000, startTime + 10000, 1, 0);
            skipNot.StartLoopGroup(startTime, 1);
                skipNot.ScaleVec(0, loopDuration, -0.3f, 0.3f, 0.3f, 0.3f);
                skipNot.Rotate(OsbEasing.InOutSine, 0, loopDuration, startRotation, endRotation);
            skipNot.EndGroup();
        }

        public void fightSound(int breakStart, int endTime)
        {
            var fightSFX = GetLayer("Points Pass").CreateSample("sb/sfx/fight-start.ogg", breakStart, 100);
            var fightSFX2 = GetLayer("Points Fail").CreateSample("sb/sfx/fight-start.ogg", breakStart, 100);
        }

        public void currentPointsPass(string text, int triggerGroup, string fontPath, int NoteStart, int NoteEnd, int breakStart, int breakEnd, int points,
                                      bool startTriggerGroup = false)
        {
            var noteStartPass = NoteStart - 500; // start range of the first note in milliseconds
            var noteEndPass = NoteEnd + 5; // end range of the last note in milliseconds

            var delay = ((breakEnd - breakStart) / 2) - 800; // calculation for the middle of the break
            var duration = breakEnd - noteStartPass; // duration for the TriggerGroup

            var appearTiming = breakStart - noteStartPass; // the duration value between the note(s) and the break start

            var fontSize = 14;
            var GlowRadius = 0;
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            var font = LoadFont(fontPath + $"{triggerGroup}/cpPass", new FontDescription()
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

            var pos = new Vector2(320, 400);


            // Trigger Start

            string[] resultPass = { $"{text}: {points}pts" };
            this.pointsPass.Setup(this, font, appearTiming + delay, duration, "Points Pass", pos.X, pos.Y, true,
                fontSize, 0.7f, 50, 1000, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/blank.ogg",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
            this.pointsPass.Generate(resultPass, 50, 1000, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, triggerGroup);
        }

        public void currentPointsFail(string text, int triggerGroup, string fontPath, int NoteStart, int NoteEnd, int breakStart, int breakEnd, int points,
                                      bool startTriggerGroup = false)
        {
            var noteStartFail = NoteStart - 500; // start range of the first note in milliseconds
            var noteEndFail = NoteEnd + 5; // end range of the last note in milliseconds

            var delay = ((breakEnd - breakStart) / 2) - 800; // calculation for the middle of the break
            var duration = breakEnd - noteStartFail; // duration for the TriggerGroup

            var appearTiming = breakStart - noteStartFail; // the duration value between the note(s) and the break start

            var fontSize = 14;
            var GlowRadius = 0;
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            var font = LoadFont(fontPath + $"{triggerGroup}/cpFail", new FontDescription()
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

            var pos = new Vector2(320, 400);


            // Trigger Start

            // string[] resultFail = { $"Overall: {points}pts" };
            string[] resultFail = { $"{text}: --hidden--" };
            this.pointsFail.Setup(this, font, appearTiming + delay, duration, "Points Fail", pos.X, pos.Y, true,
                fontSize, 0.7f, 50, 1000, Color4.White, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/blank.ogg",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
            this.pointsFail.Generate(resultFail, 50, 1000, startTriggerGroup, "Failing", noteStartFail, noteEndFail, triggerGroup + 1);
        }

        public void points(int triggerGroup, string fontPath, int NoteStart, int NoteEnd, int breakStart, int breakEnd, int speed,
                           bool startTriggerGroup = false)
        {
            // for the "you gained..." etc font
            var fontSize = 18;
            var GlowRadius = 0;
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            var font = LoadFont(fontPath + $"{triggerGroup}", new FontDescription()
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
            var font2 = LoadFont(fontPath + $"{triggerGroup}/pts", new FontDescription()
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

















            var noteStartPass = NoteStart - 500; // start range of the first note in milliseconds
            var noteEndPass = NoteEnd + 5; // end range of the last note in milliseconds
            var noteStartFail = NoteStart - 500; // start range of the first note in milliseconds
            var noteEndFail = NoteEnd + 5; // end range of the last note in milliseconds

            var appearTiming = breakStart - noteStartPass; // the duration value between the note(s) and the break start

            var delay = ((breakEnd - breakStart) / 2) - 800; // calculation for the middle of the break
            var endTime = breakStart + delay; // middle of the break
            var duration = endTime - noteStartPass; // duration for the TriggerGroup
            var durationText = (endTime + delay) - noteStartPass; // duration for the TriggerGroup for the texts

            var bitmap = GetMapsetBitmap("sb/points/n0.png");
            var scale = new Vector2(0.5f, 0.5f);
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

            // Pass Points ------------------------------------------------------------------------------------------------

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

            dot.StartTriggerGroup("HitSound", noteStartPass, noteEndPass, triggerGroup);
            one.StartTriggerGroup("HitSound", noteStartPass, noteEndPass, triggerGroup);
            ten.StartTriggerGroup("HitSound", noteStartPass, noteEndPass, triggerGroup);
            hundred.StartTriggerGroup("HitSound", noteStartPass, noteEndPass, triggerGroup);
            thousand.StartTriggerGroup("HitSound", noteStartPass, noteEndPass, triggerGroup);

            var pointSFX = GetLayer("Points Pass").CreateSample("sb/sfx/points-result.ogg", breakStart + delay, 100);

            // scale stuff

            dot.ScaleVec(appearTiming, scale.X, scale.Y);
            one.ScaleVec(appearTiming, scale.X, scale.Y);
            ten.ScaleVec(appearTiming, scale.X, scale.Y);
            hundred.ScaleVec(appearTiming, scale.X, scale.Y);
            thousand.ScaleVec(appearTiming, scale.X, scale.Y);


            // positions

            dot.Move(appearTiming, posDot.X - (numberWidth * 1.5f), posDot.Y);
            dot.Move(appearTiming + frameDelayTen, posDot.X - numberWidth, posDot.Y);
            dot.Move(appearTiming + frameDelayHundred, posDot.X - (numberWidth / 2), posDot.Y);

            one.Move(appearTiming, posOne.X - (numberWidth * 1.5f), posOne.Y);
            one.Move(appearTiming + frameDelayTen, posOne.X - numberWidth, posOne.Y);

            ten.Move(appearTiming, posTen.X - (numberWidth * 1.5f), posTen.Y);
            ten.Move(appearTiming + frameDelayTen, posTen.X - numberWidth, posTen.Y);

            hundred.Move(appearTiming, posHundred.X - (numberWidth * 1.5f), posHundred.Y);
            hundred.Move(appearTiming + frameDelayTen, posHundred.X - numberWidth, posHundred.Y);

            thousand.Move(appearTiming, posThousand.X - (numberWidth * 1.5f), posThousand.Y);
            thousand.Move(appearTiming + frameDelayTen, posThousand.X - numberWidth, posThousand.Y);

            // opacity

            dot.Fade(appearTiming, 0);

            one.Fade(appearTiming, 0);
            one.Fade(appearTiming, duration, fade, fade);
            one.Fade(duration + 1, 0);

            ten.Fade(appearTiming, 0);
            ten.Fade(appearTiming + frameDelayTen, duration, fade, fade);
            ten.Fade(duration + 1, 0);

            hundred.Fade(appearTiming, 0);
            hundred.Fade(appearTiming + frameDelayHundred, duration, fade, fade);
            hundred.Fade(duration + 1, 0);

            thousand.Fade(appearTiming, 0);


            if (randomThousandPass > 0)
            {
                string[] result = { $"You gained: +{randomThousandPass}.{randomHundredPass}{randomTenPass}{randomOnePass} points" };
                this.pointsPass.Setup(this, font, appearTiming + delay, durationText, "Points Pass", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 1000, ColorPass, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/blank.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsPass.Generate(result, 50, 1000, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, triggerGroup);

                int[] sectionPoints = new int[] { randomThousandPass, randomHundredPass, randomTenPass, randomOnePass };
                pointSystem.AddPassPoints(sectionPoints);
            }
            else if (randomTenPass == 0 && randomOnePass == 0)
            {
                string[] result = { $"You gained: +{randomHundredPass}00 points" };
                this.pointsPass.Setup(this, font, appearTiming + delay, durationText, "Points Pass", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 1000, ColorPass, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/blank.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsPass.Generate(result, 50, 1000, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, triggerGroup);

                int[] sectionPoints = new int[] { randomThousandPass, randomHundredPass, 0, 0 };
                pointSystem.AddPassPoints(sectionPoints);
            }
            else if (randomTenPass == 0)
            {
                string[] result = { $"You gained: +{randomThousandPass}{randomHundredPass}0{randomOnePass} points" };
                this.pointsPass.Setup(this, font, appearTiming + delay, durationText, "Points Pass", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 1000, ColorPass, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/blank.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsPass.Generate(result, 50, 1000, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, triggerGroup);

                int[] sectionPoints = new int[] { randomThousandPass, randomHundredPass, 0, randomOnePass };
                pointSystem.AddPassPoints(sectionPoints);
            }
            else if (randomOnePass == 0)
            {
                string[] result = { $"You gained: +{randomThousandPass}{randomHundredPass}{randomTenPass}0 points" };
                this.pointsPass.Setup(this, font, appearTiming + delay, durationText, "Points Pass", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 1000, ColorPass, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/blank.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsPass.Generate(result, 50, 1000, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, triggerGroup);

                int[] sectionPoints = new int[] { randomThousandPass, randomHundredPass, randomTenPass, 0 };
                pointSystem.AddPassPoints(sectionPoints);
            }
            else if (randomThousandPass == 0)
            {
                string[] result = { $"You gained: +{randomHundredPass}{randomTenPass}{randomOnePass} points" };
                this.pointsPass.Setup(this, font, appearTiming + delay, durationText, "Points Pass", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 1000, ColorPass, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/blank.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsPass.Generate(result, 50, 1000, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, triggerGroup);

                int[] sectionPoints = new int[] { 0, randomHundredPass, randomTenPass, randomOnePass };
                pointSystem.AddPassPoints(sectionPoints);
            }

            if (randomThousandPass == 0)
            {
                dot.Move(appearTiming + frameDelayHundred, posDot.X - (numberWidth / 2), posDot.Y);
                one.Move(appearTiming + frameDelayHundred, posOne.X - (numberWidth / 2), posOne.Y);
                ten.Move(appearTiming + frameDelayHundred, posTen.X - (numberWidth / 2), posTen.Y);
                thousand.Move(appearTiming + frameDelayHundred, posThousand.X - (numberWidth / 2), posThousand.Y);

                string[] pts = { "pts" };

                var PTS2 = true;
                if (PTS2 == true)
                {
                    this.pointsPass.Setup(this, font2, appearTiming, appearTiming + frameDelayTen, "Points Pass", pos.X - (numberWidth * 1.5f) + (numberWidth * 2.7f), pos.Y - 2, true,
                        fontSize2, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/blank.ogg",
                        DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                    this.pointsPass.Generate(pts, 0, 0, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, triggerGroup);
                }
                
                if (PTS2 == true)
                {
                    this.pointsPass.Setup(this, font2, appearTiming + frameDelayTen, appearTiming + frameDelayHundred, "Points Pass", pos.X - numberWidth + (numberWidth * 2.7f), pos.Y - 2, true,
                        fontSize2, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/blank.ogg",
                        DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                    this.pointsPass.Generate(pts, 0, 0, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, triggerGroup);
                }

                
                if (PTS2 == true)
                {
                    this.pointsPass.Setup(this, font2, appearTiming + frameDelayHundred, duration, "Points Pass", pos.X - (numberWidth / 2) + (numberWidth * 2.7f), pos.Y - 2, true,
                        fontSize2, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/blank.ogg",
                        DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                    this.pointsPass.Generate(pts, 0, 0, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, triggerGroup);
                }

                thousand.Fade(appearTiming + frameDelayThousand, duration, 0, 0);
                dot.Fade(appearTiming + frameDelayThousand, duration, 0, 0);
            }
            else
            {
                dot.Move(appearTiming + frameDelayThousand, posDot);
                one.Move(appearTiming + frameDelayThousand, posOne);
                ten.Move(appearTiming + frameDelayThousand, posTen);
                hundred.Move(appearTiming + frameDelayThousand, posHundred);
                thousand.Move(appearTiming + frameDelayThousand, posThousand);

                string[] pts = { "pts" };

                var PTS2 = true;
                if (PTS2 == true)
                {
                    this.pointsPass.Setup(this, font2, appearTiming, appearTiming + frameDelayTen, "Points Pass", pos.X - (numberWidth * 1.5f) + (numberWidth * 2.7f), pos.Y - 2, true,
                        fontSize2, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/blank.ogg",
                        DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                    this.pointsPass.Generate(pts, 0, 0, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, triggerGroup);
                }

                if (PTS2 == true)
                {
                    this.pointsPass.Setup(this, font2, appearTiming + frameDelayTen, appearTiming + frameDelayHundred, "Points Pass", pos.X - numberWidth + (numberWidth * 2.7f), pos.Y - 2, true,
                        fontSize2, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/blank.ogg",
                        DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                    this.pointsPass.Generate(pts, 0, 0, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, triggerGroup);
                }

                if (PTS2 == true)
                {
                    this.pointsPass.Setup(this, font2, appearTiming + frameDelayHundred, appearTiming + frameDelayThousand, "Points Pass", pos.X - (numberWidth / 2) + (numberWidth * 2.7f), pos.Y - 2, true,
                        fontSize2, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/blank.ogg",
                        DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                    this.pointsPass.Generate(pts, 0, 0, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, triggerGroup);
                }

                if (PTS2 == true)
                {
                    this.pointsPass.Setup(this, font2, appearTiming + frameDelayThousand, duration, "Points Pass", pos.X + (numberWidth * 2.7f), pos.Y - 2, true,
                        fontSize2, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/blank.ogg",
                        DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                    this.pointsPass.Generate(pts, 0, 0, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, triggerGroup);
                }

                thousand.Fade(appearTiming + frameDelayThousand, duration, fade, fade);
                thousand.Fade(duration + 1, 0);
                dot.Fade(appearTiming + frameDelayThousand, duration, fade, fade);
                dot.Fade(duration + 1, 0);
            }

            dot.EndGroup();
            one.EndGroup();
            ten.EndGroup();
            hundred.EndGroup();
            thousand.EndGroup();



















            // Fail Points ------------------------------------------------------------------------------------------------

            var randomOneFail = Random(0, 10);
            var randomTenFail = Random(0, 10);
            var randomHundredFail = Random(1, 9);


            // numbers

            var oneFail = GetLayer("Points Fail").CreateAnimation("sb/points/n.png", randomOneFail + 1, frameDelayOne, OsbLoopType.LoopOnce, OsbOrigin.CentreLeft);
            var tenFail = GetLayer("Points Fail").CreateAnimation("sb/points/n.png", randomTenFail + 1, frameDelayTen, OsbLoopType.LoopOnce, OsbOrigin.CentreLeft);
            var hundredFail = GetLayer("Points Fail").CreateAnimation("sb/points/n.png", randomHundredFail + 1, frameDelayHundred, OsbLoopType.LoopOnce, OsbOrigin.CentreRight);


            // Trigger Pass

            oneFail.StartTriggerGroup("Failing", noteStartFail, noteEndFail, triggerGroup + 1);
            tenFail.StartTriggerGroup("Failing", noteStartFail, noteEndFail, triggerGroup + 1);
            hundredFail.StartTriggerGroup("Failing", noteStartFail, noteEndFail, triggerGroup + 1);


            // scale stuff

            oneFail.ScaleVec(appearTiming, scale.X, scale.Y);
            tenFail.ScaleVec(appearTiming, scale.X, scale.Y);
            hundredFail.ScaleVec(appearTiming, scale.X, scale.Y);


            // positions

            oneFail.Move(appearTiming, posOne.X - (numberWidth * 1.5f), posOne.Y);
            oneFail.Move(appearTiming + frameDelayTen, posOne.X - numberWidth, posOne.Y);
            oneFail.Move(appearTiming + frameDelayHundred, posOne.X - (numberWidth / 2), posOne.Y);

            tenFail.Move(appearTiming, posTen.X - (numberWidth * 1.5f), posTen.Y);
            tenFail.Move(appearTiming + frameDelayTen, posTen.X - numberWidth, posTen.Y);
            tenFail.Move(appearTiming + frameDelayHundred, posTen.X - (numberWidth / 2), posTen.Y);

            hundredFail.Move(appearTiming, posHundred.X - (numberWidth * 1.5f), posHundred.Y);
            hundredFail.Move(appearTiming + frameDelayTen, posHundred.X - numberWidth, posHundred.Y);
            hundredFail.Move(appearTiming + frameDelayHundred, posHundred.X - (numberWidth / 2), posHundred.Y);


            // opacity

            oneFail.Fade(appearTiming, 0);
            oneFail.Fade(appearTiming, duration, fade, fade);
            oneFail.Fade(duration + 1, 0);

            tenFail.Fade(appearTiming, 0);
            tenFail.Fade(appearTiming + frameDelayTen, duration, fade, fade);
            tenFail.Fade(duration + 1, 0);

            hundredFail.Fade(appearTiming, 0);
            hundredFail.Fade(appearTiming + frameDelayHundred, duration, fade, fade);
            hundredFail.Fade(duration + 1, 0);


            if (randomHundredFail > 0)
            {
                string[] result = { $"You gained: -{randomHundredFail}{randomTenFail}{randomOneFail} points" };
                this.pointsFail.Setup(this, font, appearTiming + delay, durationText, "Points Fail", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 1000, ColorFail, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/blank.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsFail.Generate(result, 50, 1000, startTriggerGroup, "Failing", noteStartFail, noteEndFail, triggerGroup + 1);

                int[] sectionPoints = new int[] { 0, randomHundredFail, randomTenFail, randomOneFail };
                pointSystem.AddFailPoints(sectionPoints);
            }
            else if (randomTenFail == 0 && randomOneFail == 0)
            {
                string[] result = { $"You gained: -{randomHundredFail}00 points" };
                this.pointsFail.Setup(this, font, appearTiming + delay, durationText, "Points Fail", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 1000, ColorFail, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/blank.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsFail.Generate(result, 50, 1000, startTriggerGroup, "Failing", noteStartFail, noteEndFail, triggerGroup + 1);

                int[] sectionPoints = new int[] { 0, randomHundredFail, 0, 0 };
                pointSystem.AddFailPoints(sectionPoints);
            }
            else if (randomTenFail == 0)
            {
                string[] result = { $"You gained: -{randomHundredFail}0{randomOneFail} points" };
                this.pointsFail.Setup(this, font, appearTiming + delay, durationText, "Points Fail", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 1000, ColorFail, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/blank.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsFail.Generate(result, 50, 1000, startTriggerGroup, "Failing", noteStartFail, noteEndFail, triggerGroup + 1);

                int[] sectionPoints = new int[] { 0, randomHundredFail, 0, randomOneFail };
                pointSystem.AddFailPoints(sectionPoints);
            }
            else if (randomOneFail == 0)
            {
                string[] result = { $"You gained: -{randomHundredFail}{randomTenFail}0 points" };
                this.pointsFail.Setup(this, font, appearTiming + delay, durationText, "Points Fail", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 1000, ColorFail, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/blank.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsFail.Generate(result, 50, 1000, startTriggerGroup, "Failing", noteStartFail, noteEndFail, triggerGroup + 1);

                int[] sectionPoints = new int[] { 0, randomHundredFail, randomTenFail, 0 };
                pointSystem.AddFailPoints(sectionPoints);
            }

            string[] pts2 = { "pts" };

            var PTS = true;
            if (PTS == true)
            {
                this.pointsFail.Setup(this, font2, appearTiming, appearTiming + frameDelayTen, "Points Fail", pos.X - (numberWidth * 1.5f) + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/blank.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsFail.Generate(pts2, 0, 0, startTriggerGroup, "Failing", noteStartFail, noteEndFail, triggerGroup + 1);
            }

            if (PTS == true)
            {
                this.pointsFail.Setup(this, font2, appearTiming + frameDelayTen, appearTiming + frameDelayHundred, "Points Fail", pos.X - numberWidth + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/blank.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsFail.Generate(pts2, 0, 0, startTriggerGroup, "Failing", noteStartFail, noteEndFail, triggerGroup + 1);
            }

            if (PTS == true)
            {
                this.pointsFail.Setup(this, font2, appearTiming + frameDelayHundred, duration, "Points Fail", pos.X - (numberWidth / 2) + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/blank.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsFail.Generate(pts2, 0, 0, startTriggerGroup, "Failing", noteStartFail, noteEndFail, triggerGroup + 1);
            }

            oneFail.EndGroup();
            tenFail.EndGroup();
            hundredFail.EndGroup();

            Log($"Pass/Fail:                         {pointSystem.pointsPass} ; {pointSystem.pointsFail}");
        }

















        public void Background()
        {
            var bitmap = GetMapsetBitmap("sb/bgs/1/bg.jpg");
            var bg = GetLayer("Background").CreateSprite("sb/bgs/1/bg.jpg", OsbOrigin.Centre, new Vector2(320, 240));

            bg.Scale(StartTime - 2000, 480.0f / bitmap.Height);
            bg.Fade(StartTime - 2000, StartTime, 0, 0.4);
            bg.Fade(EndTime, EndTime + 2000, 0.4, 0);
        }

        public void CyanRain(float Fade, string LayerName, int interval, float ScaleX, float ScaleY)
        {
            var startTime = StartTime;
            var endTime = EndTime;
            for (var i = startTime; i < endTime; i += interval)
            {
                var rain = GetLayer(LayerName).CreateSprite("sb/cyanRain.png", OsbOrigin.BottomCentre);

                var speed = Random(1000, 8000);

                var StartPosX = Random(-107, 747);
                var EndPosX = StartPosX; // + Random(0, 0);
                var StartPosY = Random(-100, 0);
                var EndPosY = Random(400, 460);

                var DeltaX = EndPosX - StartPosX;
                var DeltaY = EndPosY - StartPosY;
                // var Rotation = Math.Atan2(DeltaY, DeltaX) * Math.PI;

                rain.Fade(i, Fade);
                rain.MoveX(i, i + speed, StartPosX, EndPosX);
                rain.MoveY(i, i + speed, StartPosY, EndPosY);
                // rain.Rotate(i, i + speed, 0, Rotation);

                if (speed >= 5000)
                {
                    rain.ScaleVec(i + speed, i + speed + Random(1000, 2000), ScaleX, ScaleY, ScaleX, 0);
                }

                if (speed <= 5000)
                {
                    rain.ScaleVec(i + speed, i + speed + Random(500, 1000), ScaleX, ScaleY, ScaleX, 0);
                }


                // SPLASH EFFECT

                var delay = 300;
                var end = i + speed + delay + interval + 500;
                for (var s = i + speed; s < end; s += 1000)
                {
                    var splashStart = new Vector2(0, 0f);
                    var splashEnd = new Vector2(0.5f, 0.05f);
                    var splash = GetLayer("splash").CreateSprite("sb/splash.png", OsbOrigin.Centre);

                    splash.MoveX(s, EndPosX);
                    splash.MoveY(s, EndPosY);

                    var fade = (float)Random(0.1f, 0.2f);
                    splash.Fade(s - delay, s, 0, fade);
                    splash.Fade(s + delay, s + delay + interval + 500, fade, 0);

                    splash.ScaleVec(s, s + delay + interval + 500, splashStart, splashEnd);
                }
            }
        }

        public void ResultsPass(int NoteStart, int NoteEnd, int startTime, int endTime, int speed,
                           bool startTriggerGroup = false)
        {
            var noteStartPass = NoteStart - 5; // start range of the first note in milliseconds
            var noteEndPass = NoteEnd + 5; // end range of the last note in milliseconds

            var duration = endTime - noteStartPass; // duration for the TriggerGroup
            var appearTiming = startTime - noteStartPass; // the duration value between the note(s) and the break start

            // DIALOG BOXES STARTS HERE
            var fontSize = 15; //  japanese
            // var fontSize = 20; // english
            var GlowRadius = 0;
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            var font = LoadFont("sb/dialog/txt/pass/1/jp", new FontDescription() // japanese
            // var font = LoadFont("sb/dialog/txt/pass/1", new FontDescription() // english
            {
                FontPath = "font/jp/KozGoPro-Light.otf", // japanese
                // FontPath = "Microsoft Yi Baiti", // english
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
                Color = GlowColor,
            },
            new FontOutline()
            {
                Thickness = OutlineThickness,
                Color = OutlineColor,
            },
            new FontShadow()
            {
                Thickness = ShadowThickness,
                Color = ShadowColor,
            });


            // // DIALOG 1 -----------------------------------------
            // string[] result = { "The system is safe from collapse and our",
            //                     "mappers are back to normal again!" };
            // this.pointsPass.Setup(this, font, appearTiming, duration, "Dialog - Text", 340, 270, true,
            //     fontSize, 1, 50, 50, TextColor, true, 0.8f, BoxColor, "Dialog - Box", 0, "sb/sfx/blank.ogg",
            //     DialogBoxes.Pointer.CentreLeft, DialogBoxes.Push.Right);
            // this.pointsPass.Generate(result, 50, 50, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, 51);

            // // DIALOG 2 -----------------------------------------
            // string[] text = { "DON'T SKIP YET" };
            // var dialog = new DialogManager(this, font, 490192, 490192 + 18000, "Dialog - Text", 320, 340, true,
            //     fontSize, 1, 50, 50, Color4.IndianRed, false, 0.7f, BoxColor, "Dialog - Box", 0, "sb/sfx/blank.ogg",
            //     DialogBoxes.Pointer.None, DialogBoxes.Push.Right, text);

            // DIALOG 1 -----------------------------------------
            string[] result = { "システムが崩壊する前に無事にマッパー達も通常に戻りました！" };
            this.pointsPass.Setup(this, font, appearTiming, duration, "Dialog - Text", 340, 270, true,
                fontSize, 1, 50, 50, TextColor, true, 0.8f, BoxColor, "Dialog - Box", 0, "sb/sfx/blank.ogg",
                DialogBoxes.Pointer.CentreLeft, DialogBoxes.Push.Right);
            this.pointsPass.Generate(result, 50, 50, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, 51);

            // DIALOG 2 -----------------------------------------
            string[] text = { "DON'T SKIP YET" };
            var dialog = new DialogManager(this, font, 490192, 490192 + 18000, "Dialog - Text", 320, 340, true,
                fontSize, 1, 50, 50, Color4.IndianRed, false, 0.7f, BoxColor, "Dialog - Box", 0, "sb/sfx/blank.ogg",
                DialogBoxes.Pointer.None, DialogBoxes.Push.Right, text);
        }

        public void ResultsPass2(int NoteStart, int NoteEnd, int startTime, int endTime, int speed,
                           bool startTriggerGroup = false)
        {
            var noteStartPass = NoteStart - 5; // start range of the first note in milliseconds
            var noteEndPass = NoteEnd + 5; // end range of the last note in milliseconds

            var duration = endTime - noteStartPass; // duration for the TriggerGroup
            var appearTiming = startTime - noteStartPass; // the duration value between the note(s) and the break start

            // DIALOG BOXES STARTS HERE
            var fontSize = 15; //  japanese
            // var fontSize = 20; // english
            var GlowRadius = 0;
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            var font = LoadFont("sb/dialog/txt/pass/2/jp", new FontDescription() // japanese
            // var font = LoadFont("sb/dialog/txt/pass/2", new FontDescription() // english
            {
                FontPath = "font/jp/KozGoPro-Light.otf", // japanese
                // FontPath = "Microsoft Yi Baiti", // english
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
                Color = GlowColor,
            },
            new FontOutline()
            {
                Thickness = OutlineThickness,
                Color = OutlineColor,
            },
            new FontShadow()
            {
                Thickness = ShadowThickness,
                Color = ShadowColor,
            });

            // // DIALOG 1 -----------------------------------------
            // string[] result2 = { "You have saved the mission and have completed",
            //                      "the map! Thank you for playing!" };
            // this.pointsPass.Setup(this, font, appearTiming, duration, "Dialog - Text", 330, 270, true,
            //     fontSize, 1, 50, 50, TextColor, true, 0.8f, BoxColor, "Dialog - Box", 0, "sb/sfx/blank.ogg",
            //     DialogBoxes.Pointer.CentreLeft, DialogBoxes.Push.Right);
            // this.pointsPass.Generate(result2, 50, 50, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, 51);

            // DIALOG 1 -----------------------------------------
            string[] result2 = { "あなたはミッションを成し遂げマップをクリアしました！",
                                 "プレイをしていただきありがとうございました！" };
            this.pointsPass.Setup(this, font, appearTiming, duration, "Dialog - Text", 330, 270, true,
                fontSize, 1, 50, 50, TextColor, true, 0.8f, BoxColor, "Dialog - Box", 0, "sb/sfx/blank.ogg",
                DialogBoxes.Pointer.CentreLeft, DialogBoxes.Push.Right);
            this.pointsPass.Generate(result2, 50, 50, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, 51);
        }

        public void ResultsFail(int NoteStart, int NoteEnd, int startTime, int endTime, int speed,
                           bool startTriggerGroup = false)
        {
            var noteStartPass = NoteStart - 5; // start range of the first note in milliseconds
            var noteEndPass = NoteEnd + 5; // end range of the last note in milliseconds

            var duration = endTime - noteStartPass; // duration for the TriggerGroup
            var appearTiming = startTime - noteStartPass; // the duration value between the note(s) and the break start

            // DIALOG BOXES STARTS HERE
            var fontSize = 15; //  japanese
            // var fontSize = 20; // english
            var GlowRadius = 0;
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            var font = LoadFont("sb/dialog/txt/fail/1/jp", new FontDescription() // japanese
            // var font = LoadFont("sb/dialog/txt/fail/1", new FontDescription() // english
            {
                FontPath = "font/jp/KozGoPro-Light.otf", // japanese
                // FontPath = "Microsoft Yi Baiti", // english
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
                Color = GlowColor,
            },
            new FontOutline()
            {
                Thickness = OutlineThickness,
                Color = OutlineColor,
            },
            new FontShadow()
            {
                Thickness = ShadowThickness,
                Color = ShadowColor,
            });
            
            // // DIALOG 1 -----------------------------------------
            // string[] result = { "Perhaps a system restore is due for all of us...",
            //                     "Unfortunately you have failed to save the mission." };
            // this.pointsFail.Setup(this, font, appearTiming, duration, "Dialog - Text", 350, 270, true,
            //     fontSize, 1, 50, 50, TextColor, true, 0.8f, BoxColor, "Dialog - Box", 0, "sb/sfx/blank.ogg",
            //     DialogBoxes.Pointer.CentreLeft, DialogBoxes.Push.Left);
            // this.pointsFail.Generate(result, 50, 50, startTriggerGroup, "Failing", noteStartPass, noteEndPass, 57);
            
            // DIALOG 1 -----------------------------------------
            string[] result = { "おそらくシステムの復元を私たちのためにしなければなりません...",
                                "残念ながらミッションを成し遂げることができませんでした。" };
            this.pointsFail.Setup(this, font, appearTiming, duration, "Dialog - Text", 350, 270, true,
                fontSize, 1, 50, 50, TextColor, true, 0.8f, BoxColor, "Dialog - Box", 0, "sb/sfx/blank.ogg",
                DialogBoxes.Pointer.CentreLeft, DialogBoxes.Push.Left);
            this.pointsFail.Generate(result, 50, 50, startTriggerGroup, "Failing", noteStartPass, noteEndPass, 57);
        }

        public void ResultsFail2(int NoteStart, int NoteEnd, int startTime, int endTime, int speed,
                           bool startTriggerGroup = false)
        {
            var noteStartPass = NoteStart - 5; // start range of the first note in milliseconds
            var noteEndPass = NoteEnd + 5; // end range of the last note in milliseconds

            var duration = endTime - noteStartPass; // duration for the TriggerGroup
            var appearTiming = startTime - noteStartPass; // the duration value between the note(s) and the break start

            // DIALOG BOXES STARTS HERE
            var fontSize = 15; //  japanese
            // var fontSize = 20; // english
            var GlowRadius = 0;
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            var font = LoadFont("sb/dialog/txt/fail/2/jp", new FontDescription() // japanese
            // var font = LoadFont("sb/dialog/txt/fail/2", new FontDescription() // english
            {
                FontPath = "font/jp/KozGoPro-Light.otf", // japanese
                // FontPath = "Microsoft Yi Baiti", // english
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
                Color = GlowColor,
            },
            new FontOutline()
            {
                Thickness = OutlineThickness,
                Color = OutlineColor,
            },
            new FontShadow()
            {
                Thickness = ShadowThickness,
                Color = ShadowColor,
            });

            // // DIALOG 1 -----------------------------------------
            // string[] result2 = { "Do try the map again for a better grade!" };
            // this.pointsFail.Setup(this, font, appearTiming, duration, "Dialog - Text", 360, 270, true,
            //     fontSize, 1, 50, 50, TextColor, true, 0.8f, BoxColor, "Dialog - Box", 0, "sb/sfx/blank.ogg",
            //     DialogBoxes.Pointer.CentreLeft, DialogBoxes.Push.Left);
            // this.pointsFail.Generate(result2, 50, 50, startTriggerGroup, "Failing", noteStartPass, noteEndPass, 57);

            // DIALOG 1 -----------------------------------------
            string[] result2 = { "良い成績を取るにはもう一度プレイをしてみてください！" };
            this.pointsFail.Setup(this, font, appearTiming, duration, "Dialog - Text", 360, 270, true,
                fontSize, 1, 50, 50, TextColor, true, 0.8f, BoxColor, "Dialog - Box", 0, "sb/sfx/blank.ogg",
                DialogBoxes.Pointer.CentreLeft, DialogBoxes.Push.Left);
            this.pointsFail.Generate(result2, 50, 50, startTriggerGroup, "Failing", noteStartPass, noteEndPass, 57);
        }

        public void Avatars()
        {
            var duration = 10000;
            var startTime = StartTime;
            var HoverEndTime = EndTime;
            var HoverVelocity = 10;
            var HoverDuration = 3000;
            var HoverLoopCount = (HoverEndTime - startTime) / HoverDuration;

            Tochi(130, 430, 1, startTime, HoverEndTime, duration, (int)HoverLoopCount, HoverDuration, HoverVelocity);
        }

        public void Tochi(float posX, float posY, float Fade, int startTime, int endTime, int duration, int LoopCount, int HoverDuration, int HoverVelocity)
        {
            var reMoveY = 320;
            var reMoveX = -70;
            // var b = GetMapsetBitmap("sb/avatars/-Tochi.png");
            var nameTag = GetLayer("nameTag").CreateSprite("sb/avatars/-TochiTag.png", OsbOrigin.Centre);
            var avatar = GetLayer("-Tochi").CreateSprite("sb/avatars/-Tochi.png", OsbOrigin.BottomCentre);

            // hovering stuff
            avatar.StartLoopGroup(startTime, LoopCount);
            avatar.MoveY(OsbEasing.InOutSine, 0, HoverDuration, posY - HoverVelocity, posY + HoverVelocity);
            avatar.MoveY(OsbEasing.InOutSine, HoverDuration, HoverDuration * 2, posY + HoverVelocity, posY - HoverVelocity);
            avatar.EndGroup();
            nameTag.StartLoopGroup(startTime, LoopCount);
            nameTag.MoveY(OsbEasing.InOutSine, 100, HoverDuration, posY - reMoveY - HoverVelocity, posY - reMoveY + HoverVelocity);
            nameTag.MoveY(OsbEasing.InOutSine, HoverDuration, HoverDuration * 2, posY - reMoveY + HoverVelocity, posY - reMoveY - HoverVelocity);
            nameTag.EndGroup();

            // avatar stuff
            avatar.MoveX(startTime, posX);
            avatar.Scale(startTime, 0.25);
            avatar.Fade(startTime, startTime + (HoverDuration / 2), 0, Fade);
            avatar.Fade(endTime - (HoverDuration / 2), endTime, Fade, 0);
            nameTag.MoveX(startTime, posX + reMoveX);
            nameTag.Scale(startTime, 0.5);
            nameTag.Fade(startTime, startTime + (HoverDuration / 2), 0, Fade);
            nameTag.Fade(endTime - (HoverDuration / 2), endTime, Fade, 0);
            nameTag.Color(startTime, TochiTagColor);


            // REFLECTION IN THE WATER

            var avatarReflection = GetLayer("-Tochi").CreateSprite("sb/avatars/-Tochi.png", OsbOrigin.BottomCentre);

            // hovering stuff
            avatarReflection.StartLoopGroup(startTime, LoopCount);
            avatarReflection.MoveY(OsbEasing.InOutSine, 0, HoverDuration, posY + HoverVelocity, posY - HoverVelocity);
            avatarReflection.MoveY(OsbEasing.InOutSine, HoverDuration, HoverDuration * 2, posY - HoverVelocity, posY + HoverVelocity);
            avatarReflection.EndGroup();

            // avatar stuff
            avatarReflection.MoveX(startTime, posX);
            avatarReflection.Scale(startTime, 0.25);
            avatarReflection.Fade(startTime, startTime + (HoverDuration / 2), 0, Fade / 5);
            avatarReflection.Fade(endTime - (HoverDuration / 2), endTime, Fade / 5, 0);
            avatarReflection.Rotate(startTime, MathHelper.DegreesToRadians(180));
        }
    }
}

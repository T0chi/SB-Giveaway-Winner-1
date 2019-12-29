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

        public override void Generate()
        {

            pointSystem = new PointSystem();
            pointsPass = new DialogManager();
            pointsFail = new DialogManager();

            points(1, 2, "sb/points/1", 48476, 48476, 61399, 72571, 2, true); // break 1
            currentPointsPass("sb/points/1", 48476, 48476, 61399, 72571, pointSystem.totalPass, true);
            currentPointsFail("sb/points/1", 48476, 48476, 61399, 72571, pointSystem.totalFail, true);


            points(1, 2, "sb/points/2", 106905, 107572, 115238, 128262, 2, true); // break 2
            currentPointsPass("sb/points/2", 106905, 107572, 115238, 128262, pointSystem.totalPass, true);
            currentPointsFail("sb/points/2", 106905, 107572, 115238, 128262, pointSystem.totalFail, true);

            points(1, 2, "sb/points/3", 146863, 147063, 157062, 167640, 2, true); // break 3
            currentPointsPass("sb/points/3", 146863, 147063, 157062, 167640, pointSystem.totalPass, true);
            currentPointsFail("sb/points/3", 146863, 147063, 157062, 167640, pointSystem.totalFail, true);

            points(1, 2, "sb/points/4", 197982, 198240, 213925, 221716, 2, true); // break 4
            currentPointsPass("sb/points/4", 197982, 198240, 213925, 221716, pointSystem.totalPass, true);
            currentPointsFail("sb/points/4", 197982, 198240, 213925, 221716, pointSystem.totalFail, true);

            points(1, 2, "sb/points/5", 237589, 237589, 247113, 254732, 2, true); // break 5
            currentPointsPass("sb/points/5", 237589, 237589, 247113, 254732, pointSystem.totalPass, true);
            currentPointsFail("sb/points/5", 237589, 237589, 247113, 254732, pointSystem.totalFail, true);

            points(1, 2, "sb/points/6", 274561, 274639, 283671, 290901, 2, true); // break 6
            currentPointsPass("sb/points/6", 274561, 274639, 283671, 290901, pointSystem.totalPass, true);
            currentPointsFail("sb/points/6", 274561, 274639, 283671, 290901, pointSystem.totalFail, true);

            points(1, 2, "sb/points/7", 320430, 321059, 331111, 339380, 2, true); // break 7
            currentPointsPass("sb/points/7", 320430, 321059, 331111, 339380, pointSystem.totalPass, true);
            currentPointsFail("sb/points/7", 320430, 321059, 331111, 339380, pointSystem.totalFail, true);

            points(1, 2, "sb/points/8", 484268, 484343, 488992, 488992 + 8000, 2, true); // break 8
            currentPointsPass("sb/points/8", 484268, 484343, 488992, 488992 + 8000, pointSystem.totalPass, true);
            currentPointsFail("sb/points/8", 484268, 484343, 488992, 488992 + 8000, pointSystem.totalFail, true);

            fightSound(398622, 488992);


            Log($"TOTALPOINTS:                {pointSystem.totalPass} ; {pointSystem.totalFail}");
        }

        public void fightSound(int breakStart, int endTime)
        {
            var fightSFX = GetLayer("Points Pass").CreateSample("sb/sfx/fight-start.ogg", breakStart, 100);
            var fightSFX2 = GetLayer("Points Fail").CreateSample("sb/sfx/fight-start.ogg", breakStart, 100);
        }

        public void currentPointsPass(string fontPath, int NoteStart, int NoteEnd, int breakStart, int breakEnd, int points,
                                      bool startTriggerGroup = false)
        {
            var noteStartPass = NoteStart - 5; // start range of the first note in milliseconds
            var noteEndPass = NoteEnd + 5; // end range of the last note in milliseconds

            var delay = ((breakEnd - breakStart) / 2) - 800; // calculation for the middle of the break
            var endTime = breakStart + delay; // middle of the break
            var duration = breakEnd - noteStartPass; // duration for the TriggerGroup

            var appearTiming = breakStart - noteStartPass; // the duration value between the note(s) and the break start

            var fontSize = 14;
            var GlowRadius = 0;
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            var font = LoadFont(fontPath + "/cpPass", new FontDescription()
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


            // Trigger Start

            string[] resultPass = { $"Overall: {points}pts" };
            this.pointsPass.Setup(this, font, appearTiming + delay, duration, "Points Pass", pos.X, pos.Y, true,
                fontSize, 0.7f, 50, 1000, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/blank.ogg",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
            this.pointsPass.Generate(resultPass, 50, 1000, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, 1);
        }

        public void currentPointsFail(string fontPath, int NoteStart, int NoteEnd, int breakStart, int breakEnd, int points,
                                      bool startTriggerGroup = false)
        {
            var noteStartFail = NoteStart - 20; // start range of the first note in milliseconds
            var noteEndFail = NoteEnd + 20; // end range of the last note in milliseconds

            var delay = ((breakEnd - breakStart) / 2) - 800; // calculation for the middle of the break
            var endTime = breakStart + delay; // middle of the break
            var duration = breakEnd - noteStartFail; // duration for the TriggerGroup

            var appearTiming = breakStart - noteStartFail; // the duration value between the note(s) and the break start

            var fontSize = 14;
            var GlowRadius = 0;
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            var font = LoadFont(fontPath + "/cpFail", new FontDescription()
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


            // Trigger Start

            string[] resultFail = { $"Overall: {points}pts" };
            this.pointsFail.Setup(this, font, appearTiming + delay, duration, "Points Fail", pos.X, pos.Y, true,
                fontSize, 0.7f, 50, 1000, Color4.White, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/blank.ogg",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
            this.pointsFail.Generate(resultFail, 50, 1000, startTriggerGroup, "Failing", noteStartFail, noteEndFail, 2);
        }

        public void points(int GroupNumberPass, int GroupNumberFail, string fontPath, int NoteStart, int NoteEnd, int breakStart, int breakEnd, int speed,
                           bool startTriggerGroup = false)
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
            var font2 = LoadFont(fontPath + "/pts", new FontDescription()
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

















            var noteStartPass = NoteStart - 5; // start range of the first note in milliseconds
            var noteEndPass = NoteEnd + 5; // end range of the last note in milliseconds
            var noteStartFail = NoteStart - 20; // start range of the first note in milliseconds
            var noteEndFail = NoteEnd + 20; // end range of the last note in milliseconds

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

            dot.StartTriggerGroup("HitSound", noteStartPass, noteEndPass, GroupNumberPass);
            one.StartTriggerGroup("HitSound", noteStartPass, noteEndPass, GroupNumberPass);
            ten.StartTriggerGroup("HitSound", noteStartPass, noteEndPass, GroupNumberPass);
            hundred.StartTriggerGroup("HitSound", noteStartPass, noteEndPass, GroupNumberPass);
            thousand.StartTriggerGroup("HitSound", noteStartPass, noteEndPass, GroupNumberPass);

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
                    fontSize, 0.7f, 50, 1000, ColorPass, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsPass.Generate(result, 50, 1000, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, 1);

                int[] sectionPoints = new int[] { randomThousandPass, randomHundredPass, randomTenPass, randomOnePass };
                pointSystem.AddPassPoints(sectionPoints);
            }
            else if (randomTenPass == 0 && randomOnePass == 0)
            {
                string[] result = { $"You gained: +{randomHundredPass}00pts" };
                this.pointsPass.Setup(this, font, appearTiming + delay, durationText, "Points Pass", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 1000, ColorPass, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsPass.Generate(result, 50, 1000, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, 1);

                int[] sectionPoints = new int[] { randomThousandPass, randomHundredPass, 0, 0 };
                pointSystem.AddPassPoints(sectionPoints);
            }
            else if (randomTenPass == 0)
            {
                string[] result = { $"You gained: +{randomThousandPass}{randomHundredPass}0{randomOnePass} points" };
                this.pointsPass.Setup(this, font, appearTiming + delay, durationText, "Points Pass", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 1000, ColorPass, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsPass.Generate(result, 50, 1000, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, 1);

                int[] sectionPoints = new int[] { randomThousandPass, randomHundredPass, 0, randomOnePass };
                pointSystem.AddPassPoints(sectionPoints);
            }
            else if (randomOnePass == 0)
            {
                string[] result = { $"You gained: +{randomThousandPass}{randomHundredPass}{randomTenPass}0pts" };
                this.pointsPass.Setup(this, font, appearTiming + delay, durationText, "Points Pass", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 1000, ColorPass, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsPass.Generate(result, 50, 1000, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, 1);

                int[] sectionPoints = new int[] { randomThousandPass, randomHundredPass, randomTenPass, 0 };
                pointSystem.AddPassPoints(sectionPoints);
            }
            else if (randomThousandPass == 0)
            {
                string[] result = { $"You gained: +{randomHundredPass}{randomTenPass}{randomOnePass} points" };
                this.pointsPass.Setup(this, font, appearTiming + delay, durationText, "Points Pass", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 1000, ColorPass, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsPass.Generate(result, 50, 1000, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, 1);

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
                        fontSize2, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                        DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                    this.pointsPass.Generate(pts, 0, 0, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, 1);
                }
                
                if (PTS2 == true)
                {
                    this.pointsPass.Setup(this, font2, appearTiming + frameDelayTen, appearTiming + frameDelayHundred, "Points Pass", pos.X - numberWidth + (numberWidth * 2.7f), pos.Y - 2, true,
                        fontSize2, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                        DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                    this.pointsPass.Generate(pts, 0, 0, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, 1);
                }

                
                if (PTS2 == true)
                {
                    this.pointsPass.Setup(this, font2, appearTiming + frameDelayHundred, duration, "Points Pass", pos.X - (numberWidth / 2) + (numberWidth * 2.7f), pos.Y - 2, true,
                        fontSize2, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                        DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                    this.pointsPass.Generate(pts, 0, 0, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, 1);
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
                        fontSize2, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                        DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                    this.pointsPass.Generate(pts, 0, 0, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, 1);
                }

                if (PTS2 == true)
                {
                    this.pointsPass.Setup(this, font2, appearTiming + frameDelayTen, appearTiming + frameDelayHundred, "Points Pass", pos.X - numberWidth + (numberWidth * 2.7f), pos.Y - 2, true,
                        fontSize2, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                        DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                    this.pointsPass.Generate(pts, 0, 0, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, 1);
                }

                if (PTS2 == true)
                {
                    this.pointsPass.Setup(this, font2, appearTiming + frameDelayHundred, appearTiming + frameDelayThousand, "Points Pass", pos.X - (numberWidth / 2) + (numberWidth * 2.7f), pos.Y - 2, true,
                        fontSize2, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                        DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                    this.pointsPass.Generate(pts, 0, 0, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, 1);
                }

                if (PTS2 == true)
                {
                    this.pointsPass.Setup(this, font2, appearTiming + frameDelayThousand, duration, "Points Pass", pos.X + (numberWidth * 2.7f), pos.Y - 2, true,
                        fontSize2, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "Points Pass", 300, "sb/sfx/points-result.ogg",
                        DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                    this.pointsPass.Generate(pts, 0, 0, startTriggerGroup, "HitSound", noteStartPass, noteEndPass, 1);
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

            oneFail.StartTriggerGroup("Failing", noteStartFail, noteEndFail, GroupNumberFail);
            tenFail.StartTriggerGroup("Failing", noteStartFail, noteEndFail, GroupNumberFail);
            hundredFail.StartTriggerGroup("Failing", noteStartFail, noteEndFail, GroupNumberFail);


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
                string[] result = { $"You gained: +{randomHundredFail}{randomTenFail}{randomOneFail} points" };
                this.pointsFail.Setup(this, font, appearTiming + delay, durationText, "Points Fail", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 1000, ColorFail, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsPass.Generate(result, 50, 1000, startTriggerGroup, "Failing", noteStartFail, noteEndFail, 2);

                int[] sectionPoints = new int[] { 0, randomHundredFail, randomTenFail, randomOneFail };
                pointSystem.AddFailPoints(sectionPoints);
            }
            else if (randomTenFail == 0 && randomOneFail == 0)
            {
                string[] result = { $"You gained: +{randomHundredFail}00pts" };
                this.pointsFail.Setup(this, font, appearTiming + delay, durationText, "Points Fail", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 1000, ColorFail, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsPass.Generate(result, 50, 1000, startTriggerGroup, "Failing", noteStartFail, noteEndFail, 2);

                int[] sectionPoints = new int[] { 0, randomHundredFail, 0, 0 };
                pointSystem.AddFailPoints(sectionPoints);
            }
            else if (randomTenFail == 0)
            {
                string[] result = { $"You gained: +{randomHundredFail}0{randomOneFail} points" };
                this.pointsFail.Setup(this, font, appearTiming + delay, durationText, "Points Fail", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 1000, ColorFail, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsPass.Generate(result, 50, 1000, startTriggerGroup, "Failing", noteStartFail, noteEndFail, 2);

                int[] sectionPoints = new int[] { 0, randomHundredFail, 0, randomOneFail };
                pointSystem.AddFailPoints(sectionPoints);
            }
            else if (randomOneFail == 0)
            {
                string[] result = { $"You gained: +{randomHundredFail}{randomTenFail}0pts" };
                this.pointsFail.Setup(this, font, appearTiming + delay, durationText, "Points Fail", pos.X, pos.Y - (numberHeight / 4), true,
                    fontSize, 0.7f, 50, 1000, ColorFail, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsPass.Generate(result, 50, 1000, startTriggerGroup, "Failing", noteStartFail, noteEndFail, 2);

                int[] sectionPoints = new int[] { 0, randomHundredFail, randomTenFail, 0 };
                pointSystem.AddFailPoints(sectionPoints);
            }

            string[] pts2 = { "pts" };

            var PTS = true;
            if (PTS == true)
            {
                this.pointsFail.Setup(this, font2, appearTiming, appearTiming + frameDelayTen, "Points Fail", pos.X - (numberWidth * 1.5f) + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsPass.Generate(pts2, 0, 0, startTriggerGroup, "Failing", noteStartFail, noteEndFail, 2);
            }

            if (PTS == true)
            {
                this.pointsFail.Setup(this, font2, appearTiming + frameDelayTen, appearTiming + frameDelayHundred, "Points Fail", pos.X - numberWidth + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsPass.Generate(pts2, 0, 0, startTriggerGroup, "Failing", noteStartFail, noteEndFail, 2);
            }

            if (PTS == true)
            {
                this.pointsFail.Setup(this, font2, appearTiming + frameDelayHundred, duration, "Points Fail", pos.X - (numberWidth / 2) + (numberWidth * 2.7f), pos.Y - 2, true,
                    fontSize2, 0.7f, 0, 0, Color4.White, false, 0.3f, Color4.Black, "Points Fail", 300, "sb/sfx/points-result.ogg",
                    DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None);
                this.pointsPass.Generate(pts2, 0, 0, startTriggerGroup, "Failing", noteStartFail, noteEndFail, 2);
            }

            oneFail.EndGroup();
            tenFail.EndGroup();
            hundredFail.EndGroup();

            Log($"Pass/Fail:                         {pointSystem.pointsPass} ; {pointSystem.pointsFail}");
        }
    }
}

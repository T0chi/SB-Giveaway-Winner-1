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
    public class Section2 : StoryboardObjectGenerator
    {
        [Configurable]
        public Color4 BoxColor = new Color4(255, 255, 255, 255);
        
        [Configurable]
        public Color4 BoxColorText = new Color4(255, 255, 255, 255);

        [Configurable]
        public Color4 BoxColorNarrator = new Color4(255, 255, 255, 255);

        [Configurable]
        public Color4 BoxColorNarratorText = new Color4(255, 255, 255, 255);

        [Configurable]
        public Color4 OutlineColor = new Color4(50, 50, 50, 200);

        [Configurable]
        public Color4 ShadowColor = new Color4(0, 0, 0, 200);

        private DialogManager dialog;

        private DialogManager dialog2;

        public override void Generate()
        {
            Background();
            HUD(61860, 115238, 72572, "Mission #1", "Ein", "sb/HUD/txt/nameTag/Pino.png", 0, "sb/avatars/PinoProfile.png");

            // Mission is to destroy all the enemies (aircrafts)
            Mission();
        }

        public void Background()
        {  
            var bitmap = GetMapsetBitmap("sb/bgs/2/bg.jpg");
            var bg = GetLayer("Background").CreateSprite("sb/bgs/2/bg.jpg", OsbOrigin.Centre, new Vector2(320, 240));

            bg.Scale(57245, 480.0f / bitmap.Height);
            bg.Fade(57245, 72572, 0, 0.5);
            bg.Fade(115238, 128263, 0.5, 0);
        }

        public void HUD(int startTime, int endTime, int loadingTextEndtime, string mission, string songName, string nameTag, int progressBarDelay, string avatar)
        {
            var hud = new HUD(this, startTime, endTime, loadingTextEndtime, mission, songName, nameTag, progressBarDelay, avatar);
        }

        public void AircraftIntro()
        {
            var startTime = 72571;
            var endTime = 79238;
            var duration3 = 1500;
            var tiltAt = startTime + (duration3 * 3);

            var loopCount3 = (tiltAt - startTime) / (duration3 * 2);
            var jetSound = GetLayer("Aircraft 3 Front").CreateSample("sb/sfx/jet-1.ogg", startTime, 50);
            var jetSound2 = GetLayer("Aircraft 3 Front").CreateSample("sb/sfx/jet-3.ogg", startTime, 50);
            var jetSound3 = GetLayer("Aircraft 3 Front").CreateSample("sb/sfx/jet_passing-1.ogg", tiltAt - 700, 80);
            var aircraft3 = GetLayer("Aircraft 3 Front").CreateSprite("sb/missions/1/aircrafts/3_front.png", OsbOrigin.Centre);
        
            aircraft3.Scale(OsbEasing.Out, startTime, startTime + 4000, 0.05, 0.3);
            aircraft3.Fade(OsbEasing.Out, startTime, startTime + 2000, 0, 1);
            aircraft3.Fade(endTime - 500, endTime, 1, 0);

            aircraft3.StartLoopGroup(startTime, loopCount3);
            // x
            aircraft3.MoveX(OsbEasing.InOutSine, 0, duration3 / 2, 310, 330);
            aircraft3.MoveX(OsbEasing.InOutSine, duration3 / 2, duration3, 330, 300);
            aircraft3.MoveX(OsbEasing.InOutSine, duration3, duration3 * 2, 300, 310);
            aircraft3.MoveX(OsbEasing.InOutSine, duration3 * 2, duration3 * 3, 310, 330);
            // aircraft3.MoveX(OsbEasing.InOutSine, duration3 * 3, duration3 * 4, 330, 320);
            // aircraft3.MoveX(OsbEasing.InOutSine, duration3 * 4, duration3 * 5, 320, 310);
            // y
            aircraft3.MoveY(OsbEasing.InOutSine, 0, duration3 / 2, 270, 240);
            aircraft3.MoveY(OsbEasing.InOutSine, duration3 / 2, duration3, 240, 230);
            aircraft3.MoveY(OsbEasing.InOutSine, duration3, duration3 * 2, 230, 250);
            aircraft3.MoveY(OsbEasing.InOutSine, duration3 * 2, duration3 * 3, 250, 210);
            // aircraft3.MoveY(OsbEasing.InOutSine, duration3 * 3, duration3 * 4, 210, 230);
            // aircraft3.MoveY(OsbEasing.InOutSine, duration3 * 4, duration3 * 5, 230, 270);
            //
            aircraft3.EndGroup();

            // tilt
            aircraft3.MoveX(OsbEasing.InSine, tiltAt, endTime, 330, -180);
            aircraft3.MoveY(OsbEasing.InSine, tiltAt, endTime, 210, 330);
            var rotation5 = MathHelper.DegreesToRadians(5);
            var rotation = MathHelper.DegreesToRadians(-20);
            aircraft3.Rotate(OsbEasing.InOutSine, startTime, tiltAt - 3000, rotation5, -rotation5);
            aircraft3.Rotate(OsbEasing.InOutSine, tiltAt - 3000, tiltAt, -rotation5, rotation5 / 2);
            aircraft3.Rotate(OsbEasing.InSine, tiltAt, endTime, rotation5 / 2, rotation);

            // SMOKE
            float X = 0;
            float scale = 0.15f;
            float fade = 0;
            var smokeAmount = 40;
            var smokeDuration = 200;
            for (int i = startTime; i <= endTime; i += smokeAmount)
            {
                var smokeLeft = GetLayer("Aircraft 3 Front - Smoke").CreateAnimation("sb/missions/1/aircrafts/smoke/smoke.png",
                                                                4, 100, OsbLoopType.LoopForever, OsbOrigin.Centre);
                var smokeRight = GetLayer("Aircraft 3 Front - Smoke").CreateAnimation("sb/missions/1/aircrafts/smoke/smoke.png",
                                                                4, 100, OsbLoopType.LoopForever, OsbOrigin.Centre);
                int x = 42;
                int y = 15;
                int delay = 4;
                int fadeDelay = 10;

                if (i > startTime + 4000)
                {
                    smokeLeft.Fade(i, 0.2);
                    smokeLeft.Additive(i, i + smokeDuration);
                    smokeLeft.Scale(OsbEasing.In, i, i + (smokeDuration * fadeDelay), 0.3, 0.3 / 4);
                    smokeLeft.Fade(i + smokeDuration, i + (smokeDuration * fadeDelay), 0.2f, 0);
                    smokeLeft.Move(OsbEasing.Out, i, i + smokeDuration, aircraft3.PositionAt(i).X - x, aircraft3.PositionAt(i).Y - y,
                            aircraft3.PositionAt(i - (smokeDuration * 2)).X - x, aircraft3.PositionAt(i - (smokeDuration * delay)).Y - y);
                            
                    smokeRight.Fade(i, 0.2);
                    smokeRight.Additive(i, i + smokeDuration);
                    smokeRight.Scale(OsbEasing.In, i, i + (smokeDuration * fadeDelay), 0.3, 0.3 / 4);
                    smokeRight.Fade(i + smokeDuration, i + (smokeDuration * fadeDelay), 0.2f, 0);
                    smokeRight.Move(OsbEasing.Out, i, i + smokeDuration, aircraft3.PositionAt(i).X + x, aircraft3.PositionAt(i).Y - y,
                            aircraft3.PositionAt(i - (smokeDuration * 2)).X + x, aircraft3.PositionAt(i - (smokeDuration * delay)).Y - y);
                }

                else
                {
                    X += 0.4f;
                    scale += 0.0015f;
                    fade += 0.002f;

                    smokeLeft.Fade(i, fade);
                    smokeLeft.Additive(i, i + smokeDuration);
                    smokeLeft.Scale(OsbEasing.In, i, i + (smokeDuration * fadeDelay), scale, scale / 4);
                    smokeLeft.Fade(i + smokeDuration, i + (smokeDuration * fadeDelay), fade, 0);
                    smokeLeft.Move(OsbEasing.Out, i, i + smokeDuration, aircraft3.PositionAt(i).X - X, aircraft3.PositionAt(i).Y - y,
                            aircraft3.PositionAt(i - (smokeDuration * 2)).X - X, aircraft3.PositionAt(i - (smokeDuration * delay)).Y - y);

                    smokeRight.Fade(i, fade);
                    smokeRight.Additive(i, i + smokeDuration);
                    smokeRight.Scale(OsbEasing.In, i, i + (smokeDuration * fadeDelay), scale, scale / 4);
                    smokeRight.Fade(i + smokeDuration, i + (smokeDuration * fadeDelay), fade, 0);
                    smokeRight.Move(OsbEasing.Out, i, i + smokeDuration, aircraft3.PositionAt(i).X + X, aircraft3.PositionAt(i).Y - y,
                            aircraft3.PositionAt(i - (smokeDuration * 2)).X + X, aircraft3.PositionAt(i - (smokeDuration * delay)).Y - y);
                }
            }
        }

        public void CloudsIntro()
        {
            
            var startTime = 72571;
            var endTime = 79238;
            float cloudVelocity = 3;
            var d = (endTime - startTime) / 4;

            for (float i = startTime; i < startTime + d; i += cloudVelocity)
            {
                var duration = Random(400, 800);
                var RandomX = Random(-107, 747);
                var RandomFade = Random(0.005, 0.1);
                var RandomScale = Random(0.4, 2);
                var startPos  = new Vector2(RandomX, Random(355, 360));
                var endPos  = new Vector2(RandomX, Random(340, 345));

                var sprite = GetLayer("Clouds").CreateSprite("sb/missions/1/cloud/cloud" + Random(1, 11) + ".png", OsbOrigin.TopCentre);

                var loopCount = (endTime - startTime) / duration;
                sprite.StartLoopGroup(i, loopCount);
                sprite.Additive(0, duration);
                sprite.Fade(0, duration / 4, 0, RandomFade);
                sprite.Fade(duration - duration / 4, duration, RandomFade, 0);
                sprite.MoveX(0, duration, startPos.X, endPos.X);
                sprite.MoveY(0, duration, startPos.Y, endPos.Y);
                sprite.ScaleVec(OsbEasing.OutSine, 0, duration, RandomScale, RandomScale, RandomScale / 4, RandomScale / 4);
                sprite.EndGroup();
            }
        }

        public void Mission()
        {
            var startTime = 79238;
            var endTime = 112571;

            // INTRODUCTION TO THE MISSION
            AircraftIntro();
            CloudsIntro();

            // THE MISSION
            Aircraft(startTime, endTime);
            Clouds(startTime, endTime);
            AircraftEnemy(startTime, endTime);
        }

        public void Aircraft(int StartTime, int EndTime)
        {
            var startTime = StartTime;
            var endTime = EndTime + 2000;
            var duration3 = 1500;
            var enemyDuration = 1000;

            var loopCount3 = (startTime) / (duration3 * 5);
            var enemyloopCount = (startTime) / (enemyDuration * 5);
            // var jetSound = GetLayer("Aircraft 3 Front").CreateSample("sb/sfx/jet-1.ogg", startTime, 70);
            // var jetSound2 = GetLayer("Aircraft 3 Front").CreateSample("sb/sfx/jet-3.ogg", startTime, 70);
            // var jetSound3 = GetLayer("Aircraft 3 Front").CreateSample("sb/sfx/jet_passing-1.ogg", 700, 100);
            var aircraft = GetLayer("Aircraft 3 Back").CreateSprite("sb/missions/1/aircrafts/3_back.png", OsbOrigin.Centre);
            var enemy = GetLayer("Aircraft 1 & 2 Front").CreateSprite("sb/missions/1/aircrafts/2_back.png", OsbOrigin.Centre);

            aircraft.Scale(OsbEasing.Out, startTime, startTime + 4000, 4, 0.3);
            aircraft.Fade(OsbEasing.Out, startTime, startTime + 2000, 0, 1);
            aircraft.Fade(endTime - 500, endTime, 1, 0);

            aircraft.StartLoopGroup(startTime, loopCount3 - 1);
            // x
            aircraft.MoveX(OsbEasing.InOutSine, 0, duration3 / 2, 310, 330);
            aircraft.MoveX(OsbEasing.InOutSine, duration3 / 2, duration3, 330, 300);
            aircraft.MoveX(OsbEasing.InOutSine, duration3, duration3 * 2, 300, 310);
            aircraft.MoveX(OsbEasing.InOutSine, duration3 * 2, duration3 * 3, 310, 330);
            aircraft.MoveX(OsbEasing.InOutSine, duration3 * 3, duration3 * 4, 330, 320);
            aircraft.MoveX(OsbEasing.InOutSine, duration3 * 4, duration3 * 5, 320, 310);
            // y
            aircraft.MoveY(OsbEasing.InOutSine, 0, duration3 / 2, 270, 340);
            aircraft.MoveY(OsbEasing.InOutSine, duration3 / 2, duration3, 340, 330);
            aircraft.MoveY(OsbEasing.InOutSine, duration3, duration3 * 2, 330, 350);
            aircraft.MoveY(OsbEasing.InOutSine, duration3 * 2, duration3 * 3, 350, 310);
            aircraft.MoveY(OsbEasing.InOutSine, duration3 * 3, duration3 * 4, 310, 330);
            aircraft.MoveY(OsbEasing.InOutSine, duration3 * 4, duration3 * 5, 330, 270);
            //
            aircraft.EndGroup();

            enemy.Scale(OsbEasing.Out, startTime, startTime + 4000, 0.05, 0.1);
            enemy.Fade(OsbEasing.Out, startTime, startTime + 2000, 0, 1);
            enemy.Fade(endTime - 500, endTime, 1, 0);

            enemy.StartLoopGroup(startTime, enemyloopCount - 1);
            // x
            enemy.MoveX(OsbEasing.InOutSine, 0, enemyDuration / 2, 310, 330);
            enemy.MoveX(OsbEasing.InOutSine, enemyDuration / 2, enemyDuration, 330, 300);
            enemy.MoveX(OsbEasing.InOutSine, enemyDuration, enemyDuration * 2, 300, 310);
            enemy.MoveX(OsbEasing.InOutSine, enemyDuration * 2, enemyDuration * 3, 310, 330);
            enemy.MoveX(OsbEasing.InOutSine, enemyDuration * 3, enemyDuration * 4, 330, 320);
            enemy.MoveX(OsbEasing.InOutSine, enemyDuration * 4, enemyDuration * 5, 320, 310);
            // y
            enemy.MoveY(OsbEasing.InOutSine, 0, enemyDuration / 2, 270, 240);
            enemy.MoveY(OsbEasing.InOutSine, enemyDuration / 2, enemyDuration, 240, 230);
            enemy.MoveY(OsbEasing.InOutSine, enemyDuration, enemyDuration * 2, 230, 250);
            enemy.MoveY(OsbEasing.InOutSine, enemyDuration * 2, enemyDuration * 3, 250, 210);
            enemy.MoveY(OsbEasing.InOutSine, enemyDuration * 3, enemyDuration * 4, 210, 230);
            enemy.MoveY(OsbEasing.InOutSine, enemyDuration * 4, enemyDuration * 5, 230, 270);
            //
            enemy.EndGroup();

            // PEW PEW
            var randomStep = Random(100, 1000);
            for (var i = startTime + 3500; i < endTime; i += randomStep)
            {
                var speed = 500;
                var fadeTime = 50;
                var angle = Math.Atan2((enemy.PositionAt(i + (speed / 1.5f)).Y - aircraft.PositionAt(i).Y),
                                        (enemy.PositionAt(i + (speed / 1.5f)).X - aircraft.PositionAt(i).X)) + (Math.PI / 2);
                var pewRight = GetLayer("Laser").CreateSprite("sb/pixel.png", OsbOrigin.Centre);
                var pewLeft = GetLayer("Laser").CreateSprite("sb/pixel.png", OsbOrigin.Centre);
                var laserSound = GetLayer("Laser").CreateSample("sb/sfx/laser-1.ogg", i + 80, Random(5, 15));

                pewRight.Rotate(i, angle);
                pewRight.Color(i, Color4.Red);
                pewRight.Additive(i, i + speed + fadeTime);
                pewRight.ScaleVec(OsbEasing.OutExpo, i, i + (speed / 4), 3, 0, 3, 13);
                pewRight.ScaleVec(OsbEasing.Out, i + (speed / 4), i + speed, 3, 13, 2, 2);
                pewRight.Move(i, i + speed, aircraft.PositionAt(i).X + 20, aircraft.PositionAt(i).Y,
                                    enemy.PositionAt(i + (speed / Random(1, 1.5f))).X, enemy.PositionAt(i + (speed / Random(1, 1.5f))).Y);

                pewLeft.Rotate(i, angle);
                pewLeft.Color(i, Color4.Red);
                pewLeft.Additive(i, i + speed + fadeTime);
                pewLeft.ScaleVec(OsbEasing.OutExpo, i, i + (speed / 4), 3, 0, 3, 13);
                pewLeft.ScaleVec(OsbEasing.Out, i + (speed / 4), i + speed, 3, 13, 2, 2);
                pewLeft.Move(i, i + speed, aircraft.PositionAt(i).X - 20, aircraft.PositionAt(i).Y,
                                    enemy.PositionAt(i + (speed / Random(1, 1.5f))).X, enemy.PositionAt(i + (speed / Random(1, 1.5f))).Y);
                
                // when it hits the enemy
                if (enemy.PositionAt(i + (speed / Random(1, 1.5f))).Y <= enemy.PositionAt(i + (speed)).Y)
                {
                    var FrameDelay = 100;
                    var LaserHit = GetLayer("Laser").CreateSample("sb/sfx/laser-hit.ogg", i + speed, Random(10, 20));
                    var laserHit = GetLayer("Laser").CreateAnimation("sb/missions/1/explosion/explode.jpg", 12, FrameDelay, OsbLoopType.LoopOnce, OsbOrigin.Centre);
                    
                    laserHit.Additive(i + speed, i + speed + fadeTime + FrameDelay);
                    laserHit.Scale(i + speed, i + speed + fadeTime + FrameDelay, 0.2, 0.1);
                    laserHit.Move(i + speed + fadeTime, enemy.PositionAt(i + (speed / Random(1, 1.5f))));
                }
            }

            // SMOKE
            float X = 370;
            float scale = 4f;
            float fade = 0;
            var smokeAmount = 40;
            var smokeDuration = 50;
            for (int i = startTime; i <= endTime; i += smokeAmount)
            {
                var smokeLeft = GetLayer("Aircraft 3 Back - Smoke").CreateAnimation("sb/missions/1/aircrafts/smoke/smoke.png",
                                                                4, 100, OsbLoopType.LoopForever, OsbOrigin.Centre);
                var smokeRight = GetLayer("Aircraft 3 Back - Smoke").CreateAnimation("sb/missions/1/aircrafts/smoke/smoke.png",
                                                                4, 100, OsbLoopType.LoopForever, OsbOrigin.Centre);
                int x = 42;
                int y = 15;
                int delay = 4;
                int fadeDelay = 10;

                if (i > startTime + 4000)
                {
                    smokeLeft.Fade(i, 0.2);
                    smokeLeft.Additive(i, i + smokeDuration);
                    smokeLeft.Scale(OsbEasing.In, i, i + (smokeDuration * fadeDelay), 0.3, 0.3 / 4);
                    smokeLeft.Fade(i + smokeDuration, i + (smokeDuration * fadeDelay), 0.2f, 0);
                    smokeLeft.Move(OsbEasing.Out, i, i + smokeDuration, aircraft.PositionAt(i).X - x, aircraft.PositionAt(i).Y - y,
                            aircraft.PositionAt(i - (smokeDuration * 2)).X - x, aircraft.PositionAt(i - (smokeDuration * delay)).Y - y);

                    smokeRight.Fade(i, 0.2);
                    smokeRight.Additive(i, i + smokeDuration);
                    smokeRight.Scale(OsbEasing.In, i, i + (smokeDuration * fadeDelay), 0.3, 0.3 / 4);
                    smokeRight.Fade(i + smokeDuration, i + (smokeDuration * fadeDelay), 0.2f, 0);
                    smokeRight.Move(OsbEasing.Out, i, i + smokeDuration, aircraft.PositionAt(i).X + x, aircraft.PositionAt(i).Y - y,
                            aircraft.PositionAt(i - (smokeDuration * 2)).X + x, aircraft.PositionAt(i - (smokeDuration * delay)).Y - y);
                }

                else
                {
                    X -= 3.35f;
                    scale -= 0.034f;
                    fade += 0.002f;

                    smokeLeft.Fade(i, fade);
                    smokeLeft.Additive(i, i + smokeDuration);
                    smokeLeft.Scale(OsbEasing.OutExpo, i, i + (smokeDuration * fadeDelay), scale, scale / 4);
                    smokeLeft.Fade(i + smokeDuration, i + (smokeDuration * fadeDelay), fade, 0);
                    smokeLeft.Move(OsbEasing.In, i, i + smokeDuration, aircraft.PositionAt(i).X - X, aircraft.PositionAt(i).Y - y,
                            aircraft.PositionAt(i - (smokeDuration * 2)).X - X, aircraft.PositionAt(i - (smokeDuration * delay)).Y - y);

                    smokeRight.Fade(i, fade);
                    smokeRight.Additive(i, i + smokeDuration);
                    smokeRight.Scale(OsbEasing.OutExpo, i, i + (smokeDuration * fadeDelay), scale, scale / 4);
                    smokeRight.Fade(i + smokeDuration, i + (smokeDuration * fadeDelay), fade, 0);
                    smokeRight.Move(OsbEasing.In, i, i + smokeDuration, aircraft.PositionAt(i).X + X, aircraft.PositionAt(i).Y - y,
                            aircraft.PositionAt(i - (smokeDuration * 2)).X + X, aircraft.PositionAt(i - (smokeDuration * delay)).Y - y);
                }
            }

            var eSmokeAmount = 40;
            var eSmokeDuration = 100;
            for (int i = startTime; i <= endTime; i += eSmokeAmount)
            {
                var smokeLeft = GetLayer("Aircraft 1 & 2 Front - Smoke").CreateAnimation("sb/missions/1/aircrafts/smoke/smoke.png",
                                                                4, 100, OsbLoopType.LoopForever, OsbOrigin.Centre);
                var smokeRight = GetLayer("Aircraft 1 & 2 Front - Smoke").CreateAnimation("sb/missions/1/aircrafts/smoke/smoke.png",
                                                                4, 100, OsbLoopType.LoopForever, OsbOrigin.Centre);
                int x = 10;
                int y = -5;
                float delay = 2f;
                int fadeDelay = 2;

                if (i > startTime + 1000)
                {
                    var Scale = 0.15f;
                    smokeLeft.Fade(i, 0.09f);
                    smokeLeft.Additive(i, i + eSmokeDuration);
                    smokeLeft.Scale(OsbEasing.In, i, i + (eSmokeDuration * fadeDelay), Scale, Scale / 4);
                    smokeLeft.Fade(i + eSmokeDuration, i + (eSmokeDuration * fadeDelay), 0.09f, 0);
                    smokeLeft.Move(OsbEasing.Out, i, i + eSmokeDuration, enemy.PositionAt(i).X - x, enemy.PositionAt(i).Y - y,
                            enemy.PositionAt(i - (eSmokeDuration * 2)).X - x, enemy.PositionAt(i - (eSmokeDuration * delay)).Y - y);

                    smokeRight.Fade(i, 0.09f);
                    smokeRight.Additive(i, i + eSmokeDuration);
                    smokeRight.Scale(OsbEasing.In, i, i + (eSmokeDuration * fadeDelay), Scale, Scale / 4);
                    smokeRight.Fade(i + eSmokeDuration, i + (eSmokeDuration * fadeDelay), 0.09f, 0);
                    smokeRight.Move(OsbEasing.Out, i, i + eSmokeDuration, enemy.PositionAt(i).X + x, enemy.PositionAt(i).Y - y,
                            enemy.PositionAt(i - (eSmokeDuration * 2)).X + x, enemy.PositionAt(i - (eSmokeDuration * delay)).Y - y);
                }
            }
        }

        public void Clouds(int StartTime, int EndTime)
            {
                
                var startTime = StartTime;
                var endTime = EndTime;
                float cloudVelocity = 20;
                var d = (endTime - startTime) / 4;

                for (float i = startTime; i < startTime + d; i += cloudVelocity)
                {
                    var duration = Random(800, 1200);
                    var RandomX = Random(-107, 747);
                    var RandomFade = Random(0.015, 0.15);
                    var RandomScale = Random(2.5, 3);
                    var startPos  = new Vector2(RandomX, Random(300, 315));
                    var endPos  = new Vector2(RandomX, Random(500, 520));

                    var sprite = GetLayer("Clouds").CreateSprite("sb/missions/1/cloud/cloud" + Random(1, 11) + ".png", OsbOrigin.TopCentre);

                    var loopCount = (endTime - startTime) / duration;
                    sprite.StartLoopGroup(i, loopCount);
                    sprite.Additive(0, duration);
                    sprite.Fade(OsbEasing.InExpo, 0, duration / 2, 0, RandomFade);
                    sprite.Fade(duration - duration / 8, duration, RandomFade, 0);
                    sprite.MoveX(0, duration, startPos.X, endPos.X);
                    sprite.MoveY(OsbEasing.InExpo, 0, duration, startPos.Y, endPos.Y);
                    sprite.ScaleVec(OsbEasing.InExpo, 0, duration, RandomScale / 8, RandomScale / 8, RandomScale, RandomScale);
                    sprite.EndGroup();
                }
            }

        public void AircraftEnemy(int StartTime, int EndTime)
        {
            var startTime = StartTime;
            var endTime = EndTime + 2000;
            var enemyDuration = 1000;

            var enemyloopCount = (startTime) / (enemyDuration * 5);
            var enemy = GetLayer("Aircraft 1 & 2 Front").CreateSprite("sb/missions/1/aircrafts/" + Random(1, 3) + "_back.png", OsbOrigin.Centre);

            enemy.Scale(OsbEasing.Out, startTime, startTime + 4000, 0.05, 0.1);
            enemy.Fade(OsbEasing.Out, startTime, startTime + 2000, 0, 1);
            enemy.Fade(endTime - 500, endTime, 1, 0);

            enemy.StartLoopGroup(startTime, enemyloopCount - 1);
            // x
            enemy.MoveX(OsbEasing.InOutSine, 0, enemyDuration / 2, 310, 330);
            enemy.MoveX(OsbEasing.InOutSine, enemyDuration / 2, enemyDuration, 330, 300);
            enemy.MoveX(OsbEasing.InOutSine, enemyDuration, enemyDuration * 2, 300, 310);
            enemy.MoveX(OsbEasing.InOutSine, enemyDuration * 2, enemyDuration * 3, 310, 330);
            enemy.MoveX(OsbEasing.InOutSine, enemyDuration * 3, enemyDuration * 4, 330, 320);
            enemy.MoveX(OsbEasing.InOutSine, enemyDuration * 4, enemyDuration * 5, 320, 310);
            // y
            enemy.MoveY(OsbEasing.InOutSine, 0, enemyDuration / 2, 270, 240);
            enemy.MoveY(OsbEasing.InOutSine, enemyDuration / 2, enemyDuration, 240, 230);
            enemy.MoveY(OsbEasing.InOutSine, enemyDuration, enemyDuration * 2, 230, 250);
            enemy.MoveY(OsbEasing.InOutSine, enemyDuration * 2, enemyDuration * 3, 250, 210);
            enemy.MoveY(OsbEasing.InOutSine, enemyDuration * 3, enemyDuration * 4, 210, 230);
            enemy.MoveY(OsbEasing.InOutSine, enemyDuration * 4, enemyDuration * 5, 230, 270);
            //
            enemy.EndGroup();

            // SMOKE
            var eSmokeAmount = 40;
            var eSmokeDuration = 100;
            for (int i = startTime; i <= endTime; i += eSmokeAmount)
            {
                var smokeLeft = GetLayer("Aircraft 1 & 2 Front - Smoke").CreateAnimation("sb/missions/1/aircrafts/smoke/smoke.png",
                                                                4, 100, OsbLoopType.LoopForever, OsbOrigin.Centre);
                var smokeRight = GetLayer("Aircraft 1 & 2 Front - Smoke").CreateAnimation("sb/missions/1/aircrafts/smoke/smoke.png",
                                                                4, 100, OsbLoopType.LoopForever, OsbOrigin.Centre);
                int x = 10;
                int y = -5;
                float delay = 2f;
                int fadeDelay = 2;

                if (i > startTime + 1000)
                {
                    var Scale = 0.15f;
                    smokeLeft.Fade(i, 0.09f);
                    smokeLeft.Additive(i, i + eSmokeDuration);
                    smokeLeft.Scale(OsbEasing.In, i, i + (eSmokeDuration * fadeDelay), Scale, Scale / 4);
                    smokeLeft.Fade(i + eSmokeDuration, i + (eSmokeDuration * fadeDelay), 0.09f, 0);
                    smokeLeft.Move(OsbEasing.Out, i, i + eSmokeDuration, enemy.PositionAt(i).X - x, enemy.PositionAt(i).Y - y,
                            enemy.PositionAt(i - (eSmokeDuration * 2)).X - x, enemy.PositionAt(i - (eSmokeDuration * delay)).Y - y);

                    smokeRight.Fade(i, 0.09f);
                    smokeRight.Additive(i, i + eSmokeDuration);
                    smokeRight.Scale(OsbEasing.In, i, i + (eSmokeDuration * fadeDelay), Scale, Scale / 4);
                    smokeRight.Fade(i + eSmokeDuration, i + (eSmokeDuration * fadeDelay), 0.09f, 0);
                    smokeRight.Move(OsbEasing.Out, i, i + eSmokeDuration, enemy.PositionAt(i).X + x, enemy.PositionAt(i).Y - y,
                            enemy.PositionAt(i - (eSmokeDuration * 2)).X + x, enemy.PositionAt(i - (eSmokeDuration * delay)).Y - y);
                }
            }
        }
    }
}

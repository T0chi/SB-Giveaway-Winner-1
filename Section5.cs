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

        private DialogManager dialog;

        private DialogManager dialog2;

        public override void Generate()
        {
            Flowers(220447, 247113);
            Background(211525, 247113, 221081);
            Mission(224256, 247113);
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

            Balloons(startTime, endTime, balloon, avatar, interval, balloonColorMin, balloonColorMax);
        }

        public void Balloons(int startTime, int endTime, OsbSprite targetBalloon, OsbSprite avatarData, int interval, Color4 balloonColorMin, Color4 balloonColorMax)
        {
            var timeStepBalloon = interval;
            var Beat = Beatmap.GetTimingPointAt(startTime).BeatDuration;

            // BALLOONS
            for (int time = startTime; time < endTime; time += (timeStepBalloon * 3))
            {
                var shiftX = Random(-100, 100);
                var shiftY = Random(-500, -380);
                var balloonScale = Random(0.02, 0.55);
                var balloonDuration = Random(6000, 10000);
                var balloonStartPos = new Vector2(Random(0, 640), Random(350, 400));
                var balloonEndPos = new Vector2(balloonStartPos.X + shiftX, balloonStartPos.Y + shiftY);

                // if balloons are small then they're NOT blurry ////////////////////////////////////////////////////////////////////////////////////
                if (balloonScale <= 0.2)
                {
                var balloon = GetLayer("BalloonsBack").CreateSprite("sb/missions/4/balloon.png", OsbOrigin.BottomCentre);

                if (time < 244653)
                {
                    balloon.Fade(time + (balloonDuration / 10), time + (balloonDuration / 10) + (balloonDuration / 6), 0, 1);
                }
                else if (time > 244653)
                {
                    balloon.Fade(244653, endTime, 0, 0);
                }
                if (time + balloonDuration >= endTime)
                {
                    balloon.Fade(endTime - (balloonDuration / 6), endTime, 1, 0);
                }
                else
                {
                    balloon.Fade(time + balloonDuration - (balloonDuration / 6), time + balloonDuration, 1, 0);
                }
                // loop x position from left to right
                balloon.MoveX(OsbEasing.InOutSine, time, time + (balloonDuration / 2), balloonStartPos.X, balloonEndPos.X);
                balloon.MoveX(OsbEasing.InOutSine, time + (balloonDuration / 2), time + (balloonDuration / 1), balloonEndPos.X, balloonStartPos.X);
                // ending the x position loop
                balloon.Scale(time, balloonScale);
                // starts higher up if the balloon is smaller
                balloon.MoveY(time, time + balloonDuration, balloonStartPos.Y - 25, balloonEndPos.Y);

                var RandomColor = true;
                var balloonColor = RandomColor ? new Color4((float)Random(balloonColorMin.R, balloonColorMax.R),
                                                        (float)Random(balloonColorMin.G, balloonColorMax.G),
                                                        (float)Random(balloonColorMin.B, balloonColorMax.B),
                                                        255
                                                        ) : balloonColorMin;
                balloon.Color(time, balloonColor);

                var currentAngle = balloon.RotationAt(time);
                var newAngle = Math.Atan2((balloon.PositionAt(time + (balloonDuration / 2)).Y - balloon.PositionAt(time).Y), (balloon.PositionAt(time + (balloonDuration / 2)).X - balloon.PositionAt(time).X)) + (Math.PI / 2);
                balloon.Rotate(OsbEasing.InOutSine, time, time + (balloonDuration / 2), newAngle, currentAngle);
                balloon.Rotate(OsbEasing.InOutSine, time + (balloonDuration / 2), time + balloonDuration, -currentAngle, -newAngle);
                balloon.Rotate(OsbEasing.InOutSine, time + balloonDuration, time + (balloonDuration * 2), -newAngle, -currentAngle);
                balloon.Rotate(OsbEasing.InOutSine, time + (balloonDuration * 2), time + (balloonDuration * 4), currentAngle, newAngle);


                // balloon string
                var stringCount = Random(10, 20);
                var prevPosX = balloonStartPos.X;
                var prevPosY = balloonStartPos.Y;
                var stringDelay = Random(60, 100);
                for (var i = 0; i < stringCount; i++)
                {
                    var balloonString = GetLayer("BalloonsBack").CreateSprite("sb/particle2.png", OsbOrigin.Centre);

                    if (time < 244653)
                    {
                        balloonString.Fade(time + (balloonDuration / 10), time + (balloonDuration / 10) + (balloonDuration / 6), 0, 1);
                    }
                    else if (time > 244653)
                    {
                        balloonString.Fade(244653, endTime, 0, 0);
                    }
                    if (time + balloonDuration >= endTime)
                    {
                        balloonString.Fade(endTime - (balloonDuration / 6), endTime, 1, 0);
                    }

                    else
                    {
                        balloonString.Fade(time + balloonDuration - (balloonDuration / 6), time + balloonDuration, 1, 0);
                    }

                    var timeStepBalloonString = Beat;
                    for (int s = time; s < time + balloonDuration; s += (int)timeStepBalloonString)
                    {
                        var Scale = balloonScale * 0.1;
                        var heightOffset = 52 * balloonScale;
                        var stringDistance = stringCount * (balloonScale * 1.2);
                        var position = new Vector2(balloon.PositionAt(s + timeStepBalloonString).X, balloon.PositionAt(s + timeStepBalloonString).Y + i * stringDistance);

                        balloonString.ScaleVec(s, Scale / 4, Scale * 3);
                        balloonString.ColorHsb(s, (i * 30 / stringCount) + Random(-5.0, 10.0), 0.18 + Random(0.4), 0.90);
                        balloonString.MoveX(s + (stringDelay * i), s + timeStepBalloonString + (stringDelay * i), prevPosX, position.X);
                        balloonString.MoveY(s, s + timeStepBalloonString, prevPosY - heightOffset, position.Y - heightOffset);

                        var currentAngle2 = balloonString.RotationAt(time);
                        var newAngle2 = Math.Atan2((balloonString.PositionAt(time + (balloonDuration / 2)).Y - balloonString.PositionAt(time).Y), (balloonString.PositionAt(time + (balloonDuration / 2)).X - balloonString.PositionAt(time).X)) + (Math.PI / 2);
                        balloonString.Rotate(OsbEasing.InOutSine, time + (stringDelay * i), time + (balloonDuration / 2) + (stringDelay * i), currentAngle2, newAngle2);
                        balloonString.Rotate(OsbEasing.InOutSine, time + (balloonDuration / 2) + (stringDelay * i), time + balloonDuration + (stringDelay * i), newAngle2, currentAngle2);

                        prevPosX = position.X;
                        prevPosY = position.Y;
                        // i++;
                    }
                }
                }

                // if balloons are large then they ARE blurry ////////////////////////////////////////////////////////////////////////////////////
                else
                {
                    var balloon = GetLayer("BalloonsFront").CreateSprite("sb/missions/4/balloonBlurry.png", OsbOrigin.BottomCentre);

                if (time < 244653)
                {
                    balloon.Fade(time + (balloonDuration / 10), time + (balloonDuration / 10) + (balloonDuration / 6), 0, 0.9);
                }
                else if (time > 244653)
                {
                    balloon.Fade(244653, endTime, 0, 0);
                }
                if (time + balloonDuration >= endTime)
                {
                    balloon.Fade(endTime - (balloonDuration / 6), endTime, 0.9, 0);
                }
                else
                {
                    balloon.Fade(time + balloonDuration - (balloonDuration / 6), time + balloonDuration, 0.9, 0);
                }
                // loop x position from left to right
                balloon.MoveX(OsbEasing.InOutSine, time, time + (balloonDuration / 2), balloonStartPos.X, balloonEndPos.X);
                balloon.MoveX(OsbEasing.InOutSine, time + (balloonDuration / 2), time + (balloonDuration / 1), balloonEndPos.X, balloonStartPos.X);
                // ending the x position loop
                balloon.Scale(time, balloonScale);
                // starts higher up if the balloon is smaller
                balloon.MoveY(time, time + balloonDuration, balloonStartPos.Y + 80, balloonEndPos.Y);

                var RandomColor = true;
                var balloonColor = RandomColor ? new Color4((float)Random(balloonColorMin.R, balloonColorMax.R),
                                                        (float)Random(balloonColorMin.G, balloonColorMax.G),
                                                        (float)Random(balloonColorMin.B, balloonColorMax.B),
                                                        255
                                                        ) : balloonColorMin;
                balloon.Color(time, balloonColor);

                var currentAngle = balloon.RotationAt(time);
                var newAngle = Math.Atan2((balloon.PositionAt(time + (balloonDuration / 2)).Y - balloon.PositionAt(time).Y), (balloon.PositionAt(time + (balloonDuration / 2)).X - balloon.PositionAt(time).X)) + (Math.PI / 2);
                balloon.Rotate(OsbEasing.InOutSine, time, time + (balloonDuration / 2), newAngle, currentAngle);
                balloon.Rotate(OsbEasing.InOutSine, time + (balloonDuration / 2), time + balloonDuration, -currentAngle, -newAngle);
                balloon.Rotate(OsbEasing.InOutSine, time + balloonDuration, time + (balloonDuration * 2), -newAngle, -currentAngle);
                balloon.Rotate(OsbEasing.InOutSine, time + (balloonDuration * 2), time + (balloonDuration * 4), currentAngle, newAngle);


                // balloon string
                var stringCount = Random(10, 20);
                var prevPosX = balloonStartPos.X;
                var prevPosY = balloonStartPos.Y;
                var stringDelay = Random(60, 100);
                for (var i = 0; i < stringCount; i++)
                {
                    var balloonString = GetLayer("BalloonsFront").CreateSprite("sb/particleBlurry2.png", OsbOrigin.Centre);

                    if (time < 244653)
                    {
                        balloonString.Fade(time + (balloonDuration / 10), time + (balloonDuration / 10) + (balloonDuration / 6), 0, 0.9);
                    }
                    else if (time > 244653)
                    {
                        balloonString.Fade(244653, endTime, 0, 0);
                    }
                    if (time + balloonDuration >= endTime)
                    {
                        balloonString.Fade(endTime - (balloonDuration / 6), endTime, 0.9, 0);
                    }

                    else
                    {
                        balloonString.Fade(time + balloonDuration - (balloonDuration / 6), time + balloonDuration, 0.9, 0);
                    }

                    var timeStepBalloonString = Beat;
                    for (int s = time; s < time + balloonDuration; s += (int)timeStepBalloonString)
                    {
                        var Scale = balloonScale * 0.1;
                        var heightOffset = 52 * balloonScale;
                        var stringDistance = stringCount * (balloonScale * 1.2);
                        var position = new Vector2(balloon.PositionAt(s + timeStepBalloonString).X, balloon.PositionAt(s + timeStepBalloonString).Y + i * stringDistance);

                        balloonString.ScaleVec(s, Scale / 4, Scale * 3);
                        balloonString.ColorHsb(s, (i * 30 / stringCount) + Random(-5.0, 10.0), 0.18 + Random(0.4), 0.90);
                        balloonString.MoveX(s + (stringDelay * i), s + timeStepBalloonString + (stringDelay * i), prevPosX, position.X);
                        balloonString.MoveY(s, s + timeStepBalloonString, prevPosY - heightOffset, position.Y - heightOffset);

                        var currentAngle2 = balloonString.RotationAt(time);
                        var newAngle2 = Math.Atan2((balloonString.PositionAt(time + (balloonDuration / 2)).Y - balloonString.PositionAt(time).Y), (balloonString.PositionAt(time + (balloonDuration / 2)).X - balloonString.PositionAt(time).X)) + (Math.PI / 2);
                        balloonString.Rotate(OsbEasing.InOutSine, time + (stringDelay * i), time + (balloonDuration / 2) + (stringDelay * i), currentAngle2, newAngle2);
                        balloonString.Rotate(OsbEasing.InOutSine, time + (balloonDuration / 2) + (stringDelay * i), time + balloonDuration + (stringDelay * i), newAngle2, currentAngle2);

                        prevPosX = position.X;
                        prevPosY = position.Y;
                        // i++;
                    }
                }
                }
            }

            // TARGET BALLOONS, ARROWS & AVATAR
            for (int time = startTime; time < endTime; time += timeStepBalloon)
            {
                var shiftX = Random(-100, 100);
                var shiftY = Random(-400, -100);
                var popDuration = Random(1500, 3000);
                var balloonScale = Random(0.1, 0.55);
                var balloonDuration = Random(4000, 8000);
                var balloonStartPos = new Vector2(Random(0, 640), Random(430, 480));
                var balloonEndPos = new Vector2(balloonStartPos.X + shiftX, balloonStartPos.Y + shiftY);

                // if balloons are small then they're NOT blurry ////////////////////////////////////////////////////////////////////////////////////
                if (balloonScale <= 0.27)
                {
                    var balloon = GetLayer("targetBalloonsBack").CreateSprite("sb/missions/4/balloon.png", OsbOrigin.BottomCentre);

                    balloon.Fade(time + (balloonDuration / 10), time + (balloonDuration / 10) + (balloonDuration / 6), 0, 1);
                    balloon.Fade(time + balloonDuration, time + balloonDuration + 50, 1, 0);

                    // loop x position from left to right
                    balloon.MoveX(OsbEasing.InOutSine, time, time + (balloonDuration / 2), balloonStartPos.X, balloonEndPos.X);
                    balloon.MoveX(OsbEasing.InOutSine, time + (balloonDuration / 2), time + (balloonDuration / 1), balloonEndPos.X, balloonStartPos.X);
                    // ending the x position loop
                    balloon.Scale(time, balloonScale);
                    // starts higher up if the balloon is smaller
                    balloon.MoveY(time, time + balloonDuration, balloonStartPos.Y - 25, balloonEndPos.Y);

                    var RandomColor = true;
                    var balloonColor = RandomColor ? new Color4((float)Random(balloonColorMin.R, balloonColorMax.R),
                                                            (float)Random(balloonColorMin.G, balloonColorMax.G),
                                                            (float)Random(balloonColorMin.B, balloonColorMax.B),
                                                            255
                                                            ) : balloonColorMin;
                    balloon.Color(time, balloonColor);

                    var currentAngle = balloon.RotationAt(time);
                    var newAngle = Math.Atan2((balloon.PositionAt(time + (balloonDuration / 2)).Y - balloon.PositionAt(time).Y), (balloon.PositionAt(time + (balloonDuration / 2)).X - balloon.PositionAt(time).X)) + (Math.PI / 2);
                    balloon.Rotate(OsbEasing.InOutSine, time, time + (balloonDuration / 2), newAngle, currentAngle);
                    balloon.Rotate(OsbEasing.InOutSine, time + (balloonDuration / 2), time + balloonDuration, -currentAngle, -newAngle);
                    balloon.Rotate(OsbEasing.InOutSine, time + balloonDuration, time + (balloonDuration * 2), -newAngle, -currentAngle);
                    balloon.Rotate(OsbEasing.InOutSine, time + (balloonDuration * 2), time + (balloonDuration * 4), currentAngle, newAngle);

                    // balloon pop stuff
                    var balloonPopCount = Random(4, 7);
                    for (var i = 0; i < balloonPopCount; i++)
                    {
                        var balloonPopScale = balloon.ScaleAt(time + balloonDuration) * Random(0.005, 0.2);
                        var randomScale = Random(-balloonPopScale.X, balloonPopScale.X);

                        var balloonPop = GetLayer("targetBalloonsBack").CreateSprite("sb/missions/4/balloon.png", OsbOrigin.Centre);
                        var balloonPopSound = GetLayer("targetBalloonsBack").CreateSample("sb/sfx/balloon-pop.ogg", time + balloonDuration, 7);

                        if (time >= endTime - (popDuration * 4))
                        {
                            balloonPop.Fade(time + balloonDuration, time + balloonDuration + popDuration, 1, 0);
                        }
                        else
                        {
                            balloonPop.Fade(endTime, endTime + popDuration, 1, 0);
                        }
                        
                        balloonPop.Color(time + balloonDuration, balloon.ColorAt(time + balloonDuration));
                        balloonPop.MoveY(OsbEasing.InSine, time + balloonDuration, time + balloonDuration + popDuration, balloonEndPos.Y + Random(-70, -20), balloonStartPos.Y);
                        balloonPop.MoveX(OsbEasing.OutSine, time + balloonDuration, time + balloonDuration + popDuration, balloonStartPos.X + Random(-10, 10), balloonStartPos.X + Random(-100, 100));
                        balloonPop.ScaleVec(OsbEasing.InOutSine, time + balloonDuration, time + balloonDuration + (popDuration / 2), balloonPopScale.X, balloonPopScale.Y, -balloonPopScale.X, randomScale);
                        balloonPop.ScaleVec(OsbEasing.InOutSine, time + balloonDuration + (popDuration / 2), time + balloonDuration + (popDuration / 1), -randomScale, balloonPopScale.Y, Random(-balloonPopScale.X, balloonPopScale.X), balloonPopScale.Y);
                        balloonPop.Rotate(OsbEasing.InOutSine, time + balloonDuration, time + balloonDuration + popDuration, balloon.RotationAt(time + balloonDuration), balloon.RotationAt(time + balloonDuration) + Random(-5, 5));
                    }
                    // balloon pop  stuff ends

                    // arrows
                    var arrowSpeed = Random(400, 550);
                    var arrow = GetLayer("Arrows").CreateSprite("sb/missions/4/arrow.png", OsbOrigin.Centre);
                    var arrowSound = GetLayer("Arrows").CreateSample("sb/sfx/arrow-1.ogg", time + balloonDuration - arrowSpeed, 50);

                    var arrowScale = 0.2;
                    var randomX = Random(50, 320);
                    var arrowStartPos = new Vector2(avatarData.PositionAt(time + balloonDuration - arrowSpeed).X, 450);
                    var arrowEndPos = balloon.PositionAt(time + balloonDuration);
                    var arrowAngle = Math.Atan2(((balloon.PositionAt(time + balloonDuration - arrowSpeed).Y) - (avatarData.PositionAt(time + balloonDuration).Y) + 40), ((balloon.PositionAt(time + balloonDuration - arrowSpeed).X + 20) - avatarData.PositionAt(time + balloonDuration).X)) + (Math.PI / 2);

                    arrow.Rotate(time + balloonDuration - arrowSpeed, arrowAngle);
                    arrow.Fade(time + balloonDuration - arrowSpeed, time + balloonDuration - arrowSpeed + (arrowSpeed / 4), 0, 1);
                    arrow.Fade(time + balloonDuration, time + balloonDuration + (arrowSpeed / 4), 1, 0);
                    arrow.ScaleVec(time + balloonDuration - arrowSpeed, time + balloonDuration, arrowScale * 0.7, arrowScale * 0.7, arrowScale * 0.6, arrowScale * 0.5);
                    arrow.MoveX(time + balloonDuration - arrowSpeed, time + balloonDuration, arrowStartPos.X, arrowEndPos.X);
                    arrow.MoveY(time + balloonDuration - arrowSpeed, time + balloonDuration, arrowStartPos.Y - 100, arrowEndPos.Y);

                    Log(avatarData.PositionAt(time + balloonDuration).X.ToString());


                    // balloon string
                    var stringCount = Random(10, 20);
                    var prevPosX = balloonStartPos.X;
                    var prevPosY = balloonStartPos.Y;
                    var stringDelay = Random(60, 100);
                    var heightOffset = 52 * balloonScale;
                    var stringDistance = stringCount * (balloonScale * 1.2);
                    for (var i = 0; i < stringCount; i++)
                    {
                        var balloonStringTarget = GetLayer("targetBalloonsBack").CreateSprite("sb/particle2.png", OsbOrigin.Centre);

                        balloonStringTarget.Fade(time + (balloonDuration / 10), time + (balloonDuration / 10) + (balloonDuration / 6), 0, 1);

                        var timeStepBalloonString = Beat;
                        for (int s = time; s < time + balloonDuration; s += (int)timeStepBalloonString)
                        {
                            var Scale = balloonScale * 0.1;
                            var position = new Vector2(balloon.PositionAt(s + timeStepBalloonString).X, balloon.PositionAt(s + timeStepBalloonString).Y + i * stringDistance);

                            balloonStringTarget.ScaleVec(s, Scale / 4, Scale * 3);
                            balloonStringTarget.ColorHsb(s, (i * 30 / stringCount) + Random(-5.0, 10.0), 0.18 + Random(0.4), 0.90);
                            balloonStringTarget.MoveX(s + (stringDelay * i), s + timeStepBalloonString + (stringDelay * i), prevPosX, position.X);
                            balloonStringTarget.MoveY(s, s + timeStepBalloonString, prevPosY - heightOffset, position.Y - heightOffset);

                            var currentAngle2 = balloonStringTarget.RotationAt(time);
                            var newAngle2 = Math.Atan2((balloonStringTarget.PositionAt(time + (balloonDuration / 2)).Y - balloonStringTarget.PositionAt(time).Y), (balloonStringTarget.PositionAt(time + (balloonDuration / 2)).X - balloonStringTarget.PositionAt(time).X)) + (Math.PI / 2);
                            balloonStringTarget.Rotate(OsbEasing.InOutSine, time + (stringDelay * i), time + (balloonDuration / 2) + (stringDelay * i), currentAngle2, newAngle2);
                            balloonStringTarget.Rotate(OsbEasing.InOutSine, time + (balloonDuration / 2) + (stringDelay * i), time + balloonDuration + (stringDelay * i), newAngle2, currentAngle2);

                            prevPosX = position.X;
                            prevPosY = position.Y;
                            // i++;

                        }

                        // balloonStringTarget pop stuff
                        var balloonStringScale = balloonStringTarget.ScaleAt(time + balloonDuration);
                        var balloonStringPosX = balloonStringTarget.PositionAt(time + balloonDuration + (stringDelay * i)).X;
                        var balloonStringPosStartY = balloon.PositionAt(time + balloonDuration).Y + i * stringDistance;
                        var balloonStringPosEndY = balloon.PositionAt(time).Y + i * stringDistance;

                        if (time >= endTime - (popDuration * 4))
                        {
                            balloonStringTarget.Fade(time + balloonDuration, time + balloonDuration + popDuration, 1, 0);
                        }
                        else
                        {
                            balloonStringTarget.Fade(endTime, endTime + popDuration, 1, 0);
                        }

                        balloonStringTarget.MoveY(OsbEasing.InSine, time + balloonDuration, time + balloonDuration + popDuration, balloonStringPosStartY - heightOffset, balloonStringPosEndY - heightOffset);
                        balloonStringTarget.MoveX(OsbEasing.InSine, time + balloonDuration + (stringDelay * i), time + balloonDuration + popDuration, balloonStringPosX, balloonStringPosX + Random(-40, 40));
                        balloonStringTarget.ScaleVec(OsbEasing.InOutSine, time + balloonDuration, time + balloonDuration + (popDuration / 2), balloonStringScale.X, balloonStringScale.Y, -balloonStringScale.X, balloonStringScale.Y);
                        balloonStringTarget.ScaleVec(OsbEasing.InOutSine, time + balloonDuration + (popDuration / 2), time + balloonDuration + (popDuration / 1), -balloonStringScale.X, balloonStringScale.Y, balloonStringScale.X, balloonStringScale.Y);
                        balloonStringTarget.Rotate(OsbEasing.InOutSine, time + balloonDuration + (stringDelay * i), time + balloonDuration + (stringDelay * i) + popDuration, balloonStringTarget.RotationAt(time + balloonDuration + (stringDelay * i)), balloonStringTarget.RotationAt(time + balloonDuration + (stringDelay * i)) + Random(-5, 5));
                        // balloonStringTarget pop  stuff ends
                    }
                    balloon = targetBalloon;
                }
                
                // if balloons are large then they ARE blurry ////////////////////////////////////////////////////////////////////////////////////
                else
                {
                    var balloon = GetLayer("targetBalloonsFront").CreateSprite("sb/missions/4/balloonBlurry.png", OsbOrigin.BottomCentre);

                    balloon.Fade(time + (balloonDuration / 10), time + (balloonDuration / 10) + (balloonDuration / 6), 0, 0.9);
                    balloon.Fade(time + balloonDuration, time + balloonDuration + 50, 0.9, 0);

                    // loop x position from left to right
                    balloon.MoveX(OsbEasing.InOutSine, time, time + (balloonDuration / 2), balloonStartPos.X, balloonEndPos.X);
                    balloon.MoveX(OsbEasing.InOutSine, time + (balloonDuration / 2), time + (balloonDuration / 1), balloonEndPos.X, balloonStartPos.X);
                    // ending the x position loop
                    balloon.Scale(time, balloonScale);
                    // starts higher up if the balloon is smaller
                    balloon.MoveY(time, time + balloonDuration, balloonStartPos.Y + 80, balloonEndPos.Y);

                    var RandomColor = true;
                    var balloonColor = RandomColor ? new Color4((float)Random(balloonColorMin.R, balloonColorMax.R),
                                                            (float)Random(balloonColorMin.G, balloonColorMax.G),
                                                            (float)Random(balloonColorMin.B, balloonColorMax.B),
                                                            255
                                                            ) : balloonColorMin;
                    balloon.Color(time, balloonColor);

                    var currentAngle = balloon.RotationAt(time);
                    var newAngle = Math.Atan2((balloon.PositionAt(time + (balloonDuration / 2)).Y - balloon.PositionAt(time).Y), (balloon.PositionAt(time + (balloonDuration / 2)).X - balloon.PositionAt(time).X)) + (Math.PI / 2);
                    balloon.Rotate(OsbEasing.InOutSine, time, time + (balloonDuration / 2), newAngle, currentAngle);
                    balloon.Rotate(OsbEasing.InOutSine, time + (balloonDuration / 2), time + balloonDuration, -currentAngle, -newAngle);
                    balloon.Rotate(OsbEasing.InOutSine, time + balloonDuration, time + (balloonDuration * 2), -newAngle, -currentAngle);
                    balloon.Rotate(OsbEasing.InOutSine, time + (balloonDuration * 2), time + (balloonDuration * 4), currentAngle, newAngle);

                    // balloon pop stuff
                    var balloonPopCount = Random(4, 7);
                    for (var i = 0; i < balloonPopCount; i++)
                    {
                        var balloonPopScale = balloon.ScaleAt(time + balloonDuration) * Random(0.005, 0.2);
                        var randomScale = Random(-balloonPopScale.X, balloonPopScale.X);

                        var balloonPop = GetLayer("targetBalloonsFront").CreateSprite("sb/missions/4/balloon.png", OsbOrigin.Centre);
                        var balloonPopSound = GetLayer("targetBalloonsFront").CreateSample("sb/sfx/balloon-pop.ogg", time + balloonDuration, 7);

                        if (time >= endTime - (popDuration * 4))
                        {
                            balloonPop.Fade(time + balloonDuration, time + balloonDuration + popDuration, 0.9, 0);
                        }
                        else
                        {
                            balloonPop.Fade(endTime, endTime + popDuration, 0.9, 0);
                        }

                        balloonPop.Color(time + balloonDuration, balloon.ColorAt(time + balloonDuration));
                        balloonPop.MoveY(OsbEasing.InSine, time + balloonDuration, time + balloonDuration + popDuration, balloonEndPos.Y + Random(-70, -20), balloonStartPos.Y);
                        balloonPop.MoveX(OsbEasing.OutSine, time + balloonDuration, time + balloonDuration + popDuration, balloonStartPos.X + Random(-10, 10), balloonStartPos.X + Random(-100, 100));
                        balloonPop.ScaleVec(OsbEasing.InOutSine, time + balloonDuration, time + balloonDuration + (popDuration / 2), balloonPopScale.X, balloonPopScale.Y, -balloonPopScale.X, randomScale);
                        balloonPop.ScaleVec(OsbEasing.InOutSine, time + balloonDuration + (popDuration / 2), time + balloonDuration + (popDuration / 1), -randomScale, balloonPopScale.Y, Random(-balloonPopScale.X, balloonPopScale.X), balloonPopScale.Y);
                        balloonPop.Rotate(OsbEasing.InOutSine, time + balloonDuration, time + balloonDuration + popDuration, balloon.RotationAt(time + balloonDuration), balloon.RotationAt(time + balloonDuration) + Random(-5, 5));
                    }
                    // balloon pop  stuff ends

                    // arrows
                    var arrowSpeed = Random(400, 550);
                    var arrow = GetLayer("Arrows").CreateSprite("sb/missions/4/arrow2.png", OsbOrigin.Centre);
                    var arrowSound = GetLayer("Arrows").CreateSample("sb/sfx/arrow-1.ogg", time + balloonDuration - arrowSpeed, 50);

                    var arrowScale = 0.2;
                    var randomX = Random(50, 320);
                    var arrowStartPos = new Vector2(avatarData.PositionAt(time + balloonDuration - arrowSpeed).X, 450);
                    var arrowEndPos = balloon.PositionAt(time + balloonDuration);
                    var arrowAngle = Math.Atan2(((balloon.PositionAt(time + balloonDuration - arrowSpeed).Y - 50 ) - (avatarData.PositionAt(time + balloonDuration).Y) + 40), ((balloon.PositionAt(time + balloonDuration - arrowSpeed).X + 50) - avatarData.PositionAt(time + balloonDuration).X)) + (Math.PI / 2);

                    arrow.Rotate(time + balloonDuration - arrowSpeed, arrowAngle);
                    arrow.Fade(time + balloonDuration - arrowSpeed, time + balloonDuration - arrowSpeed + (arrowSpeed / 4), 0, 1);
                    arrow.Fade(time + balloonDuration, time + balloonDuration + (arrowSpeed / 4), 1, 0);
                    arrow.ScaleVec(time + balloonDuration - arrowSpeed, time + balloonDuration, arrowScale * 0.05, arrowScale * 1, arrowScale * 1.3, arrowScale * 0.5);
                    arrow.MoveX(time + balloonDuration - arrowSpeed, time + balloonDuration, arrowStartPos.X, arrowEndPos.X);
                    arrow.MoveY(time + balloonDuration - arrowSpeed, time + balloonDuration, arrowStartPos.Y - 100, arrowEndPos.Y - 50);

                    Log(avatarData.PositionAt(time + balloonDuration).X.ToString());


                    // balloon string
                    var stringCount = Random(10, 20);
                    var prevPosX = balloonStartPos.X;
                    var prevPosY = balloonStartPos.Y;
                    var stringDelay = Random(60, 100);
                    var heightOffset = 52 * balloonScale;
                    var stringDistance = stringCount * (balloonScale * 1.2);
                    for (var i = 0; i < stringCount; i++)
                    {
                        var balloonStringTarget = GetLayer("targetBalloonsFront").CreateSprite("sb/particle2.png", OsbOrigin.Centre);

                        balloonStringTarget.Fade(time + (balloonDuration / 10), time + (balloonDuration / 10) + (balloonDuration / 6), 0, 0.9);

                        var timeStepBalloonString = Beat;
                        for (int s = time; s < time + balloonDuration; s += (int)timeStepBalloonString)
                        {
                            var Scale = balloonScale * 0.1;
                            var position = new Vector2(balloon.PositionAt(s + timeStepBalloonString).X, balloon.PositionAt(s + timeStepBalloonString).Y + i * stringDistance);

                            balloonStringTarget.ScaleVec(s, Scale / 4, Scale * 3);
                            balloonStringTarget.ColorHsb(s, (i * 30 / stringCount) + Random(-5.0, 10.0), 0.18 + Random(0.4), 0.90);
                            balloonStringTarget.MoveX(s + (stringDelay * i), s + timeStepBalloonString + (stringDelay * i), prevPosX, position.X);
                            balloonStringTarget.MoveY(s, s + timeStepBalloonString, prevPosY - heightOffset, position.Y - heightOffset);

                            var currentAngle2 = balloonStringTarget.RotationAt(time);
                            var newAngle2 = Math.Atan2((balloonStringTarget.PositionAt(time + (balloonDuration / 2)).Y - balloonStringTarget.PositionAt(time).Y), (balloonStringTarget.PositionAt(time + (balloonDuration / 2)).X - balloonStringTarget.PositionAt(time).X)) + (Math.PI / 2);
                            balloonStringTarget.Rotate(OsbEasing.InOutSine, time + (stringDelay * i), time + (balloonDuration / 2) + (stringDelay * i), currentAngle2, newAngle2);
                            balloonStringTarget.Rotate(OsbEasing.InOutSine, time + (balloonDuration / 2) + (stringDelay * i), time + balloonDuration + (stringDelay * i), newAngle2, currentAngle2);

                            prevPosX = position.X;
                            prevPosY = position.Y;
                            // i++;

                        }

                        // balloonStringTarget pop stuff
                        var balloonStringScale = balloonStringTarget.ScaleAt(time + balloonDuration);
                        var balloonStringPosX = balloonStringTarget.PositionAt(time + balloonDuration + (stringDelay * i)).X;
                        var balloonStringPosStartY = balloon.PositionAt(time + balloonDuration).Y + i * stringDistance;
                        var balloonStringPosEndY = balloon.PositionAt(time).Y + i * stringDistance;

                        if (time >= endTime - (popDuration * 4))
                        {
                            balloonStringTarget.Fade(time + balloonDuration, time + balloonDuration + popDuration, 0.9, 0);
                        }
                        else
                        {
                            balloonStringTarget.Fade(endTime, endTime + popDuration, 0.9, 0);
                        }

                        balloonStringTarget.MoveY(OsbEasing.InSine, time + balloonDuration, time + balloonDuration + popDuration, balloonStringPosStartY - heightOffset, balloonStringPosEndY - heightOffset);
                        balloonStringTarget.MoveX(OsbEasing.InSine, time + balloonDuration + (stringDelay * i), time + balloonDuration + popDuration, balloonStringPosX, balloonStringPosX + Random(-40, 40));
                        balloonStringTarget.ScaleVec(OsbEasing.InOutSine, time + balloonDuration, time + balloonDuration + (popDuration / 2), balloonStringScale.X, balloonStringScale.Y, -balloonStringScale.X, balloonStringScale.Y);
                        balloonStringTarget.ScaleVec(OsbEasing.InOutSine, time + balloonDuration + (popDuration / 2), time + balloonDuration + (popDuration / 1), -balloonStringScale.X, balloonStringScale.Y, balloonStringScale.X, balloonStringScale.Y);
                        balloonStringTarget.Rotate(OsbEasing.InOutSine, time + balloonDuration + (stringDelay * i), time + balloonDuration + (stringDelay * i) + popDuration, balloonStringTarget.RotationAt(time + balloonDuration + (stringDelay * i)), balloonStringTarget.RotationAt(time + balloonDuration + (stringDelay * i)) + Random(-5, 5));
                        // balloonStringTarget pop  stuff ends
                    }
                    balloon = targetBalloon;
                }
            }
        }
    }
}

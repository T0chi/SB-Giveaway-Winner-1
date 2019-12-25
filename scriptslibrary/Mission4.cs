using OpenTK;
using System;
using System.Linq;
using System.Drawing;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;
using System.Collections.Generic;

// public class popTime
// {
//     public int t;
// }

public class Mission4
{
    //This is the parameters we pass in the class, these lines are used to make them available to use everywhere in the class.
    private StoryboardObjectGenerator generator;
    private int startTime;
    private int endTime;
    // private popTime t;
    private OsbSprite targetBalloon;
    private Color4 balloonColorMin;
    private Color4 balloonColorMax;

    public Mission4(StoryboardObjectGenerator generator, OsbSprite targetBalloon, OsbSprite avatarData, int startTime, int endTime, int interval, Color4 balloonColorMin, Color4 balloonColorMax)
    {
        //And this pack of lines are just the way we set our local variable with the parameters values of the constructor.
        // this.t = t;
        this.generator = generator;
        this.startTime = startTime;
        this.endTime = endTime;
        this.targetBalloon = targetBalloon;
        this.balloonColorMin = balloonColorMin;
        this.balloonColorMax = balloonColorMax;

        Mission(startTime, endTime, avatarData, interval, balloonColorMin, balloonColorMax);
    }

    public void Mission(int StartTime, int EndTime, OsbSprite avatarData, int interval, Color4 balloonColorMin, Color4 balloonColorMax)
    {
        var timeStepBalloon = interval;
        var Beat = generator.Beatmap.GetTimingPointAt(startTime).BeatDuration;

        // BALLOONS
        for (int time = startTime; time < endTime; time += (timeStepBalloon * 3))
        {
            var shiftX = generator.Random(-100, 100);
            var shiftY = generator.Random(-500, -380);
            var balloonScale = generator.Random(0.02, 0.55);
            var balloonDuration = generator.Random(6000, 10000);
            var balloonStartPos = new Vector2(generator.Random(0, 640), generator.Random(350, 400));
            var balloonEndPos = new Vector2(balloonStartPos.X + shiftX, balloonStartPos.Y + shiftY);

            // if balloons are small then they're NOT blurry ////////////////////////////////////////////////////////////////////////////////////
            if (balloonScale <= 0.2)
            {
            var balloon = generator.GetLayer("BalloonsBack").CreateSprite("sb/missions/4/balloon.png", OsbOrigin.BottomCentre);

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
            var balloonColor = RandomColor ? new Color4((float)generator.Random(balloonColorMin.R, balloonColorMax.R),
                                                    (float)generator.Random(balloonColorMin.G, balloonColorMax.G),
                                                    (float)generator.Random(balloonColorMin.B, balloonColorMax.B),
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
            var stringCount = generator.Random(10, 20);
            var prevPosX = balloonStartPos.X;
            var prevPosY = balloonStartPos.Y;
            var stringDelay = generator.Random(60, 100);
            for (var i = 0; i < stringCount; i++)
            {
                var balloonString = generator.GetLayer("BalloonsBack").CreateSprite("sb/particle2.png", OsbOrigin.Centre);

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
                    balloonString.ColorHsb(s, (i * 30 / stringCount) + generator.Random(-5.0, 10.0), 0.18 + generator.Random(0.4), 0.90);
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
                var balloon = generator.GetLayer("BalloonsFront").CreateSprite("sb/missions/4/balloonBlurry.png", OsbOrigin.BottomCentre);

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
            var balloonColor = RandomColor ? new Color4((float)generator.Random(balloonColorMin.R, balloonColorMax.R),
                                                    (float)generator.Random(balloonColorMin.G, balloonColorMax.G),
                                                    (float)generator.Random(balloonColorMin.B, balloonColorMax.B),
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
            var stringCount = generator.Random(10, 20);
            var prevPosX = balloonStartPos.X;
            var prevPosY = balloonStartPos.Y;
            var stringDelay = generator.Random(60, 100);
            for (var i = 0; i < stringCount; i++)
            {
                var balloonString = generator.GetLayer("BalloonsFront").CreateSprite("sb/particleBlurry2.png", OsbOrigin.Centre);

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
                    balloonString.ColorHsb(s, (i * 30 / stringCount) + generator.Random(-5.0, 10.0), 0.18 + generator.Random(0.4), 0.90);
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
            var shiftX = generator.Random(-100, 100);
            var shiftY = generator.Random(-400, -100);
            var popDuration = generator.Random(1500, 3000);
            var balloonScale = generator.Random(0.1, 0.55);
            var balloonDuration = generator.Random(4000, 8000);
            var balloonStartPos = new Vector2(generator.Random(0, 640), generator.Random(430, 480));
            var balloonEndPos = new Vector2(balloonStartPos.X + shiftX, balloonStartPos.Y + shiftY);

            // if balloons are small then they're NOT blurry ////////////////////////////////////////////////////////////////////////////////////
            if (balloonScale <= 0.27)
            {
                var balloon = generator.GetLayer("targetBalloonsBack").CreateSprite("sb/missions/4/balloon.png", OsbOrigin.BottomCentre);

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
                var balloonColor = RandomColor ? new Color4((float)generator.Random(balloonColorMin.R, balloonColorMax.R),
                                                        (float)generator.Random(balloonColorMin.G, balloonColorMax.G),
                                                        (float)generator.Random(balloonColorMin.B, balloonColorMax.B),
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
                var balloonPopCount = generator.Random(4, 7);
                for (var i = 0; i < balloonPopCount; i++)
                {
                    var balloonPopScale = balloon.ScaleAt(time + balloonDuration) * generator.Random(0.005, 0.2);
                    var randomScale = generator.Random(-balloonPopScale.X, balloonPopScale.X);

                    var balloonPop = generator.GetLayer("targetBalloonsBack").CreateSprite("sb/missions/4/balloon.png", OsbOrigin.Centre);
                    var balloonPopSound = generator.GetLayer("targetBalloonsBack").CreateSample("sb/sfx/balloon-pop.ogg", time + balloonDuration, 7);

                    if (time >= endTime - (popDuration * 4))
                    {
                        balloonPop.Fade(time + balloonDuration, time + balloonDuration + popDuration, 1, 0);
                    }
                    else
                    {
                        balloonPop.Fade(endTime, endTime + popDuration, 1, 0);
                    }
                    
                    balloonPop.Color(time + balloonDuration, balloon.ColorAt(time + balloonDuration));
                    balloonPop.MoveY(OsbEasing.InSine, time + balloonDuration, time + balloonDuration + popDuration, balloonEndPos.Y + generator.Random(-70, -20), balloonStartPos.Y);
                    balloonPop.MoveX(OsbEasing.OutSine, time + balloonDuration, time + balloonDuration + popDuration, balloonStartPos.X + generator.Random(-10, 10), balloonStartPos.X + generator.Random(-100, 100));
                    balloonPop.ScaleVec(OsbEasing.InOutSine, time + balloonDuration, time + balloonDuration + (popDuration / 2), balloonPopScale.X, balloonPopScale.Y, -balloonPopScale.X, randomScale);
                    balloonPop.ScaleVec(OsbEasing.InOutSine, time + balloonDuration + (popDuration / 2), time + balloonDuration + (popDuration / 1), -randomScale, balloonPopScale.Y, generator.Random(-balloonPopScale.X, balloonPopScale.X), balloonPopScale.Y);
                    balloonPop.Rotate(OsbEasing.InOutSine, time + balloonDuration, time + balloonDuration + popDuration, balloon.RotationAt(time + balloonDuration), balloon.RotationAt(time + balloonDuration) + generator.Random(-5, 5));
                }
                // balloon pop  stuff ends

                // arrows
                var arrowSpeed = generator.Random(400, 550);
                var arrow = generator.GetLayer("Arrows").CreateSprite("sb/missions/4/arrow.png", OsbOrigin.Centre);
                var arrowSound = generator.GetLayer("Arrows").CreateSample("sb/sfx/arrow-1.ogg", time + balloonDuration - arrowSpeed, 70);

                var arrowScale = 0.2;
                var randomX = generator.Random(50, 320);
                var arrowStartPos = new Vector2(avatarData.PositionAt(time + balloonDuration - arrowSpeed).X, 450);
                var arrowEndPos = balloon.PositionAt(time + balloonDuration);
                var arrowAngle = Math.Atan2(((balloon.PositionAt(time + balloonDuration - arrowSpeed).Y) - (avatarData.PositionAt(time + balloonDuration).Y) + 40), ((balloon.PositionAt(time + balloonDuration - arrowSpeed).X + 20) - avatarData.PositionAt(time + balloonDuration).X)) + (Math.PI / 2);

                arrow.Rotate(time + balloonDuration - arrowSpeed, arrowAngle);
                arrow.Fade(time + balloonDuration - arrowSpeed, time + balloonDuration - arrowSpeed + (arrowSpeed / 4), 0, 1);
                arrow.Fade(time + balloonDuration, time + balloonDuration + (arrowSpeed / 4), 1, 0);
                arrow.ScaleVec(time + balloonDuration - arrowSpeed, time + balloonDuration, arrowScale * 0.7, arrowScale * 0.7, arrowScale * 0.6, arrowScale * 0.5);
                arrow.MoveX(time + balloonDuration - arrowSpeed, time + balloonDuration, arrowStartPos.X, arrowEndPos.X);
                arrow.MoveY(time + balloonDuration - arrowSpeed, time + balloonDuration, arrowStartPos.Y - 100, arrowEndPos.Y);

                generator.Log(avatarData.PositionAt(time + balloonDuration).X.ToString());


                // balloon string
                var stringCount = generator.Random(10, 20);
                var prevPosX = balloonStartPos.X;
                var prevPosY = balloonStartPos.Y;
                var stringDelay = generator.Random(60, 100);
                var heightOffset = 52 * balloonScale;
                var stringDistance = stringCount * (balloonScale * 1.2);
                for (var i = 0; i < stringCount; i++)
                {
                    var balloonStringTarget = generator.GetLayer("targetBalloonsBack").CreateSprite("sb/particle2.png", OsbOrigin.Centre);

                    balloonStringTarget.Fade(time + (balloonDuration / 10), time + (balloonDuration / 10) + (balloonDuration / 6), 0, 1);

                    var timeStepBalloonString = Beat;
                    for (int s = time; s < time + balloonDuration; s += (int)timeStepBalloonString)
                    {
                        var Scale = balloonScale * 0.1;
                        var position = new Vector2(balloon.PositionAt(s + timeStepBalloonString).X, balloon.PositionAt(s + timeStepBalloonString).Y + i * stringDistance);

                        balloonStringTarget.ScaleVec(s, Scale / 4, Scale * 3);
                        balloonStringTarget.ColorHsb(s, (i * 30 / stringCount) + generator.Random(-5.0, 10.0), 0.18 + generator.Random(0.4), 0.90);
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
                    balloonStringTarget.MoveX(OsbEasing.InSine, time + balloonDuration + (stringDelay * i), time + balloonDuration + popDuration, balloonStringPosX, balloonStringPosX + generator.Random(-40, 40));
                    balloonStringTarget.ScaleVec(OsbEasing.InOutSine, time + balloonDuration, time + balloonDuration + (popDuration / 2), balloonStringScale.X, balloonStringScale.Y, -balloonStringScale.X, balloonStringScale.Y);
                    balloonStringTarget.ScaleVec(OsbEasing.InOutSine, time + balloonDuration + (popDuration / 2), time + balloonDuration + (popDuration / 1), -balloonStringScale.X, balloonStringScale.Y, balloonStringScale.X, balloonStringScale.Y);
                    balloonStringTarget.Rotate(OsbEasing.InOutSine, time + balloonDuration + (stringDelay * i), time + balloonDuration + (stringDelay * i) + popDuration, balloonStringTarget.RotationAt(time + balloonDuration + (stringDelay * i)), balloonStringTarget.RotationAt(time + balloonDuration + (stringDelay * i)) + generator.Random(-5, 5));
                    // balloonStringTarget pop  stuff ends
                }
                balloon = targetBalloon;
            }
            
            // if balloons are large then they ARE blurry ////////////////////////////////////////////////////////////////////////////////////
            else
            {
                var balloon = generator.GetLayer("targetBalloonsFront").CreateSprite("sb/missions/4/balloonBlurry.png", OsbOrigin.BottomCentre);

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
                var balloonColor = RandomColor ? new Color4((float)generator.Random(balloonColorMin.R, balloonColorMax.R),
                                                        (float)generator.Random(balloonColorMin.G, balloonColorMax.G),
                                                        (float)generator.Random(balloonColorMin.B, balloonColorMax.B),
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
                var balloonPopCount = generator.Random(4, 7);
                for (var i = 0; i < balloonPopCount; i++)
                {
                    var balloonPopScale = balloon.ScaleAt(time + balloonDuration) * generator.Random(0.005, 0.2);
                    var randomScale = generator.Random(-balloonPopScale.X, balloonPopScale.X);

                    var balloonPop = generator.GetLayer("targetBalloonsFront").CreateSprite("sb/missions/4/balloon.png", OsbOrigin.Centre);
                    var balloonPopSound = generator.GetLayer("targetBalloonsFront").CreateSample("sb/sfx/balloon-pop.ogg", time + balloonDuration, 12);

                    if (time >= endTime - (popDuration * 4))
                    {
                        balloonPop.Fade(time + balloonDuration, time + balloonDuration + popDuration, 0.9, 0);
                    }
                    else
                    {
                        balloonPop.Fade(endTime, endTime + popDuration, 0.9, 0);
                    }

                    balloonPop.Color(time + balloonDuration, balloon.ColorAt(time + balloonDuration));
                    balloonPop.MoveY(OsbEasing.InSine, time + balloonDuration, time + balloonDuration + popDuration, balloonEndPos.Y + generator.Random(-70, -20), balloonStartPos.Y);
                    balloonPop.MoveX(OsbEasing.OutSine, time + balloonDuration, time + balloonDuration + popDuration, balloonStartPos.X + generator.Random(-10, 10), balloonStartPos.X + generator.Random(-100, 100));
                    balloonPop.ScaleVec(OsbEasing.InOutSine, time + balloonDuration, time + balloonDuration + (popDuration / 2), balloonPopScale.X, balloonPopScale.Y, -balloonPopScale.X, randomScale);
                    balloonPop.ScaleVec(OsbEasing.InOutSine, time + balloonDuration + (popDuration / 2), time + balloonDuration + (popDuration / 1), -randomScale, balloonPopScale.Y, generator.Random(-balloonPopScale.X, balloonPopScale.X), balloonPopScale.Y);
                    balloonPop.Rotate(OsbEasing.InOutSine, time + balloonDuration, time + balloonDuration + popDuration, balloon.RotationAt(time + balloonDuration), balloon.RotationAt(time + balloonDuration) + generator.Random(-5, 5));
                }
                // balloon pop  stuff ends

                // arrows
                var arrowSpeed = generator.Random(400, 550);
                var arrow = generator.GetLayer("Arrows").CreateSprite("sb/missions/4/arrow2.png", OsbOrigin.Centre);
                var arrowSound = generator.GetLayer("Arrows").CreateSample("sb/sfx/arrow-1.ogg", time + balloonDuration - arrowSpeed, 100);

                var arrowScale = 0.2;
                var randomX = generator.Random(50, 320);
                var arrowStartPos = new Vector2(avatarData.PositionAt(time + balloonDuration - arrowSpeed).X, 450);
                var arrowEndPos = balloon.PositionAt(time + balloonDuration);
                var arrowAngle = Math.Atan2(((balloon.PositionAt(time + balloonDuration - arrowSpeed).Y - 50 ) - (avatarData.PositionAt(time + balloonDuration).Y) + 40), ((balloon.PositionAt(time + balloonDuration - arrowSpeed).X + 50) - avatarData.PositionAt(time + balloonDuration).X)) + (Math.PI / 2);

                arrow.Rotate(time + balloonDuration - arrowSpeed, arrowAngle);
                arrow.Fade(time + balloonDuration - arrowSpeed, time + balloonDuration - arrowSpeed + (arrowSpeed / 4), 0, 1);
                arrow.Fade(time + balloonDuration, time + balloonDuration + (arrowSpeed / 4), 1, 0);
                arrow.ScaleVec(time + balloonDuration - arrowSpeed, time + balloonDuration, arrowScale * 0.05, arrowScale * 1, arrowScale * 1.3, arrowScale * 0.5);
                arrow.MoveX(time + balloonDuration - arrowSpeed, time + balloonDuration, arrowStartPos.X, arrowEndPos.X);
                arrow.MoveY(time + balloonDuration - arrowSpeed, time + balloonDuration, arrowStartPos.Y - 100, arrowEndPos.Y - 50);

                generator.Log(avatarData.PositionAt(time + balloonDuration).X.ToString());


                // balloon string
                var stringCount = generator.Random(10, 20);
                var prevPosX = balloonStartPos.X;
                var prevPosY = balloonStartPos.Y;
                var stringDelay = generator.Random(60, 100);
                var heightOffset = 52 * balloonScale;
                var stringDistance = stringCount * (balloonScale * 1.2);
                for (var i = 0; i < stringCount; i++)
                {
                    var balloonStringTarget = generator.GetLayer("targetBalloonsFront").CreateSprite("sb/particle2.png", OsbOrigin.Centre);

                    balloonStringTarget.Fade(time + (balloonDuration / 10), time + (balloonDuration / 10) + (balloonDuration / 6), 0, 0.9);

                    var timeStepBalloonString = Beat;
                    for (int s = time; s < time + balloonDuration; s += (int)timeStepBalloonString)
                    {
                        var Scale = balloonScale * 0.1;
                        var position = new Vector2(balloon.PositionAt(s + timeStepBalloonString).X, balloon.PositionAt(s + timeStepBalloonString).Y + i * stringDistance);

                        balloonStringTarget.ScaleVec(s, Scale / 4, Scale * 3);
                        balloonStringTarget.ColorHsb(s, (i * 30 / stringCount) + generator.Random(-5.0, 10.0), 0.18 + generator.Random(0.4), 0.90);
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
                    balloonStringTarget.MoveX(OsbEasing.InSine, time + balloonDuration + (stringDelay * i), time + balloonDuration + popDuration, balloonStringPosX, balloonStringPosX + generator.Random(-40, 40));
                    balloonStringTarget.ScaleVec(OsbEasing.InOutSine, time + balloonDuration, time + balloonDuration + (popDuration / 2), balloonStringScale.X, balloonStringScale.Y, -balloonStringScale.X, balloonStringScale.Y);
                    balloonStringTarget.ScaleVec(OsbEasing.InOutSine, time + balloonDuration + (popDuration / 2), time + balloonDuration + (popDuration / 1), -balloonStringScale.X, balloonStringScale.Y, balloonStringScale.X, balloonStringScale.Y);
                    balloonStringTarget.Rotate(OsbEasing.InOutSine, time + balloonDuration + (stringDelay * i), time + balloonDuration + (stringDelay * i) + popDuration, balloonStringTarget.RotationAt(time + balloonDuration + (stringDelay * i)), balloonStringTarget.RotationAt(time + balloonDuration + (stringDelay * i)) + generator.Random(-5, 5));
                    // balloonStringTarget pop  stuff ends
                }
                balloon = targetBalloon;
            }
        }
    }
}
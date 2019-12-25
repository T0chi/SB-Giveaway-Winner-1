using OpenTK;
using System;
using System.Linq;
using System.Drawing;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;
using System.Collections.Generic;

public class Mission1
{
    //This is the parameters we pass in the class, these lines are used to make them available to use everywhere in the class.
    private StoryboardObjectGenerator generator;
    private int startTime;
    private int endTime;

    public Mission1(StoryboardObjectGenerator generator, int startTime, int endTime)
    {
        //And this pack of lines are just the way we set our local variable with the parameters values of the constructor.
        this.generator = generator;
        this.startTime = startTime;
        this.endTime = endTime;

        Aircraft(startTime, endTime);
        Clouds(startTime, endTime);
    }

    public void Aircraft(int StartTime, int EndTime)
    {
        var startTime = StartTime;
        var endTime = EndTime;
        var duration3 = 1500;
        var enemyDuration = 1000;

        var loopCount3 = (startTime) / (duration3 * 5);
        var enemyloopCount = (startTime) / (enemyDuration * 5);
        // var jetSound = generator.GetLayer("Aircraft 3 Front").CreateSample("sb/sfx/jet-1.ogg", startTime, 70);
        // var jetSound2 = generator.GetLayer("Aircraft 3 Front").CreateSample("sb/sfx/jet-3.ogg", startTime, 70);
        // var jetSound3 = generator.GetLayer("Aircraft 3 Front").CreateSample("sb/sfx/jet_passing-1.ogg", 700, 100);
        var aircraft3 = generator.GetLayer("Aircraft 3 Back").CreateSprite("sb/missions/1/aircrafts/3_back.png", OsbOrigin.Centre);
        var enemy = generator.GetLayer("Aircraft 1 & 2 Front").CreateSprite("sb/missions/1/aircrafts/" + generator.Random(1, 3) + "_back.png", OsbOrigin.Centre);

        aircraft3.Scale(OsbEasing.Out, startTime, startTime + 4000, 4, 0.3);
        aircraft3.Fade(OsbEasing.Out, startTime, startTime + 2000, 0, 1);
        aircraft3.Fade(endTime - 500, endTime, 1, 0);

        aircraft3.StartLoopGroup(startTime, loopCount3 - 1);
        // x
        aircraft3.MoveX(OsbEasing.InOutSine, 0, duration3 / 2, 310, 330);
        aircraft3.MoveX(OsbEasing.InOutSine, duration3 / 2, duration3, 330, 300);
        aircraft3.MoveX(OsbEasing.InOutSine, duration3, duration3 * 2, 300, 310);
        aircraft3.MoveX(OsbEasing.InOutSine, duration3 * 2, duration3 * 3, 310, 330);
        aircraft3.MoveX(OsbEasing.InOutSine, duration3 * 3, duration3 * 4, 330, 320);
        aircraft3.MoveX(OsbEasing.InOutSine, duration3 * 4, duration3 * 5, 320, 310);
        // y
        aircraft3.MoveY(OsbEasing.InOutSine, 0, duration3 / 2, 270, 340);
        aircraft3.MoveY(OsbEasing.InOutSine, duration3 / 2, duration3, 340, 330);
        aircraft3.MoveY(OsbEasing.InOutSine, duration3, duration3 * 2, 330, 350);
        aircraft3.MoveY(OsbEasing.InOutSine, duration3 * 2, duration3 * 3, 350, 310);
        aircraft3.MoveY(OsbEasing.InOutSine, duration3 * 3, duration3 * 4, 310, 330);
        aircraft3.MoveY(OsbEasing.InOutSine, duration3 * 4, duration3 * 5, 330, 270);
        //
        aircraft3.EndGroup();

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
        var randomStep = generator.Random(100, 1000);
        for (var i = startTime + 3500; i < endTime; i += randomStep)
        {
            var speed = 500;
            var fadeTime = 50;
            var angle = Math.Atan2((enemy.PositionAt(i + (speed / 1.5f)).Y - aircraft3.PositionAt(i).Y),
                                      (enemy.PositionAt(i + (speed / 1.5f)).X - aircraft3.PositionAt(i).X)) + (Math.PI / 2);
            var pewRight = generator.GetLayer("Laser").CreateSprite("sb/pixel.png", OsbOrigin.Centre);
            var pewLeft = generator.GetLayer("Laser").CreateSprite("sb/pixel.png", OsbOrigin.Centre);
            var laserSound = generator.GetLayer("Laser").CreateSample("sb/sfx/laser-1.ogg", i + 80, generator.Random(5, 15));

            pewRight.Rotate(i, angle);
            pewRight.Color(i, Color4.Red);
            pewRight.Additive(i, i + speed + fadeTime);
            pewRight.ScaleVec(OsbEasing.OutExpo, i, i + (speed / 4), 3, 0, 3, 13);
            pewRight.ScaleVec(OsbEasing.Out, i + (speed / 4), i + speed, 3, 13, 2, 2);
            pewRight.Move(i, i + speed, aircraft3.PositionAt(i).X + 20, aircraft3.PositionAt(i).Y,
                                   enemy.PositionAt(i + (speed / generator.Random(1, 1.5f))).X, enemy.PositionAt(i + (speed / generator.Random(1, 1.5f))).Y);

            pewLeft.Rotate(i, angle);
            pewLeft.Color(i, Color4.Red);
            pewLeft.Additive(i, i + speed + fadeTime);
            pewLeft.ScaleVec(OsbEasing.OutExpo, i, i + (speed / 4), 3, 0, 3, 13);
            pewLeft.ScaleVec(OsbEasing.Out, i + (speed / 4), i + speed, 3, 13, 2, 2);
            pewLeft.Move(i, i + speed, aircraft3.PositionAt(i).X - 20, aircraft3.PositionAt(i).Y,
                                   enemy.PositionAt(i + (speed / generator.Random(1, 1.5f))).X, enemy.PositionAt(i + (speed / generator.Random(1, 1.5f))).Y);
            
            // when it hits the enemy
            if (enemy.PositionAt(i + (speed / generator.Random(1, 1.5f))).Y <= enemy.PositionAt(i + (speed)).Y)
            {
                var FrameDelay = 100;
                var LaserHit = generator.GetLayer("Laser").CreateSample("sb/sfx/laser-hit.ogg", i + speed, generator.Random(10, 20));
                var laserHit = generator.GetLayer("Laser").CreateAnimation("sb/missions/1/explosion/explode.jpg", 12, FrameDelay, OsbLoopType.LoopOnce, OsbOrigin.Centre);
                
                laserHit.Additive(i + speed, i + speed + fadeTime + FrameDelay);
                laserHit.Scale(i + speed, i + speed + fadeTime + FrameDelay, 0.2, 0.1);
                laserHit.Move(i + speed + fadeTime, enemy.PositionAt(i + (speed / generator.Random(1, 1.5f))));
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
            var smokeLeft = generator.GetLayer("Aircraft 3 Back - Smoke").CreateAnimation("sb/missions/1/aircrafts/smoke/smoke.png",
                                                            4, 100, OsbLoopType.LoopForever, OsbOrigin.Centre);
            var smokeRight = generator.GetLayer("Aircraft 3 Back - Smoke").CreateAnimation("sb/missions/1/aircrafts/smoke/smoke.png",
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
                X -= 3.35f;
                scale -= 0.034f;
                fade += 0.002f;

                smokeLeft.Fade(i, fade);
                smokeLeft.Additive(i, i + smokeDuration);
                smokeLeft.Scale(OsbEasing.OutExpo, i, i + (smokeDuration * fadeDelay), scale, scale / 4);
                smokeLeft.Fade(i + smokeDuration, i + (smokeDuration * fadeDelay), fade, 0);
                smokeLeft.Move(OsbEasing.In, i, i + smokeDuration, aircraft3.PositionAt(i).X - X, aircraft3.PositionAt(i).Y - y,
                        aircraft3.PositionAt(i - (smokeDuration * 2)).X - X, aircraft3.PositionAt(i - (smokeDuration * delay)).Y - y);

                smokeRight.Fade(i, fade);
                smokeRight.Additive(i, i + smokeDuration);
                smokeRight.Scale(OsbEasing.OutExpo, i, i + (smokeDuration * fadeDelay), scale, scale / 4);
                smokeRight.Fade(i + smokeDuration, i + (smokeDuration * fadeDelay), fade, 0);
                smokeRight.Move(OsbEasing.In, i, i + smokeDuration, aircraft3.PositionAt(i).X + X, aircraft3.PositionAt(i).Y - y,
                        aircraft3.PositionAt(i - (smokeDuration * 2)).X + X, aircraft3.PositionAt(i - (smokeDuration * delay)).Y - y);
            }
        }

        var eSmokeAmount = 40;
        var eSmokeDuration = 100;
        for (int i = startTime; i <= endTime; i += eSmokeAmount)
        {
            var smokeLeft = generator.GetLayer("Aircraft 1 & 2 Front - Smoke").CreateAnimation("sb/missions/1/aircrafts/smoke/smoke.png",
                                                            4, 100, OsbLoopType.LoopForever, OsbOrigin.Centre);
            var smokeRight = generator.GetLayer("Aircraft 1 & 2 Front - Smoke").CreateAnimation("sb/missions/1/aircrafts/smoke/smoke.png",
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
                var duration = generator.Random(800, 1200);
                var RandomX = generator.Random(-107, 747);
                var RandomFade = generator.Random(0.015, 0.15);
                var RandomScale = generator.Random(2.5, 3);
                var startPos  = new Vector2(RandomX, generator.Random(300, 315));
                var endPos  = new Vector2(RandomX, generator.Random(500, 520));

                var sprite = generator.GetLayer("Clouds").CreateSprite("sb/missions/1/cloud/cloud" + generator.Random(1, 11) + ".png", OsbOrigin.TopCentre);

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

    // public void AircraftEnemy(int StartTime, int EndTime)
    // {
    //     var startTime = StartTime;
    //     var endTime = EndTime;
    //     var enemyDuration = 1000;

    //     var enemyloopCount = (startTime) / (enemyDuration * 5);
    //     var enemy = generator.GetLayer("Aircraft 1 & 2 Front").CreateSprite("sb/missions/1/aircrafts/" + generator.Random(1, 3) + "_back.png", OsbOrigin.Centre);

    //     enemy.Scale(OsbEasing.Out, startTime, startTime + 4000, 0.05, 0.1);
    //     enemy.Fade(OsbEasing.Out, startTime, startTime + 2000, 0, 1);
    //     enemy.Fade(endTime - 500, endTime, 1, 0);

    //     enemy.StartLoopGroup(startTime, enemyloopCount - 1);
    //     // x
    //     enemy.MoveX(OsbEasing.InOutSine, 0, enemyDuration / 2, 310, 330);
    //     enemy.MoveX(OsbEasing.InOutSine, enemyDuration / 2, enemyDuration, 330, 300);
    //     enemy.MoveX(OsbEasing.InOutSine, enemyDuration, enemyDuration * 2, 300, 310);
    //     enemy.MoveX(OsbEasing.InOutSine, enemyDuration * 2, enemyDuration * 3, 310, 330);
    //     enemy.MoveX(OsbEasing.InOutSine, enemyDuration * 3, enemyDuration * 4, 330, 320);
    //     enemy.MoveX(OsbEasing.InOutSine, enemyDuration * 4, enemyDuration * 5, 320, 310);
    //     // y
    //     enemy.MoveY(OsbEasing.InOutSine, 0, enemyDuration / 2, 270, 240);
    //     enemy.MoveY(OsbEasing.InOutSine, enemyDuration / 2, enemyDuration, 240, 230);
    //     enemy.MoveY(OsbEasing.InOutSine, enemyDuration, enemyDuration * 2, 230, 250);
    //     enemy.MoveY(OsbEasing.InOutSine, enemyDuration * 2, enemyDuration * 3, 250, 210);
    //     enemy.MoveY(OsbEasing.InOutSine, enemyDuration * 3, enemyDuration * 4, 210, 230);
    //     enemy.MoveY(OsbEasing.InOutSine, enemyDuration * 4, enemyDuration * 5, 230, 270);
    //     //
    //     enemy.EndGroup();

    //     // SMOKE
    //     var eSmokeAmount = 40;
    //     var eSmokeDuration = 100;
    //     for (int i = startTime; i <= endTime; i += eSmokeAmount)
    //     {
    //         var smokeLeft = generator.GetLayer("Aircraft 1 & 2 Front - Smoke").CreateAnimation("sb/missions/1/aircrafts/smoke/smoke.png",
    //                                                         4, 100, OsbLoopType.LoopForever, OsbOrigin.Centre);
    //         var smokeRight = generator.GetLayer("Aircraft 1 & 2 Front - Smoke").CreateAnimation("sb/missions/1/aircrafts/smoke/smoke.png",
    //                                                         4, 100, OsbLoopType.LoopForever, OsbOrigin.Centre);
    //         int x = 10;
    //         int y = -5;
    //         float delay = 2f;
    //         int fadeDelay = 2;

    //         if (i > startTime + 1000)
    //         {
    //             var Scale = 0.15f;
    //             smokeLeft.Fade(i, 0.09f);
    //             smokeLeft.Additive(i, i + eSmokeDuration);
    //             smokeLeft.Scale(OsbEasing.In, i, i + (eSmokeDuration * fadeDelay), Scale, Scale / 4);
    //             smokeLeft.Fade(i + eSmokeDuration, i + (eSmokeDuration * fadeDelay), 0.09f, 0);
    //             smokeLeft.Move(OsbEasing.Out, i, i + eSmokeDuration, enemy.PositionAt(i).X - x, enemy.PositionAt(i).Y - y,
    //                     enemy.PositionAt(i - (eSmokeDuration * 2)).X - x, enemy.PositionAt(i - (eSmokeDuration * delay)).Y - y);

    //             smokeRight.Fade(i, 0.09f);
    //             smokeRight.Additive(i, i + eSmokeDuration);
    //             smokeRight.Scale(OsbEasing.In, i, i + (eSmokeDuration * fadeDelay), Scale, Scale / 4);
    //             smokeRight.Fade(i + eSmokeDuration, i + (eSmokeDuration * fadeDelay), 0.09f, 0);
    //             smokeRight.Move(OsbEasing.Out, i, i + eSmokeDuration, enemy.PositionAt(i).X + x, enemy.PositionAt(i).Y - y,
    //                     enemy.PositionAt(i - (eSmokeDuration * 2)).X + x, enemy.PositionAt(i - (eSmokeDuration * delay)).Y - y);
    //         }
    //     }
    // }

    public void AircraftDestruction()
    {
    }

    public void AircraftDestructionLight()
    {
    }
}
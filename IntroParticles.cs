using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class IntroParticles : StoryboardObjectGenerator
    {
        [Configurable]
        public string ParticlePath = "sb/particle.png";

        [Configurable]
        public double ParticleFadeMin = 0.8;

        [Configurable]
        public double ParticleFadeMax = 1;

        [Configurable]
        public bool RandomParticleFade = true;

        [Configurable]
        public OsbOrigin ParticleOrigin = OsbOrigin.Centre;

        [Configurable]
        public int ConnectRadiusMin = 50;
        
        [Configurable]
        public int ConnectRadiusMax = 100;

        [Configurable]
        public int TimeBetweenParticles = 400;

        [Configurable]
        public int TravelSpeed = 50;

        [Configurable]
        public bool RandomMovement = true;

        [Configurable]
        public bool RandomX;

        [Configurable]
        public bool RandomY;

        [Configurable]
        public int FadeTimeIn = 500;

        [Configurable]
        public int FadeTimeOut = 500;

        [Configurable]
        public double StartScale = 0.3;

        [Configurable]
        public double EndScale = 0.8;

        [Configurable]
        public bool RandomScale = true;

        [Configurable]
        public double StartRotation = 0;

        [Configurable]
        public double EndRotation = 0;

        [Configurable]
        public bool RandomRotation = false;

        [Configurable]
        public bool ParticleAdditive = true;

        [Configurable]
        public Color4 ParticleColor = Color4.White;

        public override void Generate()
        {
            ParticleGen(6000, 1399, 60014, "Front", new Vector2(-107, 200), new Vector2(747, 480));
            ParticleGen(4500, 1399, 60014, "Back", new Vector2(-107, 200), new Vector2(747, 480));
        }

        private void ParticleGen(int duration, int StartTime, int EndTime, string LayerName, Vector2 StartPosition, Vector2 EndPosition)
        {
            if (StartTime == EndTime)
            {
                StartTime = (int)Beatmap.HitObjects.First().StartTime;
                EndTime = (int)Beatmap.HitObjects.Last().EndTime;
            }
            EndTime = Math.Min(EndTime, (int)AudioDuration);
            StartTime = Math.Min(StartTime, EndTime);

            // This is an example of using a particle pool.
            // Sprites using the same layer, path and origin can be reused as if they were multiple sprites.

            var pos = new Dictionary<OsbSprite, Vector2d>();
            var connected = new Dictionary<int, List<OsbSprite>>();

            var layer = GetLayer(LayerName);
            using (var pool = new OsbSpritePool(layer, ParticlePath, ParticleOrigin, (particle, startTime, endTime) =>
            { }))
            {
                for (int i = StartTime; i < (EndTime); i += TimeBetweenParticles)
                {
                    var Amount = duration;
                    var ParticleFade = RandomParticleFade ? Random(ParticleFadeMin, ParticleFadeMax) : ParticleFadeMin;
                    var particle = pool.Get(i, i + Amount);

                    particle.Color(i, ParticleColor);

                    if (StartScale == EndScale && StartScale != 1)
                        particle.Scale(i, StartScale);

                    if (StartRotation == EndRotation && StartRotation != 0)
                        particle.Rotate(i, MathHelper.DegreesToRadians(StartRotation));

                    if (StartScale != EndScale)
                        if (RandomScale)
                            particle.Scale(i, i + Amount, Random(StartScale, EndScale), Random(StartScale, EndScale));
                        else particle.Scale(i, i + Amount, StartScale, EndScale);

                    if (StartRotation != EndRotation)
                        if (RandomRotation)
                            particle.Rotate(i, i + Amount, MathHelper.DegreesToRadians(Random(StartRotation, EndRotation)), MathHelper.DegreesToRadians(Random(StartRotation, EndRotation)));
                        else particle.Rotate(i, i + Amount, MathHelper.DegreesToRadians(StartRotation), MathHelper.DegreesToRadians(EndRotation));

                    if (ParticleAdditive)
                    {
                        particle.Additive(i, i + Amount);
                    }

                    if (!RandomMovement)
                    {
                        var startX = RandomX ? Random(StartPosition.X, EndPosition.X) : StartPosition.X;
                        var startY = RandomY ? Random(EndPosition.Y, StartPosition.Y) : StartPosition.Y;
                        var endX = RandomX ? startX : EndPosition.X;
                        var endY = RandomY ? startY : EndPosition.Y;
                        particle.Move(i, i + Amount, startX, startY, endX, endY);
                    }

                    else
                    {
                        var lastX = Random(StartPosition.X, EndPosition.X);
                        var lastY = Random(StartPosition.Y, EndPosition.Y);

                        var UpdateRate = 500;
                        var lastAngle = 0d;
                        var lastDistance = 0d;

                        var rVec = MathHelper.DegreesToRadians(Random(0, 360));
                        var sVec = Random(1, 16);
                        var vX = (Math.Cos(rVec) * sVec) / (TravelSpeed / 2);
                        var vY = (Math.Sin(rVec) * sVec) / (TravelSpeed / 2);

                        var startPosition = new Vector2d(lastX, lastY);
                        pos[particle] = startPosition;
                        var endPosition = new Vector2d(lastX, lastY);

                        for (var t = i; t < i + Amount; t += UpdateRate)
                        {
                            var nextX = lastX + (vX / 10);
                            var nextY = lastY + (vY / 10);

                            startPosition.X = lastX;
                            startPosition.Y = lastY;

                            particle.Move(t, t + UpdateRate, lastX, lastY, nextX, nextY);

                            vX += Random(-TravelSpeed, TravelSpeed) * UpdateRate / 1000;
                            vY += Random(-TravelSpeed, TravelSpeed) * UpdateRate / 1000;

                            lastX = nextX;
                            lastY = nextY;
                        }
                    }

                    if (i < EndTime - (FadeTimeIn + FadeTimeOut))
                    {
                        particle.Fade(i, i + FadeTimeIn, 0, ParticleFade);
                        if (i < EndTime - Amount)
                        {
                            particle.Fade(i + Amount - FadeTimeOut, i + Amount, ParticleFade, 0);
                        }
                        else
                        {
                            particle.Fade(EndTime - FadeTimeOut, EndTime, ParticleFade, 0);
                        }
                    }
                    else
                    {
                        particle.Fade(i, 0);
                    }
                }
            }
        }
    }
}
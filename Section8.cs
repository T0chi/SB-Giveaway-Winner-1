using OpenTK;
using System.Drawing;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Animations;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class Section8 : StoryboardObjectGenerator
    {
        [Configurable]
        public Color4 bgOverlayColor = Color4.White;

        [Configurable]
        public Color4 startColor = Color4.White;

        [Configurable]
        public Color4 ParticleColor = Color4.White;

        [Configurable]
        public Color4 SpectrumColor1 = Color4.White;

        [Configurable]
        public Color4 SpectrumColor2 = Color4.White;

        [Configurable]
        public int BarCount = 20;

        [Configurable]
        public int Width = 500;
        
        [Configurable]
        public int Rotation = 100;

        [Configurable]
        public float LogScale = 600;

        [Configurable]
        public Vector2 Position = new Vector2(250, 260);

        [Configurable]
        public Vector2 Scale = new Vector2(1, 200);

        private DialogManager dialog;

        private DialogManager dialog2;

        public override void Generate()
        {
            Mission();
            Background();
            Spectrum(0, SpectrumColor1);
            Spectrum(5, SpectrumColor2);
            ParticleGen(339380, 379590, GetLayer("Back"));
            ParticleGen(339380, 379590, GetLayer("Front"));
            HUD(331111, 379590, 339380, "Mission #7", "Rhuzerv", "sb/HUD/txt/nameTag/toybot.png", 4500, "sb/avatars/toybotProfile.png");
        }

        public void Background()
        {
            var bitmap = GetMapsetBitmap("sb/bgs/8/bg.jpg");
            var bg = GetLayer("Background").CreateSprite("sb/bgs/8/bg.jpg", OsbOrigin.Centre, new Vector2(320, 240));
            var bgOverlay = GetLayer("bgOverlay").CreateSprite("sb/bgs/8/bg_overlay.png", OsbOrigin.Centre, new Vector2(320, 240));

            bg.Scale(331111, 854.0f / bitmap.Width);
            bg.Fade(331111, 339380, 0, 0.5);
            bg.Fade(379590, 387313, 0.5, 0);
            bgOverlay.Scale(331111, 854.0f / bitmap.Width);
            bgOverlay.Fade(331111, 339380, 0, 1);
            bgOverlay.Fade(379590, 387313, 1, 0);
            bgOverlay.Color(331111, bgOverlayColor);
        }

        public void HUD(int startTime, int endTime, int loadingTextEndtime, string mission, string songName, string nameTag, int progressBarDelay, string avatar)
        {
            var hud = new HUD(this, startTime, endTime, loadingTextEndtime, mission, songName, nameTag, progressBarDelay, avatar);
        }

        public void Mission()
        {
            var startTime = 339380;
            var endTime = 379590;
            var Beat = Beatmap.GetTimingPointAt(startTime).BeatDuration * 4;

            // aircraft
            var duration = Beat * 12;
            for (float i = startTime; i < endTime - duration; i += (float)Beat * 8)
            {
                var startPos = new Vector2(360, Random(220, 320));
                var endPos = new Vector2(-500, startPos.Y + 200);
                var startRotation = MathHelper.DegreesToRadians(Random(-5, 10));
                var endRotation = MathHelper.DegreesToRadians(Random(-15, -5));
                
                var soundSFX = GetLayer("Aircraft").CreateSample("sb/sfx/jet-4.ogg", i + 2500, 20);
                var aircraft = GetLayer("Aircraft").CreateAnimation("sb/missions/8/aircraft.png", 2, 30, OsbLoopType.LoopForever, OsbOrigin.CentreLeft);

                aircraft.Color(i, i + duration, startColor, bgOverlayColor);
                aircraft.Rotate(OsbEasing.InOutSine, i, i + duration, startRotation, endRotation);
                aircraft.Move(OsbEasing.In, i, i + duration, startPos, endPos);
                aircraft.ScaleVec(OsbEasing.In, i, i + duration, 0.1, 0.1, 0.5, 0.6);
            }
        }

        private Vector2 transform(Vector2 position)
        {
            var Offset = new Vector2(Position.X, Position.Y);

            position = new Vector2(position.X - Offset.X, position.Y - Offset.Y);
            return Vector2.Transform(position, Quaternion.FromEulerAngles((float)(MathHelper.DegreesToRadians(Rotation)), 0, 0)) + Offset;
        }

        public void Spectrum(float offset, Color4 Color)
        {
            var MinimalHeight = 0.1f;

            var StartTime = 359485;
            var EndTime = 379590;
            var endTime = Math.Min(EndTime, (int)AudioDuration);
            var startTime = Math.Min(StartTime, endTime);
            var bitmap = GetMapsetBitmap("sb/pixel.png");

            var heightKeyframes = new KeyframedValue<float>[BarCount];
            for (var i = 0; i < BarCount; i++)
                heightKeyframes[i] = new KeyframedValue<float>(null);

            var fftTimeStep = Beatmap.GetTimingPointAt(startTime).BeatDuration / 8;
            var fftOffset = fftTimeStep * 0.2;
            for (var time = (double)startTime; time < endTime; time += fftTimeStep)
            {
                var fft = GetFft(time + fftOffset, BarCount, null, OsbEasing.InExpo);
                for (var i = 0; i < BarCount; i++)
                {
                    var height = (float)Math.Log10(1 + fft[i] * LogScale) * Scale.Y / bitmap.Height;
                    if (height < MinimalHeight) height = MinimalHeight;

                    heightKeyframes[i].Add(time, height);
                }
            }

            var barWidth = Width / BarCount;
            var posX = Position.X - (Width / 2);

            for (var i = 0; i < BarCount; i++)
            {
                var positionX = posX + i * barWidth;
                var keyframes = heightKeyframes[i];
                keyframes.Simplify1dKeyframes(0.2, h => h);

                var bar = GetLayer("Spectrum").CreateSprite("sb/pixel.png", OsbOrigin.Centre);
                
                bar.Color(startTime, Color);
                bar.Fade(startTime, startTime + 500, 0, 0.8f);
                bar.Fade(endTime, endTime + 1000, 0.8f, 0);

                bar.Rotate(OsbEasing.OutBack, startTime + BarCount * i, startTime + 1000 + BarCount * i,
                    (float)(MathHelper.DegreesToRadians(Random(-50, 50))),
                    (float)(MathHelper.DegreesToRadians(Rotation)));

                bar.Move(OsbEasing.OutBack, startTime + BarCount * i, startTime + 1000 + BarCount * i,
                    transform(new Vector2(-125 - Width + BarCount * i - offset, Random((int)Position.Y - 50, (int)Position.Y + 50) - offset)),
                    transform(new Vector2(positionX - offset, Position.Y - offset)));

                var scaleX = Scale.X * barWidth / bitmap.Width;
                scaleX = (float)Math.Floor(scaleX * 10) / 10.0f;

                var hasScale = false;
                keyframes.ForEachPair(
                    (start, end) =>
                    {
                        hasScale = true;
                        bar.ScaleVec(start.Time, end.Time,
                            scaleX, start.Value,
                            scaleX, end.Value);
                    },
                    MinimalHeight,
                    s => (float)Math.Round(s, 1)
                );
                if (!hasScale) bar.ScaleVec(startTime, scaleX, MinimalHeight);
            }
        }

        private void ParticleGen(int StartTime, int EndTime, StoryboardLayer layer)
        {
            if (StartTime == EndTime)
            {
                StartTime = (int)Beatmap.HitObjects.First().StartTime;
                EndTime = (int)Beatmap.HitObjects.Last().EndTime;
            }
            EndTime = Math.Min(EndTime, (int)AudioDuration);
            StartTime = Math.Min(StartTime, EndTime);

            var pos = new Dictionary<OsbSprite, Vector2d>();

            var MinTravelTime = 10000;
            var MaxTravelTime = 20000;
            var ParticleFadeMin = 0.1;
            var ParticleFadeMax = 0.5;
            using (var pool = new OsbSpritePool(layer, "sb/particle3.png", OsbOrigin.Centre, (particle, startTime, endTime) =>
            { }))
            {
                bool RandomTravelTime = true;
                bool RandomParticleFade = true;
                for (int i = StartTime; i < (EndTime); i += 400)
                {
                    var RealTravelTime = RandomTravelTime ? Random(MinTravelTime, MaxTravelTime) : MinTravelTime;
                    var ParticleFade = RandomParticleFade ? Random(ParticleFadeMin, ParticleFadeMax) : ParticleFadeMin;
                    var particle = pool.Get(i, i + RealTravelTime);

                    particle.Color(i, ParticleColor);
                    particle.Additive(i, i + RealTravelTime);
                    
                    if (layer == GetLayer("Front"))
                    {
                        var StartScale = Random(0.05, 0.1);
                        var EndScale = Random(0.05, 0.1);
                        if (StartScale == EndScale && StartScale != 1)
                            particle.Scale(i, StartScale);
                            
                        bool RandomScale = true;
                        if (StartScale != EndScale)
                            if (RandomScale)
                                particle.Scale(i, i + RealTravelTime, Random(StartScale, EndScale), Random(StartScale, EndScale));
                            else particle.Scale(i, i + RealTravelTime, StartScale, EndScale);
                    }

                    else
                    {
                        var StartScale = Random(0.02, 0.05);
                        var EndScale = Random(0.02, 0.05);
                        if (StartScale == EndScale && StartScale != 1)
                            particle.Scale(i, StartScale);
                            
                        bool RandomScale = true;
                        if (StartScale != EndScale)
                            if (RandomScale)
                                particle.Scale(i, i + RealTravelTime, Random(StartScale, EndScale), Random(StartScale, EndScale));
                            else particle.Scale(i, i + RealTravelTime, StartScale, EndScale);
                    }

                    bool RandomMovement = true;
                    bool RandomX = true;
                    bool RandomY = true;

                    var StartPosition = new Vector2(-107, 0);
                    var EndPosition = new Vector2(747, 480);
                    if (!RandomMovement)
                    {
                        var startX = RandomX ? Random(StartPosition.X, EndPosition.X) : StartPosition.X;
                        var startY = RandomY ? Random(EndPosition.Y, StartPosition.Y) : StartPosition.Y;
                        var endX = RandomX ? startX : EndPosition.X;
                        var endY = RandomY ? startY : EndPosition.Y;
                        particle.Move(i, i + RealTravelTime, startX, startY, endX, endY);
                    }

                    else
                    {
                        var lastX = Random(StartPosition.X, EndPosition.X);
                        var lastY = Random(StartPosition.Y, EndPosition.Y);

                        var UpdateRate = 500;
                        // var lastAngle = 0d;
                        // var lastDistance = 0d;
                        
                        var TravelSpeed = 50;
                        var rVec = MathHelper.DegreesToRadians(Random(0, 360));
                        var sVec = Random(1, 16);
                        var vX = (Math.Cos(rVec) * sVec) / (TravelSpeed / 2);
                        var vY = (Math.Sin(rVec) * sVec) / (TravelSpeed / 2);

                        var startPosition = new Vector2d(lastX, lastY);
                        pos[particle] = startPosition;
                        var endPosition = new Vector2d(lastX, lastY);

                        for (var t = i; t < i + RealTravelTime; t += UpdateRate)
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
                    
                    var FadeTimeIn = 1000;
                    var FadeTimeOut = 1000;
                    if (i < EndTime - (FadeTimeIn + FadeTimeOut))
                    {
                        particle.Fade(i, i + FadeTimeIn, 0, ParticleFade);
                        if (i < EndTime - RealTravelTime)
                        {
                            particle.Fade(i + RealTravelTime - FadeTimeOut, i + RealTravelTime, ParticleFade, 0);
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

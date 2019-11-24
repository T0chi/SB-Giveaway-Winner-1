using OpenTK;
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
    public class Vignette : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            // Section 2
            NormalVignette(65553, 128262, 0.8f, 7000, 2000);
            IntenseVignette(72014, 128262, false, 20, 100, OsbEasing.InOutSine, 0.8f, 500, 2000);
        }
        
        private void NormalVignette(int StartTime, int EndTime, float Fade, int fadeDurationIn, int fadeDurationOut)
        {
            var bitmap = GetMapsetBitmap("sb/vignette.png");
            var sprite = GetLayer("").CreateSprite("sb/vignette.png", OsbOrigin.Centre);

            sprite.ScaleVec(StartTime, 854.0f / bitmap.Width, 480.0f / bitmap.Height); 
            sprite.Fade(StartTime - fadeDurationIn, StartTime, 0,Fade);
            sprite.Fade(EndTime, EndTime + fadeDurationOut, Fade, 0);
        }
        
        private void IntenseVignette(int StartTime, int EndTime, bool Shake, int Radius, int ShakeAmount, OsbEasing ShakeEasing, float Fade, int fadeDurationIn, int fadeDurationOut)
        {
            var bitmap = GetMapsetBitmap("sb/vignette2.png");
            var sprite = GetLayer("").CreateSprite("sb/vignette2.png", OsbOrigin.Centre);

            sprite.ScaleVec(StartTime, (854.0f + (Radius * 9)) / bitmap.Width, (480.0f + (Radius * 9)) / bitmap.Height); 
            sprite.Fade(StartTime - fadeDurationIn, StartTime, 0, Fade);
            sprite.Fade(EndTime, EndTime + fadeDurationOut, Fade, 0);

            if (Shake)
            {
                var angleCurrent = 0d;
                var radiusCurrent = 0;
                // ShakeAmount -> smaller number = more shaking!
                for (int i = StartTime; i < EndTime - ShakeAmount; i += ShakeAmount)
                {
                    var angle = Random(angleCurrent - Math.PI / 4, angleCurrent + Math.PI / 4);
                    var radius = Math.Abs(Random(radiusCurrent - Radius / 4, radiusCurrent + Radius / 4));

                    while (radius > Radius)
                    {
                        radius = Math.Abs(Random(radiusCurrent - Radius / 4, radiusCurrent + Radius / 4));
                    }

                    var start = sprite.PositionAt(i);
                    var end = CirclePos(angle, radius);

                    if (i + ShakeAmount >= EndTime)
                    {
                        sprite.Move(ShakeEasing, i, EndTime, start, end);
                    }
                    else
                    {
                        sprite.Move(ShakeEasing, i, i + ShakeAmount, start, end);
                    }

                    angleCurrent = angle;
                    radiusCurrent = radius;
                }
            }
        }

        public Vector2 CirclePos(double angle, int radius)
        {
            double posX = 320 + (radius * Math.Cos(angle));
            double posY = 240 + ((radius * 5) * Math.Sin(angle));

            return new Vector2((float)posX, (float)posY);
        }
    }
}

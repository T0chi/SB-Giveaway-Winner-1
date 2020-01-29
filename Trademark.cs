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
    public class Trademark : StoryboardObjectGenerator
    {
        public override void Generate()
        {
		    trademark(0);
        }

        public void trademark(int startTime)
        {
            var delay = 0;

            var bitmap = GetMapsetBitmap("sb/trademark/p.png");
            var bitmapLogo = GetMapsetBitmap("sb/trademark/tochi.png");
            var bg = GetLayer("").CreateSprite("sb/trademark/p.png", OsbOrigin.Centre, new Vector2(320, 240));
            var logo = GetLayer("").CreateSprite("sb/trademark/tochi.png", OsbOrigin.Centre);
            var logo2 = GetLayer("").CreateSprite("sb/trademark/tochi2.png", OsbOrigin.Centre);
            var mask = GetLayer("").CreateSprite("sb/trademark/p.png", OsbOrigin.Centre);
            var mask2 = GetLayer("").CreateSprite("sb/trademark/p.png", OsbOrigin.Centre);
            var star = GetLayer("").CreateSprite("sb/trademark/s.png", OsbOrigin.Centre);
            var sound = GetLayer("").CreateSample("sb/trademark/voicetag2.ogg", startTime - 10000, 90);

            bg.ScaleVec(startTime - 10000, 854.0f / bitmap.Width, 480.0f / bitmap.Height);
            bg.Fade(startTime - 10000, startTime - 9000, 0, 1);
            bg.Fade(startTime - 800, startTime, 1, 0);

            var position = new Vector2(320, 240);

            logo2.Move(startTime - delay - 7200 - delay, position);
            logo2.Color(startTime - delay - 7200 - delay, Color4.Black);
            logo2.Scale(OsbEasing.OutSine, startTime - delay - 7200, startTime - delay - 6300, 0.4f, 0.5f);
            logo2.Fade(startTime - delay - 7200, startTime - delay - 6800, 0, 0.4f);
            logo2.Fade(startTime - delay - 7200, startTime - delay - 6300, 0.4f, 0);

            logo.Move(startTime - delay - 7200, position);
            logo.Scale(OsbEasing.OutBack, startTime - delay - 7200, startTime - delay - 6300, 0.6f, 0.5f);
            logo.Fade(startTime - 7200 - delay, startTime - delay - 6300, 0, 1);
            logo.Fade(startTime - 1300, startTime - 800, 1, 0);

            // stuff
            var sTime = startTime - 9000;
            var eTime = startTime - 800;
            var stop = startTime - delay - 7200;
            for (int i = 0; i < 50; i++)
            {
                var sprite = GetLayer("particles").CreateSprite("sb/trademark/c.png", OsbOrigin.Centre);

                int radiusStart = Random(600, 854);
                int radiusEnd = Random(200, 400);
                double angle = Random(0, Math.PI*2);

                Vector2 startPos = new Vector2(
                    (float)(320 + Math.Cos(angle) * radiusStart),
                    (float)(240 + Math.Sin(angle) * radiusStart));
                Vector2 center = new Vector2(
                    (float)(320 + Math.Cos(angle) * 1),
                    (float)(240 + Math.Sin(angle) * 1));
                Vector2 endPos = new Vector2(
                    (float)(320 + Math.Cos(angle) * radiusEnd),
                    (float)(240 + Math.Sin(angle) * radiusEnd));

                var Fade = Random(0.2, 1);
                var scale = Random(0.1, 0.5);
                var d = Random(0, 2000);

                sprite.Scale(sTime - 1000, scale);
                sprite.Scale(sTime + d, stop, scale, scale * Random(1.5f, 4f));
                sprite.Scale(stop, scale);
                sprite.Color(sTime - 1000, Color4.Black);

                sprite.Move(OsbEasing.OutSine, sTime - 1000, sTime + d, startPos, center);
                sprite.Move(OsbEasing.OutElastic, stop, stop + 2000, center, endPos);

                sprite.Fade(sTime - Random(500, 1000), sTime, 0, Fade);
                sprite.Fade(startTime - 1300, startTime - 800, Fade, 0);
    
                // mask.Fade(stop + 1400, 0.8f);
                // mask.Color(stop + 1500, Color4.Black);
                mask.Rotate(stop + 1400, MathHelper.DegreesToRadians(20));
                mask.ScaleVec(stop + 1400, 30, bitmapLogo.Height * 0.5f + 30);
                mask.Move(OsbEasing.InOutExpo, stop + 1400, stop + 2900, 320 + (bitmapLogo.Width / 2.7f), 235,
                                                    320 - (bitmapLogo.Width / 2.7f), 235);

                // mask2.Fade(stop + 2500, 0.8f);
                // mask2.Color(stop + 1500, Color4.Black);
                mask2.Rotate(stop + 2500, MathHelper.DegreesToRadians(100));
                mask2.ScaleVec(stop + 2500, 30, bitmapLogo.Width * 0.5f + 30);
                mask2.Move(OsbEasing.InOutExpo, stop + 2500, stop + 4000, 320, 235 - (bitmapLogo.Height * 0.5f),
                                                    320, 235 + (bitmapLogo.Height * 0.5f));
                
                star.Scale(stop + 3200, 0.5f);
                star.Move(stop + 3200, 380, 230);
                star.Fade(stop + 3000, stop + 3500, 0, 1);
                star.Fade(stop + 6000, stop + 6500, 1, 0);
                star.Rotate(stop + 3200, stop + 6500, MathHelper.DegreesToRadians(10), MathHelper.DegreesToRadians(30));
            }
        }
    }
}

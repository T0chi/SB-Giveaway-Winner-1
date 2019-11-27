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
    public class Section7 : StoryboardObjectGenerator
    {
        [Configurable]
        public Color4 OHColor = Color4.White;

        public override void Generate()
        {
            Dialog();
            Lyrics();
            Lyrics2();
            Avatar();
            SwingingBars(311006, 331111);
            ObjectHighlight(311006, 331111);
            Blank(303310, 307237);
            Tochi(281629, 290425);
            SnowChaos(290896, 301425, 1000);
            SnowChaos(303310, 311006, 500);
            HUD(283671, 329854, 290901, "Mission #6", "Strahv", "sb/HUD/txt/nameTag/Heilia.png", 4500, "sb/avatars/HeiliaProfile.png");
        }

        public void Blank(int startTime, int endTime)
        {
            // Data for the backgrounds are included in the OsbImport.cs

            var bitmap = GetMapsetBitmap("sb/pixel.png");
            var sprite = GetLayer("Blank").CreateSprite("sb/bgs/7/bg.jpg", OsbOrigin.Centre, new Vector2(320, 240));

            sprite.Scale(startTime, 480.0f / bitmap.Height);
            sprite.Fade(startTime, endTime, 0.2, 0);
            sprite.Additive(startTime, endTime);
        }

        public void HUD(int startTime, int endTime, int loadingTextEndtime, string mission, string songName, string nameTag, int progressBarDelay, string avatar)
        {
            var hud = new HUD(this, startTime, endTime, loadingTextEndtime, mission, songName, nameTag, progressBarDelay, avatar);
        }

        public void SwingingBars(int startTime, int endTime)
        {
            var Width = 700;
            var yOffset = 20;
            var barCount = 21;
            var distance = Width / barCount;
            var barScale = new Vector2(0.2f, 0.2f);
            var Beat = Beatmap.GetTimingPointAt(startTime).BeatDuration * 4;

            for (var i = 0; i < barCount; i++)
            {
                var position = new Vector2((340 - (Width / 2)) + i * distance, 260);
                var bar = GetLayer("Swinging Bars").CreateSprite("sb/bar" + Random(1, 3) + ".png", OsbOrigin.Centre);
                var barFill = GetLayer("Swinging Bars").CreateSprite("sb/bar1.png", OsbOrigin.Centre);
                
                bar.MoveX(startTime, position.X);
                bar.Fade(startTime, startTime + 500, 0, 0.2f);
                bar.Fade(endTime, endTime + 2000, 0.2f, 0);

                barFill.MoveX(startTime, position.X);
                barFill.Color(startTime, "#EC4E4E");

                var rotateStart = MathHelper.DegreesToRadians(Random(-180 * 6, -90 * 6));
                var rotateEnd = MathHelper.DegreesToRadians(Random(90 * 6, 180 * 6));
                bar.Rotate(startTime - 1000, endTime + 2000, rotateStart, rotateEnd);
                barFill.Rotate(startTime - 1000, endTime + 2000, rotateStart, rotateEnd);

                // loop stuff
                var delay = 200;
                var delay2 = 80;
                var randomBeatDelay = Random(Beat / 2, Beat);
                // moveY
                for (float l = startTime - (delay * barCount); l < endTime - (delay * barCount); l += ((float)Beat * 2))
                {
                    bar.MoveY(OsbEasing.InOutSine, l + delay2 * i, l + Beat + delay2 * i, position.Y - yOffset, position.Y + yOffset);
                    bar.MoveY(OsbEasing.InOutSine, l + Beat + delay2 * i, l + (Beat * 2) + delay2 * i, position.Y + yOffset, position.Y - yOffset);

                    barFill.MoveY(OsbEasing.InOutSine, l + delay2 * i, l + Beat + delay2 * i, position.Y - yOffset, position.Y + yOffset);
                    barFill.MoveY(OsbEasing.InOutSine, l + Beat + delay2 * i, l + (Beat * 2) + delay2 * i, position.Y + yOffset, position.Y - yOffset);
                }
                // scaleVec
                for (float l = startTime - (delay * barCount); l < endTime; l += ((float)Beat * 2))
                {
                    bar.ScaleVec(OsbEasing.InOutSine, l + delay * i, l + randomBeatDelay + delay * i, barScale.X, barScale.Y, -barScale.X, barScale.Y);
                    bar.ScaleVec(OsbEasing.InOutSine, l + randomBeatDelay + delay * i, l + (Beat * 2) + delay * i, barScale.X, barScale.Y, -barScale.X, barScale.Y);
                    
                    barFill.ScaleVec(OsbEasing.InOutSine, l + delay * i, l + randomBeatDelay + delay * i, barScale.X, barScale.Y, -barScale.X, barScale.Y);
                    barFill.ScaleVec(OsbEasing.InOutSine, l + randomBeatDelay + delay * i, l + (Beat * 2) + delay * i, barScale.X, barScale.Y, -barScale.X, barScale.Y);
                }

                // bars falling apart at endTime
                var r = Random(0, 600);
                bar.MoveY(OsbEasing.In, endTime + r, endTime + r + (barCount * delay2), bar.PositionAt(endTime).Y, Random(480, 520));
                bar.MoveX(endTime + r, endTime + r + (barCount * delay2), bar.PositionAt(endTime).X, Random(300, 340));

                // bar filling appearing at
                var delay3 = barCount / 2;
                for (float f = 315404; f < 325456; f += ((float)Beat) * 4)
                {
                    barFill.Fade(f - 100 + delay3 * i, f + delay3 * i, 0, 0.5f);
                    barFill.Fade(f + 300 + delay3 * i, f + 500 + delay3 * i, 0.5f, 0);
                    barFill.Additive(f + delay3 * i, f + 500 + delay3 * i);
                }
            }
        }

        public void ObjectHighlight(int startTime, int endTime)
        {
            foreach (var hitobject in Beatmap.HitObjects)
            {
                if ((startTime != 0 || endTime != 0) && 
                    (hitobject.StartTime < startTime - 5 || endTime - 5 <= hitobject.StartTime))
                    continue;

                var ScaleIn = 0.5;
                var Fade = Random(0.05, 0.15);
                var ScaleOut = Random(0, 0.1);
                var delay = Random(500, 1000);
                var delay2 = Random(0, 1000);
                var Rotation = Random(-100, 100);
                var sprite = GetLayer("ObjectHighlight").CreateSprite("sb/missions/7/h" + Random(1, 3) + ".png", OsbOrigin.Centre, hitobject.Position);

                sprite.Scale(OsbEasing.In, hitobject.StartTime, endTime + delay + delay2, ScaleIn, ScaleOut);
                sprite.Rotate(hitobject.EndTime, endTime + delay + delay2, Rotation, Rotation + Random(-1, 1));
                sprite.Fade(endTime, endTime + delay + delay2, Fade, 0);
                sprite.Additive(hitobject.StartTime, endTime + delay + delay2);
                sprite.Color(hitobject.StartTime, OHColor);

                sprite.MoveX(OsbEasing.InOutQuart, hitobject.StartTime, endTime + delay + delay2, hitobject.PositionAtTime(endTime).X, Random(310, 330));
                sprite.MoveY(OsbEasing.InOutSine, endTime, endTime + delay + delay2, hitobject.PositionAtTime(endTime).Y, 550);

                if (hitobject is OsuSlider)
                {
                    var timestep = Beatmap.GetTimingPointAt((int)hitobject.StartTime).BeatDuration / 16;
                    var sTime = hitobject.StartTime;
                    while (true)
                    {
                        var eTime = sTime + timestep;

                        var complete = hitobject.EndTime - eTime < 5;
                        if (complete) eTime = hitobject.EndTime;
                        
                        var startPosition = sprite.PositionAt(sTime);
                        sprite.MoveX(sTime, eTime, startPosition.X, hitobject.PositionAtTime(eTime).X);
                        sprite.MoveY(sTime, eTime, startPosition.Y, hitobject.PositionAtTime(hitobject.EndTime).Y);
                        if (hitobject.PositionAtTime(hitobject.EndTime).X <= 320)
                        {
                            sprite.MoveX(OsbEasing.InOutQuart, endTime, endTime + delay + delay2, hitobject.PositionAtTime(endTime).X, Random(300, 340));
                            sprite.MoveY(OsbEasing.InOutSine, endTime, endTime + delay + delay2, hitobject.PositionAtTime(endTime).Y, 550);
                        }
                        else if (hitobject.PositionAtTime(hitobject.EndTime).X >= 320)
                        {
                            sprite.MoveX(OsbEasing.InOutQuart, endTime, endTime + delay + delay2, hitobject.PositionAtTime(endTime).X, Random(300, 340));
                            sprite.MoveY(OsbEasing.InOutSine, endTime, endTime + delay + delay2, hitobject.PositionAtTime(endTime).Y, 550);
                        }

                        if (complete) break;
                        sTime += timestep;
                    }
                }
            }
        }

        public void Avatar()
        {
            // Avatar
            var sTime = 289482;
            var eTime = 311006;
            var avatarPos = new Vector2(550, 140);

            var avatar = GetLayer("Avatar").CreateSprite("sb/avatars/Heilia.png", OsbOrigin.TopCentre);

            avatar.Scale(sTime, 0.6);
            avatar.MoveX(sTime, avatarPos.X);
            avatar.Fade(sTime, 290901, 0, 0.8f);
            avatar.Fade(eTime - 1000, eTime, 0.8f, 0);
            
            // loop y
            var d = 4000;
            for (int a = sTime; a < eTime; a += d)
            {
                avatar.MoveY(OsbEasing.InOutSine, a, a + (d / 2), avatarPos.Y, avatarPos.Y + 20);
                avatar.MoveY(OsbEasing.InOutSine, a + (d / 2), a + d, avatarPos.Y + 20, avatarPos.Y);
            }
        }

        public void Lyrics()
        {
            var fontSize = 17;
            var GlowRadius = 0;
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            var fontName = "RUNE.ttf";
            var font = LoadFont("sb/missions/7/lyrics", new FontDescription()
            {
                FontPath = fontName,
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

            // this is where you generate the lyrics
            CreateLyrics(font, Color4.White, "Ha", fontName, fontSize, 0.5f, new Vector2(320, 340), 290582, 292001);
            CreateLyrics(font, Color4.White, "mell guiez arl", fontName, fontSize, 0.5f, new Vector2(320, 330), 292158, 294828);

            CreateLyrics(font, Color4.White, "Edzari", fontName, fontSize, 0.5f, new Vector2(320, 330), 295614, 296870);
            CreateLyrics(font, Color4.White, "nuilgizz fead", fontName, fontSize, 0.5f, new Vector2(320, 330), 297184, 300169);

            CreateLyrics(font, Color4.White, "Ha", fontName, fontSize, 0.5f, new Vector2(320, 330), 300640, 302053);
            CreateLyrics(font, Color4.White, "mell guiez Lell", fontName, fontSize, 0.5f, new Vector2(320, 330), 302210, 304723);

            CreateLyrics(font, Color4.White, "Ver mid ze", fontName, fontSize, 0.5f, new Vector2(320, 330), 305666, 306844);
            CreateLyrics(font, Color4.White, "en re [Strahv]", fontName, fontSize, 0.5f, new Vector2(320, 330), 307158, 310849);
        }

        public void Lyrics2()
        {
            var fontSize = 17;
            var GlowRadius = 0;
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            var fontName = "msyi.ttf";
            var font = LoadFont("sb/missions/7/lyrics/2", new FontDescription()
            {
                FontPath = fontName,
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

            var yOffset = 20;

            // this is where you generate the lyrics
            CreateLyrics(font, Color4.White, "Ha", fontName, fontSize, 0.5f, new Vector2(320, 340 + yOffset), 290582, 292001);
            CreateLyrics(font, Color4.White, "mell guiez arl", fontName, fontSize, 0.5f, new Vector2(320, 330 + yOffset), 292158, 294828);

            CreateLyrics(font, Color4.White, "Edzari", fontName, fontSize, 0.5f, new Vector2(320, 330 + yOffset), 295614, 296870);
            CreateLyrics(font, Color4.White, "nuilgizz fead", fontName, fontSize, 0.5f, new Vector2(320, 330 + yOffset), 297184, 300169);

            CreateLyrics(font, Color4.White, "Ha", fontName, fontSize, 0.5f, new Vector2(320, 330 + yOffset), 300640, 302053);
            CreateLyrics(font, Color4.White, "mell guiez Lell", fontName, fontSize, 0.5f, new Vector2(320, 330 + yOffset), 302210, 304723);

            CreateLyrics(font, Color4.White, "Ver mid ze", fontName, fontSize, 0.5f, new Vector2(320, 330 + yOffset), 305666, 306844);
            CreateLyrics(font, Color4.White, "en re [Strahv]", fontName, fontSize, 0.5f, new Vector2(320, 330 + yOffset), 307158, 310849);
        }

        private void CreateLyrics(FontGenerator font, Color4 ColorType, string Sentence, string FontName, int FontSize, float FontScale, Vector2 position, int StartTime, int EndTime)
        {
            var LetterSpacing = 5;
            var LyricsLayer = GetLayer("Lyrics");
            var letterY = position.Y;
            var lineWidth = 0f;
            var lineHeight = 0f;
            var letterSpacing = LetterSpacing * FontScale;
            foreach (var letter in Sentence)
            {
                var texture = font.GetTexture(letter.ToString());
                lineWidth += texture.BaseWidth * FontScale + letterSpacing;
                lineHeight = Math.Max(lineHeight, texture.BaseHeight * FontScale);
            }

            var letterX = position.X - lineWidth * 0.5f;

            var timePerLetter = 0;
            var i = 0;
            foreach (var letter in Sentence)
            {
                var texture = font.GetTexture(letter.ToString());
                if (!texture.IsEmpty)
                {
                    var FadeTime = 200;
                    var RandomRotate = Random(MathHelper.DegreesToRadians(-20), MathHelper.DegreesToRadians(20));
                    var letterPos = new Vector2(letterX, letterY)
                        + texture.OffsetFor(OsbOrigin.Centre) * FontScale;

                    var sprite = LyricsLayer.CreateSprite(texture.Path, OsbOrigin.Centre);

                    var preDelay = 100;
                    sprite.MoveX(OsbEasing.OutQuad, StartTime - 1000 + timePerLetter * i, StartTime - preDelay + timePerLetter * i, 610, letterPos.X);
                    sprite.MoveY(OsbEasing.Out, StartTime - 1000 + timePerLetter * i, StartTime - preDelay + timePerLetter * i, Random(330, 415), letterPos.Y);
                    sprite.Fade(StartTime - 1000 + timePerLetter * i, StartTime - 500 + timePerLetter * i, 0, 0.4);
                    sprite.Fade(EndTime - preDelay - (FadeTime * 1.5) + timePerLetter * i, EndTime - preDelay + timePerLetter * i, 0.4, 0);
                    sprite.Scale(StartTime - preDelay, FontScale);

                    sprite.Color(StartTime - preDelay, ColorType);
                    i++;
                }
                letterX += texture.BaseWidth * FontScale + letterSpacing;
            }
            letterY += lineHeight;
        }

        public void SnowChaos(int startTime, int endTime, int FadeTime)
        {
            // var mission = new Mission3(this, startTime, endTime);

            // Snow chaos
            var interval = 40;
            var MinTravelTime = 2000;
            var MaxTravelTime = 4000;

            for (int i = startTime; i < endTime - MinTravelTime; i += interval)
            {
                var sprite = GetLayer("SnowChaos").CreateSprite("sb/missions/7/snow" + Random(1, 3) + ".png", OsbOrigin.Centre);
                var duration = Random(MinTravelTime, MaxTravelTime);
                var Fade = Random(0.05, 0.2);
                var Rotation = Random(-10, 10);
                var Scale = Random(0.01, 0.3);
                var RandomScale = Random(Scale / 2, Scale);

                // sprite.Fade(i, i + 1000, 0, Fade);
                // sprite.Fade(i + duration - 1000, i + duration, Fade, 0);
                sprite.ScaleVec(i, i + duration, RandomScale, RandomScale, Random(0, Scale / 6), Random(0, Scale / 6));
                // sprite.Additive(i, i + duration);

                if (i < endTime - (FadeTime + FadeTime))
                {
                    sprite.Fade(i, i + FadeTime, 0, Fade);
                    if (i < endTime - duration)
                    {
                        sprite.Fade(i + duration - FadeTime, i + duration, Fade, 0);
                    }
                    else
                    {
                        sprite.Fade(endTime - 10, endTime, Fade, 0);
                    }
                }
                else
                {
                    sprite.Fade(i, 0);
                }

                var speed = 1000;
                for (int r = i; r < endTime - MinTravelTime; r += speed)
                {
                    sprite.Rotate(r, r + (speed / 2), Rotation / 2, Rotation);
                    sprite.Rotate(r + (speed / 2), r + speed, Rotation, Rotation);
                }

                var lastX = Random(-107, 747) - 150;
                var lastY = Random(0, 480) - 150;
                var speedMin = 100;
                var speedMax = 200;

                var rVec = MathHelper.DegreesToRadians(Random(360));
                var sVec = Random(speedMin, speedMax);
                var vX = Math.Cos(rVec) * sVec;
                var vY = Math.Sin(rVec) * sVec;

                var UpdateRate = 1000;
                for (var t = i; t < i + duration; t += UpdateRate)
                {
                    var nextX = lastX + vX;
                    var nextY = lastY + vY;
                    Log(vX.ToString());

                    sprite.Move(t, t + UpdateRate, lastX, lastY, nextX, nextY);

                    vX += Random(UpdateRate / 10) * UpdateRate / 1000;
                    vY += Random(UpdateRate / 10) * UpdateRate / 1000;

                    lastX = (int)nextX;
                    lastY = (int)nextY;
                }
            }
        }

        public void Dialog()
        {
            // DIALOG BOXES STARTS HERE
            var fontSize = 15;
            var GlowRadius = 0;
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            var font = LoadFont("sb/dialog/txt/2", new FontDescription()
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


            // DIALOG 1 -----------------------------------------
            string[] sentence = { "OK.",
                                  "The next quest is very simple.", 
                                  "It's about lyrics..." };
            var dialog = new DialogManager(this, font, 281629, 285399, "-Tochi", 105, 326, false,
                fontSize, 1, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.wav",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "Your Avatar will help you with this.",
                                   "Be consistent and the lyrics will be delivered correctly on time." };
            var dialog2 = new DialogManager(this, font, 285399, 290425, "-Tochi", 105, 326, false,
                fontSize, 1, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.wav",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence2);
        }

        public void Tochi(int startTime, int endTime)
        {
            var Hoveduration = 5000;
            var loopCount = (endTime - startTime) / Hoveduration;
            var pos = new Vector2(320, 240);
            var avatar = GetLayer("-Tochi").CreateSprite("sb/avatars/-TochiProfile.png", OsbOrigin.Centre);
            var ring = GetLayer("-Tochi").CreateSprite("sb/ring2.png", OsbOrigin.Centre);

            avatar.MoveX(startTime, 64);
            avatar.Scale(startTime, 0.3);
            avatar.Fade(startTime, startTime + 500, 0, 1);
            avatar.Fade(endTime - 500, endTime, 1, 0);

            avatar.StartLoopGroup(startTime, loopCount + 1);
            avatar.MoveY(OsbEasing.InOutSine, 0, Hoveduration / 2, 335, 345);
            avatar.MoveY(OsbEasing.InOutSine, Hoveduration / 2, Hoveduration, 345, 335);
            avatar.EndGroup();

            ring.MoveX(startTime, 64);
            ring.Scale(startTime, 0.3);
            ring.Fade(startTime, startTime + 500, 0, 1);
            ring.Fade(endTime - 500, endTime, 1, 0);
            var rotation = MathHelper.DegreesToRadians(180);
            ring.Rotate(startTime, endTime, -rotation, rotation);

            ring.StartLoopGroup(startTime, loopCount + 1);
            ring.MoveY(OsbEasing.InOutSine, 0, Hoveduration / 2, 335, 345);
            ring.MoveY(OsbEasing.InOutSine, Hoveduration / 2, Hoveduration, 345, 335);
            ring.EndGroup();
        }
    }
}

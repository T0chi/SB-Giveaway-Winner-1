using OpenTK;
using System;
using System.Drawing;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    // public enum Pointer { None, Up, Down, CentreLeft, TopLeft, BottomLeft, CentreRight, TopRight, BottomRight };
    public class Intro : StoryboardObjectGenerator
    {
        [Configurable]
        public Color4 ActiveColor = Color4.White;

        [Configurable]
        public Color4 PassiveColor = Color4.DarkGray;

        [Configurable]
        public Color4 TochiTagColor = Color4.Cyan;

        [Configurable]
        public Color4 BoxColorText = new Color4(255, 255, 255, 255);

        [Configurable]
        public Color4 BoxColorNarratorText = new Color4(255, 255, 255, 255);

        [Configurable]
        public Color4 BoxColorTochi = new Color4(255, 255, 255, 255);

        [Configurable]
        public Color4 BoxColorOthers = new Color4(255, 255, 255, 255);

        [Configurable]
        public Color4 BoxColorNarrator = new Color4(255, 255, 255, 255);

        [Configurable]
        public Color4 TorchGlowColor = new Color4(255, 255, 255, 255);

        [Configurable]
        public Color4 GlowColor = new Color4(255, 255, 255, 255);

        [Configurable]
        public Color4 OutlineColor = new Color4(50, 50, 50, 200);

        [Configurable]
        public Color4 ShadowColor = new Color4(0, 0, 0, 200);

        public override void Generate()
        {
            Avatars();
            Background();
            CyanRain(0.3f, "RainFront", Random(5000, 10000), 0.15f, (float)Random(0.2f, 0.5f));
            CyanRain(0.2f, "RainBack", Random(500, 1000), 0.1f, (float)Random(0.1f, 0.5f));
            Dialog();
            DialogNarrator();
            Torch();
            Credits();
        }

        public void Torch()
        {
            var startTime = 39245;
            var endTime = 61860;
            var movedDownAt = 40630 - startTime;

            var WidthTorch = 250;
            var WidthFlame = 270;
            var torchPosY = 315;
            var flamePosY = 270;
            var startTorchX = 320 + (WidthTorch / 2);
            var endTorchX = 320 - (WidthTorch / 2);
            var startFlameX = 320 + (WidthFlame / 2);
            var endFlameX = 320 - (WidthFlame / 2);
            var interval = 2000;
            var duration = 10000;
            var d = duration / 3f;

            var moveTorchStart = transformTorches(startTorchX, torchPosY);
            var moveTorchEnd = transformTorches(endTorchX, torchPosY);
            var moveFlameStart = transformTorches(startFlameX, flamePosY);
            var moveFlameEnd = transformTorches(endFlameX, flamePosY);

            for (var i = startTime - duration; i < endTime; i += interval)
            {
                var torchFront = GetLayer("Torch - Torches Front").CreateSprite("sb/torch/torch.png", OsbOrigin.BottomCentre);
                var torchBack = GetLayer("Torch - Torches Back").CreateSprite("sb/torch/torch.png", OsbOrigin.BottomCentre);
                var flameFront = GetLayer("Torch - Flames Front").CreateAnimation("sb/torch/flame/flame.jpg", 17, 100, OsbLoopType.LoopForever, OsbOrigin.BottomCentre);
                var flameBack = GetLayer("Torch - Flames Back").CreateAnimation("sb/torch/flame/flame.jpg", 17, 100, OsbLoopType.LoopForever, OsbOrigin.BottomCentre);
                
                var scaleStartX = 0.4;
                var scaleEndX = 0;
                var scaleStartY = 0.3;
                var scaleEndY = 0.2;
                var startRotation = MathHelper.DegreesToRadians(25);
                var endRotation = MathHelper.DegreesToRadians(-25);

                var sTime = i + movedDownAt;
                var loopCount = ((endTime - sTime) / duration);

                // loop front
                torchFront.StartLoopGroup(i - d, loopCount + 2);
                torchFront.MoveX(OsbEasing.InOutSine, 0, duration, moveTorchStart.X, moveTorchEnd.X);
                torchFront.Rotate(OsbEasing.In, 0, duration / 2, startRotation, 0);
                torchFront.Rotate(OsbEasing.Out, duration / 2, duration, 0, endRotation);
                torchFront.ScaleVec(OsbEasing.Out, 0, duration / 2, scaleEndX, scaleEndY, scaleStartX, scaleStartY);
                torchFront.ScaleVec(OsbEasing.In, duration / 2, duration, scaleStartX, scaleStartY, scaleEndX, scaleEndY);
                torchFront.MoveY(OsbEasing.Out, 0, duration / 2, moveTorchStart.Y - 40, moveTorchStart.Y);
                torchFront.MoveY(OsbEasing.In, duration / 2, duration, moveTorchStart.Y, moveTorchStart.Y - 40);
                torchFront.EndGroup();

                flameFront.StartLoopGroup(i - d, loopCount + 2);
                flameFront.MoveX(OsbEasing.InOutSine, 0, duration, moveFlameStart.X + 15, moveFlameEnd.X - 15);
                flameFront.Rotate(OsbEasing.In, 0, duration / 2, startRotation, 0);
                flameFront.Rotate(OsbEasing.Out, duration / 2, duration, 0, endRotation);
                flameFront.ScaleVec(OsbEasing.Out, 0, duration / 2, scaleEndX, scaleEndY, scaleStartX, scaleStartY);
                flameFront.ScaleVec(OsbEasing.In, duration / 2, duration, scaleStartX, scaleStartY, scaleEndX, scaleEndY);
                flameFront.MoveY(OsbEasing.Out, 0, duration / 2, moveFlameStart.Y - 45, moveFlameStart.Y - 35);
                flameFront.MoveY(OsbEasing.In, duration / 2, duration, moveFlameStart.Y - 35, moveFlameStart.Y - 45);
                flameFront.EndGroup();

                // loop back
                var s = 1.8f;
                torchBack.StartLoopGroup(i + duration - d, loopCount + 2);
                torchBack.MoveX(OsbEasing.InOutSine, 0, duration, moveTorchEnd.X, moveTorchStart.X);
                torchBack.Rotate(OsbEasing.Out, 0, duration / 2, endRotation, 0);
                torchBack.Rotate(OsbEasing.In, duration / 2, duration, 0, startRotation);
                torchBack.ScaleVec(OsbEasing.Out, 0, duration / 2, scaleEndX / s, scaleEndY / s, scaleStartX / s, scaleStartY / s);
                torchBack.ScaleVec(OsbEasing.In, duration / 2, duration, scaleStartX / s, scaleStartY / s, scaleEndX / s, scaleEndY / s);
                torchBack.MoveY(OsbEasing.InOutSine, 0, duration / 2, moveTorchStart.Y - 60, moveTorchStart.Y - 70);
                torchBack.MoveY(OsbEasing.InOutSine, duration / 2, duration, moveTorchStart.Y - 70, moveTorchStart.Y - 60);
                torchBack.EndGroup();

                flameBack.StartLoopGroup(i + duration - d, loopCount + 2);
                flameBack.MoveX(OsbEasing.InOutSine, 0, duration, moveFlameEnd.X, moveFlameStart.X);
                flameBack.Rotate(OsbEasing.Out, 0, duration / 2, endRotation, 0);
                flameBack.Rotate(OsbEasing.In, duration / 2, duration, 0, startRotation);
                flameBack.ScaleVec(OsbEasing.Out, 0, duration / 2, scaleEndX / s, scaleEndY / s, scaleStartX / s, scaleStartY / s);
                flameBack.ScaleVec(OsbEasing.In, duration / 2, duration, scaleStartX / s, scaleStartY / s, scaleEndX / s, scaleEndY / s);
                flameBack.MoveY(OsbEasing.InOutSine, 0, duration / 2, moveFlameStart.Y - 50, moveFlameStart.Y - 70);
                flameBack.MoveY(OsbEasing.InOutSine, duration / 2, duration, moveFlameStart.Y - 70, moveFlameStart.Y - 50);
                flameBack.EndGroup();

                // parameters
                torchFront.Fade(startTime - duration - d, startTime - d, 0, 0);
                torchFront.Fade(startTime - d, startTime, 0, 1);
                torchFront.Fade(endTime - d, endTime, 1, 0);
                torchBack.Fade(startTime - duration - d, startTime - d, 0, 0);
                torchBack.Fade(startTime - d, startTime, 0, 1);
                torchBack.Fade(endTime - d, endTime, 1, 0);

                flameFront.Fade(startTime - duration - d, startTime - d, 0, 0);
                flameFront.Fade(startTime - d, startTime, 0, 1);
                flameFront.Fade(endTime - d, endTime, 1, 0);
                flameFront.Additive(startTime - duration - d, endTime);
                flameBack.Fade(startTime - duration - d, startTime - d, 0, 0);
                flameBack.Fade(startTime - d, startTime, 0, 1);
                flameBack.Fade(endTime - d, endTime, 1, 0);
                flameBack.Additive(startTime - duration - d, endTime);
            }

            // lightray
            for (int l = 0; l < 2; l++)
            {
                var sprite = GetLayer("Torches - Lightray").CreateSprite("sb/light.png", OsbOrigin.CentreLeft);
                var rotateStart = MathHelper.DegreesToRadians(Random(80, 100));
                var rotateEnd = MathHelper.DegreesToRadians(Random(75, 115));
                var RandomDuration = Random(4000, 7000);
                var LoopCount = (endTime - (startTime - (d / 1.5))) / (RandomDuration * 2);

                sprite.StartLoopGroup(startTime - (d / 1.5), (int)LoopCount);
                sprite.Rotate(0, RandomDuration, rotateStart, rotateEnd);
                sprite.Rotate(RandomDuration, RandomDuration * 2, rotateEnd, rotateStart);
                sprite.EndGroup();

                var Fade = Random(0.3f, 0.6f);
                sprite.StartLoopGroup(startTime - (d / 1.5), (int)LoopCount);
                sprite.Move(0, new Vector2(Random(300, 340), -50));
                sprite.Fade(0, 1500, 0, Fade);
                sprite.Fade(1500, (RandomDuration * 2) - 1500, Fade, Fade);
                sprite.Fade((RandomDuration * 2) - 1500, RandomDuration * 2, Fade, 0);
                sprite.EndGroup();

                sprite.Scale(startTime - (d / 1.5), 0.8);
                sprite.Additive(startTime - (d / 1.5), endTime);
            }

            var pDuration = Random(1000, 8000);
            for (float p = startTime - d; p < endTime; p += 250)
            {
                var loopCount = (p + duration - p) / pDuration;
                var sprite = GetLayer("Torches - Glow").CreateSprite("sb/particle2.png", OsbOrigin.Centre);

                var pad = Random(-50, 50);
                var RandomFade = Random(0.4, 0.8);
                var MoveX = Random(200, 480);
                var MoveY = new Vector2(Random(140, 220), Random(0, 100));

                sprite.StartLoopGroup(p, (int)loopCount);
                sprite.MoveX(OsbEasing.InOutSine, 0, duration / 2, MoveX, MoveX + pad);
                sprite.MoveX(OsbEasing.InOutSine, duration / 2, duration, MoveX + pad, MoveX);
                sprite.EndGroup();

                sprite.Color(p, TorchGlowColor);
                sprite.Scale(p, Random(0.01, 0.04));
                sprite.Additive(p, p + duration);
                sprite.MoveY(p, p + duration, MoveY.X, MoveY.Y);

                sprite.Fade(p, p + (duration / 2), 0, RandomFade);
                if (p + duration > endTime)
                {
                    sprite.Fade(endTime - Random(500, 1500), p, RandomFade, 0);
                }
                else
                {
                    sprite.Fade(p + (duration / 2), p + duration, RandomFade, 0);
                }
            }
        }

        private Vector2 transformTorches(float posX, float posY)
        {
            float angle = 0;
            double Rotation = angle / 180 * Math.PI;
            return Vector2.Transform(new Vector2(posX, posY),
                                    Quaternion.FromEulerAngles((float)(Rotation), 0, 0));
        }

        public void Credits()
        {
            var startTime = 37860;
            var endTime = 61399;
            var duration = (endTime - startTime) / 4;
            var logo = GetLayer("Credits").CreateSprite("sb/credits/logo.png", OsbOrigin.BottomCentre);
            var credits1 = GetLayer("Credits").CreateSprite("sb/credits/host.png", OsbOrigin.TopCentre);
            var credits2 = GetLayer("Credits").CreateSprite("sb/credits/sb.png", OsbOrigin.TopCentre);
            var credits3 = GetLayer("Credits").CreateSprite("sb/credits/length.png", OsbOrigin.TopCentre);
            var credits4 = GetLayer("Credits").CreateSprite("sb/credits/cv.png", OsbOrigin.TopCentre);

            logo.Scale(startTime, 0.35);
            logo.MoveX(startTime, 320);
            logo.Fade(startTime, startTime + 1000, 0, 1);
            logo.Fade(endTime - 1000, endTime, 1, 0);
            logo.MoveY(startTime, 90);

            credits1.MoveX(startTime, 320);
            credits1.Fade(startTime, startTime + 1000, 0, 1);
            credits1.Fade(startTime + duration - 1000, startTime + duration, 1, 0);
            credits1.ScaleVec(OsbEasing.OutElasticQuarter, startTime, startTime + 2000, 0.4, 0, 0.3, 0.3);
            credits1.MoveY(OsbEasing.In, startTime, startTime + duration, 100, 230);

            credits2.MoveX(startTime + duration, 320);
            credits2.Fade(startTime + duration, startTime + duration + 1000, 0, 1);
            credits2.Fade(startTime + (duration * 2) - 1000, startTime + (duration * 2), 1, 0);
            credits2.ScaleVec(OsbEasing.OutElasticQuarter, startTime + duration, startTime + duration + 2000, 0.4, 0, 0.3, 0.3);
            credits2.MoveY(OsbEasing.In, startTime + duration, startTime + (duration * 2), 100, 230);

            credits3.MoveX(startTime + (duration * 2), 320);
            credits3.Fade(startTime + (duration * 2), startTime + (duration * 2) + 1000, 0, 1);
            credits3.Fade(startTime + (duration * 3) - 1000, startTime + (duration * 3), 1, 0);
            credits3.ScaleVec(OsbEasing.OutElasticQuarter, startTime + (duration * 2), startTime + (duration * 2) + 2000, 0.4, 0, 0.3, 0.3);
            credits3.MoveY(OsbEasing.In, startTime + (duration * 2), startTime + (duration * 3), 100, 230);

            credits4.MoveX(startTime + (duration * 3), 320);
            credits4.Fade(startTime + (duration * 3), startTime + (duration * 3) + 1000, 0, 1);
            credits4.Fade(startTime + (duration * 4) - 1000, startTime + (duration * 4), 1, 0);
            credits4.ScaleVec(OsbEasing.OutElasticQuarter, startTime + (duration * 3), startTime + (duration * 3) + 2000, 0.4, 0, 0.3, 0.3);
            credits4.MoveY(OsbEasing.In, startTime + (duration * 3), startTime + (duration * 4), 100, 230);
        }

        public void Dialog()
        {
            // DIALOG BOXES STARTS HERE
            var fontSize = 20;
            var GlowRadius = 0;
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            var font = LoadFont("sb/dialog/txt", new FontDescription()
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
                Color = GlowColor,
            },
            new FontOutline()
            {
                Thickness = OutlineThickness,
                Color = OutlineColor,
            },
            new FontShadow()
            {
                Thickness = ShadowThickness,
                Color = ShadowColor,
            });

            
            // DIALOG 1 -----------------------------------------
            string[] sentence = { "Hello and welcome!",
                                  "I have assembled a team for you",
                                  "that will help you complete this map!" };
            var dialog = new DialogManager(this, font, 3245, 10168, "Dialog - Text", 150, 190, true,
                fontSize, 1, BoxColorText, true, 0.7f, BoxColorTochi, "Dialog - Box", 0, "sb/sfx/message-1.wav",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.Right, sentence);
            
            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "Now, have a look!" };
            var dialog2 = new DialogManager(this, font, 10630, 13399, "Dialog - Text", 195, 190, true,
                fontSize, 1, BoxColorText, true, 0.7f, BoxColorTochi, "Dialog - Box", 0, "sb/sfx/message-1.wav",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.Right, sentence2);
            
            // DIALOG 3 -----------------------------------------
            string[] sentence3 = { "This is your team!",
                                   "Refer to them as 'mappers'" };
            var dialog3 = new DialogManager(this, font, 18476, 25399, "Dialog - Text", 390, 170, true,
                fontSize, 1, BoxColorText, true, 0.7f, BoxColorTochi, "Dialog - Box", 0, "sb/sfx/message-1.wav",
                DialogBoxes.Pointer.CentreLeft, DialogBoxes.Push.Left, sentence3);
            
            // DIALOG 4 -----------------------------------------
            string[] sentence4 = { "All you need to do is to perform well on",
                                   "each section. That's the only way you will",
                                   "be able to complete the mission!" };
            var dialog4 = new DialogManager(this, font, 26322, 37168, "Dialog - Text", 400, 170, true,
                fontSize, 1, BoxColorText, true, 0.7f, BoxColorTochi, "Dialog - Box", 0, "sb/sfx/message-1.wav",
                DialogBoxes.Pointer.CentreLeft, DialogBoxes.Push.Left, sentence4);
        }

        public void DialogNarrator()
        {
            // DIALOG BOXES STARTS HERE
            var fontSize = 20;
            var GlowRadius = 0;
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            var font = LoadFont("sb/dialog/txt/narrator", new FontDescription()
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
                Color = GlowColor,
            },
            new FontOutline()
            {
                Thickness = OutlineThickness,
                Color = OutlineColor,
            },
            new FontShadow()
            {
                Thickness = ShadowThickness,
                Color = ShadowColor,
            });

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "Get ready!",
                                  "Your mission is about to begin." };
            var dialog = new DialogManager(this, font, 40630, 48937, "Dialog - Text Narrator", 40, 430, true,
                fontSize, 1, BoxColorText, true, 0.4f, BoxColorTochi, "Dialog - Box", 0, "sb/sfx/message-1.wav",
                DialogBoxes.Pointer.None, DialogBoxes.Push.Right, sentence);
        }

        public void Avatars()
        {
            // var AvatarProfiles = new List<string>() {
            //     "Acyl", "Pino", "ScubDomino", "Dailycare", "Moecho", "Heilia", "toybot", "Necho", "Otosaka-Yu" };

            var Spacing = 1700;
            var duration = 10000;
            var startTime = 12937;
            var endTime = 35091;
            var HoverEndTime = 37860;
            // var rotation = (float)Random(-1.5, 1.5);

            var posY = 320;
            var posX = new Vector2(640, 0);
            var ScalePassive = 0.2f;
            var ScaleActive = 0.5f;
            var HoverVelocity = 10;
            var HoverDuration = 3000;
            var ProfilesLoopCount = (endTime - startTime) / duration;
            var HoverLoopCount = (HoverEndTime - startTime) / HoverDuration;

            Tochi(320, 430, 1, 1860, HoverEndTime, duration, (int)HoverLoopCount, HoverDuration, HoverVelocity);
            Acyl(startTime, duration, 0, (float)Random(-1.5, 1.5), posY, posX, ScalePassive, ScaleActive, (int)ProfilesLoopCount);
            Pino(startTime, duration, Spacing, (float)Random(-1.5, 1.5), posY, posX, ScalePassive, ScaleActive, (int)ProfilesLoopCount);
            ScubDomino(startTime, duration, Spacing * 2, (float)Random(-1.5, 1.5), posY, posX, ScalePassive, ScaleActive, (int)ProfilesLoopCount);
            Dailycare(startTime, duration, Spacing * 3, (float)Random(-1.5, 1.5), posY, posX, ScalePassive, ScaleActive, (int)ProfilesLoopCount);
            Moecho(startTime, duration, Spacing * 4, (float)Random(-1.5, 1.5), posY, posX, ScalePassive, ScaleActive, (int)ProfilesLoopCount);
            Heilia(startTime, duration, Spacing * 5, (float)Random(-1.5, 1.5), posY, posX, ScalePassive, ScaleActive, (int)ProfilesLoopCount);
            toybot(startTime, duration, Spacing * 6, (float)Random(-1.5, 1.5), posY, posX, ScalePassive, ScaleActive, (int)ProfilesLoopCount);
            Necho(startTime, duration, Spacing * 7, (float)Random(-1.5, 1.5), posY, posX, ScalePassive, ScaleActive, (int)ProfilesLoopCount);
            Otosaka_Yu(startTime, duration, Spacing * 8, (float)Random(-1.5, 1.5), posY, posX, ScalePassive, ScaleActive, (int)ProfilesLoopCount);
        }

        public void Tochi(float posX, float posY, float Fade, int startTime, int endTime, int duration, int LoopCount, int HoverDuration, int HoverVelocity)
        {
            var reMoveY = 320;
            var reMoveX = -70;
            // var b = GetMapsetBitmap("sb/avatars/-Tochi.png");
            var nameTag = GetLayer("nameTag").CreateSprite("sb/avatars/-TochiTag.png", OsbOrigin.Centre);
            var avatar = GetLayer("-Tochi").CreateSprite("sb/avatars/-Tochi.png", OsbOrigin.BottomCentre);

            // hovering stuff
            avatar.StartLoopGroup(startTime, LoopCount);
            avatar.MoveY(OsbEasing.InOutSine, 0, HoverDuration, posY - HoverVelocity, posY + HoverVelocity);
            avatar.MoveY(OsbEasing.InOutSine, HoverDuration, HoverDuration * 2, posY + HoverVelocity, posY - HoverVelocity);
            avatar.EndGroup();
            nameTag.StartLoopGroup(startTime, LoopCount);
            nameTag.MoveY(OsbEasing.InOutSine, 100, HoverDuration, posY - reMoveY - HoverVelocity, posY - reMoveY + HoverVelocity);
            nameTag.MoveY(OsbEasing.InOutSine, HoverDuration, HoverDuration * 2, posY - reMoveY + HoverVelocity, posY - reMoveY - HoverVelocity);
            nameTag.EndGroup();

            // avatar stuff
            avatar.MoveX(startTime, posX);
            avatar.MoveX(OsbEasing.InOutSine, 12937, 15707, posX, posX - 150);
            avatar.Scale(startTime, 0.25);
            avatar.Fade(startTime, startTime + (HoverDuration / 2), 0, Fade);
            avatar.Fade(endTime - (HoverDuration / 2), endTime, Fade, 0);
            nameTag.MoveX(startTime, posX + reMoveX);
            nameTag.MoveX(OsbEasing.InOutSine, 12937, 15707, posX + reMoveX, posX + reMoveX - 150);
            nameTag.Scale(startTime, 0.5);
            nameTag.Fade(startTime, startTime + (HoverDuration / 2), 0, Fade);
            nameTag.Fade(endTime - (HoverDuration / 2), endTime, Fade, 0);
            nameTag.Color(startTime, TochiTagColor);


            // REFLECTION IN THE WATER

            var avatarReflection = GetLayer("-Tochi").CreateSprite("sb/avatars/-Tochi.png", OsbOrigin.BottomCentre);

            // hovering stuff
            avatarReflection.StartLoopGroup(startTime, LoopCount);
            avatarReflection.MoveY(OsbEasing.InOutSine, 0, HoverDuration, posY + HoverVelocity, posY - HoverVelocity);
            avatarReflection.MoveY(OsbEasing.InOutSine, HoverDuration, HoverDuration * 2, posY - HoverVelocity, posY + HoverVelocity);
            avatarReflection.EndGroup();

            // avatar stuff
            avatarReflection.MoveX(startTime, posX);
            avatarReflection.MoveX(OsbEasing.InOutSine, 12937, 15707, posX, posX - 150);
            avatarReflection.Scale(startTime, 0.25);
            avatarReflection.Fade(startTime, startTime + (HoverDuration / 2), 0, Fade / 5);
            avatarReflection.Fade(endTime - (HoverDuration / 2), endTime, Fade / 5, 0);
            avatarReflection.Rotate(startTime, MathHelper.DegreesToRadians(180));
        }

        public void Background()
        {
            var bitmap = GetMapsetBitmap("sb/bgs/1/bg.jpg");
            var bg = GetLayer("Background").CreateSprite("sb/bgs/1/bg.jpg", OsbOrigin.Centre, new Vector2(320, 240));

            bg.Scale(1399, 480.0f / bitmap.Height);
            bg.Fade(1399, 3245, 0, 0.4);
            bg.Fade(57245, 72572, 0.4, 0);
        }

        public void CyanRain(float Fade, string LayerName, int interval, float ScaleX, float ScaleY)
        {
            var startTime = 1399;
            var endTime = 55860;
            for (var i = startTime; i < endTime; i += interval)
            {
                var rain = GetLayer(LayerName).CreateSprite("sb/cyanRain.png", OsbOrigin.BottomCentre);

                var speed = Random(1000, 8000);

                var StartPosX = Random(-107, 747);
                var EndPosX = StartPosX; // + Random(0, 0);
                var StartPosY = Random(-100, 0);
                var EndPosY = Random(400, 460);

                var DeltaX = EndPosX - StartPosX;
                var DeltaY = EndPosY - StartPosY;
                // var Rotation = Math.Atan2(DeltaY, DeltaX) * Math.PI;

                rain.Fade(i, Fade);
                rain.MoveX(i, i + speed, StartPosX, EndPosX);
                rain.MoveY(i, i + speed, StartPosY, EndPosY);
                // rain.Rotate(i, i + speed, 0, Rotation);

                if (speed >= 5000)
                {
                    rain.ScaleVec(i + speed, i + speed + Random(1000, 2000), ScaleX, ScaleY, ScaleX, 0);
                }

                if (speed <= 5000)
                {
                    rain.ScaleVec(i + speed, i + speed + Random(500, 1000), ScaleX, ScaleY, ScaleX, 0);
                }


                // SPLASH EFFECT

                var delay = 300;
                var end = i + speed + delay + interval + 500;
                for (var s = i + speed; s < end; s += 1000)
                {
                    var splashStart = new Vector2(0, 0f);
                    var splashEnd = new Vector2(0.5f, 0.05f);
                    var splash = GetLayer("splash").CreateSprite("sb/splash.png", OsbOrigin.Centre);

                    splash.MoveX(s, EndPosX);
                    splash.MoveY(s, EndPosY);

                    var fade = (float)Random(0.1f, 0.2f);
                    splash.Fade(s - delay, s, 0, fade);
                    splash.Fade(s + delay, s + delay + interval + 500, fade, 0);

                    splash.ScaleVec(s, s + delay + interval + 500, splashStart, splashEnd);
                }
            }
        }

        public void Acyl(int startTime, int duration, int Delay, float Rotation, float posY, Vector2 posX, float ScalePassive, float ScaleActive, int LoopCount)
        {
            var nameTag = GetLayer("nameTag").CreateSprite("sb/avatars/AcylTag.png", OsbOrigin.Centre);
            var ringFront = GetLayer("ringFront").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var ringFront2 = GetLayer("ringFront").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var ringBack = GetLayer("ringBack").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var avatarFront = GetLayer("avatarFront").CreateSprite("sb/avatars/AcylProfile.png", OsbOrigin.Centre);
            var avatarFront2 = GetLayer("avatarFront").CreateSprite("sb/avatars/AcylProfile.png", OsbOrigin.Centre);
            var avatarBack = GetLayer("avatarBack").CreateSprite("sb/avatars/AcylProfile.png", OsbOrigin.Centre);
            var ScaleBack = new Vector2(ScalePassive / 2, ScalePassive / 2);

            // front
            avatarFront.StartLoopGroup(startTime + Delay, LoopCount);
            avatarFront.MoveY(0, posY);
            avatarFront.Fade(0, 3500, 0, 1);
            avatarFront.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            avatarFront.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            avatarFront.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            avatarFront.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            avatarFront.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            avatarFront.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            avatarFront.ScaleVec(duration * 2, 0, ScalePassive);
            avatarFront.Fade(duration - 3500, duration, 1, 0);
            avatarFront.EndGroup();

            ringFront.StartLoopGroup(startTime + Delay, LoopCount);
            ringFront.MoveY(startTime, posY);
            ringFront.Fade(0, 3500, 0, 1);
            ringFront.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            ringFront.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            ringFront.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            ringFront.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            ringFront.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            ringFront.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            ringFront.ScaleVec(duration * 2, 0, ScalePassive);
            ringFront.Fade(duration - 3500, duration, 1, 0);
            ringFront.Rotate(OsbEasing.InOutSine, duration / 4, duration / 2, 0, Rotation);
            ringFront.Rotate(OsbEasing.Out, duration / 2, duration / 2 + 400, Rotation, 0);
            ringFront.EndGroup();

            nameTag.StartLoopGroup(startTime + Delay, LoopCount);
            nameTag.MoveY(0, posY + 75);
            nameTag.Fade(3000, 4000, 0, 1);
            nameTag.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            nameTag.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            nameTag.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            nameTag.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            nameTag.ScaleVec(OsbEasing.InOutSine, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            nameTag.ScaleVec(OsbEasing.InOutSine, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            nameTag.ScaleVec(duration * 2, 0, ScalePassive);
            nameTag.Fade(duration - 4000, duration - 3000, 1, 0);
            nameTag.EndGroup();

            // reflection
            avatarFront2.StartLoopGroup(startTime + Delay, LoopCount);
            avatarFront2.FlipV(0, duration);
            avatarFront2.MoveY(0, posY + 180);
            avatarFront2.Fade(0, 500, 0, 0.05);
            avatarFront2.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            avatarFront2.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            avatarFront2.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            avatarFront2.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            avatarFront2.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            avatarFront2.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            avatarFront2.ScaleVec(duration * 2, 0, ScalePassive);
            avatarFront2.Fade(duration - 500, duration, 0.05, 0);
            avatarFront2.EndGroup();

            ringFront2.StartLoopGroup(startTime + Delay, LoopCount);
            ringFront2.FlipV(0, duration / 2 + 400);
            ringFront2.MoveY(startTime, posY + 180);
            ringFront2.Fade(0, 500, 0, 0.05);
            ringFront2.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            ringFront2.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            ringFront2.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            ringFront2.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            ringFront2.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            ringFront2.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            ringFront2.ScaleVec(duration * 2, 0, ScalePassive);
            ringFront2.Fade(duration - 500, duration, 0.05, 0);
            ringFront2.Rotate(OsbEasing.InOutSine, duration / 4, duration / 2, 0, Rotation);
            ringFront2.Rotate(OsbEasing.Out, duration / 2, duration / 2 + 400, Rotation, 0);
            ringFront2.EndGroup();

            // back
            avatarBack.StartLoopGroup(startTime + Delay + duration, LoopCount);
            avatarBack.MoveY(0, posY);
            avatarBack.Fade(0, 3500, 0, 0.7);
            avatarBack.MoveX(OsbEasing.In, 0, duration / 2, posX.Y, posX.X / 2);
            avatarBack.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.X);
            avatarBack.ScaleVec(OsbEasing.Out, 0, duration / 2, 0, ScaleBack.Y * 2, ScaleBack.X / 2, ScaleBack.Y / 2);
            avatarBack.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleBack.X / 2, ScaleBack.Y / 2, 0, ScaleBack.Y * 2);
            avatarBack.ScaleVec(duration * 2, 0, ScaleBack.Y * 2);
            avatarBack.Fade(duration - 3500, duration, 0.7, 0);
            avatarBack.Color(0, PassiveColor);
            avatarBack.EndGroup();

            ringBack.StartLoopGroup(startTime + Delay + duration, LoopCount);
            ringBack.MoveY(0, posY);
            ringBack.Fade(1000, 3500, 0, 0.7);
            ringBack.MoveX(OsbEasing.In, 0, duration / 2, posX.Y, posX.X / 2);
            ringBack.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.X);
            ringBack.ScaleVec(OsbEasing.Out, 0, duration / 2, 0, ScaleBack.Y * 2, ScaleBack.X / 2, ScaleBack.Y / 2);
            ringBack.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleBack.X / 2, ScaleBack.Y / 2, 0, ScaleBack.Y * 2);
            ringBack.ScaleVec(duration * 2, 0, ScaleBack.Y * 2);
            ringBack.Fade(duration - 3500, duration - 1000, 0.7, 0);
            ringBack.Color(0, PassiveColor);
            ringBack.EndGroup();
        }

        public void Pino(int startTime, int duration, int Delay, float Rotation, float posY, Vector2 posX, float ScalePassive, float ScaleActive, int LoopCount)
        {
            var nameTag = GetLayer("nameTag").CreateSprite("sb/avatars/PinoTag.png", OsbOrigin.Centre);
            var ringFront = GetLayer("ringFront").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var ringFront2 = GetLayer("ringFront").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var ringBack = GetLayer("ringBack").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var avatarFront = GetLayer("avatarFront").CreateSprite("sb/avatars/PinoProfile.png", OsbOrigin.Centre);
            var avatarFront2 = GetLayer("avatarFront").CreateSprite("sb/avatars/PinoProfile.png", OsbOrigin.Centre);
            var avatarBack = GetLayer("avatarBack").CreateSprite("sb/avatars/PinoProfile.png", OsbOrigin.Centre);
            var ScaleBack = new Vector2(ScalePassive / 2, ScalePassive / 2);

            // front
            avatarFront.StartLoopGroup(startTime + Delay, LoopCount);
            avatarFront.MoveY(0, posY);
            avatarFront.Fade(0, 3500, 0, 1);
            avatarFront.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            avatarFront.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            avatarFront.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            avatarFront.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            avatarFront.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            avatarFront.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            avatarFront.ScaleVec(duration * 2, 0, ScalePassive);
            avatarFront.Fade(duration - 3500, duration, 1, 0);
            avatarFront.EndGroup();

            nameTag.StartLoopGroup(startTime + Delay, LoopCount);
            nameTag.MoveY(0, posY + 75);
            nameTag.Fade(3000, 4000, 0, 1);
            nameTag.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            nameTag.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            nameTag.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            nameTag.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            nameTag.ScaleVec(OsbEasing.InOutSine, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            nameTag.ScaleVec(OsbEasing.InOutSine, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            nameTag.ScaleVec(duration * 2, 0, ScalePassive);
            nameTag.Fade(duration - 4000, duration - 3000, 1, 0);
            nameTag.EndGroup();

            ringFront.StartLoopGroup(startTime + Delay, LoopCount);
            ringFront.MoveY(startTime, posY);
            ringFront.Fade(0, 3500, 0, 1);
            ringFront.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            ringFront.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            ringFront.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            ringFront.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            ringFront.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            ringFront.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            ringFront.ScaleVec(duration * 2, 0, ScalePassive);
            ringFront.Fade(duration - 3500, duration, 1, 0);
            ringFront.Rotate(OsbEasing.InOutSine, duration / 4, duration / 2, 0, Rotation);
            ringFront.Rotate(OsbEasing.Out, duration / 2, duration / 2 + 400, Rotation, 0);
            ringFront.EndGroup();

            // reflection
            avatarFront2.StartLoopGroup(startTime + Delay, LoopCount);
            avatarFront2.FlipV(0, duration);
            avatarFront2.MoveY(0, posY + 180);
            avatarFront2.Fade(0, 500, 0, 0.05);
            avatarFront2.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            avatarFront2.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            avatarFront2.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            avatarFront2.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            avatarFront2.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            avatarFront2.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            avatarFront2.ScaleVec(duration * 2, 0, ScalePassive);
            avatarFront2.Fade(duration - 500, duration, 0.05, 0);
            avatarFront2.EndGroup();

            ringFront2.StartLoopGroup(startTime + Delay, LoopCount);
            ringFront2.FlipV(0, duration / 2 + 400);
            ringFront2.MoveY(startTime, posY + 180);
            ringFront2.Fade(0, 500, 0, 0.05);
            ringFront2.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            ringFront2.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            ringFront2.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            ringFront2.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            ringFront2.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            ringFront2.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            ringFront2.ScaleVec(duration * 2, 0, ScalePassive);
            ringFront2.Fade(duration - 500, duration, 0.05, 0);
            ringFront2.Rotate(OsbEasing.InOutSine, duration / 4, duration / 2, 0, Rotation);
            ringFront2.Rotate(OsbEasing.Out, duration / 2, duration / 2 + 400, Rotation, 0);
            ringFront2.EndGroup();

            // back
            avatarBack.StartLoopGroup(startTime + Delay + duration, LoopCount);
            avatarBack.MoveY(0, posY);
            avatarBack.Fade(0, 3500, 0, 0.7);
            avatarBack.MoveX(OsbEasing.In, 0, duration / 2, posX.Y, posX.X / 2);
            avatarBack.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.X);
            avatarBack.ScaleVec(OsbEasing.Out, 0, duration / 2, 0, ScaleBack.Y * 2, ScaleBack.X / 2, ScaleBack.Y / 2);
            avatarBack.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleBack.X / 2, ScaleBack.Y / 2, 0, ScaleBack.Y * 2);
            avatarBack.ScaleVec(duration * 2, 0, ScaleBack.Y * 2);
            avatarBack.Fade(duration - 3500, duration, 0.7, 0);
            avatarBack.Color(0, PassiveColor);
            avatarBack.EndGroup();

            ringBack.StartLoopGroup(startTime + Delay + duration, LoopCount);
            ringBack.MoveY(0, posY);
            ringBack.Fade(1000, 3500, 0, 0.7);
            ringBack.MoveX(OsbEasing.In, 0, duration / 2, posX.Y, posX.X / 2);
            ringBack.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.X);
            ringBack.ScaleVec(OsbEasing.Out, 0, duration / 2, 0, ScaleBack.Y * 2, ScaleBack.X / 2, ScaleBack.Y / 2);
            ringBack.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleBack.X / 2, ScaleBack.Y / 2, 0, ScaleBack.Y * 2);
            ringBack.ScaleVec(duration * 2, 0, ScaleBack.Y * 2);
            ringBack.Fade(duration - 3500, duration - 1000, 0.7, 0);
            ringBack.Color(0, PassiveColor);
            ringBack.EndGroup();
        }

        public void ScubDomino(int startTime, int duration, int Delay, float Rotation, float posY, Vector2 posX, float ScalePassive, float ScaleActive, int LoopCount)
        {
            var nameTag = GetLayer("nameTag").CreateSprite("sb/avatars/ScubDominoTag.png", OsbOrigin.Centre);
            var ringFront = GetLayer("ringFront").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var ringFront2 = GetLayer("ringFront").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var ringBack = GetLayer("ringBack").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var avatarFront = GetLayer("avatarFront").CreateSprite("sb/avatars/ScubDominoProfile.png", OsbOrigin.Centre);
            var avatarFront2 = GetLayer("avatarFront").CreateSprite("sb/avatars/ScubDominoProfile.png", OsbOrigin.Centre);
            var avatarBack = GetLayer("avatarBack").CreateSprite("sb/avatars/ScubDominoProfile.png", OsbOrigin.Centre);
            var ScaleBack = new Vector2(ScalePassive / 2, ScalePassive / 2);

            // front
            avatarFront.StartLoopGroup(startTime + Delay, LoopCount);
            avatarFront.MoveY(0, posY);
            avatarFront.Fade(0, 3500, 0, 1);
            avatarFront.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            avatarFront.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            avatarFront.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            avatarFront.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            avatarFront.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            avatarFront.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            avatarFront.ScaleVec(duration * 2, 0, ScalePassive);
            avatarFront.Fade(duration - 3500, duration, 1, 0);
            avatarFront.EndGroup();

            nameTag.StartLoopGroup(startTime + Delay, LoopCount);
            nameTag.MoveY(0, posY + 75);
            nameTag.Fade(3000, 4000, 0, 1);
            nameTag.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            nameTag.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            nameTag.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            nameTag.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            nameTag.ScaleVec(OsbEasing.InOutSine, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            nameTag.ScaleVec(OsbEasing.InOutSine, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            nameTag.ScaleVec(duration * 2, 0, ScalePassive);
            nameTag.Fade(duration - 4000, duration - 3000, 1, 0);
            nameTag.EndGroup();

            ringFront.StartLoopGroup(startTime + Delay, LoopCount);
            ringFront.MoveY(startTime, posY);
            ringFront.Fade(0, 3500, 0, 1);
            ringFront.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            ringFront.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            ringFront.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            ringFront.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            ringFront.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            ringFront.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            ringFront.ScaleVec(duration * 2, 0, ScalePassive);
            ringFront.Fade(duration - 3500, duration, 1, 0);
            ringFront.Rotate(OsbEasing.InOutSine, duration / 4, duration / 2, 0, Rotation);
            ringFront.Rotate(OsbEasing.Out, duration / 2, duration / 2 + 400, Rotation, 0);
            ringFront.EndGroup();

            // reflection
            avatarFront2.StartLoopGroup(startTime + Delay, LoopCount);
            avatarFront2.FlipV(0, duration);
            avatarFront2.MoveY(0, posY + 180);
            avatarFront2.Fade(0, 500, 0, 0.05);
            avatarFront2.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            avatarFront2.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            avatarFront2.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            avatarFront2.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            avatarFront2.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            avatarFront2.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            avatarFront2.ScaleVec(duration * 2, 0, ScalePassive);
            avatarFront2.Fade(duration - 500, duration, 0.05, 0);
            avatarFront2.EndGroup();

            ringFront2.StartLoopGroup(startTime + Delay, LoopCount);
            ringFront2.FlipV(0, duration / 2 + 400);
            ringFront2.MoveY(startTime, posY + 180);
            ringFront2.Fade(0, 500, 0, 0.05);
            ringFront2.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            ringFront2.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            ringFront2.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            ringFront2.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            ringFront2.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            ringFront2.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            ringFront2.ScaleVec(duration * 2, 0, ScalePassive);
            ringFront2.Fade(duration - 500, duration, 0.05, 0);
            ringFront2.Rotate(OsbEasing.InOutSine, duration / 4, duration / 2, 0, Rotation);
            ringFront2.Rotate(OsbEasing.Out, duration / 2, duration / 2 + 400, Rotation, 0);
            ringFront2.EndGroup();

            // back
            avatarBack.StartLoopGroup(startTime + Delay + duration, LoopCount);
            avatarBack.MoveY(0, posY);
            avatarBack.Fade(0, 3500, 0, 0.7);
            avatarBack.MoveX(OsbEasing.In, 0, duration / 2, posX.Y, posX.X / 2);
            avatarBack.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.X);
            avatarBack.ScaleVec(OsbEasing.Out, 0, duration / 2, 0, ScaleBack.Y * 2, ScaleBack.X / 2, ScaleBack.Y / 2);
            avatarBack.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleBack.X / 2, ScaleBack.Y / 2, 0, ScaleBack.Y * 2);
            avatarBack.ScaleVec(duration * 2, 0, ScaleBack.Y * 2);
            avatarBack.Fade(duration - 3500, duration, 0.7, 0);
            avatarBack.Color(0, PassiveColor);
            avatarBack.EndGroup();

            ringBack.StartLoopGroup(startTime + Delay + duration, LoopCount);
            ringBack.MoveY(0, posY);
            ringBack.Fade(1000, 3500, 0, 0.7);
            ringBack.MoveX(OsbEasing.In, 0, duration / 2, posX.Y, posX.X / 2);
            ringBack.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.X);
            ringBack.ScaleVec(OsbEasing.Out, 0, duration / 2, 0, ScaleBack.Y * 2, ScaleBack.X / 2, ScaleBack.Y / 2);
            ringBack.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleBack.X / 2, ScaleBack.Y / 2, 0, ScaleBack.Y * 2);
            ringBack.ScaleVec(duration * 2, 0, ScaleBack.Y * 2);
            ringBack.Fade(duration - 3500, duration - 1000, 0.7, 0);
            ringBack.Color(0, PassiveColor);
            ringBack.EndGroup();
        }

        public void Dailycare(int startTime, int duration, int Delay, float Rotation, float posY, Vector2 posX, float ScalePassive, float ScaleActive, int LoopCount)
        {
            var nameTag = GetLayer("nameTag").CreateSprite("sb/avatars/DailycareTag.png", OsbOrigin.Centre);
            var ringFront = GetLayer("ringFront").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var ringFront2 = GetLayer("ringFront").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var ringBack = GetLayer("ringBack").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var avatarFront = GetLayer("avatarFront").CreateSprite("sb/avatars/DailycareProfile.png", OsbOrigin.Centre);
            var avatarFront2 = GetLayer("avatarFront").CreateSprite("sb/avatars/DailycareProfile.png", OsbOrigin.Centre);
            var avatarBack = GetLayer("avatarBack").CreateSprite("sb/avatars/DailycareProfile.png", OsbOrigin.Centre);
            var ScaleBack = new Vector2(ScalePassive / 2, ScalePassive / 2);

            // front
            avatarFront.StartLoopGroup(startTime + Delay, LoopCount);
            avatarFront.MoveY(0, posY);
            avatarFront.Fade(0, 3500, 0, 1);
            avatarFront.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            avatarFront.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            avatarFront.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            avatarFront.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            avatarFront.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            avatarFront.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            avatarFront.ScaleVec(duration * 2, 0, ScalePassive);
            avatarFront.Fade(duration - 3500, duration, 1, 0);
            avatarFront.EndGroup();

            nameTag.StartLoopGroup(startTime + Delay, LoopCount);
            nameTag.MoveY(0, posY + 75);
            nameTag.Fade(3000, 4000, 0, 1);
            nameTag.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            nameTag.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            nameTag.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            nameTag.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            nameTag.ScaleVec(OsbEasing.InOutSine, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            nameTag.ScaleVec(OsbEasing.InOutSine, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            nameTag.ScaleVec(duration * 2, 0, ScalePassive);
            nameTag.Fade(duration - 4000, duration - 3000, 1, 0);
            nameTag.EndGroup();

            ringFront.StartLoopGroup(startTime + Delay, LoopCount);
            ringFront.MoveY(startTime, posY);
            ringFront.Fade(0, 3500, 0, 1);
            ringFront.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            ringFront.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            ringFront.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            ringFront.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            ringFront.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            ringFront.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            ringFront.ScaleVec(duration * 2, 0, ScalePassive);
            ringFront.Fade(duration - 3500, duration, 1, 0);
            ringFront.Rotate(OsbEasing.InOutSine, duration / 4, duration / 2, 0, Rotation);
            ringFront.Rotate(OsbEasing.Out, duration / 2, duration / 2 + 400, Rotation, 0);
            ringFront.EndGroup();

            // reflection
            avatarFront2.StartLoopGroup(startTime + Delay, LoopCount);
            avatarFront2.FlipV(0, duration);
            avatarFront2.MoveY(0, posY + 180);
            avatarFront2.Fade(0, 500, 0, 0.05);
            avatarFront2.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            avatarFront2.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            avatarFront2.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            avatarFront2.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            avatarFront2.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            avatarFront2.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            avatarFront2.ScaleVec(duration * 2, 0, ScalePassive);
            avatarFront2.Fade(duration - 500, duration, 0.05, 0);
            avatarFront2.EndGroup();

            ringFront2.StartLoopGroup(startTime + Delay, LoopCount);
            ringFront2.FlipV(0, duration / 2 + 400);
            ringFront2.MoveY(startTime, posY + 180);
            ringFront2.Fade(0, 500, 0, 0.05);
            ringFront2.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            ringFront2.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            ringFront2.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            ringFront2.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            ringFront2.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            ringFront2.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            ringFront2.ScaleVec(duration * 2, 0, ScalePassive);
            ringFront2.Fade(duration - 500, duration, 0.05, 0);
            ringFront2.Rotate(OsbEasing.InOutSine, duration / 4, duration / 2, 0, Rotation);
            ringFront2.Rotate(OsbEasing.Out, duration / 2, duration / 2 + 400, Rotation, 0);
            ringFront2.EndGroup();

            // back
            avatarBack.StartLoopGroup(startTime + Delay + duration, LoopCount);
            avatarBack.MoveY(0, posY);
            avatarBack.Fade(0, 3500, 0, 0.7);
            avatarBack.MoveX(OsbEasing.In, 0, duration / 2, posX.Y, posX.X / 2);
            avatarBack.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.X);
            avatarBack.ScaleVec(OsbEasing.Out, 0, duration / 2, 0, ScaleBack.Y * 2, ScaleBack.X / 2, ScaleBack.Y / 2);
            avatarBack.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleBack.X / 2, ScaleBack.Y / 2, 0, ScaleBack.Y * 2);
            avatarBack.ScaleVec(duration * 2, 0, ScaleBack.Y * 2);
            avatarBack.Fade(duration - 3500, duration, 0.7, 0);
            avatarBack.Color(0, PassiveColor);
            avatarBack.EndGroup();

            ringBack.StartLoopGroup(startTime + Delay + duration, LoopCount);
            ringBack.MoveY(0, posY);
            ringBack.Fade(1000, 3500, 0, 0.7);
            ringBack.MoveX(OsbEasing.In, 0, duration / 2, posX.Y, posX.X / 2);
            ringBack.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.X);
            ringBack.ScaleVec(OsbEasing.Out, 0, duration / 2, 0, ScaleBack.Y * 2, ScaleBack.X / 2, ScaleBack.Y / 2);
            ringBack.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleBack.X / 2, ScaleBack.Y / 2, 0, ScaleBack.Y * 2);
            ringBack.ScaleVec(duration * 2, 0, ScaleBack.Y * 2);
            ringBack.Fade(duration - 3500, duration - 1000, 0.7, 0);
            ringBack.Color(0, PassiveColor);
            ringBack.EndGroup();
        }

        public void Moecho(int startTime, int duration, int Delay, float Rotation, float posY, Vector2 posX, float ScalePassive, float ScaleActive, int LoopCount)
        {
            var nameTag = GetLayer("nameTag").CreateSprite("sb/avatars/MoechoTag.png", OsbOrigin.Centre);
            var ringFront = GetLayer("ringFront").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var ringFront2 = GetLayer("ringFront").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var ringBack = GetLayer("ringBack").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var avatarFront = GetLayer("avatarFront").CreateSprite("sb/avatars/MoechoProfile.png", OsbOrigin.Centre);
            var avatarFront2 = GetLayer("avatarFront").CreateSprite("sb/avatars/MoechoProfile.png", OsbOrigin.Centre);
            var avatarBack = GetLayer("avatarBack").CreateSprite("sb/avatars/MoechoProfile.png", OsbOrigin.Centre);
            var ScaleBack = new Vector2(ScalePassive / 2, ScalePassive / 2);

            // front
            avatarFront.StartLoopGroup(startTime + Delay, LoopCount);
            avatarFront.MoveY(0, posY);
            avatarFront.Fade(0, 3500, 0, 1);
            avatarFront.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            avatarFront.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            avatarFront.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            avatarFront.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            avatarFront.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            avatarFront.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            avatarFront.ScaleVec(duration * 2, 0, ScalePassive);
            avatarFront.Fade(duration - 3500, duration, 1, 0);
            avatarFront.EndGroup();

            nameTag.StartLoopGroup(startTime + Delay, LoopCount);
            nameTag.MoveY(0, posY + 75);
            nameTag.Fade(3000, 4000, 0, 1);
            nameTag.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            nameTag.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            nameTag.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            nameTag.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            nameTag.ScaleVec(OsbEasing.InOutSine, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            nameTag.ScaleVec(OsbEasing.InOutSine, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            nameTag.ScaleVec(duration * 2, 0, ScalePassive);
            nameTag.Fade(duration - 4000, duration - 3000, 1, 0);
            nameTag.EndGroup();

            ringFront.StartLoopGroup(startTime + Delay, LoopCount);
            ringFront.MoveY(startTime, posY);
            ringFront.Fade(0, 3500, 0, 1);
            ringFront.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            ringFront.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            ringFront.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            ringFront.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            ringFront.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            ringFront.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            ringFront.ScaleVec(duration * 2, 0, ScalePassive);
            ringFront.Fade(duration - 3500, duration, 1, 0);
            ringFront.Rotate(OsbEasing.InOutSine, duration / 4, duration / 2, 0, Rotation);
            ringFront.Rotate(OsbEasing.Out, duration / 2, duration / 2 + 400, Rotation, 0);
            ringFront.EndGroup();

            // reflection
            avatarFront2.StartLoopGroup(startTime + Delay, LoopCount);
            avatarFront2.FlipV(0, duration);
            avatarFront2.MoveY(0, posY + 180);
            avatarFront2.Fade(0, 500, 0, 0.05);
            avatarFront2.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            avatarFront2.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            avatarFront2.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            avatarFront2.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            avatarFront2.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            avatarFront2.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            avatarFront2.ScaleVec(duration * 2, 0, ScalePassive);
            avatarFront2.Fade(duration - 500, duration, 0.05, 0);
            avatarFront2.EndGroup();

            ringFront2.StartLoopGroup(startTime + Delay, LoopCount);
            ringFront2.FlipV(0, duration / 2 + 400);
            ringFront2.MoveY(startTime, posY + 180);
            ringFront2.Fade(0, 500, 0, 0.05);
            ringFront2.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            ringFront2.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            ringFront2.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            ringFront2.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            ringFront2.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            ringFront2.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            ringFront2.ScaleVec(duration * 2, 0, ScalePassive);
            ringFront2.Fade(duration - 500, duration, 0.05, 0);
            ringFront2.Rotate(OsbEasing.InOutSine, duration / 4, duration / 2, 0, Rotation);
            ringFront2.Rotate(OsbEasing.Out, duration / 2, duration / 2 + 400, Rotation, 0);
            ringFront2.EndGroup();

            // back
            avatarBack.StartLoopGroup(startTime + Delay + duration, LoopCount);
            avatarBack.MoveY(0, posY);
            avatarBack.Fade(0, 3500, 0, 0.7);
            avatarBack.MoveX(OsbEasing.In, 0, duration / 2, posX.Y, posX.X / 2);
            avatarBack.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.X);
            avatarBack.ScaleVec(OsbEasing.Out, 0, duration / 2, 0, ScaleBack.Y * 2, ScaleBack.X / 2, ScaleBack.Y / 2);
            avatarBack.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleBack.X / 2, ScaleBack.Y / 2, 0, ScaleBack.Y * 2);
            avatarBack.ScaleVec(duration * 2, 0, ScaleBack.Y * 2);
            avatarBack.Fade(duration - 3500, duration, 0.7, 0);
            avatarBack.Color(0, PassiveColor);
            avatarBack.EndGroup();

            ringBack.StartLoopGroup(startTime + Delay + duration, LoopCount);
            ringBack.MoveY(0, posY);
            ringBack.Fade(1000, 3500, 0, 0.7);
            ringBack.MoveX(OsbEasing.In, 0, duration / 2, posX.Y, posX.X / 2);
            ringBack.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.X);
            ringBack.ScaleVec(OsbEasing.Out, 0, duration / 2, 0, ScaleBack.Y * 2, ScaleBack.X / 2, ScaleBack.Y / 2);
            ringBack.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleBack.X / 2, ScaleBack.Y / 2, 0, ScaleBack.Y * 2);
            ringBack.ScaleVec(duration * 2, 0, ScaleBack.Y * 2);
            ringBack.Fade(duration - 3500, duration - 1000, 0.7, 0);
            ringBack.Color(0, PassiveColor);
            ringBack.EndGroup();
        }

        public void Heilia(int startTime, int duration, int Delay, float Rotation, float posY, Vector2 posX, float ScalePassive, float ScaleActive, int LoopCount)
        {
            var nameTag = GetLayer("nameTag").CreateSprite("sb/avatars/HeiliaTag.png", OsbOrigin.Centre);
            var ringFront = GetLayer("ringFront").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var ringFront2 = GetLayer("ringFront").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var ringBack = GetLayer("ringBack").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var avatarFront = GetLayer("avatarFront").CreateSprite("sb/avatars/HeiliaProfile.png", OsbOrigin.Centre);
            var avatarFront2 = GetLayer("avatarFront").CreateSprite("sb/avatars/HeiliaProfile.png", OsbOrigin.Centre);
            var avatarBack = GetLayer("avatarBack").CreateSprite("sb/avatars/HeiliaProfile.png", OsbOrigin.Centre);
            var ScaleBack = new Vector2(ScalePassive / 2, ScalePassive / 2);

            // front
            avatarFront.StartLoopGroup(startTime + Delay, LoopCount);
            avatarFront.MoveY(0, posY);
            avatarFront.Fade(0, 3500, 0, 1);
            avatarFront.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            avatarFront.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            avatarFront.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            avatarFront.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            avatarFront.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            avatarFront.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            avatarFront.ScaleVec(duration * 2, 0, ScalePassive);
            avatarFront.Fade(duration - 3500, duration, 1, 0);
            avatarFront.EndGroup();

            nameTag.StartLoopGroup(startTime + Delay, LoopCount);
            nameTag.MoveY(0, posY + 75);
            nameTag.Fade(3000, 4000, 0, 1);
            nameTag.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            nameTag.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            nameTag.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            nameTag.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            nameTag.ScaleVec(OsbEasing.InOutSine, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            nameTag.ScaleVec(OsbEasing.InOutSine, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            nameTag.ScaleVec(duration * 2, 0, ScalePassive);
            nameTag.Fade(duration - 4000, duration - 3000, 1, 0);
            nameTag.EndGroup();

            ringFront.StartLoopGroup(startTime + Delay, LoopCount);
            ringFront.MoveY(startTime, posY);
            ringFront.Fade(0, 3500, 0, 1);
            ringFront.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            ringFront.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            ringFront.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            ringFront.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            ringFront.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            ringFront.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            ringFront.ScaleVec(duration * 2, 0, ScalePassive);
            ringFront.Fade(duration - 3500, duration, 1, 0);
            ringFront.Rotate(OsbEasing.InOutSine, duration / 4, duration / 2, 0, Rotation);
            ringFront.Rotate(OsbEasing.Out, duration / 2, duration / 2 + 400, Rotation, 0);
            ringFront.EndGroup();

            // reflection
            avatarFront2.StartLoopGroup(startTime + Delay, LoopCount);
            avatarFront2.FlipV(0, duration);
            avatarFront2.MoveY(0, posY + 180);
            avatarFront2.Fade(0, 500, 0, 0.05);
            avatarFront2.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            avatarFront2.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            avatarFront2.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            avatarFront2.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            avatarFront2.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            avatarFront2.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            avatarFront2.ScaleVec(duration * 2, 0, ScalePassive);
            avatarFront2.Fade(duration - 500, duration, 0.05, 0);
            avatarFront2.EndGroup();

            ringFront2.StartLoopGroup(startTime + Delay, LoopCount);
            ringFront2.FlipV(0, duration / 2 + 400);
            ringFront2.MoveY(startTime, posY + 180);
            ringFront2.Fade(0, 500, 0, 0.05);
            ringFront2.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            ringFront2.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            ringFront2.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            ringFront2.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            ringFront2.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            ringFront2.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            ringFront2.ScaleVec(duration * 2, 0, ScalePassive);
            ringFront2.Fade(duration - 500, duration, 0.05, 0);
            ringFront2.Rotate(OsbEasing.InOutSine, duration / 4, duration / 2, 0, Rotation);
            ringFront2.Rotate(OsbEasing.Out, duration / 2, duration / 2 + 400, Rotation, 0);
            ringFront2.EndGroup();

            // back
            avatarBack.StartLoopGroup(startTime + Delay + duration, LoopCount);
            avatarBack.MoveY(0, posY);
            avatarBack.Fade(0, 3500, 0, 0.7);
            avatarBack.MoveX(OsbEasing.In, 0, duration / 2, posX.Y, posX.X / 2);
            avatarBack.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.X);
            avatarBack.ScaleVec(OsbEasing.Out, 0, duration / 2, 0, ScaleBack.Y * 2, ScaleBack.X / 2, ScaleBack.Y / 2);
            avatarBack.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleBack.X / 2, ScaleBack.Y / 2, 0, ScaleBack.Y * 2);
            avatarBack.ScaleVec(duration * 2, 0, ScaleBack.Y * 2);
            avatarBack.Fade(duration - 3500, duration, 0.7, 0);
            avatarBack.Color(0, PassiveColor);
            avatarBack.EndGroup();

            ringBack.StartLoopGroup(startTime + Delay + duration, LoopCount);
            ringBack.MoveY(0, posY);
            ringBack.Fade(1000, 3500, 0, 0.7);
            ringBack.MoveX(OsbEasing.In, 0, duration / 2, posX.Y, posX.X / 2);
            ringBack.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.X);
            ringBack.ScaleVec(OsbEasing.Out, 0, duration / 2, 0, ScaleBack.Y * 2, ScaleBack.X / 2, ScaleBack.Y / 2);
            ringBack.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleBack.X / 2, ScaleBack.Y / 2, 0, ScaleBack.Y * 2);
            ringBack.ScaleVec(duration * 2, 0, ScaleBack.Y * 2);
            ringBack.Fade(duration - 3500, duration - 1000, 0.7, 0);
            ringBack.Color(0, PassiveColor);
            ringBack.EndGroup();
        }

        public void toybot(int startTime, int duration, int Delay, float Rotation, float posY, Vector2 posX, float ScalePassive, float ScaleActive, int LoopCount)
        {
            var nameTag = GetLayer("nameTag").CreateSprite("sb/avatars/toybotTag.png", OsbOrigin.Centre);
            var ringFront = GetLayer("ringFront").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var ringFront2 = GetLayer("ringFront").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var ringBack = GetLayer("ringBack").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var avatarFront = GetLayer("avatarFront").CreateSprite("sb/avatars/toybotProfile.png", OsbOrigin.Centre);
            var avatarFront2 = GetLayer("avatarFront").CreateSprite("sb/avatars/toybotProfile.png", OsbOrigin.Centre);
            var avatarBack = GetLayer("avatarBack").CreateSprite("sb/avatars/toybotProfile.png", OsbOrigin.Centre);
            var ScaleBack = new Vector2(ScalePassive / 2, ScalePassive / 2);

            // front
            avatarFront.StartLoopGroup(startTime + Delay, LoopCount);
            avatarFront.MoveY(0, posY);
            avatarFront.Fade(0, 3500, 0, 1);
            avatarFront.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            avatarFront.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            avatarFront.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            avatarFront.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            avatarFront.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            avatarFront.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            avatarFront.ScaleVec(duration * 2, 0, ScalePassive);
            avatarFront.Fade(duration - 3500, duration, 1, 0);
            avatarFront.EndGroup();

            nameTag.StartLoopGroup(startTime + Delay, LoopCount);
            nameTag.MoveY(0, posY + 75);
            nameTag.Fade(3000, 4000, 0, 1);
            nameTag.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            nameTag.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            nameTag.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            nameTag.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            nameTag.ScaleVec(OsbEasing.InOutSine, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            nameTag.ScaleVec(OsbEasing.InOutSine, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            nameTag.ScaleVec(duration * 2, 0, ScalePassive);
            nameTag.Fade(duration - 4000, duration - 3000, 1, 0);
            nameTag.EndGroup();

            ringFront.StartLoopGroup(startTime + Delay, LoopCount);
            ringFront.MoveY(startTime, posY);
            ringFront.Fade(0, 3500, 0, 1);
            ringFront.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            ringFront.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            ringFront.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            ringFront.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            ringFront.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            ringFront.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            ringFront.ScaleVec(duration * 2, 0, ScalePassive);
            ringFront.Fade(duration - 3500, duration, 1, 0);
            ringFront.Rotate(OsbEasing.InOutSine, duration / 4, duration / 2, 0, Rotation);
            ringFront.Rotate(OsbEasing.Out, duration / 2, duration / 2 + 400, Rotation, 0);
            ringFront.EndGroup();

            // reflection
            avatarFront2.StartLoopGroup(startTime + Delay, LoopCount);
            avatarFront2.FlipV(0, duration);
            avatarFront2.MoveY(0, posY + 180);
            avatarFront2.Fade(0, 500, 0, 0.05);
            avatarFront2.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            avatarFront2.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            avatarFront2.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            avatarFront2.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            avatarFront2.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            avatarFront2.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            avatarFront2.ScaleVec(duration * 2, 0, ScalePassive);
            avatarFront2.Fade(duration - 500, duration, 0.05, 0);
            avatarFront2.EndGroup();

            ringFront2.StartLoopGroup(startTime + Delay, LoopCount);
            ringFront2.FlipV(0, duration / 2 + 400);
            ringFront2.MoveY(startTime, posY + 180);
            ringFront2.Fade(0, 500, 0, 0.05);
            ringFront2.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            ringFront2.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            ringFront2.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            ringFront2.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            ringFront2.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            ringFront2.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            ringFront2.ScaleVec(duration * 2, 0, ScalePassive);
            ringFront2.Fade(duration - 500, duration, 0.05, 0);
            ringFront2.Rotate(OsbEasing.InOutSine, duration / 4, duration / 2, 0, Rotation);
            ringFront2.Rotate(OsbEasing.Out, duration / 2, duration / 2 + 400, Rotation, 0);
            ringFront2.EndGroup();

            // back
            avatarBack.StartLoopGroup(startTime + Delay + duration, LoopCount);
            avatarBack.MoveY(0, posY);
            avatarBack.Fade(0, 3500, 0, 0.7);
            avatarBack.MoveX(OsbEasing.In, 0, duration / 2, posX.Y, posX.X / 2);
            avatarBack.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.X);
            avatarBack.ScaleVec(OsbEasing.Out, 0, duration / 2, 0, ScaleBack.Y * 2, ScaleBack.X / 2, ScaleBack.Y / 2);
            avatarBack.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleBack.X / 2, ScaleBack.Y / 2, 0, ScaleBack.Y * 2);
            avatarBack.ScaleVec(duration * 2, 0, ScaleBack.Y * 2);
            avatarBack.Fade(duration - 3500, duration, 0.7, 0);
            avatarBack.Color(0, PassiveColor);
            avatarBack.EndGroup();

            ringBack.StartLoopGroup(startTime + Delay + duration, LoopCount);
            ringBack.MoveY(0, posY);
            ringBack.Fade(1000, 3500, 0, 0.7);
            ringBack.MoveX(OsbEasing.In, 0, duration / 2, posX.Y, posX.X / 2);
            ringBack.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.X);
            ringBack.ScaleVec(OsbEasing.Out, 0, duration / 2, 0, ScaleBack.Y * 2, ScaleBack.X / 2, ScaleBack.Y / 2);
            ringBack.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleBack.X / 2, ScaleBack.Y / 2, 0, ScaleBack.Y * 2);
            ringBack.ScaleVec(duration * 2, 0, ScaleBack.Y * 2);
            ringBack.Fade(duration - 3500, duration - 1000, 0.7, 0);
            ringBack.Color(0, PassiveColor);
            ringBack.EndGroup();
        }

        public void Necho(int startTime, int duration, int Delay, float Rotation, float posY, Vector2 posX, float ScalePassive, float ScaleActive, int LoopCount)
        {
            var nameTag = GetLayer("nameTag").CreateSprite("sb/avatars/NechoTag.png", OsbOrigin.Centre);
            var ringFront = GetLayer("ringFront").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var ringFront2 = GetLayer("ringFront").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var ringBack = GetLayer("ringBack").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var avatarFront = GetLayer("avatarFront").CreateSprite("sb/avatars/NechoProfile.png", OsbOrigin.Centre);
            var avatarFront2 = GetLayer("avatarFront").CreateSprite("sb/avatars/NechoProfile.png", OsbOrigin.Centre);
            var avatarBack = GetLayer("avatarBack").CreateSprite("sb/avatars/NechoProfile.png", OsbOrigin.Centre);
            var ScaleBack = new Vector2(ScalePassive / 2, ScalePassive / 2);

            // front
            avatarFront.StartLoopGroup(startTime + Delay, LoopCount);
            avatarFront.MoveY(0, posY);
            avatarFront.Fade(0, 3500, 0, 1);
            avatarFront.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            avatarFront.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            avatarFront.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            avatarFront.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            avatarFront.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            avatarFront.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            avatarFront.ScaleVec(duration * 2, 0, ScalePassive);
            avatarFront.Fade(duration - 3500, duration, 1, 0);
            avatarFront.EndGroup();

            nameTag.StartLoopGroup(startTime + Delay, LoopCount);
            nameTag.MoveY(0, posY + 75);
            nameTag.Fade(3000, 4000, 0, 1);
            nameTag.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            nameTag.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            nameTag.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            nameTag.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            nameTag.ScaleVec(OsbEasing.InOutSine, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            nameTag.ScaleVec(OsbEasing.InOutSine, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            nameTag.ScaleVec(duration * 2, 0, ScalePassive);
            nameTag.Fade(duration - 4000, duration - 3000, 1, 0);
            nameTag.EndGroup();

            ringFront.StartLoopGroup(startTime + Delay, LoopCount);
            ringFront.MoveY(startTime, posY);
            ringFront.Fade(0, 3500, 0, 1);
            ringFront.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            ringFront.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            ringFront.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            ringFront.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            ringFront.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            ringFront.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            ringFront.ScaleVec(duration * 2, 0, ScalePassive);
            ringFront.Fade(duration - 3500, duration, 1, 0);
            ringFront.Rotate(OsbEasing.InOutSine, duration / 4, duration / 2, 0, Rotation);
            ringFront.Rotate(OsbEasing.Out, duration / 2, duration / 2 + 400, Rotation, 0);
            ringFront.EndGroup();

            // reflection
            avatarFront2.StartLoopGroup(startTime + Delay, LoopCount);
            avatarFront2.FlipV(0, duration);
            avatarFront2.MoveY(0, posY + 180);
            avatarFront2.Fade(0, 500, 0, 0.05);
            avatarFront2.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            avatarFront2.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            avatarFront2.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            avatarFront2.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            avatarFront2.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            avatarFront2.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            avatarFront2.ScaleVec(duration * 2, 0, ScalePassive);
            avatarFront2.Fade(duration - 500, duration, 0.05, 0);
            avatarFront2.EndGroup();

            ringFront2.StartLoopGroup(startTime + Delay, LoopCount);
            ringFront2.FlipV(0, duration / 2 + 400);
            ringFront2.MoveY(startTime, posY + 180);
            ringFront2.Fade(0, 500, 0, 0.05);
            ringFront2.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            ringFront2.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            ringFront2.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            ringFront2.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            ringFront2.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            ringFront2.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            ringFront2.ScaleVec(duration * 2, 0, ScalePassive);
            ringFront2.Fade(duration - 500, duration, 0.05, 0);
            ringFront2.Rotate(OsbEasing.InOutSine, duration / 4, duration / 2, 0, Rotation);
            ringFront2.Rotate(OsbEasing.Out, duration / 2, duration / 2 + 400, Rotation, 0);
            ringFront2.EndGroup();

            // back
            avatarBack.StartLoopGroup(startTime + Delay + duration, LoopCount);
            avatarBack.MoveY(0, posY);
            avatarBack.Fade(0, 3500, 0, 0.7);
            avatarBack.MoveX(OsbEasing.In, 0, duration / 2, posX.Y, posX.X / 2);
            avatarBack.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.X);
            avatarBack.ScaleVec(OsbEasing.Out, 0, duration / 2, 0, ScaleBack.Y * 2, ScaleBack.X / 2, ScaleBack.Y / 2);
            avatarBack.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleBack.X / 2, ScaleBack.Y / 2, 0, ScaleBack.Y * 2);
            avatarBack.ScaleVec(duration * 2, 0, ScaleBack.Y * 2);
            avatarBack.Fade(duration - 3500, duration, 0.7, 0);
            avatarBack.Color(0, PassiveColor);
            avatarBack.EndGroup();

            ringBack.StartLoopGroup(startTime + Delay + duration, LoopCount);
            ringBack.MoveY(0, posY);
            ringBack.Fade(1000, 3500, 0, 0.7);
            ringBack.MoveX(OsbEasing.In, 0, duration / 2, posX.Y, posX.X / 2);
            ringBack.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.X);
            ringBack.ScaleVec(OsbEasing.Out, 0, duration / 2, 0, ScaleBack.Y * 2, ScaleBack.X / 2, ScaleBack.Y / 2);
            ringBack.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleBack.X / 2, ScaleBack.Y / 2, 0, ScaleBack.Y * 2);
            ringBack.ScaleVec(duration * 2, 0, ScaleBack.Y * 2);
            ringBack.Fade(duration - 3500, duration - 1000, 0.7, 0);
            ringBack.Color(0, PassiveColor);
            ringBack.EndGroup();
        }

        public void Otosaka_Yu(int startTime, int duration, int Delay, float Rotation, float posY, Vector2 posX, float ScalePassive, float ScaleActive, int LoopCount)
        {
            var nameTag = GetLayer("nameTag").CreateSprite("sb/avatars/Otosaka-YuTag.png", OsbOrigin.Centre);
            var ringFront = GetLayer("ringFront").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var ringFront2 = GetLayer("ringFront").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var ringBack = GetLayer("ringBack").CreateSprite("sb/ring.png", OsbOrigin.Centre);
            var avatarFront = GetLayer("avatarFront").CreateSprite("sb/avatars/Otosaka-YuProfile.png", OsbOrigin.Centre);
            var avatarFront2 = GetLayer("avatarFront").CreateSprite("sb/avatars/Otosaka-YuProfile.png", OsbOrigin.Centre);
            var avatarBack = GetLayer("avatarBack").CreateSprite("sb/avatars/Otosaka-YuProfile.png", OsbOrigin.Centre);
            var ScaleBack = new Vector2(ScalePassive / 2, ScalePassive / 2);

            // front
            avatarFront.StartLoopGroup(startTime + Delay, LoopCount);
            avatarFront.MoveY(0, posY);
            avatarFront.Fade(0, 3500, 0, 1);
            avatarFront.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            avatarFront.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            avatarFront.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            avatarFront.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            avatarFront.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            avatarFront.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            avatarFront.ScaleVec(duration * 2, 0, ScalePassive);
            avatarFront.Fade(duration - 3500, duration, 1, 0);
            avatarFront.EndGroup();

            nameTag.StartLoopGroup(startTime + Delay, LoopCount);
            nameTag.MoveY(0, posY + 75);
            nameTag.Fade(3000, 4000, 0, 1);
            nameTag.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            nameTag.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            nameTag.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            nameTag.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            nameTag.ScaleVec(OsbEasing.InOutSine, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            nameTag.ScaleVec(OsbEasing.InOutSine, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            nameTag.ScaleVec(duration * 2, 0, ScalePassive);
            nameTag.Fade(duration - 4000, duration - 3000, 1, 0);
            nameTag.EndGroup();

            ringFront.StartLoopGroup(startTime + Delay, LoopCount);
            ringFront.MoveY(startTime, posY);
            ringFront.Fade(0, 3500, 0, 1);
            ringFront.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            ringFront.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            ringFront.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            ringFront.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            ringFront.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            ringFront.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            ringFront.ScaleVec(duration * 2, 0, ScalePassive);
            ringFront.Fade(duration - 3500, duration, 1, 0);
            ringFront.Rotate(OsbEasing.InOutSine, duration / 4, duration / 2, 0, Rotation);
            ringFront.Rotate(OsbEasing.Out, duration / 2, duration / 2 + 400, Rotation, 0);
            ringFront.EndGroup();

            // reflection
            avatarFront2.StartLoopGroup(startTime + Delay, LoopCount);
            avatarFront2.FlipV(0, duration);
            avatarFront2.MoveY(0, posY + 180);
            avatarFront2.Fade(0, 500, 0, 0.05);
            avatarFront2.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            avatarFront2.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            avatarFront2.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            avatarFront2.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            avatarFront2.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            avatarFront2.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            avatarFront2.ScaleVec(duration * 2, 0, ScalePassive);
            avatarFront2.Fade(duration - 500, duration, 0.05, 0);
            avatarFront2.EndGroup();

            ringFront2.StartLoopGroup(startTime + Delay, LoopCount);
            ringFront2.FlipV(0, duration / 2 + 400);
            ringFront2.MoveY(startTime, posY + 180);
            ringFront2.Fade(0, 500, 0, 0.05);
            ringFront2.MoveX(OsbEasing.In, 0, duration / 2, posX.X, posX.X / 2);
            ringFront2.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.Y);
            ringFront2.Color(OsbEasing.In, 0, duration / 2, PassiveColor, ActiveColor);
            ringFront2.Color(OsbEasing.Out, duration / 2, duration, ActiveColor, PassiveColor);
            ringFront2.ScaleVec(OsbEasing.Out, 0, duration / 2, 0.05, ScalePassive, ScaleActive, ScaleActive);
            ringFront2.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleActive, ScaleActive, 0, ScalePassive);
            ringFront2.ScaleVec(duration * 2, 0, ScalePassive);
            ringFront2.Fade(duration - 500, duration, 0.05, 0);
            ringFront2.Rotate(OsbEasing.InOutSine, duration / 4, duration / 2, 0, Rotation);
            ringFront2.Rotate(OsbEasing.Out, duration / 2, duration / 2 + 400, Rotation, 0);
            ringFront2.EndGroup();

            // back
            avatarBack.StartLoopGroup(startTime + Delay + duration, LoopCount);
            avatarBack.MoveY(0, posY);
            avatarBack.Fade(0, 3500, 0, 0.7);
            avatarBack.MoveX(OsbEasing.In, 0, duration / 2, posX.Y, posX.X / 2);
            avatarBack.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.X);
            avatarBack.ScaleVec(OsbEasing.Out, 0, duration / 2, 0, ScaleBack.Y * 2, ScaleBack.X / 2, ScaleBack.Y / 2);
            avatarBack.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleBack.X / 2, ScaleBack.Y / 2, 0, ScaleBack.Y * 2);
            avatarBack.ScaleVec(duration * 2, 0, ScaleBack.Y * 2);
            avatarBack.Fade(duration - 3500, duration, 0.7, 0);
            avatarBack.Color(0, PassiveColor);
            avatarBack.EndGroup();

            ringBack.StartLoopGroup(startTime + Delay + duration, LoopCount);
            ringBack.MoveY(0, posY);
            ringBack.Fade(1000, 3500, 0, 0.7);
            ringBack.MoveX(OsbEasing.In, 0, duration / 2, posX.Y, posX.X / 2);
            ringBack.MoveX(OsbEasing.Out, duration / 2, duration, posX.X / 2, posX.X);
            ringBack.ScaleVec(OsbEasing.Out, 0, duration / 2, 0, ScaleBack.Y * 2, ScaleBack.X / 2, ScaleBack.Y / 2);
            ringBack.ScaleVec(OsbEasing.In, duration / 2, duration, ScaleBack.X / 2, ScaleBack.Y / 2, 0, ScaleBack.Y * 2);
            ringBack.ScaleVec(duration * 2, 0, ScaleBack.Y * 2);
            ringBack.Fade(duration - 3500, duration - 1000, 0.7, 0);
            ringBack.Color(0, PassiveColor);
            ringBack.EndGroup();
        }
    }
}

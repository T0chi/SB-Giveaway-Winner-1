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
    public class Section3 : StoryboardObjectGenerator
    {
        [Configurable]
        public Color4 AvatarColor = Color4.White;

        [Configurable]
        public Color4 PetalColorMin = Color4.White;

        [Configurable]
        public Color4 PetalColorMax = Color4.Cyan;

        public override void Generate()
        {
            Background();
            Dialog();
            Petals(128262, 157062);
            Mission(128262, 157062);
            Tochi(119905, 137262);
            HUD(115238, 157062, 128262, "Mission #2", "Adaptation Window", "sb/HUD/txt/nameTag/Acyl.png", 4500, "sb/avatars/AcylProfile.png");
        }

        public void Background()
        {
            var bitmap = GetMapsetBitmap("sb/bgs/3/bg.jpg");
            var bg = GetLayer("Background").CreateSprite("sb/bgs/3/bg.jpg", OsbOrigin.Centre, new Vector2(320, 240));

            bg.Scale(115238, 480.0f / bitmap.Height);
            bg.Fade(115238, 128262, 0, 0.3);
            bg.Fade(157062, 167640, 0.3, 0);
        }

        public void HUD(int startTime, int endTime, int loadingTextEndtime, string mission, string songName, string nameTag, int progressBarDelay, string avatar)
        {
            var hud = new HUD(this, startTime, endTime, loadingTextEndtime, mission, songName, nameTag, progressBarDelay, avatar);
        }

        public void Mission(int startTime, int endTime)
        {
            var mission = new Mission2(this, startTime, endTime);


            // Item Collect

            var sTime = 128262;
            var eTime = 157062;
            var itemVelocity = Random(2000, 6000);
            var hoverDuration = 2000;
            var prevPosition = 620;
            var loopAmount = (eTime - sTime) / (hoverDuration * 2);
            var avatar = GetLayer("Avatar").CreateSprite("sb/avatars/Acyl.png", OsbOrigin.BottomCentre);

            for (var i = sTime; i < eTime; i += itemVelocity)
            {
                var itemDuration = itemVelocity;
                Log(itemDuration.ToString());

                var book = GetLayer("Items").CreateSprite("sb/missions/items/1.png", OsbOrigin.Centre); // book
                var coins = GetLayer("Items").CreateSprite("sb/missions/items/2.png", OsbOrigin.Centre); // coins
                var diamond = GetLayer("Items").CreateSprite("sb/missions/items/3.png", OsbOrigin.Centre); // diamond
                var gold_coins = GetLayer("Items").CreateSprite("sb/missions/items/4.png", OsbOrigin.Centre); // gold&coins
                var relic = GetLayer("Items").CreateSprite("sb/missions/items/5.png", OsbOrigin.Centre); // relic
                var silverBar = GetLayer("Items").CreateSprite("sb/missions/items/6.png", OsbOrigin.Centre); // silverBar
                var anyItem = GetLayer("Items").CreateSprite("sb/missions/items/" + Random(1, 7) + ".png", OsbOrigin.Centre); // any items

                // ITEMS /////////////////////////////////////////////////////////////////////
                var moveX = Random(0, 640);
                var moveY = Random(380, 450);
                anyItem.Scale(i, Random(0.5, 0.6));
                anyItem.Fade(i, i + 500, 0, 1);
                anyItem.Fade(i + itemDuration - 500, i + itemDuration + 1300, 1, 0);
                anyItem.Move(OsbEasing.InOutQuad, i + itemDuration - 500, i + itemDuration + 1300, new Vector2(moveX, moveY), new Vector2(moveX, moveY - 200));
                anyItem.Color(i + itemDuration - 500, i + itemDuration + 1300, AvatarColor, Color4.Red);

                    var d = (eTime - sTime);
                    var itemDuration2 = (Random(d / 10, d)) + 7000;

                    book.Color(sTime, AvatarColor);
                    book.Scale(sTime, Random(0.3, 0.6));
                    book.Fade(sTime, sTime + 500, 0, 1);
                    book.Fade(sTime + itemDuration2 - 500, sTime + itemDuration2, 1, 0);
                    book.Move(sTime, new Vector2(Random(0, 640), Random(380, 450)));

                    coins.Color(sTime, AvatarColor);
                    coins.Scale(sTime, Random(0.3, 0.6));
                    coins.Fade(sTime, sTime + 500, 0, 1);
                    coins.Fade(sTime + itemDuration2 - 500, sTime + itemDuration2, 1, 0);
                    coins.Move(sTime, new Vector2(Random(0, 640), Random(380, 450)));

                    diamond.Color(sTime, AvatarColor);
                    diamond.Scale(sTime, Random(0.3, 0.6));
                    diamond.Fade(sTime, sTime + 500, 0, 1);
                    diamond.Fade(sTime + itemDuration2 - 500, sTime + itemDuration2, 1, 0);
                    diamond.Move(sTime, new Vector2(Random(0, 640), Random(380, 450)));

                    gold_coins.Color(sTime, AvatarColor);
                    gold_coins.Scale(sTime, Random(0.3, 0.6));
                    gold_coins.Fade(sTime, sTime + 500, 0, 1);
                    gold_coins.Fade(sTime + itemDuration2 - 500, sTime + itemDuration2, 1, 0);
                    gold_coins.Move(sTime, new Vector2(Random(0, 640), Random(380, 450)));

                    relic.Color(sTime, AvatarColor);
                    relic.Scale(sTime, Random(0.3, 0.6));
                    relic.Fade(sTime, sTime + 500, 0, 1);
                    relic.Fade(sTime + itemDuration2 - 500, sTime + itemDuration2, 1, 0);
                    relic.Move(sTime, new Vector2(Random(0, 640), Random(380, 450)));

                    silverBar.Color(sTime, AvatarColor);
                    silverBar.Scale(sTime, Random(0.3, 0.6));
                    silverBar.Fade(sTime, sTime + 500, 0, 1);
                    silverBar.Fade(sTime + itemDuration2 - 500, sTime + itemDuration2, 1, 0);
                    silverBar.Move(sTime, new Vector2(Random(0, 640), Random(380, 450)));
                // ///////////////////////////////////////////////////////////////////////////

                // AVATAR
                avatar.Scale(sTime, 0.25);
                avatar.Color(sTime, AvatarColor);
                avatar.Fade(sTime, sTime + 1000, 0, 1);
                avatar.Fade(eTime + 4000 - 1000, eTime + 4000, 1, 0);
                // avatar hovering
                avatar.StartLoopGroup(sTime, loopAmount);
                avatar.MoveY(OsbEasing.InOutSine, 0, hoverDuration, 470, 470 + 15);
                avatar.MoveY(OsbEasing.InOutSine, hoverDuration, hoverDuration * 2, 470 + 15, 470);
                avatar.EndGroup();

                // avatar vertical movement
                var nexItemPosX = anyItem.PositionAt(i + itemDuration).X;
                avatar.MoveX(OsbEasing.InOutSine, i - 500, i - 500 + itemDuration, prevPosition, nexItemPosX);

                // flipH
                if (i > 132312)
                {
                    if (prevPosition < nexItemPosX)
                    {
                        avatar.FlipH(i - 500, i - 500 + itemDuration);
                    }
                }


                // OBTAIN EFFECT /////////////////////////////////////////////////////////////////////
                var Amount = 10;
                var nexItemPosY = anyItem.PositionAt(i + itemDuration).Y;
                for (var o = 0; o < Amount; o++)
                {
                    var posY = nexItemPosY + 30;
                    var posStartX = Random(nexItemPosX - 5, nexItemPosX + 5);
                    var posEndX = Random(nexItemPosX - 20, nexItemPosX + 20);
                    var sprite = GetLayer("Items").CreateSprite("sb/particle2.png", OsbOrigin.Centre);
                    var light = GetLayer("Items").CreateSprite("sb/light.png", OsbOrigin.CentreLeft);

                    var randomMoveX = Random(-20, 20);
                    var randomMoveX2 = Random(-25, 25);
                    var randomFadeOut = Random(300, 1300);
                    var randomScaleOut = Random(0.003, 0.006);

                    sprite.Fade(i + itemDuration - 1000, i + itemDuration + randomFadeOut, 1, 0);
                    sprite.Additive(i + itemDuration - 1000, i + itemDuration +  + randomFadeOut);
                    sprite.Color(i + itemDuration - 1000, i + itemDuration, Color4.Red, AvatarColor);
                    sprite.MoveX(i + itemDuration - 1000, i + itemDuration - 500, posStartX, posStartX);
                    sprite.MoveY(i + itemDuration - 1000, i + itemDuration - 500, posY, posY);
                    sprite.MoveY(OsbEasing.InOutQuad, i + itemDuration - 500, i + itemDuration + randomFadeOut, posY, posY - Random(150, 200));
                    // end move loop
                    sprite.StartLoopGroup(i + itemDuration - 500, 4);
                    sprite.MoveX(OsbEasing.InOutSine, 0, (500 + randomFadeOut) / 4, posStartX + randomMoveX, posEndX + randomMoveX2);
                    sprite.MoveX(OsbEasing.InOutSine, (500 + randomFadeOut) / 4, (500 + randomFadeOut) / 3, posEndX + randomMoveX2, posStartX + randomMoveX);
                    sprite.MoveX(OsbEasing.InOutSine, (500 + randomFadeOut) / 3, (500 + randomFadeOut) / 2, posStartX + randomMoveX, posEndX + randomMoveX2);
                    sprite.MoveX(OsbEasing.InOutSine, (500 + randomFadeOut) / 2, (500 + randomFadeOut) / 1, posEndX + randomMoveX2, posStartX + randomMoveX);
                    sprite.EndGroup();
                    // end loop
                    sprite.ScaleVec(OsbEasing.OutExpo, i + itemDuration - 1000, i + itemDuration - 500, Random(0.1, 0.2), Random(0.2, 0.3), 0.4, 0.3);
                    sprite.ScaleVec(i + itemDuration - 500, i + itemDuration +  + randomFadeOut, 0.4, 0.3, randomScaleOut, randomScaleOut);

                    light.Move(i + itemDuration - 500, nexItemPosX, nexItemPosY);
                    light.Fade(i + itemDuration - 500, i + itemDuration, 0, 0.05);
                    light.Fade(i + itemDuration + randomFadeOut, i + itemDuration + randomFadeOut + 500, 0.05, 0);
                    light.ScaleVec(OsbEasing.OutExpo, i + itemDuration - 500, i + itemDuration + randomFadeOut - 500, 0.01, 0.01, Random(0.2, 0.28), 0.2);
                    light.ScaleVec(OsbEasing.In, i + itemDuration + randomFadeOut - 500, i + itemDuration + randomFadeOut + 100, Random(0.2, 0.28), 0.2, Random(0.2, 0.28), 0.005);
                    light.Additive(i + itemDuration - 500, i + itemDuration + randomFadeOut + 500);

                    var rotation = MathHelper.DegreesToRadians(-90);
                    light.Rotate(i + itemDuration - 500, rotation);

                    // sound effect
                    var obtainSFX = GetLayer("Items").CreateSample("sb/sfx/obtain-item.wav", i + itemDuration - 500, 10);

                }
                // ///////////////////////////////////////////////////////////////////////////////////

                prevPosition = (int)nexItemPosX;
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
            string[] sentence = { "What a relief! Did you enjoy your first quest?",
                                  "Well, no worry. You will have to do something easier for this section.",
                                  "Which is... to obtain a few items from the ground." };
            var dialog = new DialogManager(this, font, 119905, 131862, "-Tochi", 105, 326, false,
                fontSize, 1, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.wav",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "Easy right?",
                                   "Good luck, fellow player!" };
            var dialog2 = new DialogManager(this, font, 133662, 137262, "-Tochi", 105, 326, false,
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

        public void Petals(int startTime, int endTime)
        {
            var interval = 200;
            for (var i = startTime; i < endTime; i += interval)
            {
                var randomEasingX = true;
                var randomEasingY = true;
                var easingX = randomEasingX ? OsbEasing.OutQuad : OsbEasing.None;
                var easingY = randomEasingY ? OsbEasing.None : OsbEasing.Out;

                var duration = Random(6000, 20000);
                var sPosFront = new Vector2(Random(-127, -117), Random(-20, -10));
                var ePosFront = new Vector2(Random(-90, 640), Random(370, 470));
                var sPosBack = new Vector2(Random(-128, -118), Random(-21, -11));
                var ePosBack = new Vector2(Random(-91, 641), Random(371, 471));
                var Rotation = Random(MathHelper.DegreesToRadians(-180), MathHelper.DegreesToRadians(180));

                var petalFront = GetLayer("PetalsFront").CreateSprite("sb/p.png", OsbOrigin.Centre); // foreground
                var petalBack = GetLayer("PetalsBack").CreateSprite("sb/p.png", OsbOrigin.Centre); // background
                
                petalFront.MoveX(easingX, i, i + duration, sPosFront.X, ePosFront.X);
                petalFront.MoveY(easingY, i, i + duration, sPosFront.Y, ePosFront.Y);
                petalBack.MoveX(easingX, i, i + duration, sPosBack.X, ePosBack.X);
                petalBack.MoveY(easingY, i, i + duration, sPosBack.Y, ePosBack.Y);
                
                var Scale = Random(0.1, 0.05);
                petalFront.ScaleVec(i, i + (duration / 4), Scale, Scale, -Scale, Scale);
                petalFront.ScaleVec(i + (duration / 4), i + (duration / 3), -Scale, Scale, Scale, Scale);
                petalFront.ScaleVec(i + (duration / 3), i + (duration / 2), Scale, Scale, -Scale, Scale);
                petalFront.ScaleVec(i + (duration / 2), i + (duration / 1), -Scale, Scale, Scale, Scale);
                petalBack.ScaleVec(i, i + (duration / 4), Scale, Scale, -Scale, Scale);
                petalBack.ScaleVec(i + (duration / 4), i + (duration / 3), -Scale, Scale, Scale, Scale);
                petalBack.ScaleVec(i + (duration / 3), i + (duration / 2), Scale, Scale, -Scale, Scale);
                petalBack.ScaleVec(i + (duration / 2), i + (duration / 1), -Scale, Scale, Scale, Scale);

                // Fade stuff
                var Fade = Random(0.4, 0.08);
                petalFront.Fade(OsbEasing.InExpo, i, i + Random(2000, 4000), 0, Fade);
                petalBack.Fade(OsbEasing.InExpo, i, i + Random(2000, 4000), 0, Fade);
                if (i + duration - 2000 <= endTime)
                {
                    petalFront.Fade(i + duration - Random(500, 2000), i + duration, Fade, 0);
                    petalBack.Fade(i + duration - Random(500, 2000), i + duration, Fade, 0);
                }

                else
                {
                    petalFront.Fade(endTime, endTime + 1000, Fade, 0);
                    petalBack.Fade(endTime, endTime + 1000, Fade, 0);
                }

                petalFront.Additive(i, i + duration);
                petalFront.Rotate(i, i + duration, Rotation, Rotation + Random(-50, 50));
                petalBack.Additive(i, i + duration);
                petalBack.Rotate(i, i + duration, Rotation, Rotation + Random(-50, 50));
                
                var RandomColor = true;
                var Color = RandomColor ? new Color4((float)Random(PetalColorMin.R, PetalColorMax.R),
                                                        (float)Random(PetalColorMin.G, PetalColorMax.G),
                                                        (float)Random(PetalColorMin.B, PetalColorMax.B),
                                                        255
                                                        ) : PetalColorMin;
                petalFront.Color(i, Color);
                petalBack.Color(i, Color);
            }
        }
    }
}

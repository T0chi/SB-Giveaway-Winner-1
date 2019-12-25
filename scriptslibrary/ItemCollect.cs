using OpenTK;
using System;
using System.Linq;
using System.Drawing;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;
using System.Collections.Generic;

public class ItemCollect
{
    private StoryboardObjectGenerator generator;

    private string avatarPath;
    private float avatarScale;
    private int avatarEndDelay;
    private float startX;
    private float y;
    private string itemsPath;
    private float itemScale;
    private int yMin;
    private int yMax;
    private bool FrontAndBack;
    private int startTime;
    private int endTime;
    private Color4 ThemeColor;
    private int spawnMin;
    private int spawnMax;

    public ItemCollect(StoryboardObjectGenerator generator, string avatarPath, float avatarScale, int avatarEndDelay, float startX, float y, string itemsPath, float itemScale, int yMin, int yMax, bool FrontAndBack, int startTime, int endTime, Color4 ThemeColor, int spawnMin, int spawnMax)
    {
        this.generator = generator;

        this.avatarPath = avatarPath;
        this.avatarScale = avatarScale;
        this.avatarEndDelay = avatarEndDelay;
        this.startX = startX;
        this.y = y;
        this.itemsPath = itemsPath;
        this.itemScale = itemScale;
        this.yMin = yMin;
        this.yMax = yMax;
        this.FrontAndBack = FrontAndBack;
        this.startTime = startTime;
        this.endTime = endTime;
        this.ThemeColor = ThemeColor;
        this.spawnMin = spawnMin;
        this.spawnMax = spawnMax;

        Generate(avatarPath, avatarScale, avatarEndDelay, startX, y, itemsPath, itemScale, yMin, yMax, FrontAndBack, startTime, endTime, ThemeColor, spawnMin, spawnMax);
    }

    public void Generate(string avatarPath, float avatarScale, int avatarEndDelay, float startX, float y, string itemsPath, float itemScale, int yMin, int yMax, bool FrontAndBack, int startTime, int endTime, Color4 ThemeColor, int spawnMin, int spawnMax)
    {
        // Item Collect

        var sTime = startTime;
        var eTime = endTime;
        var itemVelocity = generator.Random(spawnMin, spawnMax);
        var hoverDuration = 2000;
        var prevPosition = startX;
        var loopAmount = (eTime - sTime) / (hoverDuration * 2);
        var avatar = generator.GetLayer("Avatar").CreateSprite(avatarPath, OsbOrigin.BottomCentre);

        for (var i = sTime; i < eTime; i += itemVelocity)
        {
            var FlipHVelocity = generator.Random(1, 3);
            var itemDuration = itemVelocity;
            generator.Log(itemDuration.ToString());

            var itemThree = generator.GetLayer("Items").CreateSprite("sb/missions/" + itemsPath + "/3.png", OsbOrigin.Centre);
            var itemFour = generator.GetLayer("Items").CreateSprite("sb/missions/" + itemsPath + "/4.png", OsbOrigin.Centre);
            var itemFive = generator.GetLayer("Items").CreateSprite("sb/missions/" + itemsPath + "/5.png", OsbOrigin.Centre);
            var itemSix = generator.GetLayer("Items").CreateSprite("sb/missions/" + itemsPath + "/6.png", OsbOrigin.Centre);
            var anyItem = generator.GetLayer("Items").CreateSprite("sb/missions/" + itemsPath + "/" + generator.Random(1, 7) + ".png", OsbOrigin.Centre); // any items

            // ITEMS /////////////////////////////////////////////////////////////////////
            var moveX = generator.Random(0, 640);
            var moveY = generator.Random(yMin, yMax);
            anyItem.Scale(i, generator.Random(0.5, 0.6) * itemScale);
            anyItem.Fade(i, i + 500, 0, 1);
            anyItem.Fade(i + itemDuration - 500, i + itemDuration + 1300, 1, 0);
            anyItem.Move(OsbEasing.InOutQuad, i + itemDuration - 500, i + itemDuration + 1300, new Vector2(moveX, moveY), new Vector2(moveX, moveY - 200));
            anyItem.Color(i + itemDuration - 500, i + itemDuration + 1300, ThemeColor, Color4.Red);

            if (i % FlipHVelocity == 1)
                {
                    anyItem.FlipH(i, i + itemDuration + 1300);
                }

            var d = (eTime - sTime);
            var itemDuration2 = (generator.Random(d / 10, d));
            
            if (FrontAndBack)
            {
                var itemOne = generator.GetLayer("ItemsFront").CreateSprite("sb/missions/" + itemsPath + "/" + generator.Random(1, 7) + ".png", OsbOrigin.Centre);
                var itemTwo = generator.GetLayer("ItemsBack").CreateSprite("sb/missions/" + itemsPath + "/" + generator.Random(1, 7) + ".png", OsbOrigin.Centre);
                
                // front
                itemOne.Color(sTime, ThemeColor);
                itemOne.Scale(sTime, generator.Random(0.3, 0.6) * itemScale);
                itemOne.Fade(sTime, sTime + 500, 0, 1);
                itemOne.Fade(sTime + itemDuration2 - 500, sTime + itemDuration2, 1, 0);
                itemOne.Move(sTime, new Vector2(generator.Random(0, 640), generator.Random((int)y, yMax)));

                // back
                itemTwo.Color(sTime, ThemeColor);
                itemTwo.Scale(sTime, generator.Random(0.3, 0.6) * itemScale);
                itemTwo.Fade(sTime, sTime + 500, 0, 1);
                itemTwo.Fade(sTime + itemDuration2 - 500, sTime + itemDuration2, 1, 0);
                itemTwo.Move(sTime, new Vector2(generator.Random(0, 640), generator.Random(yMin, (int)y)));

                if (i % FlipHVelocity == 1)
                {
                    itemOne.FlipH(sTime, sTime + itemDuration2);
                    itemTwo.FlipH(sTime, sTime + itemDuration2);
                }
            }
            
            else
            {
                var itemOne = generator.GetLayer("Items").CreateSprite("sb/missions/" + itemsPath + "/1.png", OsbOrigin.Centre);
                var itemTwo = generator.GetLayer("Items").CreateSprite("sb/missions/" + itemsPath + "/2.png", OsbOrigin.Centre);

                itemOne.Color(sTime, ThemeColor);
                itemOne.Scale(sTime, generator.Random(0.3, 0.6) * itemScale);
                itemOne.Fade(sTime, sTime + 500, 0, 1);
                itemOne.Fade(sTime + itemDuration2 - 500, sTime + itemDuration2, 1, 0);
                itemOne.Move(sTime, new Vector2(generator.Random(0, 640), generator.Random(yMin, yMax)));

                itemTwo.Color(sTime, ThemeColor);
                itemTwo.Scale(sTime, generator.Random(0.3, 0.6) * itemScale);
                itemTwo.Fade(sTime, sTime + 500, 0, 1);
                itemTwo.Fade(sTime + itemDuration2 - 500, sTime + itemDuration2, 1, 0);
                itemTwo.Move(sTime, new Vector2(generator.Random(0, 640), generator.Random(yMin, yMax)));

                itemThree.Color(sTime, ThemeColor);
                itemThree.Scale(sTime, generator.Random(0.3, 0.6) * itemScale);
                itemThree.Fade(sTime, sTime + 500, 0, 1);
                itemThree.Fade(sTime + itemDuration2 - 500, sTime + itemDuration2, 1, 0);
                itemThree.Move(sTime, new Vector2(generator.Random(0, 640), generator.Random(yMin, yMax)));

                itemFour.Color(sTime, ThemeColor);
                itemFour.Scale(sTime, generator.Random(0.3, 0.6) * itemScale);
                itemFour.Fade(sTime, sTime + 500, 0, 1);
                itemFour.Fade(sTime + itemDuration2 - 500, sTime + itemDuration2, 1, 0);
                itemFour.Move(sTime, new Vector2(generator.Random(0, 640), generator.Random(yMin, yMax)));

                itemFive.Color(sTime, ThemeColor);
                itemFive.Scale(sTime, generator.Random(0.3, 0.6) * itemScale);
                itemFive.Fade(sTime, sTime + 500, 0, 1);
                itemFive.Fade(sTime + itemDuration2 - 500, sTime + itemDuration2, 1, 0);
                itemFive.Move(sTime, new Vector2(generator.Random(0, 640), generator.Random(yMin, yMax)));

                itemSix.Color(sTime, ThemeColor);
                itemSix.Scale(sTime, generator.Random(0.3, 0.6) * itemScale);
                itemSix.Fade(sTime, sTime + 500, 0, 1);
                itemSix.Fade(sTime + itemDuration2 - 500, sTime + itemDuration2, 1, 0);
                itemSix.Move(sTime, new Vector2(generator.Random(0, 640), generator.Random(yMin, yMax)));

                if (i % FlipHVelocity == 1)
                {
                    itemOne.FlipH(sTime, sTime + itemDuration2); 
                    itemTwo.FlipH(sTime, sTime + itemDuration2);
                    itemThree.FlipH(sTime, sTime + itemDuration2);
                    itemFour.FlipH(sTime, sTime + itemDuration2);
                    itemFive.FlipH(sTime, sTime + itemDuration2);
                    itemSix.FlipH(sTime, sTime + itemDuration2);
                }
            }
            // ///////////////////////////////////////////////////////////////////////////

            // AVATAR
            avatar.Scale(sTime, avatarScale);
            avatar.Color(sTime, ThemeColor);
            avatar.Fade(sTime, sTime + 1000, 0, 1);
            avatar.Fade(eTime + avatarEndDelay - 1000, eTime + avatarEndDelay, 1, 0);
            // avatar hovering
            avatar.StartLoopGroup(sTime, loopAmount);
            avatar.MoveY(OsbEasing.InOutSine, 0, hoverDuration, y, y + 15);
            avatar.MoveY(OsbEasing.InOutSine, hoverDuration, hoverDuration * 2, y + 15, y);
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
                var posStartX = generator.Random(nexItemPosX - 5, nexItemPosX + 5);
                var posEndX = generator.Random(nexItemPosX - 20, nexItemPosX + 20);
                var sprite = generator.GetLayer("Items").CreateSprite("sb/particle2.png", OsbOrigin.Centre);
                var light = generator.GetLayer("Items").CreateSprite("sb/light.png", OsbOrigin.CentreLeft);

                var randomMoveX = generator.Random(-20, 20);
                var randomMoveX2 = generator.Random(-25, 25);
                var randomFadeOut = generator.Random(300, 1300);
                var randomScaleOut = generator.Random(0.003, 0.006);

                sprite.Fade(i + itemDuration - 1000, i + itemDuration + randomFadeOut, 1, 0);
                sprite.Additive(i + itemDuration - 1000, i + itemDuration + +randomFadeOut);
                sprite.Color(i + itemDuration - 1000, i + itemDuration, Color4.Red, ThemeColor);
                sprite.MoveX(i + itemDuration - 1000, i + itemDuration - 500, posStartX, posStartX);
                sprite.MoveY(i + itemDuration - 1000, i + itemDuration - 500, posY, posY);
                sprite.MoveY(OsbEasing.InOutQuad, i + itemDuration - 500, i + itemDuration + randomFadeOut, posY, posY - generator.Random(150, 200));
                // end move loop
                sprite.StartLoopGroup(i + itemDuration - 500, 4);
                sprite.MoveX(OsbEasing.InOutSine, 0, (500 + randomFadeOut) / 4, posStartX + randomMoveX, posEndX + randomMoveX2);
                sprite.MoveX(OsbEasing.InOutSine, (500 + randomFadeOut) / 4, (500 + randomFadeOut) / 3, posEndX + randomMoveX2, posStartX + randomMoveX);
                sprite.MoveX(OsbEasing.InOutSine, (500 + randomFadeOut) / 3, (500 + randomFadeOut) / 2, posStartX + randomMoveX, posEndX + randomMoveX2);
                sprite.MoveX(OsbEasing.InOutSine, (500 + randomFadeOut) / 2, (500 + randomFadeOut) / 1, posEndX + randomMoveX2, posStartX + randomMoveX);
                sprite.EndGroup();
                // end loop
                sprite.ScaleVec(OsbEasing.OutExpo, i + itemDuration - 1000, i + itemDuration - 500, generator.Random(0.1, 0.2), generator.Random(0.2, 0.3), 0.4, 0.3);
                sprite.ScaleVec(i + itemDuration - 500, i + itemDuration + +randomFadeOut, 0.4, 0.3, randomScaleOut, randomScaleOut);

                light.Move(i + itemDuration - 500, nexItemPosX, nexItemPosY);
                light.Fade(i + itemDuration - 500, i + itemDuration, 0, 0.05);
                light.Fade(i + itemDuration + randomFadeOut, i + itemDuration + randomFadeOut + 500, 0.05, 0);
                light.ScaleVec(OsbEasing.OutExpo, i + itemDuration - 500, i + itemDuration + randomFadeOut - 500, 0.01, 0.01, generator.Random(0.2, 0.28), 0.2);
                light.ScaleVec(OsbEasing.In, i + itemDuration + randomFadeOut - 500, i + itemDuration + randomFadeOut + 100, generator.Random(0.2, 0.28), 0.2, generator.Random(0.2, 0.28), 0.005);
                light.Additive(i + itemDuration - 500, i + itemDuration + randomFadeOut + 500);

                var rotation = MathHelper.DegreesToRadians(-90);
                light.Rotate(i + itemDuration - 500, rotation);

                // sound effect
                var obtainSFX = generator.GetLayer("Items").CreateSample("sb/sfx/obtain-item.ogg", i + itemDuration - 500, 10);

            }
            // ///////////////////////////////////////////////////////////////////////////////////

            prevPosition = (int)nexItemPosX;
        }
    }
}
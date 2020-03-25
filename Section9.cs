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
    public class Section9 : StoryboardObjectGenerator
    {
        [Configurable]
        public Color4 HitColor = Color4.Red;

        [Configurable]
        public Color4 particleColor = Color4.White;

        private DialogManager dialog;

        private DialogManager dialog2;

        private DialogManager dialog3;

        private DialogManager dialog4;

        public enum glitch { one, two };

        public override void Generate()
        {
            hud();
            Lights();
		    Dialog();
            Mission();
            Particles();
            Background();
            ParticlesIntense();
            Tochi(380218, 416214);

		    Glitch(429565, glitch.one);
		    Glitch(431450, glitch.two);
		    Glitch(435538, glitch.two);
		    Glitch(442084, glitch.one);
		    Glitch(443720, glitch.two);
		    Glitch(451902, glitch.one);
		    Glitch(461447, glitch.one);
		    Glitch(469902, glitch.one);
		    Glitch(475902, glitch.one);

		    SystemError(384616);
		    SystemError(386187);
		    SystemError(388570);
		    SystemError(391083);
		    SystemError(393596);
		    SystemError(401135);
		    SystemError(404905);
		    SystemError(408675);
		    SystemError(410717);
		    SystemError(413701);
		    SystemError(417471);
		    SystemError(418727);
		    SystemError(423753);
            var sfxSiren = GetLayer("SystemError").CreateSample("sb/sfx/siren.ogg", 379590, 60);

		    SystemGlitch(477007, 479392);
		    GlitchIntense(479392, 489000);
		    ErrorMessage(428779, 479392);
            SystemScan(383360, 397994);

            HUD(379590, 488992, 387313, "Mission #8", "Ordirehv", "sb/HUD/txt/nameTag/Necho&Otosaka.png", 4500, "sb/avatars/Necho&OtosakaProfile.png");
        }

        public void SystemScan(int startTime, int endTime)
        {
            var delay = 500;

            var bg = GetLayer("Scanning").CreateSprite("sb/HUD/scanning/bg.png", OsbOrigin.Centre, new Vector2(320, 255));
            var screen = GetLayer("Scanning").CreateAnimation("sb/HUD/scanning/screen/sprite.png", 41, 100, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320, 320));
            var screen2 = GetLayer("Scanning").CreateAnimation("sb/HUD/scanning/screen2/sprite.png", 14, 160, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320, 180));
            var lines = GetLayer("Scanning").CreateAnimation("sb/HUD/scanning/lines/sprite.jpg", 31, 90, OsbLoopType.LoopForever, OsbOrigin.CentreRight, new Vector2(244, 180));
            var graph = GetLayer("Scanning").CreateAnimation("sb/HUD/scanning/graph/sprite.png", 38, 90, OsbLoopType.LoopForever, OsbOrigin.CentreLeft, new Vector2(345, 180));

            bg.Scale(startTime, 0.6f);
            bg.Fade(startTime, startTime + 500, 0, 1);
            bg.Fade(endTime - 500, endTime, 1, 0);

            screen.Scale(startTime + delay, 0.6f);
            screen.Additive(startTime + delay, endTime);
            screen.Fade(startTime + delay, startTime + delay + 500, 0, 0.7f);
            screen.Fade(endTime - delay - 500, endTime, 0.7f, 0);

            screen2.Scale(startTime + (delay * 2), 0.6f);
            screen2.Additive(startTime + (delay * 2), endTime);
            screen2.Fade(startTime + (delay * 2), startTime + (delay * 2) + 500, 0, 0.7f);
            screen2.Fade(endTime - (delay * 2) - 500, endTime, 0.7f, 0);

            lines.Scale(startTime + (delay * 3), 0.2f);
            lines.Additive(startTime + (delay * 3), endTime);
            lines.Fade(startTime + (delay * 3), startTime + (delay * 3) + 500, 0, 0.7f);
            lines.Fade(endTime - (delay * 3) - 500, endTime, 0.7f, 0);

            graph.Scale(startTime + (delay * 4), 0.4f);
            graph.Additive(startTime + (delay * 4), endTime);
            graph.Fade(startTime + (delay * 4), startTime + (delay * 4) + 500, 0, 0.7f);
            graph.Fade(endTime - (delay * 4) - 500, endTime, 0.7f, 0);
        }

        public void ErrorMessage(int startTime, int endTime)
        {
            var speed = 70;
            var sprite = GetLayer("Glitch").CreateAnimation("sb/glitch/error.jpg", 5, speed, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320, 260));
            
            sprite.Fade(startTime, startTime + 50, 0, 0.5f);
            sprite.Fade(endTime - 200, endTime, 0.5f, 0);
            sprite.Additive(startTime, endTime);
            sprite.Scale(startTime, 0.17f);

            var loopCount = (endTime - startTime) / 1600;
            var startRotation = MathHelper.DegreesToRadians(-5);
            var endRotation = MathHelper.DegreesToRadians(5);

            sprite.StartLoopGroup(startTime, loopCount);
            sprite.Rotate(OsbEasing.InOutElastic, 0, 800, startRotation, endRotation);
            sprite.Rotate(OsbEasing.InOutElastic, 800, 1600, endRotation, startRotation);
            sprite.EndGroup();
        }

        public void Glitch(int startTime, glitch glitchType)
        {
            var speed = 100;

            var bitmap = GetMapsetBitmap("sb/glitch/g1_1.jpg");

            var sfx = GetLayer("Glitch").CreateSample("sb/sfx/glitch" + Random(1, 4) + ".ogg", startTime, 70);

            var glitch = GetLayer("Glitch").CreateAnimation("sb/glitch/g1_.jpg", 4, speed, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320, 240));
            var glitch2 = GetLayer("Glitch").CreateAnimation("sb/glitch/g2_.jpg", 3, speed, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320, 240));
            
            if (glitchType == Section9.glitch.one)
            {
                glitch.Scale(startTime, 480.0f / bitmap.Height);

                glitch.StartLoopGroup(startTime, 1);
                glitch.Fade(0, speed * 4, 1, 1);
                glitch.EndGroup();
                glitch.Fade(startTime + (speed * 4), startTime + (speed * 5), 1, 0);
            }

            if (glitchType == Section9.glitch.two)
            {
                glitch2.Scale(startTime, 480.0f / bitmap.Height);

                glitch2.StartLoopGroup(startTime, 1);
                glitch2.Fade(0, speed * 3, 1, 1);
                glitch2.EndGroup();
                glitch2.Fade(startTime + (speed * 3), startTime + (speed * 4), 1, 0);
            }
        }

        public void SystemGlitch(int startTime, int endTime)
        {
            var speed = 100;
            var bitmap = GetMapsetBitmap("sb/glitch/system/glitch0.png");

            var glitch = GetLayer("Glitch").CreateAnimation("sb/glitch/system/glitch.png", 2, speed, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320, 240));
            glitch.Scale(478208, 480.0f / bitmap.Height);
            glitch.Fade(478208, endTime, 0.2, 0.2);
            glitch.Additive(478208, endTime);

            var window = GetLayer("Glitch").CreateAnimation("sb/glitch/system/window.jpg", 13, speed, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320, 380));
            window.Scale(startTime, 0.5f);
            window.Additive(startTime, endTime);

            var code = GetLayer("Glitch").CreateAnimation("sb/glitch/system/code.jpg", 6, speed, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320, 420));
            code.Fade(428799, 0.5f);
            code.Scale(428799, 0.3f);
            code.Additive(428799, 433372);
        }
        public void GlitchIntense(int startTime, int endTime)
        {
            var speed = 100;
            var bitmap = GetMapsetBitmap("sb/glitch/g1_1.jpg");

            for (int i = startTime; i < endTime; i += (speed * Random(4, 7)))
            {
                var glitch = GetLayer("Glitch").CreateAnimation("sb/glitch/g" + Random(1, 3) + "_.jpg", 4, speed, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320, 240));
            
                glitch.Scale(i, 480.0f / bitmap.Height);
                glitch.Fade(i, i + (speed * 4), 1, 1);
                glitch.Fade(i + (speed * 4), i + (speed * 5), 1, 0);
            }
        }

         public void SystemError(int startTime)
        {
            var speed = 100;

            var bitmap = GetMapsetBitmap("sb/glitch/g3_1.jpg");
            var sfx = GetLayer("SystemError").CreateSample("sb/sfx/glitch" + Random(1, 4) + ".ogg", startTime, 70);
            var glitch = GetLayer("SystemError").CreateAnimation("sb/glitch/g3_.jpg", 6, speed, OsbLoopType.LoopForever, OsbOrigin.Centre, new Vector2(320, 240));
            
            glitch.Scale(startTime, 854.0f / bitmap.Width);

            glitch.StartLoopGroup(startTime, 1);
            glitch.Fade(0, speed * 4, 0.5, 0.5);
            glitch.EndGroup();
            glitch.Fade(startTime + (speed * 4), startTime + (speed * 5), 0.5, 0);
        }
        public void Background()
        {
            var bitmap = GetMapsetBitmap("sb/bgs/9/bg.jpg");
            var bg = GetLayer("Background").CreateSprite("sb/bgs/9/bg.jpg", OsbOrigin.Centre, new Vector2(320, 240));

            bg.Scale(379590, 854.0f / bitmap.Width);
            bg.Fade(379590, 387313, 0, 0.5);
            bg.Fade(488992, 488992 + 2000, 0.5, 0);
        }

        public void HUD(int startTime, int endTime, int loadingTextEndtime, string mission, string songName, string nameTag, int progressBarDelay, string avatar)
        {
            var hud = new HUD(this, startTime, endTime, loadingTextEndtime, mission, songName, nameTag, progressBarDelay, avatar);
        }

        public void hud()
        {
            var speed = 50;
            var startTime = 428779;
            var endTime = 487792;
            var position = new Vector2(320, 260);

            for (int i = startTime; i < endTime; i += (speed * 50))
            {
                var bitmap = GetMapsetBitmap("sb/HUD/animation/ani0.jpg");
                var sprite = GetLayer("HUD - Intro").CreateAnimation("sb/HUD/animation/ani.jpg", 21, speed, OsbLoopType.LoopOnce, OsbOrigin.Centre);

                sprite.Additive(i, i + (speed * 50));
                sprite.Scale(i, 640.0f / bitmap.Width);
                sprite.Move(OsbEasing.OutElasticHalf, i, i + 500, position.X - 50, position.Y, position.X, position.Y);
                sprite.Rotate(OsbEasing.InOutBack, i, i + (speed * 50), MathHelper.DegreesToRadians(Random(-10, 10)), MathHelper.DegreesToRadians(Random(-10, 10)));

                sprite.Fade(i, i + (speed * 21), 0.5f, 0.5f);
                sprite.Fade(i + (speed * 48), i + (speed * 50), 0.5f, 0);
            }
        }

        public void Lights()
        {
            var startTime = 387313;
            var endTime = 491092;
            var duration = 4000;

            var light1 = GetLayer("Light").CreateSprite("sb/light2.png", OsbOrigin.TopCentre);
            var light2 = GetLayer("Light").CreateSprite("sb/light2.png", OsbOrigin.TopCentre);

            var Rotation = MathHelper.DegreesToRadians(10);
            
            light1.Fade(startTime, startTime + 1000, 0, 0.1);
            light1.Fade(endTime - 1000, endTime, 0.1, 0);
            light1.StartLoopGroup(startTime, (endTime - startTime) / duration);
            light1.ScaleVec(0, 0.5, 0.9);
            light1.Additive(0, duration * 2);
            light1.Move(0, new Vector2(407, -26));
            light1.Rotate(OsbEasing.InOutSine, 0, duration, 0, Rotation);
            light1.Rotate(OsbEasing.InOutSine, duration, duration * 2, Rotation, 0);
            light1.EndGroup();

            light2.Fade(startTime, startTime + 1000, 0, 0.05);
            light2.Fade(endTime - 1000, endTime, 0.05, 0);
            light2.StartLoopGroup(startTime - 1500, (endTime - (startTime - 1500)) / duration);
            light2.ScaleVec(0, 0.3, 0.5);
            light2.Additive(0, duration * 2);
            light2.Move(0, new Vector2(470, 150));
            light2.Rotate(OsbEasing.InOutSine, 0, duration, 0, Rotation);
            light2.Rotate(OsbEasing.InOutSine, duration, duration * 2, Rotation, 0);
            light2.EndGroup();
        }

        public void ParticlesIntense()
        {
            var startTime = 433356;
            var endTime = 488992;
            var timePerParticle = Random(20, 60);

            // blue particles
            for (var i = startTime; i < endTime; i += timePerParticle)
            {
                var duration = Random(500, 2000);
                var particleBack = GetLayer("Particles Back").CreateSprite("sb/particle2.png", OsbOrigin.Centre);
                var particleFront = GetLayer("Particles Front").CreateSprite("sb/particle2.png", OsbOrigin.Centre);

                var startPos = new Vector2(Random(-107, 747), Random(400, 500));
                var endPos = new Vector2(startPos.X + Random(-40, 40), Random(260, 360));

                var Fade = Random(0.05, 0.1);
                var fadeDuration = Random(100, 200);
                var Scale = Random(0.05, 0.1);

                // particles in the back
                particleBack.Fade(i, i + fadeDuration, 0, Fade);
                particleBack.Fade(i + duration - fadeDuration, i + duration, Fade, 0);
                particleBack.Move(i, i + duration, startPos, endPos);
                particleBack.Color(i, Color4.White);
                particleBack.Scale(i, Scale);
                particleBack.Additive(i, i + duration);

                // particles in the front
                particleFront.Fade(i, i + fadeDuration, 0, Fade);
                particleFront.Fade(i + duration - fadeDuration, i + duration, Fade, 0);
                particleFront.Move(i, i + duration, startPos, endPos);
                particleFront.Color(i, Color4.White);
                particleFront.Scale(i, Scale);
                particleFront.Additive(i, i + duration);
            }
        }

        public void Particles()
        {
            var startTime = 387313;
            var endTime = 488992;
            var timePerParticle = Random(50, 100);

            // blue particles
            for (var i = startTime; i < endTime; i += timePerParticle)
            {
                var duration = Random(5000, 10000);
                var particleBack = GetLayer("Particles Back").CreateSprite("sb/particle2.png", OsbOrigin.Centre);
                var particleFront = GetLayer("Particles Front").CreateSprite("sb/particle2.png", OsbOrigin.Centre);

                var startPos = new Vector2(Random(-107, 747), Random(400, 500));
                var endPos = new Vector2(startPos.X + Random(-40, 40), Random(260, 360));

                var Fade = Random(0.1, 0.5);
                var fadeDuration = Random(1000, 2000);
                var Scale = Random(0.008, 0.05);

                // particles in the back
                particleBack.Fade(i, i + fadeDuration, 0, Fade);
                particleBack.Fade(i + duration - fadeDuration, i + duration, Fade, 0);
                particleBack.Move(i, i + duration, startPos, endPos);
                particleBack.Color(i, particleColor);
                particleBack.Scale(i, Scale);

                // particles in the front
                particleFront.Fade(i, i + fadeDuration, 0, Fade);
                particleFront.Fade(i + duration - fadeDuration, i + duration, Fade, 0);
                particleFront.Move(i, i + duration, startPos, endPos);
                particleFront.Color(i, particleColor);
                particleFront.Scale(i, Scale);
            }

            // Light Ray particles
            var amount = Random(200, 400);
            for (var i = startTime; i < endTime; i += amount)
            {
                var duration = Random(5000, 10000);
                var lightParticles = GetLayer("Particles Back").CreateSprite("sb/particle2.png", OsbOrigin.Centre);

                var startPos = new Vector2(Random(360, 440), Random(0, 20));
                var endPos = new Vector2(startPos.X + Random(-40, 40), Random(140, 260));

                var Fade = Random(0.1, 0.2);
                var fadeDuration = Random(1000, 2000);
                var Scale = Random(0.008, 0.05);

                lightParticles.Fade(i, i + fadeDuration, 0, Fade);
                lightParticles.Fade(i + duration - fadeDuration, i + duration, Fade, 0);
                lightParticles.Move(i, i + duration, startPos, endPos);
                lightParticles.Additive(i, i + duration);
                lightParticles.Scale(i, Scale);
            }
        }

        public void Mission()
        {
            // INTRO ////////////////////////////////////////////////////////////////////////

            var appearStartTime = 398622;
            var disappearStartTime = 408675;
            var endTime = 488992;

            var nechoScale = 0.2;
            var otosakaScale = 0.2;

            var nechoPosX = 107;
            var otosakaPosX = 555;
            
            var necho = GetLayer("Necho - Intro").CreateSprite("sb/avatars/Necho.png", OsbOrigin.Centre);
            var otosaka = GetLayer("Otosaka-Yu - Intro").CreateSprite("sb/avatars/Otosaka-Yu.png", OsbOrigin.Centre);
            
            int speed = 0;
            for (int i = appearStartTime; i < appearStartTime + 4000; i += speed)
            {
                speed += 200;

                necho.ScaleVec(OsbEasing.InOutSine, i, i + (speed / 2), nechoScale, nechoScale, -nechoScale, nechoScale);
                necho.ScaleVec(OsbEasing.InOutSine, i + (speed / 2), i + speed, -nechoScale, nechoScale, nechoScale, nechoScale);
                otosaka.ScaleVec(OsbEasing.InOutSine, i, i + (speed / 2), otosakaScale, otosakaScale, -otosakaScale, otosakaScale);
                otosaka.ScaleVec(OsbEasing.InOutSine, i + (speed / 2), i + speed, -nechoScale, nechoScale, nechoScale, nechoScale);
            }

            necho.Fade(appearStartTime, appearStartTime + 1500, 0, 1);
            necho.Fade(endTime, endTime + 1500, 1, 0);
            necho.Move(OsbEasing.OutSine, appearStartTime, appearStartTime + 4000, -107, 280, nechoPosX, 280);
            necho.Move(OsbEasing.InOutExpo, disappearStartTime, disappearStartTime + 1500, nechoPosX, 280, 127, 240);

            otosaka.FlipH(appearStartTime, endTime + 1500);
            otosaka.Fade(appearStartTime, appearStartTime + 1500, 0, 1);
            otosaka.Fade(endTime, endTime + 1500, 1, 0);
            otosaka.Move(OsbEasing.OutSine, appearStartTime, appearStartTime + 4000, 747, 200, otosakaPosX, 200);
            otosaka.Move(OsbEasing.InOutExpo, disappearStartTime, disappearStartTime + 1500, otosakaPosX, 200, 535, 240);

            var nechoTag = GetLayer("VS + Tag").CreateSprite("sb/missions/9/necho.png", OsbOrigin.Centre);
            var otosakaTag = GetLayer("VS + Tag").CreateSprite("sb/missions/9/otosaka-yu.png", OsbOrigin.Centre);

            var tagStartTime = appearStartTime + 4000;
            var tagEndTime = disappearStartTime;

            nechoTag.Move(OsbEasing.Out, tagStartTime, tagStartTime + 500, -107 - 20, 400, nechoPosX - 20, 400);
            nechoTag.Move(tagEndTime, tagEndTime + 500, nechoPosX - 20, 400, -107 - 20, 400);
            nechoTag.Fade(tagStartTime, tagStartTime + 500, 0, 1);
            nechoTag.Fade(tagEndTime, tagEndTime + 500, 1, 0);
            nechoTag.Scale(tagStartTime, nechoScale * 1.5);

            otosakaTag.Move(OsbEasing.Out, tagStartTime, tagStartTime + 500, 747, 280, otosakaPosX, 280);
            otosakaTag.Move(tagEndTime, tagEndTime + 500, otosakaPosX, 280, 747, 280);
            otosakaTag.Fade(tagStartTime, tagStartTime + 500, 0, 1);
            otosakaTag.Fade(tagEndTime, tagEndTime + 500, 1, 0);
            otosakaTag.Scale(tagStartTime, otosakaScale * 1.5);

            var vs = GetLayer("VS + Tag").CreateSprite("sb/missions/9/vs.png", OsbOrigin.Centre, new Vector2(320, 240));
            var vs2 = GetLayer("VS + Tag").CreateSprite("sb/missions/9/vs.png", OsbOrigin.Centre, new Vector2(320, 240));

            vs.Fade(appearStartTime, appearStartTime + 5000, 0, 1);
            vs.Fade(disappearStartTime + 500, disappearStartTime + 1500, 1, 0);
            vs.Scale(OsbEasing.InBack, appearStartTime, appearStartTime + 5000, 1, 0.4);

            vs2.Additive(appearStartTime + 5000, appearStartTime + 7000);
            vs2.Fade(OsbEasing.OutSine, appearStartTime + 5000, appearStartTime + 7000, 0.5, 0);
            vs2.Scale(OsbEasing.OutSine, appearStartTime + 5000, appearStartTime + 7000, 0.4, 0.7);

            var nechoH1 = GetLayer("HUD - Intro").CreateSprite("sb/missions/9/h1.png", OsbOrigin.Centre);
            var nechoH2 = GetLayer("HUD - Intro").CreateSprite("sb/missions/9/h2.png", OsbOrigin.Centre);
            var nechoH3 = GetLayer("HUD - Intro").CreateSprite("sb/missions/9/h3.png", OsbOrigin.Centre);
            var otosakaH1 = GetLayer("HUD - Intro").CreateSprite("sb/missions/9/h1.png", OsbOrigin.Centre);
            var otosakaH2 = GetLayer("HUD - Intro").CreateSprite("sb/missions/9/h2.png", OsbOrigin.Centre);
            var otosakaH3 = GetLayer("HUD - Intro").CreateSprite("sb/missions/9/h3.png", OsbOrigin.Centre);
            
            var hudStartTime = appearStartTime + 4000;
            var hudEndTime = disappearStartTime - 300;

            nechoH1.Scale(hudStartTime, 0.35);
            nechoH1.Move(hudStartTime, nechoPosX - 20, 280);
            nechoH1.Additive(hudStartTime, hudEndTime + 500);
            nechoH1.Rotate(hudStartTime, hudEndTime + 500, Random(-1, 1), Random(1, 2));
            nechoH1.StartLoopGroup(hudStartTime, 5);
            nechoH1.Fade(0, 100, 0.5, 0);
            nechoH1.EndGroup();
            nechoH1.Fade(hudStartTime + 500, hudEndTime, 0.5, 0.5);
            nechoH1.StartLoopGroup(hudEndTime, 3);
            nechoH1.Fade(0, 100, 0.5, 0);
            nechoH1.EndGroup();

            nechoH2.Scale(hudStartTime, 0.35);
            nechoH2.Move(hudStartTime, nechoPosX - 20, 280);
            nechoH2.Additive(hudStartTime, hudEndTime + 500);
            nechoH2.Rotate(hudStartTime, hudEndTime + 500, Random(1, 2), Random(-1.5, -1));
            nechoH2.StartLoopGroup(hudStartTime, 5);
            nechoH2.Fade(0, 100, 0.5, 0);
            nechoH2.EndGroup();
            nechoH2.Fade(hudStartTime + 500, hudEndTime, 0.5, 0.5);
            nechoH2.StartLoopGroup(hudEndTime, 3);
            nechoH2.Fade(0, 100, 0.5, 0);
            nechoH2.EndGroup();

            nechoH3.Scale(hudStartTime, 0.35);
            nechoH3.Move(hudStartTime, nechoPosX - 20, 280);
            nechoH3.Additive(hudStartTime, hudEndTime + 500);
            nechoH3.Rotate(hudStartTime, hudEndTime + 500, Random(-1, 1), Random(1, 2));
            nechoH3.StartLoopGroup(hudStartTime, 5);
            nechoH3.Fade(0, 100, 0.5, 0);
            nechoH3.EndGroup();
            nechoH3.Fade(hudStartTime + 500, hudEndTime, 0.5, 0.5);
            nechoH3.StartLoopGroup(hudEndTime, 3);
            nechoH3.Fade(0, 100, 0.5, 0);
            nechoH3.EndGroup();

            otosakaH1.Scale(hudStartTime, 0.35);
            otosakaH1.Move(hudStartTime, otosakaPosX, 200);
            otosakaH1.Additive(hudStartTime, hudEndTime + 500);
            otosakaH1.Rotate(hudStartTime, hudEndTime + 500, Random(-1, 1), Random(1, 2));
            otosakaH1.StartLoopGroup(hudStartTime, 5);
            otosakaH1.Fade(0, 100, 0.5, 0);
            otosakaH1.EndGroup();
            otosakaH1.Fade(hudStartTime + 500, hudEndTime, 0.5, 0.5);
            otosakaH1.StartLoopGroup(hudEndTime, 3);
            otosakaH1.Fade(0, 100, 0.5, 0);
            otosakaH1.EndGroup();

            otosakaH2.Scale(hudStartTime, 0.35);
            otosakaH2.Move(hudStartTime, otosakaPosX, 200);
            otosakaH2.Additive(hudStartTime, hudEndTime + 500);
            otosakaH2.Rotate(hudStartTime, hudEndTime + 500, Random(1, 2), Random(-1.5, -1));
            otosakaH2.StartLoopGroup(hudStartTime, 5);
            otosakaH2.Fade(0, 100, 0.5, 0);
            otosakaH2.EndGroup();
            otosakaH2.Fade(hudStartTime + 500, hudEndTime, 0.5, 0.5);
            otosakaH2.StartLoopGroup(hudEndTime, 3);
            otosakaH2.Fade(0, 100, 0.5, 0);
            otosakaH2.EndGroup();

            otosakaH3.Scale(hudStartTime, 0.35);
            otosakaH3.Move(hudStartTime, otosakaPosX, 200);
            otosakaH3.Additive(hudStartTime, hudEndTime + 500);
            otosakaH3.Rotate(hudStartTime, hudEndTime + 500, Random(-1, 1), Random(1, 2));
            otosakaH3.StartLoopGroup(hudStartTime, 5);
            otosakaH3.Fade(0, 100, 0.5, 0);
            otosakaH3.EndGroup();
            otosakaH3.Fade(hudStartTime + 500, hudEndTime, 0.5, 0.5);
            otosakaH3.StartLoopGroup(hudEndTime, 3);
            otosakaH3.Fade(0, 100, 0.5, 0);
            otosakaH3.EndGroup();

            // FIGHT ////////////////////////////////////////////////////////////////////////
            var sTime = disappearStartTime + 2200;
            var eTime = endTime;
            var interval = 1500;
            var rotate = MathHelper.DegreesToRadians(90);
            for (double i = sTime; i < eTime - interval; i += interval)
            {
                var Beat = Beatmap.GetTimingPointAt((int)i).BeatDuration;

                var slashLeft = GetLayer("Slashing").CreateAnimation("sb/missions/9/slashAnimation/slash" + Random(1, 3) + "_.jpg", 8, 50, OsbLoopType.LoopOnce, OsbOrigin.Centre);
                var slashRight = GetLayer("Slashing").CreateAnimation("sb/missions/9/slashAnimation/slash" + Random(1, 3) + "_.jpg", 8, 50, OsbLoopType.LoopOnce, OsbOrigin.Centre);

                var slashDelay = -50;
                var Speed = Random(10, 15);
                var Duration = interval / Speed;
                var spawnDelay = Random(Beat, Beat * 6);
                var effectDelay = Random(500, 1000);
                
                var nechoRandomHitPos = Random(-80, -50);
                var otosakaRandomHitPos = Random(50, 80);
                var randomDelay = Random(250, 500);

                var nechoCurrentPosX = necho.PositionAt(i + slashDelay + Duration).X;
                var nechoCurrentPosY = necho.PositionAt(i + slashDelay + Duration).Y;
                var otosakaCurrentPosX = otosaka.PositionAt(i + slashDelay + spawnDelay + Duration).X;
                var otosakaCurrentPosY = otosaka.PositionAt(i + slashDelay + spawnDelay + Duration).Y;
                
                var slashYL = Random(necho.PositionAt(i + slashDelay + Duration).Y - 100, necho.PositionAt(i + slashDelay + Duration).Y + 100);
                var slashYR = Random(otosaka.PositionAt(i + slashDelay + Duration).Y - 35, otosaka.PositionAt(i + slashDelay + Duration).Y + 60);

                slashLeft.Rotate(i + slashDelay + Duration, rotate);
                slashRight.Rotate(i + slashDelay + spawnDelay + Duration, rotate);
                slashLeft.Move(i + slashDelay + Duration, i + slashDelay + Duration + randomDelay, nechoCurrentPosX - 20, nechoCurrentPosY, nechoCurrentPosX - 20 + nechoRandomHitPos, nechoCurrentPosY);
                slashRight.Move(i + slashDelay + spawnDelay + Duration, i + slashDelay + spawnDelay + Duration + randomDelay, otosakaCurrentPosX, otosakaCurrentPosY, otosakaCurrentPosX + otosakaRandomHitPos, otosakaCurrentPosY);
                slashLeft.ScaleVec(i + slashDelay + Duration, i + slashDelay + Duration, Random(0.2, 0.4), Random(0.4, 0.6), Random(0.2, 0.4), Random(0.4, 0.6));
                slashRight.ScaleVec(i + slashDelay + spawnDelay + Duration, i + slashDelay + spawnDelay + Duration + effectDelay, Random(0.2, 0.4), Random(0.4, 0.6), Random(0.2, 0.4), Random(0.4, 0.6));
                slashLeft.Additive(i + slashDelay + Duration, i + slashDelay + Duration + effectDelay);
                slashRight.Additive(i + slashDelay + spawnDelay + Duration, i + slashDelay + spawnDelay + Duration + effectDelay);
                // flip vertically if it's on the right side
                if (otosaka.PositionAt(i + slashDelay + spawnDelay + Duration).X > 320)
                {
                    slashRight.FlipV(i + slashDelay + spawnDelay + Duration, i + slashDelay + spawnDelay + Duration);
                }
                // flip horizontally sometimes
                if (necho.PositionAt(i + slashDelay + Duration).Y > slashYL)
                {
                    slashLeft.FlipH(i + slashDelay + Duration, i + slashDelay + Duration);
                }
                // flip horizontally sometimes
                if (otosaka.PositionAt(i + slashDelay + spawnDelay + Duration).Y > slashYR)
                {
                    slashRight.FlipH(i + slashDelay + spawnDelay + Duration, i + slashDelay + spawnDelay + Duration);
                }
                // sound effects
                var slashLeftSFX = GetLayer("Slashing").CreateSample("sb/sfx/swoosh-" + Random(1, 4) + ".ogg", i + slashDelay + Duration, 80);
                var slashRightSFX = GetLayer("Slashing").CreateSample("sb/sfx/swoosh-" + Random(1, 4) + ".ogg", i + slashDelay + spawnDelay + Duration, 80);

                interval = Random(1500, 2000);

                // GETTING HIT EFFECT
                necho.Move(i + slashDelay + Duration, i + slashDelay + Duration + randomDelay, nechoCurrentPosX, nechoCurrentPosY, nechoCurrentPosX + nechoRandomHitPos, nechoCurrentPosY);
                necho.Move(i + slashDelay + Duration + randomDelay, i + slashDelay + Duration + (randomDelay * 2), nechoCurrentPosX + nechoRandomHitPos, nechoCurrentPosY, nechoCurrentPosX, nechoCurrentPosY);
                necho.Color(i + slashDelay + Duration, i + slashDelay + Duration + (randomDelay * 2), HitColor, Color4.White);
                necho.Color(i + slashDelay + Duration - 1, Color4.White);
                
                otosaka.Move(i + slashDelay + spawnDelay + Duration, i + slashDelay + spawnDelay + Duration + randomDelay, otosakaCurrentPosX, otosakaCurrentPosY, otosakaCurrentPosX + otosakaRandomHitPos, otosakaCurrentPosY);
                otosaka.Move(i + slashDelay + spawnDelay + Duration + randomDelay, i + slashDelay + spawnDelay + Duration + (randomDelay * 2), otosakaCurrentPosX + otosakaRandomHitPos, otosakaCurrentPosY, otosakaCurrentPosX, otosakaCurrentPosY);
                otosaka.Color(i + slashDelay + spawnDelay + Duration, i + slashDelay + spawnDelay + Duration + (randomDelay * 2), HitColor, Color4.White);
                otosaka.Color(i + slashDelay + spawnDelay + Duration - 1, Color4.White);

                // DAMAGE NUMBERS
                var nechoDamageSprites = GetLayer("Damage Numbers").CreateSprite("sb/missions/9/damage" + Random(1, 12) + ".png", OsbOrigin.Centre);
                var otosakaDamageSprites = GetLayer("Damage Numbers").CreateSprite("sb/missions/9/damage" + Random(1, 12) + ".png", OsbOrigin.Centre);

                var nechoDamagePos = new Vector2(nechoCurrentPosX - 20, nechoCurrentPosY - 140);
                var otosakaDamagePos = new Vector2(otosakaCurrentPosX, otosakaCurrentPosY - 120);
                var damageDuration = Random(500, 1000);

                var startRotation = MathHelper.DegreesToRadians(0);
                var endRotation = MathHelper.DegreesToRadians(Random(-30, 30));
                
                nechoDamageSprites.Move(OsbEasing.OutSine, i + slashDelay + Duration, i + slashDelay + Duration + damageDuration, nechoDamagePos.X, nechoDamagePos.Y, nechoDamagePos.X + Random(-50, 50), nechoDamagePos.Y - Random(5, 25));
                nechoDamageSprites.Rotate(OsbEasing.InSine, i + slashDelay + Duration, i + slashDelay + Duration + damageDuration, startRotation, endRotation);
                nechoDamageSprites.Fade(i + slashDelay + Duration, i + slashDelay + Duration + damageDuration, 1, 0);
                nechoDamageSprites.Scale(i + slashDelay + Duration, Random(0.2, 0.3));

                otosakaDamageSprites.Move(OsbEasing.OutSine, i + slashDelay + spawnDelay + Duration, i + slashDelay + spawnDelay + Duration + damageDuration, otosakaDamagePos.X, otosakaDamagePos.Y, otosakaDamagePos.X + Random(-50, 50), otosakaDamagePos.Y - Random(5, 25));
                otosakaDamageSprites.Rotate(OsbEasing.InSine, i + slashDelay + spawnDelay + Duration, i + slashDelay + spawnDelay + Duration, startRotation, endRotation);
                otosakaDamageSprites.Fade(i + slashDelay + spawnDelay + Duration, i + slashDelay + spawnDelay + Duration + damageDuration, 1, 0);
                otosakaDamageSprites.Scale(i + slashDelay + spawnDelay + Duration, Random(0.2, 0.3));
            }
        }

        public void Dialog()
        {
            // DIALOG BOXES STARTS HERE
            // var fontSize = 13; //  japanese
            var fontSize = 15; // english
            var GlowRadius = 15;
            var GlowColor = new Color4(150, 150, 150, 255);
            var ShadowThickness = 0;
            var OutlineThickness = 0;
            // var font = LoadFont("sb/dialog/txt/jp/9", new FontDescription() // japanese
            var font = LoadFont("sb/dialog/txt/2", new FontDescription() // english
            {
                // FontPath = "font/jp/KozGoPro-Light.otf", // japanese
                FontPath = "Microsoft Yi Baiti", // english
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
                Radius = false ? 0 : GlowRadius,
                Color = GlowColor,
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
            string[] sentence = { "?!",
                                  "What's going on?! Let me run a quick system scan..." };
            this.dialog = new DialogManager(this, font, 380218, 385244, "-Tochi", 105, 326, false,
                fontSize, 1, 50, 50, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.ogg",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "It seems the remaining bugs from Section 4 have infiltrated the system,", 
                                   "corrupting the mappers' databases... Wait... this was not supposed to happen...",
                                   "Why do Necho and Otosaki-Yu have to fight!" };
            this.dialog2 = new DialogManager(this, font, 394853, 407418, "-Tochi", 105, 326, false,
                fontSize, 1, 50, 50, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.ogg",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence2);

            // DIALOG 3 -----------------------------------------
            string[] sentence3 = { "Unfortunately, it seems that your score will inevitably drop, but please save them!",
                                   "We can only count on you..." };
            this.dialog3 = new DialogManager(this, font, 407418, 417785, "-Tochi", 105, 326, false,
                fontSize, 1, 50, 250, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.ogg",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence3);

            // DIALOG 4 -----------------------------------------
            string[] sentence4 = { "....",
                                   "*Scanning*" };
            this.dialog4 = new DialogManager(this, font, 385244, 394853, "-Tochi", 105, 326, false,
                fontSize, 1, 50, 250, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.ogg",
                DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence4);

            // // DIALOG 1 -----------------------------------------
            // string[] sentence = { "？！",
            //                       "何が起こっているの？！私にシステムスキャンをさせてください..." };
            // this.dialog = new DialogManager(this, font, 380218, 385244, "-Tochi", 105, 326, false,
            //     fontSize, 1, 50, 50, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.ogg",
            //     DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence);

            // // DIALOG 2 -----------------------------------------
            // string[] sentence2 = { "見た感じセクション４からの残りのバグはシステムに侵入し、マッパー達のデーターベースを破損しました...", 
            //                        "待って。こういうことがあってはダメです...なぜNechoとOtosaki-Yuは戦う必要性があるのですか！" };
            // this.dialog2 = new DialogManager(this, font, 394853, 407418, "-Tochi", 105, 326, false,
            //     fontSize, 1, 50, 50, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.ogg",
            //     DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence2);

            // // DIALOG 3 -----------------------------------------
            // string[] sentence3 = { "残念ながらあなたのスコアは必然的に落ちますが、彼たちを助けてください！",
            //                        "私たちはあなたにしか頼れません..." };
            // this.dialog3 = new DialogManager(this, font, 407418, 416214, "-Tochi", 105, 326, false,
            //     fontSize, 1, 50, 250, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.ogg",
            //     DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence3);

            // // DIALOG 4 -----------------------------------------
            // string[] sentence4 = { "....",
            //                        "*Scanning*" };
            // this.dialog4 = new DialogManager(this, font, 385244, 394853, "-Tochi", 105, 326, false,
            //     fontSize, 1, 50, 250, Color4.White, false, 0.3f, Color4.Black, "-Tochi", 300, "sb/sfx/message-1.ogg",
            //     DialogBoxes.Pointer.TopRight, DialogBoxes.Push.None, sentence4);
        }

        public void Tochi(int startTime, int endTime)
        {
            var Hoveduration = 5000;
            var loopCount = (endTime - startTime) / Hoveduration;
            var pos = new Vector2(320, 240);
            // var avatar = GetLayer("-Tochi").CreateSprite("sb/avatars/-TochiProfile.png", OsbOrigin.Centre);
            var avatar = GetLayer("-Tochi").CreateAnimation("sb/avatars/hologram/2/-TochiProfile.png", 31, 50, OsbLoopType.LoopForever, OsbOrigin.Centre);
            var ring = GetLayer("-Tochi").CreateSprite("sb/ring2.png", OsbOrigin.Centre);

            avatar.MoveX(startTime, 64);
            avatar.Scale(startTime, 0.6);
            avatar.Fade(startTime, startTime + 500, 0, 1);
            avatar.Fade(endTime, endTime + 500, 1, 0);

            avatar.StartLoopGroup(startTime, loopCount + 1);
            avatar.MoveY(OsbEasing.InOutSine, 0, Hoveduration / 2, 335, 345);
            avatar.MoveY(OsbEasing.InOutSine, Hoveduration / 2, Hoveduration, 345, 335);
            avatar.EndGroup();

            ring.MoveX(startTime, 64);
            ring.Scale(startTime, 0.3);
            ring.Fade(startTime, startTime + 500, 0, 1);
            ring.Fade(endTime, endTime + 500, 1, 0);
            var rotation = MathHelper.DegreesToRadians(180);
            ring.Rotate(startTime, endTime, -rotation, rotation);

            ring.StartLoopGroup(startTime, loopCount + 1);
            ring.MoveY(OsbEasing.InOutSine, 0, Hoveduration / 2, 335, 345);
            ring.MoveY(OsbEasing.InOutSine, Hoveduration / 2, Hoveduration, 345, 335);
            ring.EndGroup();
        }
    }
}

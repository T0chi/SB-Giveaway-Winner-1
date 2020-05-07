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
    public class Dialogues : StoryboardObjectGenerator
    {
        public Language langClass;

        public enum language { English, Japanese };

        [Configurable]
        public static language Language = language.English;

        [Configurable]
        public string fontEN = "Microsoft Yi Baiti";

        [Configurable]
        public string fontJP = "font/jp/KozGoPro-Light.otf";

        private FontGenerator font;

        private int fontSize;

        private FontStyle fontStyle;

        private int glowRadius;

        private Color4 glowColor;

        private Timing timing;

        private Vector2 position;

        private int fadeOutDelay;

        private Color4 boxColor;

        private bool showBox;

        private bool originCentre;

        private DialogBoxes.Push push;

        private DialogManager dialog;

        private int delay = 200;

        public override void Generate()
        {
            langClass = new Language();

            if (Language == language.English)
            {
                langClass.chooseLanguage(true);

                if (langClass.setEnglish == true)
                {
                    Noffy();
                    this.fontSize = 20;
                    this.glowRadius = 0;
                    this.originCentre = true;
                    this.boxColor = Color4.Black;
                    this.fontStyle = FontStyle.Regular;
                    EN_System();

                    this.boxColor = Color4.White;
                    EN_Section1();

                    this.fontSize = 15;
                    this.glowRadius = 15;
                    this.fontStyle = FontStyle.Bold;
                    this.glowColor = new Color4(150, 150, 150, 255);
                    this.push = DialogBoxes.Push.None;
                    this.showBox = false;
                    this.originCentre = false;
                    EN_Section2();
                    EN_Section3();
                    EN_Section4();
                    EN_Section5();
                    EN_Section6();
                    EN_Section7();
                    EN_Section8();
                    EN_Section9();

                    Log("LANGUAGE ............ English");
                }
            }

            else if (Language == language.Japanese)
            {
                langClass.chooseLanguage(false);

                if (langClass.setEnglish == false)
                {
                    Reey();
                    this.fontSize = 15;
                    this.glowRadius = 0;
                    this.originCentre = true;
                    this.boxColor = Color4.Black;
                    this.fontStyle = FontStyle.Regular;
                    JP_System();

                    this.boxColor = Color4.White;
                    JP_Section1();

                    this.fontSize = 13;
                    this.glowRadius = 15;
                    this.fontStyle = FontStyle.Bold;
                    this.glowColor = new Color4(150, 150, 150, 255);
                    this.push = DialogBoxes.Push.None;
                    this.showBox = false;
                    this.originCentre = false;
                    JP_Section2();
                    JP_Section3();
                    JP_Section4();
                    JP_Section5();
                    JP_Section6();
                    JP_Section7();
                    JP_Section8();
                    JP_Section9();

                    Log("LANGUAGE ............ Japanese");
                }
            }
        }

        public void StaticSFX(int startTime)
        {
            var sfx = GetLayer("Voiceover").CreateSample("sb/sfx/static.ogg", startTime + delay - 50, 50);
        }

        public void Noffy()
        {
            var volume = 100;

            var satelliteSFX = GetLayer("Voiceover").CreateSample("sb/sfx/satellite.ogg", 385244, 20);
            var section1 = GetLayer("Voiceover").CreateSample("sb/sfx/voice/noffy/section1_1.ogg", 3245 + delay, volume);
            var section2 = GetLayer("Voiceover").CreateSample("sb/sfx/voice/noffy/section2_1.ogg", 64629 + delay, volume);
            var section3 = GetLayer("Voiceover").CreateSample("sb/sfx/voice/noffy/section3_1.ogg", 117905 + delay, volume);
            var section4 = GetLayer("Voiceover").CreateSample("sb/sfx/voice/noffy/section4_1.ogg", 157678 + delay, volume);
            var section5 = GetLayer("Voiceover").CreateSample("sb/sfx/voice/noffy/section5_1.ogg", 214268 + delay, volume);
            var section6 = GetLayer("Voiceover").CreateSample("sb/sfx/voice/noffy/section6_1.ogg", 252193 + delay, volume);
            var section7 = GetLayer("Voiceover").CreateSample("sb/sfx/voice/noffy/section7_1.ogg", 280529 + delay, volume);
            var section8 = GetLayer("Voiceover").CreateSample("sb/sfx/voice/noffy/section8_1.ogg", 331111 + delay, volume);
            var section9 = GetLayer("Voiceover").CreateSample("sb/sfx/voice/noffy/section9_1.ogg", 380218 + delay, volume);
            var section9_2 = GetLayer("Voiceover").CreateSample("sb/sfx/voice/noffy/section9_2.ogg", 394853 + delay, volume);

            StaticSFX(3245); // 1
            StaticSFX(14437);
            StaticSFX(16283);
            StaticSFX(26668);

            StaticSFX(64629); // 2
            StaticSFX(74050);

            StaticSFX(117905); // 3
            StaticSFX(130512);

            StaticSFX(157678); // 4
            StaticSFX(175626);

            StaticSFX(214283); // 5
            StaticSFX(224901);

            StaticSFX(252193); // 6
            StaticSFX(257755);

            StaticSFX(280529); // 7
            StaticSFX(291471);

            StaticSFX(331111); // 8
            StaticSFX(337194);

            StaticSFX(380218); // 9
            StaticSFX(394653);
            StaticSFX(407218);

            Log("VOICE .................... Noffy");
        }

        public void Reey()
        {
            var volume = 100;

            var satelliteSFX = GetLayer("Voiceover").CreateSample("sb/sfx/satellite.ogg", 385244, 20);
            var section2 = GetLayer("Voiceover").CreateSample("sb/sfx/voice/reey/section2_1.ogg", 64629 + delay, volume);
            var section1 = GetLayer("Voiceover").CreateSample("sb/sfx/voice/reey/section1_1.ogg", 3245 + delay, volume);
            var section3 = GetLayer("Voiceover").CreateSample("sb/sfx/voice/reey/section3_1.ogg", 117905 + delay, volume);
            var section4 = GetLayer("Voiceover").CreateSample("sb/sfx/voice/reey/section4_1.ogg", 163062 - 5385 + delay, volume);
            var section5 = GetLayer("Voiceover").CreateSample("sb/sfx/voice/reey/section5_1.ogg", 214268 + delay, volume);
            var section6 = GetLayer("Voiceover").CreateSample("sb/sfx/voice/reey/section6_1.ogg", 252193 + delay, volume);
            var section7 = GetLayer("Voiceover").CreateSample("sb/sfx/voice/reey/section7_1.ogg", 280529 + delay, volume);
            var section8 = GetLayer("Voiceover").CreateSample("sb/sfx/voice/reey/section8_1.ogg", 331111 + delay, volume);
            var section9 = GetLayer("Voiceover").CreateSample("sb/sfx/voice/reey/section9_1.ogg", 380218 + delay, volume);
            var section9_2 = GetLayer("Voiceover").CreateSample("sb/sfx/voice/reey/section9_2.ogg", 394853 + delay, volume);

            StaticSFX(3245); // 1
            StaticSFX(14322);
            StaticSFX(18475);
            StaticSFX(28168);
            StaticSFX(40629);

            StaticSFX(64629); // 2
            StaticSFX(72571);

            StaticSFX(117905); // 3
            StaticSFX(130062);

            StaticSFX(163062 - 5385); // 4
            StaticSFX(179982 - 5385);

            StaticSFX(214268); // 5
            StaticSFX(224256);

            StaticSFX(252193); // 6
            StaticSFX(257755);

            StaticSFX(280529); // 7
            StaticSFX(288854);

            StaticSFX(331111); // 8
            StaticSFX(337394);

            StaticSFX(380218); // 9
            StaticSFX(394853);
            StaticSFX(407418);

            Log("VOICE .................... Reey");
        }

        public void Tochi(int startTime, int endTime)
        {
            var Hoveduration = 5000;
            var loopCount = (endTime - startTime) / Hoveduration;
            var pos = new Vector2(320, 240);
            // var avatar = GetLayer("Box").CreateSprite("sb/avatars/-TochiProfile.png", OsbOrigin.Centre);
            var avatar = GetLayer("Box").CreateAnimation("sb/avatars/hologram/2/-TochiProfile.png", 31, 50, OsbLoopType.LoopForever, OsbOrigin.Centre);
            var ring = GetLayer("Box").CreateSprite("sb/ring2.png", OsbOrigin.Centre);

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

        public language Unspecified()
        {
            language unspecified = Language;
            return unspecified;
        }

        public language English()
        {
            language english = language.English;
            return english;
        }

        public language Japanese()
        {
            language japanese = language.Japanese;
            return japanese;
        }

        class Timing
        {
            public int start;

            public int end;
        }

        FontGenerator FontGenerator(string outputPath, string fontName)
        {
            var font = LoadFont(outputPath, new FontDescription()
            {
                FontPath = fontName,
                FontSize = this.fontSize,
                Color = Color4.White,
                Padding = Vector2.Zero,
                FontStyle = this.fontStyle,
                TrimTransparency = true,
                EffectsOnly = false,
                Debug = false,
            },
            new FontGlow()
            {
                Radius = false ? 0 : this.glowRadius,
                Color = this.glowColor,
            },
            new FontOutline()
            {
                Thickness = 0,
                Color = Color4.Black,
            },
            new FontShadow()
            {
                Thickness = 0,
                Color = Color4.Black,
            });

            return font;
        }

        public void Dialog(string[] sentence)
        {
            this.dialog = new DialogManager(this, this.font, this.timing.start, this.timing.end, "Text", this.position.X, this.position.Y, this.originCentre,
                this.fontSize, 1, 50, this.fadeOutDelay, Color4.White, this.showBox, 0.8f, this.boxColor, "Box", 0, "sb/sfx/message-1.ogg",
                DialogBoxes.Pointer.None, this.push, sentence);
        }

        /* |||||||||||||||||||||||||||||||||||||||||||| SYSTEM DIALOGUES |||||||||||||||||||||||||||||||||||||||||||| */

        public void EN_System()
        {
            this.timing = new Timing();
            var font = FontGenerator("sb/dialog/txt/system", fontEN);
            this.font = font;

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "Get ready!",
                                  "Your mission is about to begin." };

            this.showBox = true;
            this.timing.start = 40630;
            this.timing.end = 48937;
            this.position = new Vector2(40, 430);
            this.push = DialogBoxes.Push.Right;
            Dialog(sentence);
        }

        public void JP_System()
        {
            this.timing = new Timing();
            var font = FontGenerator("sb/dialog/txt/system/jp", fontJP);
            this.font = font;

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "準備してください！",
                                  "あなたのミッションはもうすぐ始まります。" };

            this.showBox = true;
            this.timing.start = 40630;
            this.timing.end = 48937;
            this.position = new Vector2(40, 430);
            this.push = DialogBoxes.Push.Right;
            Dialog(sentence);
        }

        /* |||||||||||||||||||||||||||||||||||||||||||| ENGLISH SECTIONS |||||||||||||||||||||||||||||||||||||||||||| */

        public void EN_Section1()
        {
            this.fadeOutDelay = 50;
            this.timing = new Timing();
            var font = FontGenerator("sb/dialog/txt/1", fontEN);
            this.font = font;

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "Hello, and welcome to World TQR-f3!",
                                "My name is -Tochi, and I will serve as",
                                "your assistant for today's missions!" };

            this.showBox = true;
            this.timing.start = 3245;
            this.timing.end = 12475;
            this.position = new Vector2(150, 190);
            this.push = DialogBoxes.Push.Right;
            Dialog(sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "Please, follow me!" };

            this.showBox = true;
            this.timing.start = 14495;
            this.timing.end = 15822;
            this.position = new Vector2(370, 200);
            this.push = DialogBoxes.Push.Left;
            Dialog(sentence2);

            // DIALOG 3 -----------------------------------------
            string[] sentence3 = { "I have assembled a team of mappers for you ",
                                "that will help you complete this map. Do",
                                "acquaint with them well, as your synergy will",
                                "determine the final grading of your performance." };

            this.showBox = true;
            this.timing.start = 16399;
            this.timing.end = 26437;
            this.position = new Vector2(390, 170);
            this.push = DialogBoxes.Push.Left;
            Dialog(sentence3);

            // DIALOG 4 -----------------------------------------
            string[] sentence4 = { "In accordance to your performance on each section,",
                                "points will be awarded. You can only complete the ",
                                "map if your score surpasses 30,000." };

            this.fadeOutDelay = 100;
            this.showBox = true;
            this.timing.start = 26783;
            this.timing.end = 37168;
            this.position = new Vector2(400, 170);
            this.push = DialogBoxes.Push.Left;
            Dialog(sentence4);
        }

        public void EN_Section2()
        {
            this.fadeOutDelay = 50;
            this.timing = new Timing();
            var font = FontGenerator("sb/dialog/txt/2", fontEN);
            this.font = font;

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "Apologies for the haste, but are you ready for your first mission?",
                                  "Unfortunately, the warm-up phase was probably inadequate for these velocities..." };

            this.timing.start = 64514;
            this.timing.end = 74250;
            this.position = new Vector2(105, 326);
            Dialog(sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "Shoot the enemy aircraft and try not to miss.",
                                   "Missing too many times will result in a lower score, so please do your best.",
                                   "Good luck!" };

            this.fadeOutDelay = 500;
            this.timing.start = 74250;
            this.timing.end = 83571;
            this.position = new Vector2(105, 326);
            Dialog(sentence2);
            Tochi(64514, 83571);
        }

        public void EN_Section3()
        {
            this.fadeOutDelay = 50;
            this.timing = new Timing();
            var font = FontGenerator("sb/dialog/txt/3", fontEN);
            this.font = font;

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "That was close! Congratulations on the completion of the first mission.",
                                  "The next mission however is much easier, all you have to do is pick up a few miscellaneous materials!" };

            this.timing.start = 117905;
            this.timing.end = 130712;
            this.position = new Vector2(105, 326);
            Dialog(sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "Be careful, the 'Tryplet Gems' are rather sharp.",
                                   "Good luck!" };

            this.fadeOutDelay = 500;
            this.timing.start = 130712;
            this.timing.end = 135312;
            this.position = new Vector2(105, 326);
            Dialog(sentence2);
            Tochi(117905, 135312);
        }

        public void EN_Section4()
        {
            this.fadeOutDelay = 50;
            this.timing = new Timing();
            var font = FontGenerator("sb/dialog/txt/4", fontEN);
            this.font = font;

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "Thank you for faithfully completing your errands.",
                                  "The next mission requires a lot more stamina; you'll be activating your sword-",
                                  "wielding mapper to perform bug fixes within our systems.",
                                  "Do take note that the difficulty does amp up after the bass drops." };

            this.timing.start = 163062 - 5385;
            this.timing.end = 181011 - 5385;
            this.position = new Vector2(105, 326);
            Dialog(sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "Slash the Stygian viral shapes to complete the mission.",
                                   "There are risks so proceed with caution!" };

            this.fadeOutDelay = 500;
            this.timing.start = 181011 - 5385;
            this.timing.end = 187868 - 5385;
            this.position = new Vector2(105, 326);
            Dialog(sentence2);
            Tochi(163062 - 5385, 187868 - 5385);
        }

        public void EN_Section5()
        {
            this.fadeOutDelay = 50;
            this.timing = new Timing();
            var font = FontGenerator("sb/dialog/txt/5", fontEN);
            this.font = font;

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "That was intense!",
                                  "The next mission is an aim practice balloon popping challenge.",
                                  "Jump around the playfield to strike the balloons down with your bow-wielding mapper." };

            this.timing.start = 214268;
            this.timing.end = 224932;
            this.position = new Vector2(105, 326);
            Dialog(sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "You can take this as a small break to hone your mapper's abilities.",
                                  "But do give it your best shot as it still counts towards your final score!" };

            this.fadeOutDelay = 500;
            this.timing.start = 224932;
            this.timing.end = 233145;
            this.position = new Vector2(105, 326);
            Dialog(sentence2);
            Tochi(214268, 233145);
        }

        public void EN_Section6()
        {
            this.fadeOutDelay = 50;
            this.timing = new Timing();
            var font = FontGenerator("sb/dialog/txt/6", fontEN);
            this.font = font;

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "Well executed!",
                                  "However, the next mission involves slightly more technical knowledge." };

            this.timing.start = 252193;
            this.timing.end = 257955;
            this.position = new Vector2(105, 326);
            Dialog(sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "Retrieve broken machine parts for research from Ice Biome 3.5.",
                                   "Finger control is of utmost necessity, stay safe." };

            this.fadeOutDelay = 500;
            this.timing.start = 257955;
            this.timing.end = 268278;
            this.position = new Vector2(105, 326);
            Dialog(sentence);
            Tochi(252193, 268278);
        }

        public void EN_Section7()
        {
            this.fadeOutDelay = 50;
            this.timing = new Timing();
            var font = FontGenerator("sb/dialog/txt/7", fontEN);
            this.font = font;

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "These machine parts will be useful, your assistance is greatly appreciated.",
                                  "The next mission will involve some general rhythmic sense and musical knowledge." };

            this.timing.start = 280529;
            this.timing.end = 291451;
            this.position = new Vector2(105, 326);
            Dialog(sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "Your mapper will guide you with singing at the start,",
                                   "but you do have to continue by yourself in the second phase of this section.",
                                   "The more inconsistent you are, the more your points will be compromised.",
                                   "Take care." };

            this.fadeOutDelay = 500;
            this.timing.start = 291451;
            this.timing.end = 305980;
            this.position = new Vector2(105, 326);
            Dialog(sentence);
            Tochi(280529, 305980);
        }

        public void EN_Section8()
        {
            this.fadeOutDelay = 50;
            this.timing = new Timing();
            var font = FontGenerator("sb/dialog/txt/8", fontEN);
            this.font = font;

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "Alright, those are all the difficult missions over and done with!",
                                  "We should be done with the map soon." };

            this.timing.start = 331111;
            this.timing.end = 337394;
            this.position = new Vector2(105, 326);
            Dialog(sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "This mission should be relatively easier than the previous ones.",
                                   "Spy on aircrafts cruising by from behind the building, and report anything strange you find!" };

            this.fadeOutDelay = 500;
            this.timing.start = 337394;
            this.timing.end = 348176;
            this.position = new Vector2(105, 326);
            Dialog(sentence);
            Tochi(331111, 348176);
        }

        public void EN_Section9()
        {
            this.fadeOutDelay = 50;
            this.timing = new Timing();
            var font = FontGenerator("sb/dialog/txt/9", fontEN);
            this.font = font;

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "?!",
                                  "What's going on?! Let me run a quick system scan..." };

            this.timing.start = 380218;
            this.timing.end = 385244;
            this.position = new Vector2(105, 326);
            Dialog(sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "....",
                                   "*Scanning*" };

            this.timing.start = 385244;
            this.timing.end = 394853;
            this.position = new Vector2(105, 326);
            Dialog(sentence2);

            // DIALOG 3 -----------------------------------------
            string[] sentence3 = { "It seems the remaining bugs from Section 4 have infiltrated the system,",
                                   "corrupting the mappers' databases... Wait... this was not supposed to happen...",
                                   "Why do Necho and Otosaka-Yu have to fight!" };

            this.timing.start = 394853;
            this.timing.end = 408289;
            this.position = new Vector2(105, 326);
            Dialog(sentence3);

            // DIALOG 4 -----------------------------------------
            string[] sentence4 = { "Unfortunately, it seems that your score will inevitably drop, but please save them!",
                                   "We can only count on you..." };

            this.fadeOutDelay = 500;
            this.timing.start = 408289;
            this.timing.end = 417785;
            this.position = new Vector2(105, 326);
            Dialog(sentence4);
            Tochi(380218, 417785);

        }

        /* |||||||||||||||||||||||||||||||||||||||||||| JAPANESE SECTIONS |||||||||||||||||||||||||||||||||||||||||||| */

        public void JP_Section1()
        {
            this.fadeOutDelay = 50;
            this.timing = new Timing();
            var font = FontGenerator("sb/dialog/txt/1/jp", fontJP);
            this.font = font;

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "こんにちは、そして異世界TQR-f3にようこそ！",
                                    "私の名前は-Tochi、今日のミッションのために",
                                    "あなたのアシスタントを努めます！" };

            this.showBox = true;
            this.timing.start = 3245;
            this.timing.end = 12475;
            this.position = new Vector2(130, 190);
            this.push = DialogBoxes.Push.Right;
            Dialog(sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "では、ついてきてください！" };

            this.showBox = true;
            this.timing.start = 14322;
            this.timing.end = 17091;
            this.position = new Vector2(370, 205);
            this.push = DialogBoxes.Push.Left;
            Dialog(sentence2);

            // DIALOG 3 -----------------------------------------
            string[] sentence3 = { "この譜面を終わらせるためにマッパーのチームを構成しました。",
                                    "彼たちと仲間になり、全員の協力によりあなたの業績が最終評価になります。" };

            this.showBox = true;
            this.timing.start = 18476;
            this.timing.end = 27706;
            this.position = new Vector2(430, 170);
            this.push = DialogBoxes.Push.Left;
            Dialog(sentence3);

            // DIALOG 4 -----------------------------------------
            string[] sentence4 = { "各セクションのあなたのプレイに従い、ポイントが与えられます。",
                                    "合計点数が30,000を超えるのみに譜面を完成出来ます。" };

            this.showBox = true;
            this.timing.start = 28206;
            this.timing.end = 37168;
            this.position = new Vector2(400, 170);
            this.push = DialogBoxes.Push.Left;
            Dialog(sentence4);
        }

        public void JP_Section2()
        {
            this.fadeOutDelay = 50;
            this.timing = new Timing();
            var font = FontGenerator("sb/dialog/txt/2/jp", fontJP);
            this.font = font;

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "急がしてごめんなさい、でも最初のミッションの準備はいいですか？",
                                  "あいにく最初の段階であの速度のwarm-upは多分不十分だった..." };

            this.timing.start = 64514;
            this.timing.end = 72571;
            this.position = new Vector2(105, 326);
            Dialog(sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "ミスをしないように敵を撃ってください。",
                                   "ミスを出しすぎると低い点数になるので気を抜かないようにしてください。",
                                   "頑張って！" };

            this.fadeOutDelay = 500;
            this.timing.start = 72571;
            this.timing.end = 81905;
            this.position = new Vector2(105, 326);
            Dialog(sentence2);
            Tochi(64514, 81905);
        }

        public void JP_Section3()
        {
            this.fadeOutDelay = 50;
            this.timing = new Timing();
            var font = FontGenerator("sb/dialog/txt/3/jp", fontJP);
            this.font = font;

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "危なかった！最初のミッションのクリアおめでとう。",
                                  "次のミッションはもっと簡単、色々な材料を採るだけ！" };

            this.timing.start = 117905;
            this.timing.end = 125905;
            this.position = new Vector2(105, 326);
            Dialog(sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "サンレンダ宝石がとても鋭いから気を付けて。",
                                   "頑張って！" };

            this.fadeOutDelay = 500;
            this.timing.start = 130062;
            this.timing.end = 135012;
            this.position = new Vector2(105, 326);
            Dialog(sentence2);
            Tochi(117905, 135012);
        }

        public void JP_Section4()
        {
            this.fadeOutDelay = 50;
            this.timing = new Timing();
            var font = FontGenerator("sb/dialog/txt/4/jp", fontJP);
            this.font = font;

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "忠実に取り組んでくれてありがとう。",
                                  "次のミッションは体力がもっと必要になります;　剣を振っているマッパーを使い、私たちのシステムのバグを直してください。",
                                  "サビのベースドロップが始まった後に難易度が上がります！" };

            this.timing.start = 163062 - 5385;
            this.timing.end = 178611 - 5385;
            this.position = new Vector2(105, 326);
            Dialog(sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "ダークウィルスの形の物を斬りつけてミッションを完了してください。",
                                   "これらはまたリスクがあるので注意してください！" };

            this.fadeOutDelay = 500;
            this.timing.start = 179982 - 5385;
            this.timing.end = 187868 - 5385;
            this.position = new Vector2(105, 326);
            Dialog(sentence2);
            Tochi(163062 - 5385, 187868 - 5385);
        }

        public void JP_Section5()
        {
            this.fadeOutDelay = 50;
            this.timing = new Timing();
            var font = FontGenerator("sb/dialog/txt/5/jp", fontJP);
            this.font = font;

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "今のは激しかった！",
                                  "次のミッションはaim練習で風船を割るチャレンジ。",
                                  "弓を使うマッパーでフィールド内にある風船を矢で撃ってください。" };

            this.timing.start = 214268;
            this.timing.end = 224256;
            this.position = new Vector2(105, 326);
            Dialog(sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "小休憩としてマッパーの能力を磨き上げてください。",
                                  "あなたの最後のスコアとして加算されるので最大限のベストを尽くしてください！" };

            this.fadeOutDelay = 500;
            this.timing.start = 224256;
            this.timing.end = 233145;
            this.position = new Vector2(105, 326);
            Dialog(sentence2);
            Tochi(214268, 233145);
        }

        public void JP_Section6()
        {
            this.fadeOutDelay = 50;
            this.timing = new Timing();
            var font = FontGenerator("sb/dialog/txt/6/jp", fontJP);
            this.font = font;

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "うまくやりましたね！",
                                  "ただし、次のミッションは専門的な知識が必要になります。" };

            this.timing.start = 252193;
            this.timing.end = 257755;
            this.position = new Vector2(105, 326);
            Dialog(sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "研究のために氷のバイオーム3.5から壊れた機械の部品を回収してください。",
                                   "指のコントロールが最も必要になります、気を付けて。" };

            this.fadeOutDelay = 500;
            this.timing.start = 257755;
            this.timing.end = 265294;
            this.position = new Vector2(105, 326);
            Dialog(sentence2);
            Tochi(252193, 265294);
        }

        public void JP_Section7()
        {
            this.fadeOutDelay = 50;
            this.timing = new Timing();
            var font = FontGenerator("sb/dialog/txt/7/jp", fontJP);
            this.font = font;

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "あれらの機械の部品はきっと使い物になる、あなたの助けに本当に感謝します！",
                                  "次のミッションはリズム感と音楽の知識が必要になります。" };

            this.timing.start = 280529;
            this.timing.end = 288854;
            this.position = new Vector2(105, 326);
            Dialog(sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "始めはマッパーが歌いあなたのことを支えますが、2番目はあなた自身で続けなければなりません。",
                                   "適当にしてしまうと、ポイントに影響が与えられます。",
                                   "気を付けて。" };

            this.fadeOutDelay = 500;
            this.timing.start = 288854;
            this.timing.end = 300483;
            this.position = new Vector2(105, 326);
            Dialog(sentence2);
            Tochi(280529, 300483);
        }

        public void JP_Section8()
        {
            this.fadeOutDelay = 50;
            this.timing = new Timing();
            var font = FontGenerator("sb/dialog/txt/8/jp", fontJP);
            this.font = font;

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "よし、難しいミッションはもう終わり！",
                                  "もうすぐ全ての譜面も終了。" };

            this.timing.start = 331111;
            this.timing.end = 337394;
            this.position = new Vector2(105, 326);
            Dialog(sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "このミッションの方が前のミッションと比べて簡単。",
                                   "建物の後ろから飛んでくる航空機を見逃さないで、不正を見つけたら報告して！" };

            this.fadeOutDelay = 500;
            this.timing.start = 337394;
            this.timing.end = 348176;
            this.position = new Vector2(105, 326);
            Dialog(sentence2);
            Tochi(331111, 348176);
        }

        public void JP_Section9()
        {
            this.fadeOutDelay = 50;
            this.timing = new Timing();
            var font = FontGenerator("sb/dialog/txt/9/jp", fontJP);
            this.font = font;

            // DIALOG 1 -----------------------------------------
            string[] sentence = { "？！",
                                  "何が起こっているの？！私にシステムスキャンをさせてください..." };

            this.timing.start = 380218;
            this.timing.end = 385244;
            this.position = new Vector2(105, 326);
            Dialog(sentence);

            // DIALOG 2 -----------------------------------------
            string[] sentence2 = { "....",
                                   "*Scanning*" };

            this.timing.start = 385244;
            this.timing.end = 394853;
            this.position = new Vector2(105, 326);
            Dialog(sentence2);

            // DIALOG 3 -----------------------------------------
            string[] sentence3 = { "見た感じセクション４からの残りのバグはシステムに侵入し、マッパー達のデーターベースを破損しました...",
                                   "待って。こういうことがあってはダメです...なぜNechoとOtosaka-Yuは戦う必要性があるのですか！" };

            this.timing.start = 394853;
            this.timing.end = 407418;
            this.position = new Vector2(105, 326);
            Dialog(sentence3);

            // DIALOG 4 -----------------------------------------
            string[] sentence4 = { "残念ながらあなたのスコアは必然的に落ちますが、彼たちを助けてください！",
                                   "私たちはあなたにしか頼れません..." };

            this.fadeOutDelay = 500;
            this.timing.start = 407418;
            this.timing.end = 416214;
            this.position = new Vector2(105, 326);
            Dialog(sentence4);
            Tochi(380218, 416214);
        }
    }
}

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
    public class CV : StoryboardObjectGenerator
    {
        private int delay = 200;
        public override void Generate()
        {
		    Noffy();
		    // Reey();
        }

        public void Noffy()
        {
            var volume = 100;

            var section1 = GetLayer("").CreateSample("sb/sfx/voice/noffy/section1_1.ogg", 3245 + delay, volume);
            var section2 = GetLayer("").CreateSample("sb/sfx/voice/noffy/section2_1.ogg", 64629 + delay, volume);
            var section3 = GetLayer("").CreateSample("sb/sfx/voice/noffy/section3_1.ogg", 117905 + delay, volume);
            var section4 = GetLayer("").CreateSample("sb/sfx/voice/noffy/section4_1.ogg", 157678 + delay, volume);
            var section5 = GetLayer("").CreateSample("sb/sfx/voice/noffy/section5_1.ogg", 214268 + delay, volume);
            var section6 = GetLayer("").CreateSample("sb/sfx/voice/noffy/section6_1.ogg", 252193 + delay, volume);
            var section7 = GetLayer("").CreateSample("sb/sfx/voice/noffy/section7_1.ogg", 280529 + delay, volume);
            var section8 = GetLayer("").CreateSample("sb/sfx/voice/noffy/section8_1.ogg", 331111 + delay, volume);
            var section9 = GetLayer("").CreateSample("sb/sfx/voice/noffy/section9_1.ogg", 380218 + delay, volume);
            var section9_2 = GetLayer("").CreateSample("sb/sfx/voice/noffy/section9_2.ogg", 394853 + delay, volume);

            var satelliteSFX = GetLayer("").CreateSample("sb/sfx/satellite.ogg", 385244, 20);

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
        }

        public void Reey()
        {
            var volume = 100;

            var section1 = GetLayer("").CreateSample("sb/sfx/voice/reey/section1_1.ogg", 3245 + delay, volume);
            var section2 = GetLayer("").CreateSample("sb/sfx/voice/reey/section2_1.ogg", 64629 + delay, volume);
            var section3 = GetLayer("").CreateSample("sb/sfx/voice/reey/section3_1.ogg", 117905 + delay, volume);
            var section4 = GetLayer("").CreateSample("sb/sfx/voice/reey/section4_1.ogg", 163062 - 5385 + delay, volume);
            var section5 = GetLayer("").CreateSample("sb/sfx/voice/reey/section5_1.ogg", 214268 + delay, volume);
            var section6 = GetLayer("").CreateSample("sb/sfx/voice/reey/section6_1.ogg", 252193 + delay, volume);
            var section7 = GetLayer("").CreateSample("sb/sfx/voice/reey/section7_1.ogg", 280529 + delay, volume);
            var section8 = GetLayer("").CreateSample("sb/sfx/voice/reey/section8_1.ogg", 331111 + delay, volume);
            var section9 = GetLayer("").CreateSample("sb/sfx/voice/reey/section9_1.ogg", 380218 + delay, volume);
            var section9_2 = GetLayer("").CreateSample("sb/sfx/voice/reey/section9_2.ogg", 394853 + delay, volume);

            var satelliteSFX = GetLayer("").CreateSample("sb/sfx/satellite.ogg", 385244, 20);

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
        }

        public void StaticSFX(int startTime)
        {
            var sfx = GetLayer("").CreateSample("sb/sfx/static.ogg", startTime + delay - 50, 50);
        }
    }
}

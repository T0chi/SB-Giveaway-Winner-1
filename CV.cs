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
            var section1 = GetLayer("").CreateSample("sb/sfx/voice/noffy/sections/section 1_1.ogg", 3245 + delay, 100);
            var section2 = GetLayer("").CreateSample("sb/sfx/voice/noffy/sections/section 2_1.ogg", 64629 + delay, 100);
            var section3 = GetLayer("").CreateSample("sb/sfx/voice/noffy/sections/section 3_1.ogg", 117905 + delay, 100);
            var section4 = GetLayer("").CreateSample("sb/sfx/voice/noffy/sections/section 4_1.ogg", 163062 + delay, 100);
            var section5 = GetLayer("").CreateSample("sb/sfx/voice/noffy/sections/section 5_1.ogg", 214268 + delay, 100);
            var section6 = GetLayer("").CreateSample("sb/sfx/voice/noffy/sections/section 6_1.ogg", 252193 + delay, 100);
            var section7 = GetLayer("").CreateSample("sb/sfx/voice/noffy/sections/section 7_1.ogg", 280529 + delay, 100);
            var section8 = GetLayer("").CreateSample("sb/sfx/voice/noffy/sections/section 8_1.ogg", 331111 + delay, 100);
            var section9 = GetLayer("").CreateSample("sb/sfx/voice/noffy/sections/section 9_1.ogg", 380218 + delay, 100);
            var section9_2 = GetLayer("").CreateSample("sb/sfx/voice/noffy/sections/section 9_2.ogg", 394853 + delay, 100);

            var satelliteSFX = GetLayer("").CreateSample("sb/sfx/satellite.ogg", 385244, 35);

            StaticSFX(3045); // 1
            StaticSFX(13822);
            StaticSFX(16199);
            StaticSFX(26583);

            StaticSFX(64629); // 2
            StaticSFX(74050);

            StaticSFX(117905); // 3
            StaticSFX(130512);

            StaticSFX(163062); // 4
            StaticSFX(180811);

            StaticSFX(214268); // 5
            StaticSFX(224732);

            StaticSFX(252193); // 6
            StaticSFX(257755);

            StaticSFX(280529); // 7
            StaticSFX(291251);

            StaticSFX(331111); // 8
            StaticSFX(337194);

            StaticSFX(380218); // 9
            StaticSFX(394653);
            StaticSFX(407218);
        }

        public void Reey()
        {
            var section1 = GetLayer("").CreateSample("sb/sfx/voice/reey/sections/section1_1.ogg", 3245 + delay, 100);
            var section2 = GetLayer("").CreateSample("sb/sfx/voice/reey/sections/section2_1.ogg", 64629 + delay, 100);
            var section3 = GetLayer("").CreateSample("sb/sfx/voice/reey/sections/section3_1.ogg", 117905 + delay, 100);
            var section4 = GetLayer("").CreateSample("sb/sfx/voice/reey/sections/section4_1.ogg", 163062 + delay, 100);
            var section5 = GetLayer("").CreateSample("sb/sfx/voice/reey/sections/section5_1.ogg", 214268 + delay, 100);
            var section6 = GetLayer("").CreateSample("sb/sfx/voice/reey/sections/section6_1.ogg", 252193 + delay, 100);
            var section7 = GetLayer("").CreateSample("sb/sfx/voice/reey/sections/section7_1.ogg", 280529 + delay, 100);
            var section8 = GetLayer("").CreateSample("sb/sfx/voice/reey/sections/section8_1.ogg", 331111 + delay, 100);
            var section9 = GetLayer("").CreateSample("sb/sfx/voice/reey/sections/section9_1-1.ogg", 380218 + delay, 100);
            var section9_2 = GetLayer("").CreateSample("sb/sfx/voice/reey/sections/section9_2-1.ogg", 394853 + delay, 100);

            var satelliteSFX = GetLayer("").CreateSample("sb/sfx/satellite.ogg", 385244, 35);

            StaticSFX(3245); // 1
            StaticSFX(14322);
            StaticSFX(18475);
            StaticSFX(28168);
            StaticSFX(40629);

            StaticSFX(64629); // 2
            StaticSFX(72571);

            StaticSFX(117905); // 3
            StaticSFX(130062);

            StaticSFX(163062); // 4
            StaticSFX(179982);

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
            var sfx = GetLayer("").CreateSample("sb/sfx/static.ogg", startTime + delay - 50, 80);
        }
    }
}

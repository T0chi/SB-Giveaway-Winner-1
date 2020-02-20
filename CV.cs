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
        private int delay = 350;
        public override void Generate()
        {
		    // Noffy();
		    Reey();
        }

        public void Noffy()
        {
            // SECTION 1
            var section1_1 = GetLayer("").CreateSample("sb/sfx/voice/noffy/sections/section1_1.ogg", 3245 + delay, 100);
            var section1_2 = GetLayer("").CreateSample("sb/sfx/voice/noffy/sections/section1_2.ogg", 10629 + delay, 100);
            var section1_3 = GetLayer("").CreateSample("sb/sfx/voice/noffy/sections/section1_3.ogg", 18475 + delay, 100);
            var section1_4 = GetLayer("").CreateSample("sb/sfx/voice/noffy/sections/section1_4.ogg", 26322 + delay, 100);

            // SECTION 2
            var section2_1 = GetLayer("").CreateSample("sb/sfx/voice/noffy/sections/section2_1.ogg", 66937 + delay, 100);
            var section2_2 = GetLayer("").CreateSample("sb/sfx/voice/noffy/sections/section2_2.ogg", 70629 + delay, 100);

            // SECTION 3
            var section3_1 = GetLayer("").CreateSample("sb/sfx/voice/noffy/sections/section3_1.ogg", 119905 + delay, 100);

            // SECTION 4
            var section4_1 = GetLayer("").CreateSample("sb/sfx/voice/noffy/sections/section4_1.ogg", 163962 + delay, 100);
            var section4_2 = GetLayer("").CreateSample("sb/sfx/voice/noffy/sections/section4_2.ogg", 175611 + delay, 100);
        }

        public void Reey()
        {
            var section1_1 = GetLayer("").CreateSample("sb/sfx/voice/reey/sections/section1_1.ogg", 3245 + delay, 100);
            var section2_1 = GetLayer("").CreateSample("sb/sfx/voice/reey/sections/section2_1.ogg", 64629 + delay, 100);
            var section3_1 = GetLayer("").CreateSample("sb/sfx/voice/reey/sections/section3_1.ogg", 117905 + delay, 100);
            var section4_1 = GetLayer("").CreateSample("sb/sfx/voice/reey/sections/section4_1.ogg", 163062 + delay, 100);
            var section5_1 = GetLayer("").CreateSample("sb/sfx/voice/reey/sections/section5_1.ogg", 214268 + delay, 100);
        }
    }
}

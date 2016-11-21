﻿//Copyright (c) 2007-2016 ppy Pty Ltd <contact@ppy.sh>.
//Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using System.Collections.Generic;
using osu.Framework.GameModes;
using osu.Game.Screens.Backgrounds;
using osu.Game.Screens.Play;
using OpenTK.Graphics;
using osu.Game.Screens.Select;

namespace osu.Game.Screens.Multiplayer
{
    class Match : GameModeWhiteBox
    {
        protected override IEnumerable<Type> PossibleChildren => new[] {
            typeof(MatchSongSelect),
            typeof(Player),
        };

        protected override BackgroundMode CreateBackground() => new BackgroundModeCustom(@"Backgrounds/bg4");

        protected override void OnEntering(GameMode last)
        {
            base.OnEntering(last);

            Background.Schedule(() => Background.FadeColour(Color4.DarkGray, 500));
        }

        protected override bool OnExiting(GameMode next)
        {
            Background.Schedule(() => Background.FadeColour(Color4.White, 500));
            return base.OnExiting(next);
        }
    }
}

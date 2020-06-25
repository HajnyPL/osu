﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using Humanizer;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osu.Game.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.SearchableList;
using osu.Game.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Screens.Multi
{
    public class Header : Container
    {
        public const float HEIGHT = 100;

        public Header(ScreenStack stack)
        {
            RelativeSizeAxes = Axes.X;
            Height = HEIGHT;

            HeaderBreadcrumbControl breadcrumbs;
            MultiHeaderTitle title;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4Extensions.FromHex(@"#1f1921"),
                },
                new Container
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding { Left = SearchableListOverlay.WIDTH_PADDING + OsuScreen.HORIZONTAL_OVERFLOW_PADDING },
                    Children = new Drawable[]
                    {
                        title = new MultiHeaderTitle
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                        },
                        breadcrumbs = new HeaderBreadcrumbControl(stack)
                        {
                            Anchor = Anchor.BottomLeft,
                            Origin = Anchor.BottomLeft
                        }
                    },
                },
            };

            breadcrumbs.Current.ValueChanged += screen =>
            {
                if (screen.NewValue is IMultiplayerSubScreen multiScreen)
                    title.Screen = multiScreen;

                if (breadcrumbs.Items.Any() && screen.NewValue == breadcrumbs.Items.First())
                    breadcrumbs.FadeOut(500, Easing.OutQuint);
                else
                    breadcrumbs.FadeIn(500, Easing.OutQuint);
            };

            breadcrumbs.Current.TriggerChange();
        }

        private class MultiHeaderTitle : CompositeDrawable
        {
            private const float spacing = 6;

            private readonly OsuSpriteText dot;
            private readonly OsuSpriteText pageTitle;

            public IMultiplayerSubScreen Screen
            {
                set => pageTitle.Text = value.ShortTitle.Titleize();
            }

            public MultiHeaderTitle()
            {
                AutoSizeAxes = Axes.Both;

                InternalChildren = new Drawable[]
                {
                    new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Spacing = new Vector2(spacing, 0),
                        Direction = FillDirection.Horizontal,
                        Children = new Drawable[]
                        {
                            new OsuSpriteText
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Font = OsuFont.GetFont(size: 24),
                                Text = "Multiplayer"
                            },
                            dot = new OsuSpriteText
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Font = OsuFont.GetFont(size: 24),
                                Text = "·"
                            },
                            pageTitle = new OsuSpriteText
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Font = OsuFont.GetFont(size: 24),
                                Text = "Lounge"
                            }
                        }
                    },
                };
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                pageTitle.Colour = dot.Colour = colours.Yellow;
            }
        }

        private class HeaderBreadcrumbControl : ScreenBreadcrumbControl
        {
            public HeaderBreadcrumbControl(ScreenStack stack)
                : base(stack)
            {
                RelativeSizeAxes = Axes.X;
                StripColour = Color4.Transparent;
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();
                AccentColour = Color4Extensions.FromHex("#e35c99");
            }

            protected override TabItem<IScreen> CreateTabItem(IScreen value) => new HeaderBreadcrumbTabItem(value)
            {
                AccentColour = AccentColour
            };

            private class HeaderBreadcrumbTabItem : BreadcrumbTabItem
            {
                public HeaderBreadcrumbTabItem(IScreen value)
                    : base(value)
                {
                    Bar.Colour = Color4.Transparent;
                }
            }
        }
    }
}

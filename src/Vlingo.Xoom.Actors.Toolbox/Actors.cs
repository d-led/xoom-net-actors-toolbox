// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Linq.Expressions;

namespace Vlingo.Xoom.Actors.Toolbox
{
    /// <summary>
    /// The three calls that start, use and stop an actor system. Nothing here hides anything: every
    /// method is a one-line delegation to the xoom-actors API it stands for.
    /// </summary>
    public static class Actors
    {
        /// <summary>
        /// Starts a <see cref="World"/> named <paramref name="name"/>, configured with the sensible
        /// defaults of xoom-actors. No <c>vlingo-actors.json</c> file, and no plugin configuration,
        /// is needed.
        /// </summary>
        /// <param name="name">The name of the world.</param>
        /// <returns>The started world.</returns>
        public static World World(string name) => Vlingo.Xoom.Actors.World.StartWithDefaults(name);

        /// <summary>
        /// Starts the actor that the <paramref name="factory"/> creates and answers its protocol.
        /// </summary>
        /// <typeparam name="TProtocol">The protocol the actor implements.</typeparam>
        /// <param name="world">The world in which the actor starts.</param>
        /// <param name="factory">The constructor call of the actor, for example <c>() => new TickerActor()</c>.</param>
        /// <returns>The protocol of the started actor.</returns>
        public static TProtocol Actor<TProtocol>(World world, Expression<Func<TProtocol>> factory) =>
            world.ActorFor(factory);

        /// <summary>
        /// Stops the <paramref name="world"/> and all of its actors.
        /// </summary>
        /// <param name="world">The world to stop.</param>
        public static void Shutdown(World world) => world.Terminate();
    }
}

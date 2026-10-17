// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System.Collections.Generic;
using System.Threading.Tasks;
using Vlingo.Xoom.Common;

namespace Vlingo.Xoom.Actors.Toolbox.Tests
{
    /// <summary>
    /// The ticker implementation. This is all an actor needs: the behaviour, and — only because it
    /// sends messages to itself — its own reference to itself.
    /// </summary>
    public class TickerActor : Actor, ITicker
    {
        private readonly Reported<int> selfTicks;
        private readonly ITicker self;
        private readonly List<object> tags = new List<object>();
        private int count;

        public TickerActor(Reported<int> selfTicks)
        {
            this.selfTicks = selfTicks;
            self = SelfAs<ITicker>();
        }

        public void Tick() => count++;

        public void TickTwice()
        {
            self.Tick();
            self.Tick();
            // Self-sent messages are delivered after the messages already queued, so this third
            // message observes the two ticks above.
            self.Ticks().AndThenConsume(selfTicks.Report);
        }

        public void TickTagged<T>(T tag) => tags.Add(tag!);

        public ICompletes<int> Ticks() => Completes<int>().With(count);

        public Task<int> TicksAsync() => Task.FromResult(count);

        public ICompletes<List<object>> Tags() => Completes<List<object>>().With(tags);
    }
}

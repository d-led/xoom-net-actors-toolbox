// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vlingo.Xoom.Common;
using Vlingo.Xoom.Common.Compiler;
using Xunit;

namespace Vlingo.Xoom.Actors.Toolbox.Tests
{
    /// <summary>
    /// Proves that the proxy the xoom-actors runtime uses is the one generated while the tests were
    /// compiled, and that messages reach the actor through it.
    /// </summary>
    public class TickerTest : IDisposable
    {
        private readonly World world;
        private readonly Reported<int> selfTicks = Reported<int>.Once();
        private readonly ITicker ticker;

        public TickerTest()
        {
            world = Actors.World("toolbox-tests");
            ticker = Actors.Actor<ITicker>(world, () => new TickerActor(selfTicks));
        }

        public void Dispose() => Actors.Shutdown(world);

        [Fact]
        public void TheRuntimeUsesTheBuildTimeGeneratedProxy()
        {
            // Ticker__Proxy is the class of this compilation, so nothing was compiled at run time.
            Assert.Equal(typeof(Ticker__Proxy), ticker.GetType());
            Assert.Same(typeof(Ticker__Proxy).Assembly, ticker.GetType().Assembly);
        }

        [Fact]
        public void TheProxyNameIsTheProtocolNameWithoutTheInterfacePrefix()
        {
            var counter = Actors.Actor<Counter>(world, () => new CounterActor());

            Assert.Equal(typeof(Counter__Proxy), counter.GetType());
        }

        [Fact]
        public void TheRuntimeFindsTheProxyInThisAssembly()
        {
            // ActorProxy looks a proxy up with DynaClassLoader.LoadClass, and that searches only the
            // assemblies whose name starts with "Vlingo". This test assembly is deliberately named
            // Vlingo.Xoom.Actors.Toolbox.Tests; an application that wants its proxies generated at
            // build time has to keep its protocols in a Vlingo-named assembly as well.
            var proxy = new DynaClassLoader().LoadClass("Vlingo.Xoom.Actors.Toolbox.Tests.Ticker__Proxy");

            Assert.Equal(typeof(Ticker__Proxy), proxy);
        }

        [Fact]
        public void VoidMessagesReachTheActorInOrder()
        {
            ticker.Tick();
            ticker.Tick();
            ticker.Tick();

            Assert.Equal(3, Awaited(ticker.Ticks()));
        }

        [Fact]
        public void AnActorSendsToItselfThroughItsGeneratedProxy()
        {
            ticker.TickTwice();

            Assert.Equal(2, selfTicks.Value());
        }

        [Fact]
        public void GenericMessagesReachTheActor()
        {
            ticker.TickTagged("hello");
            ticker.TickTagged(42);

            Assert.Equal(new List<object> { "hello", 42 }, Awaited(ticker.Tags()));
        }

        [Fact]
        public async Task TaskMessagesReachTheActor()
        {
            ticker.Tick();

            Assert.Equal(1, await ticker.TicksAsync());
        }

        private static T Awaited<T>(ICompletes<T> completes)
        {
            var reported = Reported<T>.Once();
            completes.AndThenConsume(reported.Report);
            return reported.Value();
        }
    }
}

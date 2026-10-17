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
    /// The protocol of the ticker. The generator writes <c>Ticker__Proxy</c> from this interface
    /// while the tests are compiled.
    /// </summary>
    [ActorProtocol]
    public interface ITicker
    {
        void Tick();

        /// <summary>Ticks twice, by sending two <c>Tick</c> messages to itself.</summary>
        void TickTwice();

        void TickTagged<T>(T tag);

        ICompletes<int> Ticks();

        Task<int> TicksAsync();

        ICompletes<List<object>> Tags();
    }
}

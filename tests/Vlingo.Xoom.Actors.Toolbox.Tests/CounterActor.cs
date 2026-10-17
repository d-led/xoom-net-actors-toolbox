// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Vlingo.Xoom.Common;

namespace Vlingo.Xoom.Actors.Toolbox.Tests
{
    public class CounterActor : Actor, Counter
    {
        private int count;

        public CounterActor() => count = 0;

        public ICompletes<int> Count() => Completes<int>().With(++count);
    }
}

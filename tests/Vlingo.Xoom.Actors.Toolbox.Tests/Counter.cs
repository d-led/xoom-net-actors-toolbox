// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Vlingo.Xoom.Common;

namespace Vlingo.Xoom.Actors.Toolbox.Tests
{
    /// <summary>
    /// A protocol that does not start with the C#-style <c>I</c> prefix, which pins the name the
    /// proxy gets: the toolbox must not strip an <c>I</c> that is not an interface prefix.
    /// </summary>
    [ActorProtocol]
    public interface Counter
    {
        ICompletes<int> Count();
    }
}

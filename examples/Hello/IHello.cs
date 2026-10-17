// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Vlingo.Xoom.Actors.Toolbox;
using Vlingo.Xoom.Common;

namespace HelloExample;

/// <summary>
/// What the actor does, as seen from the outside. The toolbox writes <c>Hello__Proxy</c> from this
/// interface while the project is compiled.
/// </summary>
[ActorProtocol]
public interface IHello
{
    /// <summary>Answers a greeting for <paramref name="name"/>.</summary>
    ICompletes<string> Hello(string name);
}

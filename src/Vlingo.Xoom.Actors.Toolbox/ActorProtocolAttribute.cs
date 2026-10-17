// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;

namespace Vlingo.Xoom.Actors.Toolbox
{
    /// <summary>
    /// Tags an interface as the protocol of an <see cref="Actor"/>. The toolbox writes the
    /// <c>&lt;Protocol&gt;__Proxy</c> class while the project is compiled, so the xoom-actors
    /// runtime never has to compile a proxy while your application is starting up.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, Inherited = false)]
    public sealed class ActorProtocolAttribute : Attribute
    {
    }
}

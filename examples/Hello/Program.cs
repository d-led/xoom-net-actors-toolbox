// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Vlingo.Xoom.Actors.Toolbox;
using HelloExample;

// Hello, World!: start a world, start the actor in it, greet, stop.
var world = Actors.World("hello");
var hello = Actors.Actor<IHello>(world, () => new HelloActor());

Console.WriteLine(hello.Hello("World").Await());

Actors.Shutdown(world);

// Copyright © 2012-2023 VLINGO LABS. All rights reserved.
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using Vlingo.Xoom.Actors.TestKit;

namespace Vlingo.Xoom.Actors.Toolbox.Tests
{
    /// <summary>
    /// Reports a value from an actor to a test, and waits for it the way the xoom-actors test kit
    /// does: <see cref="AccessSafely"/> blocks the reading side until the expected write happened.
    /// </summary>
    /// <remarks>
    /// Public because an actor that is created from an expression, as <c>Definition.Has</c> does,
    /// must be reachable from the compiled expression.
    /// </remarks>
    public sealed class Reported<T>
    {
        private readonly AccessSafely access;
        private T value = default!;

        private Reported()
        {
            access = AccessSafely
                .AfterCompleting(1)
                .WritingWith<T>("value", reported => value = reported)
                .ReadingWith("value", () => value);
        }

        public static Reported<T> Once() => new Reported<T>();

        public void Report(T reportedValue) => access.WriteUsing("value", reportedValue);

        public T Value() => access.ReadFrom<T>("value");
    }}

# Vlingo.Xoom.Actors.Toolbox

Compile-time proxy generation for [Vlingo.Xoom.Actors](https://github.com/vlingo-net/xoom-net-actors),
plus the three calls that start, use and stop an actor system.

Nothing in Vlingo.Xoom.Actors changes. Its runtime already looks for a `<Protocol>__Proxy` class
before it compiles one with Roslyn while your application runs; the toolbox provides that class while
your project is built, and wires the `World` up so no `vlingo-actors.json` is needed.

## Quickstart

Two dependencies:

```xml
<PackageReference Include="Vlingo.Xoom.Actors" Version="1.10.1" />
<PackageReference Include="Vlingo.Xoom.Actors.Toolbox" Version="0.1.0" />
```

All the code:

```csharp
[ActorProtocol]
public interface IHello
{
    ICompletes<string> Hello(string name);
}

public class HelloActor : Actor, IHello
{
    public ICompletes<string> Hello(string name) => Completes<string>().With($"Hello, {name}!");
}

var world = Actors.World("hello");
var hello = Actors.Actor<IHello>(world, () => new HelloActor());

Console.WriteLine(hello.Hello("World").Await());   // Hello, World!

Actors.Shutdown(world);
```

Run the example, which is exactly this:

```bash
dotnet pack -o artifacts && dotnet run --project examples/Hello
```

## Notes

* **The assembly that holds the protocols must be named with a `Vlingo` prefix.** The runtime looks
  proxies up through `DynaClassLoader.LoadClass`, which searches only
  `AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("Vlingo"))`. Put the
  protocols in their own library, e.g. `Vlingo.YourApp.Actors`; the actors and the rest of the
  application can live anywhere. The example sets `<AssemblyName>Vlingo.HelloExample</AssemblyName>`.
* **One line of plumbing stays**, and only for actors that send messages to themselves:
  `private readonly IHello self = SelfAs<IHello>();`
* **Every protocol shape is generated:** `void`, `ICompletes<T>`, `Task`/`Task<T>`, generic methods,
  constraints, properties.
* **Nested protocols** are left to the runtime (warning `XOOM001`).
* **The generator is Roslyn's answer to Lombok** — it comes inside this package
  (`analyzers/dotnet/cs`), so there is no extra step, no `vlingo-actors.json`, nothing to run.

## Try it before the official release

The `Publish` workflow pushes every build to GitHub Packages: each push to `main` as
`0.1.0-snapshot.<build-number>`, each `v*` tag as that tag's version (`v0.1.0` → `0.1.0`).
GitHub Packages asks for a token with `read:packages` — your user name, and a personal access token
as the password.

```bash
dotnet nuget add source "https://nuget.pkg.github.com/d-led/index.json" \
  --name d-led --username YOUR_GITHUB_USERNAME --password YOUR_GITHUB_TOKEN \
  --store-password-in-clear-text
```

Or in a `nuget.config` next to your project:

```xml
<configuration>
  <packageSources>
    <add key="d-led" value="https://nuget.pkg.github.com/d-led/index.json" />
  </packageSources>
  <packageSourceCredentials>
    <d-led>
      <add key="Username" value="YOUR_GITHUB_USERNAME" />
      <add key="ClearTextPassword" value="YOUR_GITHUB_TOKEN" />
    </d-led>
  </packageSourceCredentials>
</configuration>
```

Then use `Vlingo.Xoom.Actors.Toolbox` version `0.1.0`, or a snapshot from the packages page.

## Releasing

Pushing a `v*` tag publishes that version, for example `git tag v0.1.0 && git push origin v0.1.0`.
By hand:

```bash
dotnet pack -c Release -o artifacts
dotnet nuget push "artifacts/*.nupkg" \
  --source "https://nuget.pkg.github.com/d-led/index.json" --api-key "$GITHUB_TOKEN"
```

## Building

```bash
dotnet test tests/Vlingo.Xoom.Actors.Toolbox.Tests   # proxy identity, loader, every message shape
dotnet pack -o artifacts && dotnet run --project examples/Hello
```

To read the generated code, build with `-p:EmitCompilerGeneratedFiles=true`; the proxies appear under
`obj/<configuration>/<tfm>/generated/`.

## License

Mozilla Public License, v. 2.0. See [LICENSE](LICENSE).

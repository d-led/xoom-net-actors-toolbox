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

## Publishing

```bash
dotnet pack -c Release -o artifacts
dotnet nuget push "artifacts/*.nupkg" \
  --source "https://nuget.pkg.github.com/d-led/index.json" --api-key "$GITHUB_TOKEN"
```

Set `RepositoryUrl` in `src/Vlingo.Xoom.Actors.Toolbox/Vlingo.Xoom.Actors.Toolbox.csproj` to the
repository you publish to. See [`.github/workflows/publish.yml`](.github/workflows/publish.yml).

## Building

```bash
dotnet test tests/Vlingo.Xoom.Actors.Toolbox.Tests   # proxy identity, loader, every message shape
dotnet pack -o artifacts && dotnet run --project examples/Hello
```

To read the generated code, build with `-p:EmitCompilerGeneratedFiles=true`; the proxies appear under
`obj/<configuration>/<tfm>/generated/`.

## License

Mozilla Public License, v. 2.0. See [LICENSE](LICENSE).

# A back-reference under `Func<T1, T>` makes the override rewrite visit the graph once per path again

**Pure.DI master (da6574d)** and **2.5.7**, .NET SDK 10.0.403, macOS. Repro project attached (`dotnet build`, about 27 s).

Follow-up to #172. On master, the graph under a delegate argument is now rewritten once per override scope, and the #172 diamond builds in about 1 s. When a node of that graph refers back to a node above it on the current path, for example through `Lazy<T>`, the rewrite is not cached (`minBackReferenceDepth >= depth` in `GraphOverrider.Override`). The nodes under the cycle are then rewritten once per dependency path again.

## Minimal code

The #172 diamond of singletons under a `Func<string, L23>`. The leaf consumes the delegate argument and refers back to the top through `Lazy<L23>`.

```csharp
using Pure.DI;

var composition = new Composition();
Console.WriteLine(composition.Root.GetType().Name);

public sealed class L0(string label, Lazy<L23> top);
public sealed class L1(L0 first, L0 second);
// ... same pattern ...
public sealed class L23(L22 first, L22 second);
public sealed class Top(Func<string, L23> factory) { public L23 Make(string label) => factory(label); }

partial class Composition
{
    private static void Setup() => DI.Setup(nameof(Composition))
        .Bind<string>().To(_ => "default")
        .Bind<L0>().As(Lifetime.Singleton).To<L0>()
        // ... every Lk as Singleton ...
        .Bind<L23>().As(Lifetime.Singleton).To<L23>()
        .Root<Top>("Root");
}
```

## Expected

About the same time as without the back-reference, or with the same cycle outside the delegate: 2 s for every size below. Only `L0` and `Lazy<L23>` are on a cycle; the 23 other levels are the same nodes on every path.

## Actual

`levels` = number of `Lk` classes; the attached project has 24. Full `dotnet build` of a fresh project, master (da6574d).

| levels | `L0(string, Lazy<Ltop>)` under `Func<string, Ltop>` | `L0(string, Func<string, Ltop>)` under `Func<string, Ltop>` | no back-reference | same cycle, root `Top(Ltop)` |
|---|---|---|---|---|
| 22 | 8 s | 5 s | | |
| 24 (attached) | 27 s | 14 s | 2 s | |
| 26 | 100 s | 48 s | 2 s | 2 s |

The generated code is small in every case (about 650 lines). 2.5.7 takes about the same time on the attached project (25 s), but it already rewrote the #172 diamond once per path without the back-reference.

## Real-world impact

On a real application (about 2,500 bindings), master no longer gets stuck the way #172 / #175 described: in the first two minutes the generator stored only 84,000 rewrites in `RewriteState.Rewritten`. But it made 13 million `Override` calls. A few singleton services were rewritten 270,000 to 510,000 times each. They sit in a cycle of singletons through `Lazy<T>` (a projection service whose builders depend on a project-context provider, which creates the context through `Func<IUser, IProject, IProjectContext>`). That cycle is reached under delegates with arguments (`Func<string, ...>`, `Func<Options?, ...>`, `Func<IUser, IProject, ...>`) from many places. We stopped the build after 6 minutes.

# Pure.DI: a back-reference under `Func<T1, T>` makes the override rewrite visit the graph once per path again

Minimal repro for Pure.DI master (da6574d) and 2.5.7: the dependency diamond from #172 (24 singletons under a `Func<string, L23>`), where the leaf also refers back to the top through `Lazy<L23>`, takes about 27 s to generate. Without the `Lazy<L23>`, or with the same cycle outside the delegate, it takes 2 s. Every added level multiplies the time by about 2.

[`ISSUE.md`](ISSUE.md) is the text to paste into a Pure.DI GitHub issue; it includes the measurements from 22 to 26 levels and the controls.

```
dotnet build                                                                                   # 2.5.7 from nuget.org
dotnet build -p:PureDIVersion=<master build version> -p:RestoreAdditionalProjectSources=<folder with the package>
```

Controls:

- remove `Lazy<L23> top` from `L0`: 2 s;
- replace `Top(Func<string, L23> factory)` with `Top(L23 root)` (same cycle, no delegate argument): 2 s.

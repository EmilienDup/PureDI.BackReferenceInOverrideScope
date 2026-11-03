using Pure.DI;

var composition = new Composition();
Console.WriteLine(composition.Root.GetType().Name);

// L0 consumes the delegate argument and refers back to the top of the graph through Lazy<T>
public sealed class L0(string label, Lazy<L23> top);
public sealed class L1(L0 first, L0 second);
public sealed class L2(L1 first, L1 second);
public sealed class L3(L2 first, L2 second);
public sealed class L4(L3 first, L3 second);
public sealed class L5(L4 first, L4 second);
public sealed class L6(L5 first, L5 second);
public sealed class L7(L6 first, L6 second);
public sealed class L8(L7 first, L7 second);
public sealed class L9(L8 first, L8 second);
public sealed class L10(L9 first, L9 second);
public sealed class L11(L10 first, L10 second);
public sealed class L12(L11 first, L11 second);
public sealed class L13(L12 first, L12 second);
public sealed class L14(L13 first, L13 second);
public sealed class L15(L14 first, L14 second);
public sealed class L16(L15 first, L15 second);
public sealed class L17(L16 first, L16 second);
public sealed class L18(L17 first, L17 second);
public sealed class L19(L18 first, L18 second);
public sealed class L20(L19 first, L19 second);
public sealed class L21(L20 first, L20 second);
public sealed class L22(L21 first, L21 second);
public sealed class L23(L22 first, L22 second);
public sealed class Top(Func<string, L23> factory) { public L23 Make(string label) => factory(label); }

partial class Composition
{
    private static void Setup() => DI.Setup(nameof(Composition))
        .Bind<string>().To(_ => "default")
        .Bind<L0>().As(Lifetime.Singleton).To<L0>()
        .Bind<L1>().As(Lifetime.Singleton).To<L1>()
        .Bind<L2>().As(Lifetime.Singleton).To<L2>()
        .Bind<L3>().As(Lifetime.Singleton).To<L3>()
        .Bind<L4>().As(Lifetime.Singleton).To<L4>()
        .Bind<L5>().As(Lifetime.Singleton).To<L5>()
        .Bind<L6>().As(Lifetime.Singleton).To<L6>()
        .Bind<L7>().As(Lifetime.Singleton).To<L7>()
        .Bind<L8>().As(Lifetime.Singleton).To<L8>()
        .Bind<L9>().As(Lifetime.Singleton).To<L9>()
        .Bind<L10>().As(Lifetime.Singleton).To<L10>()
        .Bind<L11>().As(Lifetime.Singleton).To<L11>()
        .Bind<L12>().As(Lifetime.Singleton).To<L12>()
        .Bind<L13>().As(Lifetime.Singleton).To<L13>()
        .Bind<L14>().As(Lifetime.Singleton).To<L14>()
        .Bind<L15>().As(Lifetime.Singleton).To<L15>()
        .Bind<L16>().As(Lifetime.Singleton).To<L16>()
        .Bind<L17>().As(Lifetime.Singleton).To<L17>()
        .Bind<L18>().As(Lifetime.Singleton).To<L18>()
        .Bind<L19>().As(Lifetime.Singleton).To<L19>()
        .Bind<L20>().As(Lifetime.Singleton).To<L20>()
        .Bind<L21>().As(Lifetime.Singleton).To<L21>()
        .Bind<L22>().As(Lifetime.Singleton).To<L22>()
        .Bind<L23>().As(Lifetime.Singleton).To<L23>()
        .Root<Top>("Root");
}

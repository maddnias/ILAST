using System;
using dnlib.DotNet;
using ILAST.AST.Base;

namespace ILAST.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var asmDef = AssemblyDef.Load(args[0]);
            var method = asmDef.ManifestModule.EntryPoint.DeclaringType.FindMethod("Foo");
            var asmResolver = new AssemblyResolver();
            var modCtx = new ModuleContext(asmResolver);

            var ast = new ILAST(method, modCtx);

            Console.WriteLine("-------------------------------");
            Console.WriteLine("         Standard AST");
            Console.WriteLine("-------------------------------");

            foreach (Element element in ast.Elements)
                Console.WriteLine("{0}: {1}", element.GetType().Name, element);

            ILAST.SimplifyElements(ast.Elements);
            Console.WriteLine("\n-------------------------------");
            Console.WriteLine("        Simplified AST");
            Console.WriteLine("-------------------------------");

            foreach (Element element in ast.Elements)
                Console.WriteLine("{0}: {1}", element.GetType().Name, element);

            Console.WriteLine(asmDef);
        }
    }
}

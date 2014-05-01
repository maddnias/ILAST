using System.Collections.Generic;
using System.Linq;
using de4dot.blocks;
using dnlib.DotNet;
using ILAST.AST;
using ILAST.AST.Base;
using ILAST.Visitor;

namespace ILAST
{
    public class ILAST
    {
        public static MethodDef Method { get; set; }
        public List<Element> Elements { get; set; }

        private List<BlockAnalyzer> BlockAnalyzers { get; set; }

        public ILAST(MethodDef method)
        {
            Method = method;
            BlockAnalyzers = new List<BlockAnalyzer>();
            Elements = new List<Element>();

            ProcessMethod();
        }

        public ILAST(MethodDef method, ModuleContext modCtx)
        {
            Method = method;
            Method.DeclaringType.Module.Context = modCtx;
            BlockAnalyzers = new List<BlockAnalyzer>();
            Elements = new List<Element>();

            ProcessMethod();
        }

        void ProcessMethod()
        {
            PopulateBlockAnalyzers();
            PopulateElements();
            ProcessElements();
            VerifyElements();
        }

        void PopulateBlockAnalyzers()
        {
            var blocks = new Blocks(Method).MethodBlocks.GetAllBlocks();

            foreach (var block in blocks)
                BlockAnalyzers.Add(new BlockAnalyzer(block, Method));
        }

        void PopulateElements()
        {
            foreach (var analyzer in BlockAnalyzers)
                Elements.AddRange(analyzer.Elements);
        }

        void ProcessElements()
        {
            foreach (var expr in Elements)
            {
                if (expr is ReturnExpression)
                {
                    if (Method.ReturnType.ToReflectionType() != typeof (void))
                        expr.Populate();
                }
                else expr.Populate();
            }
        }

        public static void SimplifyElements(IList<Element> elements)
        {
            var removals = new List<Element>();

            foreach (var element in elements.Where(ele => ele.CanSimplify))
                if (element is ReturnExpression)
                {
                    if (Method.ReturnType.ToReflectionType() != typeof (void))
                        removals.AddRange(element.RemovableElements);
                }
                else removals.AddRange(element.RemovableElements);

            foreach (var element in removals)
                elements.Remove(element);
        }

        void VerifyElements()
        {
            var tcv = new TypeCheckVisitor();
            var csv = new CallStatementVisitor();

            foreach (var element in Elements)
                element.AcceptVisitor(tcv);

            foreach (var call in Elements.Where(ele => ele is CallStatement))
                call.AcceptVisitor(csv);
        }
    }
}

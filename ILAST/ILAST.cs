using System.Collections.Generic;
using System.Linq;
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

        public ILAST(MethodDef method)
        {
            Method = method;
            Elements = new List<Element>();

            ProcessMethod();
        }

        public ILAST(MethodDef method, ModuleContext modCtx)
        {
            Method = method;
            Method.DeclaringType.Module.Context = modCtx;
            Elements = new List<Element>();

            ProcessMethod();
        }

        void ProcessMethod()
        {
            PopulateElements();
            LinkElements();
            ProcessElements();
            VerifyElements();
        }

        void LinkElements()
        {
            for (var i = 0; i < Elements.Count; i++)
            {
                if (i != 0)
                    Elements[i].Previous = Elements[i - 1];
                if (i != Elements.Count - 1)
                    Elements[i].Next = Elements[i + 1];
            }
        }

        void PopulateElements()
        {
            var exprFactory = new ExpressionFactory();
            Elements = Method.Body.Instructions.SelectMany(instr => exprFactory.GetElements(instr, Method)).ToList();
        }

        void ProcessElements()
        {
            for (int i = 0; i < Elements.Count; i++)
            {
                var expr = Elements[i];
                if (expr is ReturnExpression)
                {
                    if (Method.ReturnType.ToReflectionType() != typeof (void))
                        expr.Populate();
                }
                else if (expr is ConditionalBranchExpression)
                {
                    expr.Populate();
                    if ((expr as ConditionalBranchExpression).IsForLoop(Method))
                    {
                        var branch = expr;
                        Elements[i] = new ForLoopStatement(expr.AssociatedInstruction);
                        expr = Elements[i];
                        (expr as ForLoopStatement).Condition = branch as Expression;
                        expr.Previous = branch.Previous;
                        expr.Next = branch.Next;
                        expr.Populate();
                    }
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

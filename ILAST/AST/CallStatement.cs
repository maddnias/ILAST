using System.Collections.Generic;
using System.Linq;
using System.Text;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using ILAST.AST.Base;
using ILAST.Visitor.Base;

namespace ILAST.AST
{
    public class CallStatement : Statement
    {
        public MethodDef Target { get; set; }
        public IList<Expression> ArgumentExpressions { get; set; }

        public CallStatement(Instruction instr, MethodDef target)
            : base(instr)
        {
            Target = target;
            ArgumentExpressions = new List<Expression>();
        }

        public override void AcceptVisitor(ElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder();
            strBuilder.Append(Target.Name + "(");

            foreach (var expr in ArgumentExpressions)
                strBuilder.Append(expr + (expr == ArgumentExpressions.Last() ? "" : ", "));

            strBuilder.Append(")");
            return strBuilder.ToString();
        }

        public override int ElementSize { get { return 2; } }
        public override bool CanSimplify { get { return true; } }

        public override IEnumerable<Element> RemovableElements
        {
            get { return ArgumentExpressions; }
        }

        public override void Populate()
        {
            Element cur = this;
            for (var i = 0; i < Target.Parameters.Count; i++)
            {
                cur = cur.GetPrevious(1);
                ArgumentExpressions.Add(cur as Expression);
            }
        }
    }
}

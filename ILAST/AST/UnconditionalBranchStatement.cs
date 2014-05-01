using System.Collections.Generic;
using dnlib.DotNet.Emit;
using ILAST.AST.Base;
using ILAST.Visitor.Base;

namespace ILAST.AST
{
    public class UnconditionalBranchExpression : Expression
    {
        public UnconditionalBranchExpression(Instruction instr)
            : base(instr)
        {
        }

        public Element TargetElement { get; set; }
        public override int ElementSize { get { return 1; } }
        public override bool CanSimplify { get { return false; } }

        public override IEnumerable<Element> RemovableElements
        {
            get { throw new System.NotImplementedException(); }
        }

        public override void Populate()
        {
            TargetElement = this.ResolveTargetElement();
        }

        public override void AcceptVisitor(ElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return @"""";
        }
    }
}

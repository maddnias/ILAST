using System.Collections.Generic;
using dnlib.DotNet.Emit;
using ILAST.AST.Base;
using ILAST.Visitor.Base;

namespace ILAST.AST
{
    public class AssignmentStatement : Statement
    {
        public AssignmentStatement(Instruction instr)
            : base(instr)
        {
        }

        public Expression Target { get; set; }
        public Expression Value { get; set; }
        public override int ElementSize { get { return 2; } }
        public override bool CanSimplify { get { return true; } }

        public override IEnumerable<Element> RemovableElements
        {
            get
            {
                yield return Value;
                yield return Target;
            }
        }

        public override void Populate()
        {
            Value = this.GetPrevious(2) as Expression;
            Target = this.GetPrevious(1) as Expression;
        }

        public override void AcceptVisitor(ElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return string.Format("{0} = {1}", Target, Value);
        }

    }
}
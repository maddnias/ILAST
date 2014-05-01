using System;
using System.Collections.Generic;
using dnlib.DotNet.Emit;
using ILAST.AST.Base;
using ILAST.Visitor.Base;

namespace ILAST.AST
{
    public class ForLoopStatement : Loop
    {
        public ForLoopStatement(Instruction instr)
            : base(instr)
        {
        }

        public Element Iterator { get; set; }
        public Element BodyStart { get; set; }
        public Expression Condition { get; set; }
        public AssignmentStatement Incrementor { get; set; }
        public override bool CanSimplify { get { return false; } }

        public override IEnumerable<Element> RemovableElements
        {
            get { throw new System.NotImplementedException(); }
        }

        public override void Populate()
        {
            var target = Condition.ResolveTargetElement();
            BodyStart = target;
            Iterator = target.GetPrevious(2);
            Incrementor = Condition.GetPrevious(3) as AssignmentStatement;
        }

        public override int ElementSize
        {
            get { return 4; }
        }

        public override void AcceptVisitor(ElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return "for(" + Iterator + "; " + Condition + "; " + Incrementor + ")";
        }
    }
}
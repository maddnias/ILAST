using System;
using System.Collections.Generic;
using de4dot.blocks;
using ILAST.AST.Base;
using ILAST.Visitor;
using ILAST.Visitor.Base;

namespace ILAST.AST
{
    public class ReturnExpression : Expression
    {
        public ReturnExpression(Instr instr)
            : base(instr)
        {
        }

        public override void Populate()
        {
            ReturnValue = this.GetPrevious(1);
        }

        public Expression ReturnValue { get; set; }
        public override int ElementSize { get { return 2; } }
        public override bool CanSimplify { get { return true; } }

        public override IEnumerable<Element> RemovableElements
        {
            get
            {
                yield return ReturnValue;
            }
        }

        public override void AcceptVisitor(ElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return "ret " + ReturnValue;
        }
    }
}

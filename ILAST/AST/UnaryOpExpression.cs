using System;
using System.Collections.Generic;
using dnlib.DotNet.Emit;
using ILAST.AST.Base;
using ILAST.Visitor.Base;

namespace ILAST.AST
{
    public enum UnaryOps
    {
        Not,
        Negate
    }

    public class UnaryOpExpression : Expression
    {
        public UnaryOpExpression(Instruction instr)
            : base(instr)
        {
        }

        public override void Populate()
        {
            Value = this.GetPrevious(1) as Expression;
        }

        public Expression Value { get; set; }
        public UnaryOps Operation { get; set; }
        public override int ElementSize { get { return 1; } }
        public override bool CanSimplify { get { return true; } }

        public override IEnumerable<Element> RemovableElements
        {
            get
            {
                yield return Value;    
            }
        }

        public override void AcceptVisitor(ElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            string op;
            switch (Operation)
            {
                case UnaryOps.Not: op = "~"; break;
                case UnaryOps.Negate: op = "-"; break;
                default: throw new Exception();
            }
            return op + Value.ToString();
        }
    }
}
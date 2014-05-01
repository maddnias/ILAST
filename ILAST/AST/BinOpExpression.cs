using System;
using System.Collections.Generic;
using dnlib.DotNet.Emit;
using ILAST.AST.Base;
using ILAST.Visitor.Base;

namespace ILAST.AST
{
    public enum BinOps
    {
        Add,
        Sub,
        Div,
        Mul,
        Or,
        And,
        Xor,
        Lsh,
        Rsh,
    }

    public class BinOpExpression : Expression
    {
        public BinOpExpression(Instruction instr)
            : base(instr)
        {
        }

        public Expression Left { get; set; }
        public Expression Right { get; set; }
        public BinOps Operation { get; set; }
        public override int ElementSize { get { return 2; } }
        public override bool CanSimplify { get { return true; } }

        public override IEnumerable<Element> RemovableElements
        {
            get
            {
                yield return Left;
                yield return Right;
            }
        }

        public override void Populate()
        {
            Left = this.GetPrevious(2) as Expression;
            Right = this.GetPrevious(1) as Expression;
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
                case BinOps.Add: op = "+"; break;
                case BinOps.Sub: op = "-"; break;
                case BinOps.Div: op = "/"; break;
                case BinOps.Mul: op = "*"; break;
                case BinOps.Or: op = "|"; break;
                case BinOps.And: op = "&"; break;
                case BinOps.Xor: op = "^"; break;
                case BinOps.Lsh: op = "<<"; break;
                case BinOps.Rsh: op = ">>"; break;
                default: throw new Exception();
            }
            return string.Format("({0} {1} {2})", Left, op, Right);
        }
    }
}
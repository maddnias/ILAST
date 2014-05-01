using System;
using System.Collections.Generic;
using dnlib.DotNet.Emit;
using ILAST.AST.Base;
using ILAST.Visitor.Base;

namespace ILAST.AST
{
    public enum ConditionalOps
    {
        GreaterThanOrEqual,
        GreaterThan,
        LessThanOrEqual,
        LessThan
    }

    public class ConditionalBranchExpression : Expression
    {
        public ConditionalBranchExpression(Instruction instr)
            : base(instr)
        {
            
        }

        public override void Populate()
        {
            Left = this.GetPrevious(2) as Expression;
            Right = this.GetPrevious(1) as Expression;
            TargetElement = this.ResolveTargetElement();
        }

        public ConditionalOps Operation { get; set; }
        public Expression Left { get; set; }
        public Expression Right { get; set; }
        public Element TargetElement { get; set; }
        public override bool CanSimplify { get { return true; } }

        public override IEnumerable<Element> RemovableElements
        {
            get
            {
                yield return Left;
                yield return Right;
            }
        }

        public override void AcceptVisitor(ElementVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override int ElementSize
        {
            get { return 2; }
        }

        public override string ToString()
        {
            switch (Operation)
            {
                case ConditionalOps.GreaterThanOrEqual:
                    return Left + " >= " + Right;

                case ConditionalOps.GreaterThan:
                    return Left + " > " + Right;

                case ConditionalOps.LessThanOrEqual:
                    return Left + " <= " + Right;

                case ConditionalOps.LessThan:
                    return Left + " < " + Right;
            }

            throw new Exception();
        }
    }
}
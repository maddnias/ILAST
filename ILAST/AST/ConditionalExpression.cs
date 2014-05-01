using System;
using System.Collections.Generic;
using de4dot.blocks;
using ILAST.AST.Base;
using ILAST.Visitor;
using ILAST.Visitor.Base;

namespace ILAST.AST
{
    public class ConditionalExpression : Expression
    {
        public ConditionalExpression(Instr instr)
            : base(instr)
        {
        }

        public override void Populate()
        {
            throw new NotImplementedException();
        }

        public Expression Left { get; set; }
        public Expression Right { get; set; }
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
            throw new NotImplementedException();
        }
    }
}
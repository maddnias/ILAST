using System;
using System.Collections.Generic;
using de4dot.blocks;
using ILAST.AST.Base;
using ILAST.Visitor;
using ILAST.Visitor.Base;

namespace ILAST.AST
{
    public class LoopStatement : Statement
    {
        public LoopStatement(Instr instr) : base(instr)
        {
        }

        public Expression Start { get; set; }
        public Expression Ender { get; set; }
        public override bool CanSimplify { get { return true; } }

        public override IEnumerable<Element> RemovableElements
        {
            get { throw new System.NotImplementedException(); }
        }

        public override void Populate()
        {
            throw new NotImplementedException();
        }

        public override int ElementSize
        {
            get { throw new System.NotImplementedException(); }
        }

        public override void AcceptVisitor(ElementVisitor visitor)
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return "";
        }
    }
}
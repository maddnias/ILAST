using System.Collections.Generic;
using dnlib.DotNet.Emit;
using ILAST.Visitor.Base;

namespace ILAST.AST.Base
{
    public abstract class Element
    {
        public abstract void AcceptVisitor(ElementVisitor visitor);
        public abstract override string ToString();
        public abstract int ElementSize { get; }
        public abstract bool CanSimplify { get; }
        public Element Previous { get; set; }
        public Element Next { get; set; }
        public Instruction AssociatedInstruction { get; set; }

        protected Element(Instruction instr)
        {
            AssociatedInstruction = instr;
        }

        public abstract IEnumerable<Element> RemovableElements { get; } 
        public abstract void Populate();
    }
}

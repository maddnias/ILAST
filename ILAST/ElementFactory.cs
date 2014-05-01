using System.Collections.Generic;
using dnlib.DotNet;
using dnlib.DotNet.Emit;
using ILAST.AST.Base;

namespace ILAST
{
    public abstract class ElementFactory
    {
        public abstract IEnumerable<Element> GetElements(Instruction instr, MethodDef method);
    }
}

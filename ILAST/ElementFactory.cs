using System.Collections.Generic;
using de4dot.blocks;
using dnlib.DotNet;
using ILAST.AST;
using ILAST.AST.Base;

namespace ILAST
{
    public abstract class ElementFactory
    {
        public abstract IEnumerable<Element> GetElements(Instr instr, MethodDef method);
    }
}

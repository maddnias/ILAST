using de4dot.blocks;

namespace ILAST.AST.Base
{
    public abstract class Expression : Element
    {
        protected Expression(Instr instr) : base(instr)
        {
        }
    }
}
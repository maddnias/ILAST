using de4dot.blocks;

namespace ILAST.AST.Base
{
    public abstract class Statement : Element
    {
        protected Statement(Instr instr) : base(instr)
        {
        }
    }
}
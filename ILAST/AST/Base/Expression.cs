using dnlib.DotNet.Emit;

namespace ILAST.AST.Base
{
    public abstract class Expression : Element
    {
        protected Expression(Instruction instr)
            : base(instr)
        {
        }
    }
}
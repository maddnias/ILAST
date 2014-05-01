using dnlib.DotNet.Emit;

namespace ILAST.AST.Base
{
    public abstract class Loop : Statement
    {
        protected Loop(Instruction instr)
            : base(instr)
        {
        }
    }
}